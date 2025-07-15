using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using LitJson;
using UnityEngine.Events;

public class ObjectKeeper_Singleton:SingletonMono<ObjectKeeper_Singleton>
{

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
    public List<GameObject> Work_NPCs;//工作的NPC组
    public GameObject[] NPCs;//NPC总组
    public List<GameObject> Waiting_NPCs;//等待工作的NPC组
    [Header("————非存储数据————")]
    public List<GiftData> Gift_list;

    public List<ClothItem_Data> Cloth_list;

    public List<FoodItem_Data> Food_List;

    public List<PlantItem_Data> PlantItem_List;

    public List<Map_Data> Map_list;
    [Header("————存储数据————")]
    public GamerData gamerData;
    protected override void Awake()
    {
        base.Awake();
        //注册事件
        Object_Get();
        JsonData_Get();
        //EventCenter.GetInstance().AddEventListener<Button>("Gifted", Buy);//送礼事件
    }
    private void Start()
    {
        PlayerFirst_SignIn();

        Farm_Field = Field_SignIn();

        Current_Food(null);
    }
    /// <summary>
    /// 获取场景中的对象
    /// </summary>
    public void Object_Get()
    {
        WareHouse = GameObject.FindGameObjectWithTag("WareHouse_Area");
        Freight_Target = GameObject.FindGameObjectsWithTag("Freight_Target");
        Farm_Group = GameObject.FindGameObjectWithTag("Farm_Field");
        NPCs = GameObject.FindGameObjectsWithTag("NPC");
        Farm_Machine = GameObject.FindGameObjectsWithTag("Farm_Machine");
        Eat_Area = GameObject.FindGameObjectsWithTag("Eat_Area");
        Rest_Area = GameObject.FindGameObjectWithTag("Rest_Area");
        TouchFish_Area = GameObject.FindGameObjectsWithTag("TouchFish_Area");
    }
    /// <summary>
    /// 获取存储的Json数据
    /// </summary>
    public void JsonData_Get()
    {
        //获取数据列表中的GiftJson数据
        string GiftData = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Gift_Data").text;
        string ClothData = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Cloth_Data").text;
        string FoodInfo = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/FoodItem_Data").text;
        string PlantInfo = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Plant_Data").text;
        string MapInfo = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Map_Data").text;
        //反序列化解析为对应的数据结构
        GiftData[] Gift_Datas = JsonMapper.ToObject<GiftData[]>(GiftData);
        ClothItem_Data[] Cloth_Datas = JsonMapper.ToObject<ClothItem_Data[]>(ClothData);
        FoodItem_Data[] foodItem_Datas = JsonMapper.ToObject<FoodItem_Data[]>(FoodInfo);
        PlantItem_Data[] PlantItem_Datas = JsonMapper.ToObject<PlantItem_Data[]>(PlantInfo);
        Map_Data[] Map_Datas = JsonMapper.ToObject<Map_Data[]>(MapInfo);
        //数组转化为列表
        Gift_list = new List<GiftData>(Gift_Datas);
        Cloth_list = new List<ClothItem_Data>(Cloth_Datas);
        Food_List = new List<FoodItem_Data>(foodItem_Datas);
        PlantItem_List = new List<PlantItem_Data>(PlantItem_Datas);
        Map_list = new List<Map_Data>(Map_Datas);
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
    public void Buy(GiftData giftData)
    {
        if (giftData != null&&gamerData.Money>giftData.Cost)//如果是礼物
        {
            gamerData.Money -= giftData.Cost;//玩家扣除相应的礼物花销
            if (gamerData.Player_GiftNum != null && gamerData.Player_GiftNum.ContainsKey(giftData.ID))//玩家已经拥有该礼物了
            {
                gamerData.Player_GiftNum[giftData.ID] += 1;//数量+1
            }
            else//玩家没有该礼物
            {
                gamerData.Player_GiftNum.Add(giftData.ID, 1);//添加该礼物到字典
            }
            EventCenter.GetInstance().EventTrigger("Info_Update");
        }
        else
        {
            return;
        }
    }
    public void Buy(ClothItem_Data clothData,UnityAction callBack_Action=null)
    {
        if (clothData != null&&gamerData.Money > clothData.Cost&&!gamerData.Player_ClothBought.Contains(clothData.ID))//如果是衣服
        {
            gamerData.Money -= clothData.Cost;//玩家扣除相应的服装花销
            gamerData.Player_ClothBought.Add(clothData.ID);
            if(callBack_Action!=null)
            {
                callBack_Action();
            }

            EventCenter.GetInstance().EventTrigger("Info_Update");
        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// 公用购买方法
    /// </summary>
    /// <param name="Info">被购买的对象的信息</param>
    public void Buy(Map_Data mapData)
    {
        if(mapData != null)//如果是地图
        {
            gamerData.Money-=mapData.Cost;
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
            gamerData=new GamerData();
            //设置第一次进入游戏时间
            gamerData.First_SignIn_Date = DateTime.Now.ToString("yyyy年MM月dd号");
            //保存第一次进入游戏的时间
            JsonManager.Instance.SaveData(gamerData, "GamerData");
        }
        //if(gamerData.Player_PlantNum==null)
        //{
        //    gamerData.Player_PlantNum = 
        //    JsonManager.Instance.SaveData(gamerData, "GamerData");
        //}
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
