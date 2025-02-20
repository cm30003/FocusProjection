using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class CharaController : MonoBehaviour
{
    public float Distance;//人物与目标之间的距离

    public AIDestinationSetter Target;//目标地点

    public NPC_status NPC_Status;//小动物状态

    private AIPath aiPath;

    [Tooltip("工作的田地")]
    public plant_State Work_Field;
    [Tooltip("搬运的货物")]
    public PlantData freight;//货物
    [Header("————动物/数据————")]
    public NPCData Template_data;
    public NPCData data;

    private void Start()
    {
        //data=JsonManager.Instance.LoadData<NPCData>(this.name);

        Template_data = new NPCData(data) ;//生成副本

        Event_SignIn();

        Target = GetComponent<AIDestinationSetter>();
        Target.target = null;

        aiPath = GetComponent<AIPath>();
    }
    #region 事件
    /// <summary>
    /// 事件注册
    /// </summary>
    private void Event_SignIn()
    {
        /*————事件————*/
        //专注时间设置事件，当专注时间被设置，小动物会去工作
        EventCenter.GetInstance().AddEventListener("FocusTime_Set", () =>
        {
            //Debug.Log("FocusTime_Set");
            Target.target = null;
            NPC_Status = NPC_status.GoToWork;
        });
        //礼物事件，增加好感度，减少玩家金钱
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Gifted);
        //工作事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Work", Order_Work);
        //休息事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Rest", Order_Rest);
        //加餐事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Eat", Order_Eat);
    }

    /// <summary>
    /// 被送了礼物事件
    /// </summary>
    /// <param name="Info">礼物信息</param>
    public void Gifted(Button Info)
    {
        if(Info.name==data.Name)
        {
            GiftData gift_data = Info.GetComponent<Gift>().Data;
            //如果小动物喜欢这个礼物，则增加好感度增加
            if(data.FavorvateThing==gift_data.Name)
            {
                data.Favorability += gift_data.favorability_Plus_Num+5;
            }
            else//如果普通，则按数值增加
            {
                data.Favorability += gift_data.favorability_Plus_Num;
            }
            
        }
    }

    public void Order_Work(Button info)
    {
        string button_Name= info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == data.Name)
        {
            NPC_Status= NPC_status.GoToWork;
            data.Favorability--;
            EventCenter.GetInstance().EventTrigger("Info_Update");
            print("强制工作指令");
        }
    }
    public void Order_Rest(Button info)
    {
        string button_Name = info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == data.Name)
        {
            NPC_Status = NPC_status.Rest;
            print("强制休息指令");
        }
    }
    public void Order_Eat(Button info)
    {
        string button_Name = info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == data.Name)
        {
            NPC_Status = NPC_status.GoToEat;
            print("强制恰饭指令");
        }
    }
    #endregion
    private void FixedUpdate()
    {
        //专注时间结束了就不管三七二十一下班
        if (!ObjectKeeper_Singleton.Instance.Is_Set)
        {
            NPC_Status = NPC_status.Rest;
        }

        Switch_Status();
        aiPath.maxSpeed = data.MoveSpeed;
    }
    public void Switch_Status()
    {
        switch (NPC_Status)
        {
            case NPC_status.GoToWork://当小动物处于去工作状态，则寻找距离最近的种田机器
                Target.target = null;
                HungryTime_calculation();//计算饥饿时间
                Freight_Wait();//查找是否存在货物需要搬运
                if (Target.target==null)//如果没有找到工作地点，则搜寻最近的
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Farm_Machine);
                }
                //若找到了工作地点并前往，但工作地点已经被其他小动物提前占用，则重新寻找工作地点
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                break;

            case NPC_status.Work:

                HungryTime_calculation();//计算饥饿时间

                //若查找到的田地不为空，
                if(Work_Field != null)
                {
                    //工作的田地上的植物处于收获状态，则进入运输状态
                    if (Work_Field.State == Plant_State.harvest)
                    {
                        NPC_Status = NPC_status.Transport;
                    }
                    //小动物进入工作状态，开始进入工作时间倒计时,在这里使用GameManager中的公用计时器方法
                    GameManager.GetInstance().FixedUpdate_Timer(ref data.Work_Time, 1f);
                    //工作时间归零进入偷懒结算
                    if (data.Work_Time <= 0)
                    {
                        if (Lazy_Check(data.Favorability) == 0)
                        {
                            print(name + ":偷懒了！");
                            NPC_Status = NPC_status.TouchFish;

                            data.Work_Time = Template_data.Work_Time;//重置时间
                        }
                    }
                }
                //若没有需要工作的田地，则进入偷懒状态
                else
                {
                    Work_Field = Choosen_Field();
                    //查找存在工作的田地
                    if (Work_Field == null)
                    {
                        NPC_Status = NPC_status.TouchFish;
                    }
                    
                    //NPC_Status =NPC_status.TouchFish;
                }
                break;

            case NPC_status.Rest://小动物回休息室
                Leave_Field();//离开田地，田地的负责人置空
                Target.target = ObjectKeeper_Singleton.Instance.Rest_Area.transform;

                break;

            case NPC_status.TouchFish://小动物摸鱼
                Leave_Field();//离开田地，田地的负责人置空
                HungryTime_calculation();//计算饥饿时间
                
                Target.target = ObjectKeeper_Singleton.Instance.TouchFish_Area.transform;
                break;
            
            case NPC_status.GoToEat://小动物去吃东西
                Target.target = null;

                Leave_Field();//离开田地，田地的负责人置空

                if (Target.target == null)
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Eat_Area);
                }
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                break;

            case NPC_status.Eat://小动物吃东西
                Leave_Field();
                //开始吃饭倒计时
                GameManager.GetInstance().FixedUpdate_Timer(ref data.Eat_Time,1f);
                //就餐完毕
                if(data.Eat_Time<=0)
                {
                    //加食物Buff
                    data.Work_Speed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    data.MoveSpeed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    //进行数值计算(小动物每次进食扣除餐费)
                    ObjectKeeper_Singleton.Instance.gamerData.Money += ObjectKeeper_Singleton.Instance.foodData.Money_Cost_reward;
                    //更新数值显示
                    EventCenter.GetInstance().EventTrigger("Info_Update");

                    NPC_Status = NPC_status.GoToWork;
                    //重置所有时间
                    Time_Data_Reset();
                }
                break;

            case NPC_status.Transport://搬运
                if(freight.Name!=null)
                {
                    Target.target = ObjectKeeper_Singleton.Instance.WareHouse.transform;
                }
                else
                {
                    Freight_Wait();
                }
                break;
        }
    }
    /// <summary>
    /// 如果有等待搬运的货物
    /// </summary>
    public void Freight_Wait()
    {
        GameObject[] Freights = ObjectKeeper_Singleton.Instance.Freight_Target;

        foreach (var gameObject in Freights)
        {
            Map_Target map_Target = gameObject.GetComponent<Map_Target>();
            if(map_Target.Freight.Name!=null&&map_Target.Is_Empty&&map_Target.GetComponent<SpriteRenderer>().enabled)
            {
                NPC_Status=NPC_status.Transport;//搬运
                Target.target = gameObject.transform;
            }
            else
            {
                NPC_Status = NPC_status.GoToWork;
            }
        }
    }
    /// <summary>
    /// 计算饥饿时间，饿了就不管三七二十一去吃饭
    /// </summary>
    public void HungryTime_calculation()
    {
        GameManager.GetInstance().FixedUpdate_Timer(ref data.Hungry_Time, 1f);
        if (data.Hungry_Time <= 0)
        {
            NPC_Status = NPC_status.GoToEat;

            data.Hungry_Time = Template_data.Hungry_Time;//重置饥饿时间
            //重置数据
            data.Work_Speed = Template_data.Work_Speed;
            data.MoveSpeed=Template_data.MoveSpeed;
        }
    }
    /// <summary>
    /// 重置时间
    /// </summary>
    public void Time_Data_Reset()
    {
        data.Work_Time = Template_data.Work_Time;
        data.Eat_Time= Template_data.Eat_Time;
        data.Hungry_Time = Template_data.Hungry_Time;
    }
    /// <summary>
    /// 随着好感度更新，更新偷懒概率
    /// </summary>
    /// <param name="favorability">好感度</param>
    /// <returns>偷懒数</returns>
    public int Lazy_Check(int favorability)
    {
        int LazyNum = Random.Range(0, favorability);
        return LazyNum;
    }

    /// <summary>
    /// 选择工作的田地
    /// </summary>
    /// <returns>工作的田地</returns>
    public plant_State Choosen_Field()
    {
        for(int i=0; i < ObjectKeeper_Singleton.Instance.Farm_Field.Length; i++)
        {
            GameObject Field = ObjectKeeper_Singleton.Instance.Farm_Field[i];
            plant_State plant_State = Field.GetComponent<plant_State>();
            //当该土地确实存在植物，且没有小动物在土地上工作,且植物没有处于自由生长状态时
            if(plant_State.State!= Plant_State.Empty&&plant_State.State!=Plant_State.Germinate&& plant_State.State!=Plant_State.Grown&& 
                plant_State.State!=Plant_State.Mature && plant_State.npc_Data.Name=="")
            {
                plant_State.npc_Data = data;//为田地设置工作者
                return plant_State;
            }
        }
        return null;
    }
    /// <summary>
    /// 离开田地
    /// </summary>
    public void Leave_Field()
    {
        if(Work_Field!=null)//如果有工作田地
        {
            //print(Target.target);
            Target.target= null;//将目标置空
            Work_Field.npc_Data = new NPCData(null);//使田地回到无主之地状态
            Work_Field = null;//置空工作田地
        }
    }
    /// <summary>
    /// 寻找最短距离
    /// </summary>
    /// <param name="gameObjects">寻路目标组</param>
    public void ShortestPath(GameObject[] gameObjects)
    {
        Distance = Mathf.Infinity;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            bool is_empty=gameObjects[i].GetComponent<Map_Target>().Is_Empty;
            if (Distance > Vector3.Distance(transform.position, gameObjects[i].transform.position)&&is_empty)
            {
                Distance = Vector3.Distance(transform.position, gameObjects[i].transform.position);
                int TargetIndex = i;
                Target.target = gameObjects[TargetIndex].transform;
            }
        }
    }
}
