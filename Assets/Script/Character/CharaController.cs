using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UI;
using LitJson;
using Spine;

public class CharaController : MonoBehaviour
{
    [Header("————需要交互的动态对象————")]
    public float RestWalk_InRest_RangeY;//漫游范围Y
    public float RestWalk_InRest_RangeX;//漫游范围X

    public float RestIde_Time;//完成一个漫游后的待机时间
    public float CurrentRestIde_Time;//完成一个漫游后的待机时间

    public float Distance;//人物与目标之间的距离

    public AIDestinationSetter Target;//目标地点

    public NPC_status NPC_Status;//小动物状态
    [Tooltip("工作的田地")]
    public plant_State Work_Field;
    [Tooltip("搬运的货物")]
    public PlantItem_Data freight;//货物
    [Header("————动物/数据————")]
    public string NPC_DATA_PATH = "JsonDataAsset/NPC_Data";
    public string NPC_INFO_PATH = "JsonDataAsset/NPCInformation";

    public NPCInformation npc_Information;
    
    public NPCData Template_data;
    public NPCData data;
    [Header("————组件————")]
    private Animator animator;//动画状态机
    private AIPath aiPath;//A Star 寻路
    [Header("————计时————")]
    private float Standard_Second = 1;

    private void Start()
    {
        NPCData_Information_SignIn();//注册NPC描述信息

        animator = GetComponentInChildren<Animator>();//获取动画状态机

        Event_SignIn();//事件注册

        Target = GetComponent<AIDestinationSetter>();

        Target.target = null;

        aiPath = GetComponent<AIPath>();
    }
   /// <summary>
   /// 注册NPC数据与描述信息
   /// </summary>
    public void NPCData_Information_SignIn()
    {
        //获取数据列表中的NPCJson数据
        string NPCData = ResourceManager.GetInstance().Load<TextAsset>(NPC_DATA_PATH).text;
        string NPCInfo = ResourceManager.GetInstance().Load<TextAsset>(NPC_INFO_PATH).text;
        //反序列化解析为对应的数据结构
        NPCData[] NPC_Data = JsonMapper.ToObject<NPCData[]>(NPCData);
        NPCInformation[] NPC_Info = JsonMapper.ToObject<NPCInformation[]>(NPCInfo);
        //遍历数据结构，寻找对应名字的小动物,获取小动物信息
        for (int i = 0; i < NPC_Info.Length; i++)
        {
            string name = NPC_Info[i].Name;
            if (name == this.name)
            {
                npc_Information = NPC_Info[i];
                break;
            }
        }
        //小动物信息获取后，通过其信息获取其数据
        for (int i = 0; i < NPC_Data.Length; i++)
        {
            int ID = NPC_Data[i].ID;
            if (ID == npc_Information.ID)
            {
                Template_data = NPC_Data[i];
                data=new NPCData(Template_data);
                break;
            }
        }
    }
    #region 事件
    /// <summary>
    /// 事件注册
    /// </summary>
    private void Event_SignIn()
    {
        /*————事件————*/
        //专注时间设置事件，当专注时间被设置，小动物会去工作
        //EventCenter.GetInstance().AddEventListener("FocusTime_Set", IsChoosen);
        //玩家选中事件
        EventCenter.GetInstance().AddEventListener<string>("Is_Choosen",IsChoosen);//玩家选中，Trigger位于Phone_NPCManager中
        //待命事件
        EventCenter.GetInstance().AddEventListener<string>("Is_Waiting", IsWaiting);//待命，Trigger位于Phone_NPCManager中
        //礼物事件，增加好感度，减少玩家金钱
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Gifted);//送礼，Trigger位于Phone_WareHouse中
        //工作事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Work", Order_Work);
        //休息事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Rest", Order_Rest);
        //加餐事件
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Eat", Order_Eat);
    }
    public void IsChoosen(string name)
    {
        if (ObjectKeeper_Singleton.Instance.Is_Set&&name==npc_Information.Name)
        {
            NPC_Status = NPC_status.GoToWork;
        }
    }
    public void IsWaiting(string name)
    {
        if (name== npc_Information.Name)
        {
            NPC_Status = NPC_status.Go_To_Rest;
        }
    }
    /// <summary>
    /// 被送了礼物事件
    /// </summary>
    /// <param name="Info">礼物信息</param>
    public void Gifted(Button Info)
    {
        if(Info.name== npc_Information.Name)
        {
            GiftData gift_data = Info.GetComponent<Gift>().Data;
            //比对小动物ID。如果小动物喜欢这个礼物，则增加好感度增加
            if(data.CherishThing_ID == gift_data.ID)
            {
                data.Favorability += gift_data.Favorate_Affinity;
            }
            else if(data.FavorvateThing_ID==gift_data.ID)
            {
                data.Favorability += gift_data.Like_Affinity;
            }
            else//如果普通，则按默认数值增加
            {
                data.Favorability += gift_data.Default_Affinity;
            }
        }
    }
    #region 小动物指令
    public void Order_Work(Button info)
    {
        string button_Name= info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == npc_Information.Name)
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
        if (button_Name == npc_Information.Name)
        {
            NPC_Status = NPC_status.Go_To_Rest;
            print("强制休息指令");
        }
    }
    public void Order_Eat(Button info)
    {
        string button_Name = info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == npc_Information.Name)
        {
            NPC_Status = NPC_status.GoToEat;
            print("强制恰饭指令");
        }
    }
    #endregion
    #endregion
    private void FixedUpdate()
    {
        //专注时间结束了就不管三七二十一下班,除非已经在休息室休息了
        if (!ObjectKeeper_Singleton.Instance.Is_Set && NPC_Status != NPC_status.Rest&&!Check_PlantedField())
        {
            NPC_Status = NPC_status.Go_To_Rest;
        }
        Switch_Status();
        aiPath.maxSpeed = data.MoveSpeed;
    }
    /// <summary>
    /// 状态切换
    /// </summary>
    public void Switch_Status()
    {
        switch (NPC_Status)
        {
            case NPC_status.GoToWork://当小动物处于去工作状态，则寻找距离最近的种田机器

                ObjectKeeper_Singleton.Instance.Add_WorkNpcs(gameObject);//添加到工作NPC列表

                Target.target = null;//首先目标置空

                HungryTime_calculation();

                Freight_Wait();//查找是否存在货物需要搬运，若有则优先搬运货物

                if (Target.target==null)//如果没有找到工作地点，则搜寻最近的
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Farm_Machine);
                }
                //若找到了工作地点并前往，但工作地点已经被其他小动物提前占用，则重新寻找工作地点
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                Move_Right_OR_Left();//判断左右并进行翻转
                Move_Up_OR_Down();

                break;

            case NPC_status.Work://工作

                this.transform.rotation = Quaternion.Euler(0, 180, 0);

                animator.Play("b_work1");//播放 工作动画

                HungryTime_calculation();

                //如果NPC找到了自己的工作田地
                if (Work_Field != null)
                {
                    //如果田地状态进入自由生长的状态，则重新计算NPC状态
                    if(Work_Field.State == Plant_State.Germinate||Work_Field.State == Plant_State.Grown1||Work_Field.State == Plant_State.Grown2||Work_Field.State == Plant_State.Mature||Work_Field.State == Plant_State.Empty)
                    {
                        Leave_Field();
                        NPC_Status = NPC_status.GoToWork;
                    }
                    GameManager.GetInstance().FixedUpdate_Timer(ref data.Work_Time, 1f,ref Standard_Second);
                    //工作时间归零进入偷懒结算
                    if (data.Work_Time <= 0)
                    {
                        if (Lazy_Check(data.Favorability) == 0)
                        {
                            print(name + ":偷懒了！");
                            NPC_Status = NPC_status.GoTo_TouchFish;

                            data.Work_Time = Template_data.Work_Time;//重置时间
                        }
                    }
                }
                //若没有需要工作的田地，则进入偷懒状态
                else
                {
                    //查找存在工作的田地
                    Work_Field = Choosen_Field();
                }
                break;

            case NPC_status.Go_To_Rest://小动物回休息室

                ObjectKeeper_Singleton.Instance.Remove_WorkNpc(gameObject);
                //判断左右
                Move_Right_OR_Left();
                //判断上下
                Move_Up_OR_Down();

                Leave_Field();//离开田地，田地的负责人置空
                Target.target = ObjectKeeper_Singleton.Instance.Rest_Area.transform;//目标为休息室
                //当小动物抵达休息室时，进入休息状态
                if(aiPath.reachedEndOfPath)
                {
                    NPC_Status = NPC_status.Rest;
                }
                break;

            case NPC_status.Rest://小动物休息

                ObjectKeeper_Singleton.Instance.Remove_WorkNpc(gameObject);

                if (aiPath.reachedEndOfPath)
                {
                    //在散步目标上的等待时间
                    CurrentRestIde_Time -= Time.deltaTime;

                    animator.Play("z_idle");

                    if (CurrentRestIde_Time <= 0)
                    {
                        Target.target  = null;

                        float randomX = Random.Range(-RestWalk_InRest_RangeX, RestWalk_InRest_RangeX);//随机X
                        float randomY = Random.Range(-RestWalk_InRest_RangeY, RestWalk_InRest_RangeY);//随机Y

                        float RestArea_X = ObjectKeeper_Singleton.Instance.Rest_Area.transform.position.x;//休息区域x
                        float RestArea_Y = ObjectKeeper_Singleton.Instance.Rest_Area.transform.position.y;//休息区域Y

                        Vector2 stroll = new Vector2(RestArea_X + randomX, RestArea_Y + randomY);

                        aiPath.destination = stroll;

                        CurrentRestIde_Time = RestIde_Time;
                    }
                }
                else
                {
                    Move_Right_OR_Left();
                    Move_Up_OR_Down();
                }
                break;


            case NPC_status.GoTo_TouchFish://小动物去摸鱼

                Move_Right_OR_Left();
                Move_Up_OR_Down();

                if (Target.target == null)
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.TouchFish_Area);
                }
                //若找到了工作地点并前往，但工作地点已经被其他小动物提前占用，则重新寻找工作地点
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                
                Leave_Field();//离开田地，田地的负责人置空
                HungryTime_calculation();
                break;

            case NPC_status.TouchFish://摸鱼

                this.transform.rotation = Quaternion.Euler(0, 180, 0);

                animator.Play("b_sit");//坐

                HungryTime_calculation();

                break;
        
            case NPC_status.GoToEat://小动物去吃东西
                //判断左右
                Move_Right_OR_Left();
                //判断上下
                Move_Up_OR_Down();

                Target.target = null;

                Leave_Field();//离开田地，田地的负责人置空
                 
                if (Target.target == null)
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Eat_Area);
                }
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)//如果目标点被占用，则重新寻找目标点
                {
                    Target.target = null;
                }
                break;

            case NPC_status.Eat://小动物吃东西

                this.transform.rotation = Quaternion.Euler(0, 0, 0);

                animator.Play("b_sit");//播放 背对 坐姿动画
                //重置田地状态
                Leave_Field();
                //开始吃饭倒计时
                GameManager.GetInstance().FixedUpdate_Timer(ref data.Eat_Time,1f, ref Standard_Second);
                //就餐完毕
                if(data.Eat_Time<=0)
                {
                    //加食物Buff
                    //data.Work_Speed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    //data.MoveSpeed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    //进行数值计算(小动物每次进食扣除餐费)
                    ObjectKeeper_Singleton.Instance.gamerData.Money += ObjectKeeper_Singleton.Instance.foodData.Cost;
                    //更新数值显示
                    EventCenter.GetInstance().EventTrigger("Info_Update");
                    //更新NPC状态
                    NPC_Status = NPC_status.GoToWork;
                    //重置所有时间
                    Time_Data_Reset();
                }
                break;

            case NPC_status.Transport://搬运

                Move_Right_OR_Left();//判断左右并进行翻转
                Move_Up_OR_Down();

                if (freight.ID!=0)
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
    /// 检查田地是否有种子
    /// </summary>
    /// <returns></returns>
    public bool Check_PlantedField()
    {
        for(int i = 0; i < ObjectKeeper_Singleton.Instance.Farm_Group.transform.childCount; i++)
        {
            plant_State state= ObjectKeeper_Singleton.Instance.Farm_Group.transform.GetChild(i).GetComponent<plant_State>();
            if(state.State!=Plant_State.Empty)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 判断小动物是往右还是往左走了
    /// </summary>
    public void Move_Right_OR_Left()
    {
        if (aiPath.desiredVelocity.x > 0)//右边
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (aiPath.desiredVelocity.x < 0)//左边
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    /// <summary>
    /// 判断小动物是往上还是往下走了
    /// </summary>
    public void Move_Up_OR_Down()
    {
        if(NPC_Status!=NPC_status.Transport)
        {
            if (aiPath.desiredVelocity.y > 0)//上边
            {
                animator.Play("b_move");//背面行走动画
            }
            else if (aiPath.desiredVelocity.y < 0)//下边
            {
                animator.Play("z_move");//行走动画
            }
        }
        else
        {
            if (aiPath.desiredVelocity.y > 0)//上边
            {
                animator.Play("b_hug");//背面搬运动画
            }
            else if (aiPath.desiredVelocity.y < 0)//下边
            {
                animator.Play("z_hug");//搬运动画
            }
        }
    }
    /// <summary>
    /// 如果有等待搬运的货物，则优先搬运货物
    /// </summary>
    public void Freight_Wait()
    {
        //获取所有货物
        GameObject[] Freights = ObjectKeeper_Singleton.Instance.Freight_Target;
        //遍历所有等待搬运的货物
        foreach (var gameObject in Freights)
        {
            Map_Target map_Target = gameObject.GetComponent<Map_Target>();
            //如果货物没有被占用，则搬走
            if(map_Target.Freight.Name!=null&&map_Target.Is_Empty&&map_Target.GetComponent<SpriteRenderer>().enabled)
            {
                NPC_Status=NPC_status.Transport;//搬运
                Target.target = gameObject.transform;
            }
            else//如果被搬走了，就重新计算
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
        Standard_Second -= Time.fixedDeltaTime;
        if (Standard_Second <= 0)
        {
            Standard_Second = 1;
            data.Hungry_Time -= 1;
            if (data.Hungry_Time <= 0)
            {
                NPC_Status = NPC_status.GoToEat;
                //重置数据
                Time_Data_Reset();
            }
        }
    }
    /// <summary>
    /// 重置时间/工作 吃饭 饥饿等时间
    /// </summary>
    public void Time_Data_Reset()
    {
        data.Work_Time = Template_data.Work_Time;//重置工作时间
        data.Eat_Time= Template_data.Eat_Time;//重置吃饭时间
        data.Hungry_Time = Template_data.Hungry_Time;//重置饥饿时间
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
            if (plant_State.State != Plant_State.Empty &&//不为空
                plant_State.State != Plant_State.Germinate &&//不在发芽阶段
                plant_State.State != Plant_State.Grown1 && plant_State.State != Plant_State.Grown2&&//不在成长阶段
                plant_State.State!=Plant_State.Mature &&//不在成熟阶段
                plant_State.npc_Data.ID==0)// 没有小动物在田地工作
            {

                plant_State.npc_Data = data;//为田地设置工作者

                return plant_State;
            }
        }
        NPC_Status = NPC_status.GoTo_TouchFish;
        return null;
    }
    /// <summary>
    /// 离开田地,用于置空工作田地
    /// </summary>
    public void Leave_Field()
    {
        if(Work_Field!=null)//如果有工作田地
        {
            Target.target= null;//将目标置空
            Work_Field.npc_Data = new NPCData(null);//使田地回到无主之地状态
            Work_Field = null;//置空工作田地
        }
    }
    /// <summary>
    /// 寻找最短距离的目标点
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
                Target.target = gameObjects[i].transform;
            }
        }
    }
}
