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
    public FoodItem_Data foodData;//设定的食堂食物

    public GameObject WareHouse;//库房

    public GameObject Farm_Group;

    public GameObject[] Freight_Target;//收获的货物

    public GameObject[] Farm_Field;//田地组

    public GameObject[] Farm_Machine;//平板触摸屏

    public GameObject[] Eat_Area;//吃饭的区域

    public GameObject Rest_Area;//休息的区域

    public GameObject[] TouchFish_Area;//摸鱼的区域
    [Header("————音效————")]
    public string DayBGM_Path;
    public string NightBGM_Path;
    [Header("————小动物————")]
    public List<GameObject> Work_NPCs;//工作的NPC
    public GameObject[] NPCs;//NPC总组
    public List<GameObject> Waiting_NPCs;

    protected override void Awake()
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
        TouchFish_Area = GameObject.FindGameObjectsWithTag("TouchFish_Area");
        //注册事件
        EventCenter.GetInstance().AddEventListener<Button>("Gifted", Buy);//送礼事件
    }
    private void Start()
    {
        PlayerFirst_SignIn();

        Farm_Field = Field_SignIn();

        Current_Food(null);
    }
    /// <summary>
    /// 添加工作NPC到列表,列表最大值为3
    /// </summary>
    /// <param name="worknpc"></param>
    public void Add_WorkNpcs(GameObject worknpc)
    {
        if(Work_NPCs.Contains(worknpc)||Work_NPCs.Count>=3)
        {
            return;
        }
        else
        {
            Work_NPCs.Add(worknpc);
            Waiting_NPCs.Remove(worknpc);
        }
    }
    /// <summary>
    /// 移除在列表中的工作小动物，这通常代表小动物去休息了
    /// </summary>
    /// <param name="worknpc"></param>
    public void Remove_WorkNpc(GameObject worknpc)
    {
        if (Work_NPCs.Contains(worknpc))
        {
            Work_NPCs.Remove(worknpc);
            Waiting_NPCs.Add(worknpc);
        }
        else if (Waiting_NPCs.Contains(worknpc))
        {
            return ;
        }
        else
        {
            Waiting_NPCs.Add(worknpc);
        }
    }
    /// <summary>
    /// 公用购买方法
    /// </summary>
    /// <param name="Info">被购买的对象的信息</param>
    public void Buy(Button Info)
    {
        if(Info.GetComponent<Gift>()!=null)
        {
            gamerData.Money -= Info.GetComponent<Gift>().Data.Cost;//玩家扣除相应的礼物花销
        }
        else if(Info.GetComponent<Item_Gift_Cloth>()!=null)
        {
            gamerData.Money -= Info.GetComponent<Item_Gift_Cloth>().Data.Cost;//玩家扣除相应的植物花销
        }
        EventCenter.GetInstance().EventTrigger("Info_Update");
    }
    /// <summary>
    /// 玩家种植了,更新金钱
    /// </summary>
    /// <param name="Info"></param>
    public void Planted(Button Info)
    {
        gamerData.Money -= Info.GetComponent<Plant>().Data.Cost;//玩家扣除相应的植物花销
        EventCenter.GetInstance().EventTrigger("Info_Update");
    }
    /// <summary>
    /// 当前在食堂选择的食物
    /// </summary>
    /// <param name="foodData">食物数据</param>
    /// <returns>食物数据</returns>
    public FoodItem_Data Current_Food(FoodItem_Data foodData)
    {
        if(foodData==null)
        {
            foodData = new FoodItem_Data();
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
            //设置第一次进入游戏时间
            gamerData.First_SignIn_Date = DateTime.Now.ToString("yyyy年MM月dd号");
            //保存第一次进入游戏的时间
            JsonManager.Instance.SaveData(gamerData, "GamerData");
        }
        if(gamerData.HarvestItems==null)
        {
            gamerData.HarvestItems=new List<PlantItem_Data>();
            JsonManager.Instance.SaveData(gamerData, "GamerData");
        }
        for(int i=0;i<NPCs.Length;i++)
        {
            //加载存储的NPC好感度
            NPCs[i].GetComponent<CharaController>().data.Favorability = JsonManager.Instance.LoadData<NPCData>(NPCs[i].name).Favorability;
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
            NPCInformation npcData = NPCs[i].GetComponent<CharaController>().npc_Information;
            JsonManager.Instance.SaveData(npcData, npcData.Name);
        }
        //存储玩家数据
        JsonManager.Instance.SaveData(gamerData, "GamerData");
    }
}
[Serializable]
public class Mission_ADay//数据类，统计玩家每天完成的任务及其专注时间
{
    public int Focus_Time;//专注时间
    public string Day;//日期
    public List<string> Options=new List<string>();//完成项目
    public Dictionary<string,int> ADay_FocusTime_Dic=new Dictionary<string, int>();//该日期的专注时间
    public Dictionary<string, List<string>> ADay_Options_Dic=new Dictionary<string, List<string>>();//该日期下的完成项目
}
[Serializable]
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
    public List<PlantItem_Data> HarvestItems;//收获的作物
    //自定义部分
    public Sprite Player_HeadImage;//玩家头像
    public string PlayerBirthDay;//玩家生日
    public string PlayerName;//玩家名
    public string PlayerTitle;//玩家称号
    public string PlayerMotto;//玩家座右铭
}