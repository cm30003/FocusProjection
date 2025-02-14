using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ObjectKeeper_Singleton:SingletonMono<ObjectKeeper_Singleton>
{
    [Header("���������洢���ݡ�������")]
    public GamerData gamerData;
    [Header("��������������������")]
    public bool Is_Set;//�Ƿ��趨רעʱ��
    [Header("���������ڳ��Ķ��󡪡�����")]
    public FoodData foodData;//�趨��ʳ��ʳ��

    public GameObject WareHouse;//�ⷿ

    public GameObject Farm_Group;

    public GameObject[] Freight_Target;

    public GameObject[] Farm_Field;//�����

    public GameObject[] NPCs;//NPC

    public GameObject[] Farm_Machine;//ƽ�崥����

    public GameObject[] Eat_Area;//�Է�������

    public GameObject Rest_Area;//��Ϣ������

    public GameObject TouchFish_Area;//���������


    private void Awake()
    {
        base.Awake();
        //�洢�ڳ�����

        WareHouse=GameObject.FindGameObjectWithTag("WareHouse_Area");
        Freight_Target=GameObject.FindGameObjectsWithTag("Freight_Target");
        Farm_Group = GameObject.FindGameObjectWithTag("Farm_Field");
        NPCs=GameObject.FindGameObjectsWithTag("NPC");
        Farm_Machine = GameObject.FindGameObjectsWithTag("Farm_Machine");
        Eat_Area=GameObject.FindGameObjectsWithTag("Eat_Area");
        Rest_Area = GameObject.FindGameObjectWithTag("Rest_Area");
        TouchFish_Area = GameObject.FindGameObjectWithTag("TouchFish_Area");
        //ע���¼�
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Gifted);
    }
    /// <summary>
    /// �����ۿ��ˣ����½�Ǯ
    /// </summary>
    /// <param name="Info">��Ʒ��Ϣ</param>
    private void Gifted(Button Info)
    {
        gamerData.Money += Info.GetComponent<Gift>().Data.Money_Cost_reward;
        EventCenter.GetInstance().EventTrigger("Info_Update");
    }

    private void Start()
    {
        PlayerFirst_SignIn();

        Farm_Field=Field_SignIn();

        Current_Food(null);
    }
    /// <summary>
    /// ��ǰ��ʳ��ѡ���ʳ��
    /// </summary>
    /// <param name="foodData">ʳ������</param>
    /// <returns>ʳ������</returns>
    public FoodData Current_Food(FoodData foodData)
    {
        if(foodData==null)
        {
            foodData = new FoodData();
        }

        return foodData;
    }
    /// <summary>
    /// ��ȡ�����
    /// </summary>
    /// <returns>��������</returns>
    public GameObject[] Field_SignIn()
    {
        // ����һ���������洢������
        GameObject[] children = new GameObject[Farm_Group.transform.childCount];

        // �������������岢��������ӵ�������
        for (int i = 0; i < Farm_Group.transform.childCount; i++)
        {
            children[i] = Farm_Group.transform.GetChild(i).gameObject;
        }

        return children;
    }
    /// <summary>
    /// ����״ε���
    /// </summary>
    public void PlayerFirst_SignIn()
    {
        gamerData = JsonManager.Instance.LoadData<GamerData>("GamerData");
        //��һ�ν�����Ϸʱ�����õ�һ�ν�����Ϸʱ��
        if (gamerData.First_SignIn_Date == null)
        {
            gamerData.First_SignIn_Date = DateTime.Now.ToString("yyyy��MM��dd��");
            JsonManager.Instance.SaveData(gamerData, "GamerData");
        }
        if(gamerData.Items==null)
        {
            gamerData.Items=new List<PlantData>();
            JsonManager.Instance.SaveData(gamerData, "GamerData");
        }
        for(int i=0;i<NPCs.Length;i++)
        {
            NPCs[i].GetComponent<CharaController>().data = JsonManager.Instance.LoadData<NPCData>(NPCs[i].name);
        }
        EventCenter.GetInstance().AddEventListener("SaveGamerData", Save_GamerData);//�洢�¼�
    }
    /// <summary>
    /// �洢����
    /// </summary>
    public void Save_GamerData()
    {
        //�洢NPC����
        for (int i = 0; i < NPCs.Length; i++)
        {
            NPCData npcData = NPCs[i].GetComponent<CharaController>().data;
            JsonManager.Instance.SaveData(npcData, npcData.Name);
        }
        //�洢�������
        JsonManager.Instance.SaveData(gamerData, "GamerData");
    }
    //private void Update()
    //{
    //    print(gamerData.Items);
    //}
}
[System.Serializable]
public class Mission_ADay//�����࣬ͳ�����ÿ����ɵ�������רעʱ��
{
    public int Focus_Time;//רעʱ��
    public string Day;//����
    public List<string> Options=new List<string>();//�����Ŀ
    public Dictionary<string,int> ADay_FocusTime_Dic=new Dictionary<string, int>();//�����ڵ�רעʱ��
    public Dictionary<string, List<string>> ADay_Options_Dic=new Dictionary<string, List<string>>();//�������µ������Ŀ
}
[System.Serializable]
public class NPCData//�����࣬���ڴ洢NPC����
{
    //public Sprite Sprite;

