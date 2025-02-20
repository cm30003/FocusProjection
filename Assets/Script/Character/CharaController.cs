using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class CharaController : MonoBehaviour
{
    public float Distance;//������Ŀ��֮��ľ���

    public AIDestinationSetter Target;//Ŀ��ص�

    public NPC_status NPC_Status;//С����״̬

    private AIPath aiPath;

    [Tooltip("���������")]
    public plant_State Work_Field;
    [Tooltip("���˵Ļ���")]
    public PlantData freight;//����
    [Header("������������/���ݡ�������")]
    public NPCData Template_data;
    public NPCData data;

    private void Start()
    {
        //data=JsonManager.Instance.LoadData<NPCData>(this.name);

        Template_data = new NPCData(data) ;//���ɸ���

        Event_SignIn();

        Target = GetComponent<AIDestinationSetter>();
        Target.target = null;

        aiPath = GetComponent<AIPath>();
    }
    #region �¼�
    /// <summary>
    /// �¼�ע��
    /// </summary>
    private void Event_SignIn()
    {
        /*���������¼���������*/
        //רעʱ�������¼�����רעʱ�䱻���ã�С�����ȥ����
        EventCenter.GetInstance().AddEventListener("FocusTime_Set", () =>
        {
            //Debug.Log("FocusTime_Set");
            Target.target = null;
            NPC_Status = NPC_status.GoToWork;
        });
        //�����¼������Ӻøжȣ�������ҽ�Ǯ
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Gifted);
        //�����¼�
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Work", Order_Work);
        //��Ϣ�¼�
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Rest", Order_Rest);
        //�Ӳ��¼�
        EventCenter.GetInstance().AddEventListener<Button>("NPC_Eat", Order_Eat);
    }

    /// <summary>
    /// �����������¼�
    /// </summary>
    /// <param name="Info">������Ϣ</param>
    public void Gifted(Button Info)
    {
        if(Info.name==data.Name)
        {
            GiftData gift_data = Info.GetComponent<Gift>().Data;
            //���С����ϲ�������������Ӻøж�����
            if(data.FavorvateThing==gift_data.Name)
            {
                data.Favorability += gift_data.favorability_Plus_Num+5;
            }
            else//�����ͨ������ֵ����
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
            print("ǿ�ƹ���ָ��");
        }
    }
    public void Order_Rest(Button info)
    {
        string button_Name = info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == data.Name)
        {
            NPC_Status = NPC_status.Rest;
            print("ǿ����Ϣָ��");
        }
    }
    public void Order_Eat(Button info)
    {
        string button_Name = info.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.name;
        if (button_Name == data.Name)
        {
            NPC_Status = NPC_status.GoToEat;
            print("ǿ��ǡ��ָ��");
        }
    }
    #endregion
    private void FixedUpdate()
    {
        //רעʱ������˾Ͳ������߶�ʮһ�°�
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
            case NPC_status.GoToWork://��С���ﴦ��ȥ����״̬����Ѱ�Ҿ���������������
                Target.target = null;
                HungryTime_calculation();//���㼢��ʱ��
                Freight_Wait();//�����Ƿ���ڻ�����Ҫ����
                if (Target.target==null)//���û���ҵ������ص㣬����Ѱ�����
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Farm_Machine);
                }
                //���ҵ��˹����ص㲢ǰ�����������ص��Ѿ�������С������ǰռ�ã�������Ѱ�ҹ����ص�
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                break;

            case NPC_status.Work:

                HungryTime_calculation();//���㼢��ʱ��

                //�����ҵ�����ز�Ϊ�գ�
                if(Work_Field != null)
                {
                    //����������ϵ�ֲ�ﴦ���ջ�״̬�����������״̬
                    if (Work_Field.State == Plant_State.harvest)
                    {
                        NPC_Status = NPC_status.Transport;
                    }
                    //С������빤��״̬����ʼ���빤��ʱ�䵹��ʱ,������ʹ��GameManager�еĹ��ü�ʱ������
                    GameManager.GetInstance().FixedUpdate_Timer(ref data.Work_Time, 1f);
                    //����ʱ��������͵������
                    if (data.Work_Time <= 0)
                    {
                        if (Lazy_Check(data.Favorability) == 0)
                        {
                            print(name + ":͵���ˣ�");
                            NPC_Status = NPC_status.TouchFish;

                            data.Work_Time = Template_data.Work_Time;//����ʱ��
                        }
                    }
                }
                //��û����Ҫ��������أ������͵��״̬
                else
                {
                    Work_Field = Choosen_Field();
                    //���Ҵ��ڹ��������
                    if (Work_Field == null)
                    {
                        NPC_Status = NPC_status.TouchFish;
                    }
                    
                    //NPC_Status =NPC_status.TouchFish;
                }
                break;

            case NPC_status.Rest://С�������Ϣ��
                Leave_Field();//�뿪��أ���صĸ������ÿ�
                Target.target = ObjectKeeper_Singleton.Instance.Rest_Area.transform;

                break;

            case NPC_status.TouchFish://С��������
                Leave_Field();//�뿪��أ���صĸ������ÿ�
                HungryTime_calculation();//���㼢��ʱ��
                
                Target.target = ObjectKeeper_Singleton.Instance.TouchFish_Area.transform;
                break;
            
            case NPC_status.GoToEat://С����ȥ�Զ���
                Target.target = null;

                Leave_Field();//�뿪��أ���صĸ������ÿ�

                if (Target.target == null)
                {
                    ShortestPath(ObjectKeeper_Singleton.Instance.Eat_Area);
                }
                else if (!Target.target.GetComponent<Map_Target>().Is_Empty)
                {
                    Target.target = null;
                }
                break;

            case NPC_status.Eat://С����Զ���
                Leave_Field();
                //��ʼ�Է�����ʱ
                GameManager.GetInstance().FixedUpdate_Timer(ref data.Eat_Time,1f);
                //�Ͳ����
                if(data.Eat_Time<=0)
                {
                    //��ʳ��Buff
                    data.Work_Speed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    data.MoveSpeed += ObjectKeeper_Singleton.Instance.foodData.Buff;
                    //������ֵ����(С����ÿ�ν�ʳ�۳��ͷ�)
                    ObjectKeeper_Singleton.Instance.gamerData.Money += ObjectKeeper_Singleton.Instance.foodData.Money_Cost_reward;
                    //������ֵ��ʾ
                    EventCenter.GetInstance().EventTrigger("Info_Update");

                    NPC_Status = NPC_status.GoToWork;
                    //��������ʱ��
                    Time_Data_Reset();
                }
                break;

            case NPC_status.Transport://����
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
    /// ����еȴ����˵Ļ���
    /// </summary>
    public void Freight_Wait()
    {
        GameObject[] Freights = ObjectKeeper_Singleton.Instance.Freight_Target;

        foreach (var gameObject in Freights)
        {
            Map_Target map_Target = gameObject.GetComponent<Map_Target>();
            if(map_Target.Freight.Name!=null&&map_Target.Is_Empty&&map_Target.GetComponent<SpriteRenderer>().enabled)
            {
                NPC_Status=NPC_status.Transport;//����
                Target.target = gameObject.transform;
            }
            else
            {
                NPC_Status = NPC_status.GoToWork;
            }
        }
    }
    /// <summary>
    /// ���㼢��ʱ�䣬���˾Ͳ������߶�ʮһȥ�Է�
    /// </summary>
    public void HungryTime_calculation()
    {
        GameManager.GetInstance().FixedUpdate_Timer(ref data.Hungry_Time, 1f);
        if (data.Hungry_Time <= 0)
        {
            NPC_Status = NPC_status.GoToEat;

            data.Hungry_Time = Template_data.Hungry_Time;//���ü���ʱ��
            //��������
            data.Work_Speed = Template_data.Work_Speed;
            data.MoveSpeed=Template_data.MoveSpeed;
        }
    }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public void Time_Data_Reset()
    {
        data.Work_Time = Template_data.Work_Time;
        data.Eat_Time= Template_data.Eat_Time;
        data.Hungry_Time = Template_data.Hungry_Time;
    }
    /// <summary>
    /// ���źøжȸ��£�����͵������
    /// </summary>
    /// <param name="favorability">�øж�</param>
    /// <returns>͵����</returns>
    public int Lazy_Check(int favorability)
    {
        int LazyNum = Random.Range(0, favorability);
        return LazyNum;
    }

    /// <summary>
    /// ѡ���������
    /// </summary>
    /// <returns>���������</returns>
    public plant_State Choosen_Field()
    {
        for(int i=0; i < ObjectKeeper_Singleton.Instance.Farm_Field.Length; i++)
        {
            GameObject Field = ObjectKeeper_Singleton.Instance.Farm_Field[i];
            plant_State plant_State = Field.GetComponent<plant_State>();
            //��������ȷʵ����ֲ���û��С�����������Ϲ���,��ֲ��û�д�����������״̬ʱ
            if(plant_State.State!= Plant_State.Empty&&plant_State.State!=Plant_State.Germinate&& plant_State.State!=Plant_State.Grown&& 
                plant_State.State!=Plant_State.Mature && plant_State.npc_Data.Name=="")
            {
                plant_State.npc_Data = data;//Ϊ������ù�����
                return plant_State;
            }
        }
        return null;
    }
    /// <summary>
    /// �뿪���
    /// </summary>
    public void Leave_Field()
    {
        if(Work_Field!=null)//����й������
        {
            //print(Target.target);
            Target.target= null;//��Ŀ���ÿ�
            Work_Field.npc_Data = new NPCData(null);//ʹ��ػص�����֮��״̬
            Work_Field = null;//�ÿչ������
        }
    }
    /// <summary>
    /// Ѱ����̾���
    /// </summary>
    /// <param name="gameObjects">Ѱ·Ŀ����</param>
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
