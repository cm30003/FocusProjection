using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_WareHouse : NewUIBase
{
    public List<GameObject> Plant_Item_Button;
    public List<GameObject> Gift_Item_Buttons;
    public List<GameObject> Cloth_Item_Buttons;
    [Header("————组————")]
    public GameObject Top_Button_Group;
    public Transform Object_Group;
    [Header("————素材————")]
    public Sprite Start_Button_Image;
    public Sprite After_Button_Image;
    [Header("————元素列表————")]
    public List<ClothItem_Data> WareHouse_Cloth_List;

    public List<GiftData> WareHouse_Gift_List;

    public List<PlantItem_Data> WareHouse_Plant_List;

    public List<Map_Data> WareHouse_Map_List;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener("ItemList_Update", Update_Event);//监听物品列表更新，Trigger在Map_Target脚本中
        //用于送礼物事件后实时更新送礼界面好感度显示,Trigger在Open_GradeTwo_UI函数和Sell函数中
        EventCenter.GetInstance().AddEventListener("Info_Update",GiftGiving_Page);

        Update_Event();

        MapItem_SignIn();
        ClothItem_SIgnIn();
        GiftItem_SignIn();
        PlantItem_SignIn();
    }
    protected override void OnClick(string btnName, Button button)
    {
        Current_Button = button;
        Clear_ObjectGroup();
        Button_Image_Change(Top_Button_Group, Clicked_Image: After_Button_Image, Start_Image: Start_Button_Image);
        switch (btnName)
        {
            case "Gitf_Button":
                GiftButton_Create();
                break;
            case "Cloth_Button":
                ClothButton_Create();
                break;
            case "Map_Button":
                
                break;
            case "Plant_Button":
                //PlantButton_Create();
                break;
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_WareHouse");
                break;
        }
    }
    public void Update_Event()
    {
        Clear_ObjectGroup();
        //PlantButton_Create();
    }
    public void MapItem_SignIn()
    {
        if (ObjectKeeper_Singleton.Instance.Map_list != null)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.Map_list.Count; i++)
            {
                Map_Data map = ObjectKeeper_Singleton.Instance.Map_list[i];
                if (ObjectKeeper_Singleton.Instance.gamerData.Player_MapBought.Contains(map.ID))
                {
                    WareHouse_Map_List.Add(map);
                }
            }
        }
    }
    public void ClothItem_SIgnIn()
    {
        if (ObjectKeeper_Singleton.Instance.Cloth_list != null)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.Cloth_list.Count; i++)
            {
                ClothItem_Data cloth = ObjectKeeper_Singleton.Instance.Cloth_list[i];
                if (ObjectKeeper_Singleton.Instance.gamerData.Player_ClothBought.Contains(cloth.ID))
                {
                    WareHouse_Cloth_List.Add(cloth);
                }
            }
        }
    }
    public void GiftItem_SignIn()
    {
        if (ObjectKeeper_Singleton.Instance.Gift_list != null)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.Gift_list.Count; i++)
            {
                GiftData gift = ObjectKeeper_Singleton.Instance.Gift_list[i];
                if (ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum.ContainsKey(gift.ID)&&ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[gift.ID]>0)
                {
                    WareHouse_Gift_List.Add(gift);
                }
            }
        }
    }
    public void PlantItem_SignIn()
    {
        if (ObjectKeeper_Singleton.Instance.PlantItem_List != null)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.PlantItem_List.Count; i++)
            {
                PlantItem_Data plant = ObjectKeeper_Singleton.Instance.PlantItem_List[i];
                if (ObjectKeeper_Singleton.Instance.gamerData.Player_PlantNum.ContainsKey(plant.ID) && ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[plant.ID] > 0)
                {
                    WareHouse_Plant_List.Add(plant);
                }
            }
        }
    }
    #region 植物
    /// <summary>
    /// 物品按钮注册并生成点击事件
    /// </summary>
    public void PlantButton_Create()
    {
        if (WareHouse_Plant_List != null && WareHouse_Plant_List.Count > 0)
        {
            for (int i = 0; i < WareHouse_Plant_List.Count; i++)
            {
                //创建Item
                GameObject Item = Instantiate(Plant_Item_Button[i % Plant_Item_Button.Count], Object_Group);
                //为Item赋值
                Item.GetComponent<Plant>().Data = WareHouse_Plant_List[i];
                //获取卖出按钮
                Button Sell_Button = Item.transform.GetChild(2).GetComponent<Button>();
                //创建Item信息
                Item.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(WareHouse_Plant_List[i].Mature_SpriteResPath);//图标
                Item.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = WareHouse_Plant_List[i].Name;//商品名称
                //创建卖出事件
                Sell_Button.onClick.AddListener(() => Sell(Item));
            }
        }
    }
    /// <summary>
    /// 卖出事件
    /// </summary>
    /// <param name="Item">挂载数据的Item</param>
    public void Sell(GameObject Item)
    {
        PlantItem_Data plantData = Item.GetComponent<Plant>().Data;
        if(ObjectKeeper_Singleton.Instance.gamerData.Player_PlantNum.ContainsKey(plantData.ID)&& ObjectKeeper_Singleton.Instance.gamerData.Player_PlantNum[plantData.ID]>0)
        {
            ObjectKeeper_Singleton.Instance.gamerData.Player_PlantNum[plantData.ID]--;
            ObjectKeeper_Singleton.Instance.gamerData.Money += plantData.Sell_Price;
        }
        EventCenter.GetInstance().EventTrigger("Info_Update");//画面左上角玩家信息更新信息
    }
    #endregion
    #region 礼物
    /// <summary>
    /// 物品按钮注册并生成点击事件
    /// </summary>
    public void GiftButton_Create()
    {
        if (WareHouse_Gift_List != null && WareHouse_Gift_List.Count > 0)
        {
            for (int i = 0; i < WareHouse_Gift_List.Count; i++)
            {
                //创建Item
                GameObject Item = Instantiate(Gift_Item_Buttons[i % Gift_Item_Buttons.Count], Object_Group);
                //为Item赋值
                Item.GetComponent<Gift>().Data = WareHouse_Gift_List[i];
                //创建Item信息
                Item.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(WareHouse_Gift_List[i].Sprite_ResPath);//图标
                Item.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = WareHouse_Gift_List[i].Name;//商品名称
                Item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = WareHouse_Gift_List[i].Description;
                //获取送出界面
                Item.GetComponent<Button>().onClick.AddListener(() =>
                { 
                    Open_GradeTwo_UI(Item.GetComponent<Button>());
                });
            }
        }
    }
    /// <summary>
    /// 打开礼物界面的二级界面
    /// </summary>
    /// <param name="button">被选中的礼物</param>
    private void Open_GradeTwo_UI(Button button)
    {
        Current_Button = button;

        for (int i = 0; i < Object_Group.transform.childCount; i++)//遍历所有Objetc_Group下的所有对象
        {
            //获取被选中的礼物按钮
            Button button1 = Object_Group.transform.GetChild(i).GetComponent<Button>();
            //获取礼物界面二级画面底图，该底图是该二级界面的基本父物体
            GameObject BaseImage = button1.transform.GetChild(3).gameObject;
            if (Current_Button == button1)//如果是当前按钮，则显示该界面
            {
                BaseImage.GetComponent<CanvasGroup>().alpha = 1;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
                BaseImage.GetComponent<CanvasGroup>().interactable = true;
                EventCenter.GetInstance().EventTrigger("Info_Update");
                //礼物界面的中间那个图
                BaseImage.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(button1.GetComponent<Gift>().Data.Sprite_ResPath);
                Grade_Two_EventSignIn(BaseImage,button1.GetComponent<Gift>().Data);
            }
            else//如果不是则隐藏该界面
            {
                BaseImage.GetComponent<CanvasGroup>().alpha = 0;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                BaseImage.GetComponent<CanvasGroup>().interactable = false;
            }
        }
    }
    /// <summary>
    /// 注册礼物界面二级界面的事件
    /// </summary>
    public void Grade_Two_EventSignIn(GameObject BaseImage, GiftData data)
    {
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            //按钮Image全部转为NPC的Sprite
            Image image = button.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            image.sprite = ResourceManager.GetInstance().Load<Sprite>(npcs[i % npcs.Length].GetComponent<CharaController>().Template_data.Sprite_Res);
            //获取二级界面中的好感度显示文本
            TextMeshProUGUI favorability = button.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            //获取按钮上的商品数据
            Gift giftdata = button.GetComponent<Gift>();
            //获取数据
            giftdata.Data = data;
            //将按钮的名字全部转为现有的NPC的名字
            button.gameObject.name = npcs[i % npcs.Length].name;
            

            //为二级界面按钮注册送礼事件
            button.onClick.AddListener(() =>
            {
                if(ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum.ContainsKey(giftdata.Data.ID) && ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[giftdata.Data.ID]>0)//如果玩家手中有这个礼物
                {
                    EventCenter.GetInstance().EventTrigger("Gifted", button);

                    ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[giftdata.Data.ID]--;

                    Debug.Log(giftdata.Data.Name + "送礼" + ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[giftdata.Data.ID]);
                    
                    GiftGiving_Page();

                    if (ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum[giftdata.Data.ID] <= 0)//如果礼物已经送完了
                    {
                        Destroy(Current_Button.gameObject);
                        WareHouse_Gift_List.Remove(giftdata.Data);
                        ObjectKeeper_Singleton.Instance.gamerData.Player_GiftNum.Remove(giftdata.Data.ID);
                    }
                }
            });
        }
    }
    /// <summary>
    /// 送礼界面的好感度更新
    /// </summary>
    public void GiftGiving_Page()
    {
        if(Current_Button!=null)
        {
            GameObject BaseImage = Current_Button.transform.GetChild(3).gameObject;//获取送礼界面的底图
            Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
            GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
            for (int a = 0; a < buttons.Length; a++)//遍历所有二级界面的按钮
            {
                Button button = buttons[a];
                //获取二级界面中的好感度
                TextMeshProUGUI favorability = button.transform.GetComponentInChildren<TextMeshProUGUI>();
                //更新送礼的NPC的好感度
                favorability.text = npcs[a % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();
            }

        }
    }
    #endregion
    #region 衣服
    public void ClothButton_Create()
    {
        if (WareHouse_Cloth_List != null && WareHouse_Cloth_List.Count > 0)
        {
            for (int i = 0; i < WareHouse_Cloth_List.Count; i++)
            {
                //创建Item
                GameObject Item = Instantiate(Cloth_Item_Buttons[i % Cloth_Item_Buttons.Count], Object_Group);
                //为Item赋值
                Item.GetComponent<Item_Gift_Cloth>().Data = WareHouse_Cloth_List[i];
                //获取Item数据
                ClothItem_Data data=Item.GetComponent<Item_Gift_Cloth>().Data;
                //创建服装对应的NPC对象
                GameObject TargetAnimal = new GameObject();
                /*————创建Item信息————*/
                Item.transform.GetChild(0).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(WareHouse_Cloth_List[i].ResPath);//图标
                Item.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = WareHouse_Cloth_List[i].Name;//商品名称
                Item.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = WareHouse_Cloth_List[i].Description;//描述
                //获取NPC头像
                for (int a = 0; a < ObjectKeeper_Singleton.Instance.NPCs.Length; a++)
                {
                    if (ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.ID == data.Target_Animal_ID)
                    {
                        TargetAnimal=ObjectKeeper_Singleton.Instance.NPCs[a];
                        Sprite Target_Animal_Sprite = ResourceManager.GetInstance().Load<Sprite>(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.Sprite_Res);
                        Item.GetComponent<Item_Gift_Cloth>().HeadImage.sprite = Target_Animal_Sprite;
                    }
                }
                //获取卖出按钮
                Button Dress_Button = Item.transform.GetChild(4).GetComponent<Button>();
                Dress_Button.onClick.AddListener(() => Dress(TargetAnimal,ResourceManager.GetInstance().Load<Sprite>(data.ResPath)));
            }
        }
    }
    public void Dress(GameObject TargetAnimal,Sprite Cloth)
    {
        Debug.Log(TargetAnimal.GetComponent<CharaController>().npc_Information.Name+ "Dress!"+Cloth.name);
    }
    #endregion
    /// <summary>
    /// 清除显示物体，方便重新生成
    /// </summary>
    public void Clear_ObjectGroup()
    {
        for (int i = 0; i < Object_Group.transform.childCount; i++)
        {
            Destroy(Object_Group.transform.GetChild(i).gameObject);
        }
    }
}