    public string Name;//����
    public string Hobby;//����
    public string Personality;//�Ը�
    public string Description;//�����ı�
    public string BirthDay;//����
    [Header("��������ϲ�á�������")]
    public int Favorability;//�øж�
    public string FavorvateThing;//ϲ����
    [Header("��������Ч�ʡ�������")]
    public float MoveSpeed;//�ƶ��ٶ�
    public float Work_Speed;//�����ٶ�
    [Header("��������ʱ�䡪������")]
    public float Work_Time;//����ʱ��

    public float Eat_Time;//�Է�ʱ��

    public float Hungry_Time;//����ʱ��

    // �������캯��
    public NPCData(NPCData other)
    {
        if (other == null)
        {
            // �������Ĳ���Ϊ null�����ʼ��ΪĬ��ֵ
            //Sprite = null;
            Name = string.Empty;
            Hobby = string.Empty;
            Personality = string.Empty;
            Description = string.Empty;
            BirthDay = string.Empty;
            Favorability = 0;
            FavorvateThing = string.Empty;
            MoveSpeed = 0f;
            Work_Speed = 0f;
            Work_Time = 0f;
            Eat_Time = 0f;
            Hungry_Time = 0f;
        }
        else
        {
            //Sprite = other.Sprite;
            Name = other.Name;
            Hobby = other.Hobby;
            Personality = other.Personality;
            Description = other.Description;
            BirthDay = other.BirthDay;
            Favorability = other.Favorability;
            FavorvateThing = other.FavorvateThing;
            MoveSpeed = other.MoveSpeed;
            Work_Speed = other.Work_Speed;
            Work_Time = other.Work_Time;
            Eat_Time = other.Eat_Time;
            Hungry_Time = other.Hungry_Time;
        }
    }
    // �����޲ι��캯��
    public NPCData()
    {
        Name = string.Empty;
        Hobby = string.Empty;
        Personality = string.Empty;
        Description = string.Empty;
        BirthDay = string.Empty;
        Favorability = 0;
        FavorvateThing = string.Empty;
        MoveSpeed = 0f;
        Work_Speed = 0f;
        Work_Time = 0f;
        Eat_Time = 0f;
        Hungry_Time = 0f;
    }
}
[System.Serializable]
public class GamerData//�������
{
    //ͳ�Ʋ���
    public float PlantTime;//�ֵ�ʱ��
    public int HarvestNum;//ũ������ջ�����
    public string First_SignIn_Date;//��һ�ν�����Ϸ��ʱ��
    public int Money;//��Ǯͳ��
    public int Level;//��ҵȼ�
    public float Current_XP;//��Ҿ���
    public float Max_XP;//��������
    public List<PlantData> Items;
    //�Զ��岿��
    public Sprite Player_HeadImage;//���ͷ��
    public string PlayerBirthDay;//�������
    public string PlayerName;//�����
    public string PlayerTitle;//��ҳƺ�
    public string PlayerMotto;//���������
}
[System.Serializable]
public class ItemData
{
    public string Name;//����
    public Sprite Sprite;//ͼ��
    public string Description;//����

    public int Money_Cost_reward;//����/�����۸�

}
[System.Serializable]
public class PlantData: ItemData
{
    [Header("��������ͼ�񡪡�����")]
    [Tooltip("��ѿͼ��")]
    public Sprite Germinate_Image;//��ѿͼ��
    [Tooltip("�ɳ�ͼ��")]
    public Sprite Grown_Image;
    [Tooltip("����ͼ��")]
    public Sprite Mature_Image;

    [Header("��������ʱ�䡪������")]
    [Tooltip("��ѿʱ��")]
    public float Germinate_Time;
    [Tooltip("����ʱ��")]
    public float Grown_Time;
    [Tooltip("����ʱ��")]
    public float Mature_Time;
    [Tooltip("����ʱ��")]
    public float Plant_Time;//����ʱ��
    [Tooltip("��ˮʱ��")]
    public float Water_Time;//��ˮʱ��
    [Tooltip("ʩ��ʱ��")]
    public float fertilize_Time;//ʩ��ʱ��
    [Tooltip("����ʱ��")]
    public float BugControl_Time;//����ʱ��
    //[Tooltip("����ʱ��")]
    //public float plow_Time;//����ʱ��
    [Tooltip("�ջ�ʱ��")]
    public float Harvest_Time;//�ջ�ʱ��

    [Tooltip("�ջ�����")]
    public int Num;//   �ջ�����
    // �������캯��
    public PlantData(PlantData other)
    {
        if (other == null)
        {
            Germinate_Image = null;
            Grown_Image = null;
            Mature_Image = null;

            Germinate_Time = 0;
            Grown_Time = 0;
            Mature_Time = 0;
            Plant_Time = 0;
            Water_Time = 0;
            fertilize_Time = 0;
            BugControl_Time = 0;
            Harvest_Time = 0;
        }
        else
        {
            Germinate_Image = other.Germinate_Image;
            Grown_Image = other.Grown_Image;
            Mature_Image = other.Mature_Image;

            Germinate_Time = other.Germinate_Time;
            Grown_Time = other.Grown_Time;
            Mature_Time = other.Mature_Time;
            Plant_Time = other.Plant_Time;
            Water_Time = other.Water_Time;
            fertilize_Time = other.fertilize_Time;
            BugControl_Time = other.BugControl_Time;
            Harvest_Time = other.Harvest_Time;

            Num = other.Num;
        }
        
    }
}
[System.Serializable]
public class GiftData: ItemData
{
    [Tooltip("�øжȼӳ�")]
    public int favorability_Plus_Num;//�øжȼӳ�
}
[System.Serializable]
public class FoodData: ItemData
{
    public float Buff;//����
}





