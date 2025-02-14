using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ObjectKeeper_Singleton:SingletonMono<ObjectKeeper_Singleton>
{
    [Header("————存储数据————")]
    public GamerData gamerData;
    [Header("————条件————")]
    public bool Is_Set;//是否设定专注时间
    [Header("————在场的对象————")]
    public FoodData foodData;//设定的食堂食物

    public GameObject WareHouse;//库房

    public GameObject Farm_Group;

    public GameObject[] Freight_Target;

    public GameObject[] Farm_Field;//田地组

    public GameObject[] NPCs;//NPC

    public GameObject[] Farm_Machine;//平板触摸屏

    public GameObject[] Eat_Area;//吃饭的区域

    public GameObject Rest_Area;//休息的区域

    public GameObject TouchFish_Area;//摸鱼的区域


    private void Awake()
    {
        base.Awake();
        //存储在场对象

        WareHouse=GameObject.FindGameObjectWithTag("WareHouse_Area");
        Freight_Target=GameObject.FindGameObjectsWithTag("Freight_Target");
        Farm_Group = GameObject.FindGameObjectWithTag("Farm_Field");
        NPCs=GameObject.FindGameObjectsWithTag("NPC");
        Farm_Machine = GameObject.FindGameObjectsWithTag("Farm_Machine");
        Eat_Area=GameObject.FindGameObjectsWithTag("Eat_Area");
        Rest_Area = GameObject.FindGameObjectWithTag("Rest_Area");
        TouchFish_Area = GameObject.FindGameObjectWithTag("TouchFish_Area");
        //注册事件
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Gifted);
    }
    /// <summary>
    /// 送礼后扣款了，更新金钱
    /// </summary>
    /// <param name="Info">礼品信息</param>
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
    /// 当前在食堂选择的食物
    /// </summary>
    /// <param name="foodData">食物数据</param>
    /// <returns>食物数据</returns>
    public FoodData Current_Food(FoodData foodData)
    {
        if(foodData==null)
        {
            foodData = new FoodData();
        }

        return foodData;
    }
    /// <summary>
    /// 获取田地组
    /// </summary>
    /// <returns>返回田组</returns>
    public GameObject[] Field_SignIn()
    {
        // 创建一个数组来存储子物体
        GameObject[] children = new GameObject[Farm_Group.transform.childCount];

        // 遍历所有子物体并将它们添加到数组中
        for (int i = 0; i < Farm_Group.transform.childCount; i++)
        {
            children[i] = Farm_Group.transform.GetChild(i).gameObject;
        }

        return children;
    }
    /// <summary>
    /// 玩家首次登入
    /// </summary>
    public void PlayerFirst_SignIn()
    {
        gamerData = JsonManager.Instance.LoadData<GamerData>("GamerData");
        //第一次进入游戏时，设置第一次进入游戏时间
        if (gamerData.First_SignIn_Date == null)
        {
            gamerData.First_SignIn_Date = DateTime.Now.ToString("yyyy年MM月dd号");
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
        EventCenter.GetInstance().AddEventListener("SaveGamerData", Save_GamerData);//存储事件
    }
    /// <summary>
    /// 存储数据
    /// </summary>
    public void Save_GamerData()
    {
        //存储NPC数据
        for (int i = 0; i < NPCs.Length; i++)
        {
            NPCData npcData = NPCs[i].GetComponent<CharaController>().data;
            JsonManager.Instance.SaveData(npcData, npcData.Name);
        }
        //存储玩家数据
        JsonManager.Instance.SaveData(gamerData, "GamerData");
    }
    //private void Update()
    //{
    //    print(gamerData.Items);
    //}
}
[System.Serializable]
public class Mission_ADay//数据类，统计玩家每天完成的任务及其专注时间
{
    public int Focus_Time;//专注时间
    public string Day;//日期
    public List<string> Options=new List<string>();//完成项目
    public Dictionary<string,int> ADay_FocusTime_Dic=new Dictionary<string, int>();//该日期的专注时间
    public Dictionary<string, List<string>> ADay_Options_Dic=new Dictionary<string, List<string>>();//该日期下的完成项目
}
[System.Serializable]
public class NPCData//数据类，用于存储NPC数据
{
    //public Sprite Sprite;

    public string Name;//名字
    public string Hobby;//爱好
    public string Personality;//性格
    public string Description;//描述文本
    public string BirthDay;//生日
    [Header("————喜好————")]
    public int Favorability;//好感度
    public string FavorvateThing;//喜好物
    [Header("————效率————")]
    public float MoveSpeed;//移动速度
    public float Work_Speed;//工作速度
    [Header("————时间————")]
    public float Work_Time;//工作时间

    public float Eat_Time;//吃饭时间

    public float Hungry_Time;//饥饿时间

    // 拷贝构造函数
    public NPCData(NPCData other)
    {
        if (other == null)
        {
            // 如果传入的参数为 null，则初始化为默认值
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
    // 公共无参构造函数
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
public class GamerData//玩家数据
{
    //统计部分
    public float PlantTime;//种地时间
    public int HarvestNum;//农作物的收获数量
    public string First_SignIn_Date;//第一次进入游戏的时间
    public int Money;//金钱统计
    public int Level;//玩家等级
    public float Current_XP;//玩家经验
    public float Max_XP;//玩家最大经验
    public List<PlantData> Items;
    //自定义部分
    public Sprite Player_HeadImage;//玩家头像
    public string PlayerBirthDay;//玩家生日
    public string PlayerName;//玩家名
    public string PlayerTitle;//玩家称号
    public string PlayerMotto;//玩家座右铭
}
[System.Serializable]
public class ItemData
{
    public string Name;//名字
    public Sprite Sprite;//图像
    public string Description;//描述

    public int Money_Cost_reward;//购买/售卖价格

}
[System.Serializable]
public class PlantData: ItemData
{
    [Header("————图像————")]
    [Tooltip("发芽图像")]
    public Sprite Germinate_Image;//发芽图像
    [Tooltip("成长图像")]
    public Sprite Grown_Image;
    [Tooltip("成熟图像")]
    public Sprite Mature_Image;

    [Header("————时间————")]
    [Tooltip("发芽时间")]
    public float Germinate_Time;
    [Tooltip("生长时间")]
    public float Grown_Time;
    [Tooltip("成熟时间")]
    public float Mature_Time;
    [Tooltip("播种时间")]
    public float Plant_Time;//播种时间
    [Tooltip("浇水时间")]
    public float Water_Time;//浇水时间
    [Tooltip("施肥时间")]
    public float fertilize_Time;//施肥时间
    [Tooltip("除虫时间")]
    public float BugControl_Time;//除虫时间
    //[Tooltip("松土时间")]
    //public float plow_Time;//松土时间
    [Tooltip("收获时间")]
    public float Harvest_Time;//收获时间

    [Tooltip("收获数量")]
    public int Num;//   收获数量
    // 拷贝构造函数
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
    [Tooltip("好感度加成")]
    public int favorability_Plus_Num;//好感度加成
}
[System.Serializable]
public class FoodData: ItemData
{
    public float Buff;//增益
}





