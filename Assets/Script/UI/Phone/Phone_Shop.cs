using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Shop : NewUIBase
{
    [Header("————表————")]
    public List<GiftData> Gift_list;
    public List<ClothItem_Data>Cloth_list;

    public List<Button> Gift_Buttons_Prefab;
    public List<Button> Cloth_Buttons_Prefab;
    [Header("————用于更换的图片素材————")]
    public Sprite Start_Button_Image;
    public Sprite After_Button_Image;
    [Header("————组————")]
    public GameObject Top_Button_Group;
    public GameObject Gift_UI;
    private void Start()
    {
        JsonData_Get();//Json数据获取
        GiftItem_Create();

        //这个是用于送礼物事件后实时更新好感度显示的，虽然确实是有点不美观但就先这样吧
        EventCenter.GetInstance().AddEventListener("Info_Update", Grade_Two_Info_Update);
    }
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_Shop");
                EventCenter.GetInstance().RemoveEventListener("Info_Update", Grade_Two_Info_Update);
                break;
            case "Gitf_Button":
                Open_Gift_UI(button);
                break;
            case "Cloth_Button":
                Open_Cloth_UI(button);
                break;
            case "Building_Button":
                Open_Building_UI(button);
                break;
        }
    }
    public void JsonData_Get()
    {
        //获取数据列表中的GiftJson数据
        string GiftData = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Gift_Data").text;
        string ClothData = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/Cloth_Data").text;
        //反序列化解析为对应的数据结构
        GiftData[] Gift_Datas = JsonMapper.ToObject<GiftData[]>(GiftData);
        ClothItem_Data[] Cloth_Datas = JsonMapper.ToObject<ClothItem_Data[]>(ClothData);
        //数组转化为列表
        Gift_list = new List<GiftData>(Gift_Datas);
        Cloth_list = new List<ClothItem_Data>(Cloth_Datas);
    }
    /// <summary>
    /// 打开礼物界面
    /// </summary>
    private void Open_Gift_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        GiftItem_Create();
    }
    /// <summary>
    /// 打开建筑UI
    /// </summary>
    private void Open_Building_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        BuildingItem_Create();
    }
    /// <summary>
    /// 打开装扮界面
    /// </summary>
    private void Open_Cloth_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        ClothItem_Create();
    }
    private void BuildingItem_Create()
    {
        return;
    }
    #region 商城 礼物界面
    /// <summary>
    /// 根据读表动态生成商品按钮
    /// </summary>
    public void GiftItem_Create()
    {
        for (int i = 0; i < Gift_list.Count; i++)
        {
            //创建按钮
            Button Gift_Button = Instantiate(Gift_Buttons_Prefab[i % Gift_Buttons_Prefab.Count], Gift_UI.transform);//生成按钮
            Gift_Button.GetComponent<Gift>().Data = Gift_list[i];//给按钮赋值
            Gift data = Gift_Button.GetComponent<Gift>();
            //更改按钮细节
            Gift_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Data.Description;//Gift的描述
            Gift_Button.transform.GetChild(1).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(data.Data.Sprite_ResPath);//Gift的Sprite
            Gift_Button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = data.Data.Name;//Gift的名称
            //为二级界面注册事件
            GameObject Gift_Grade_Two_UI= Gift_Button.transform.GetChild(3).gameObject;
            Gift GiftData = Gift_Button.GetComponent<Gift>();
            Grade_Two_EventSignIn(Gift_Grade_Two_UI, GiftData);
            //为按钮注册点击事件
            Gift_Button.onClick.AddListener(() => Open_GradeTwo_UI(Gift_Button));
        }
    }
    /// <summary>
    /// 打开礼物界面的二级界面
    /// </summary>
    private void Open_GradeTwo_UI(Button button)
    {
        Current_Button = button;

        for(int i= 0; i < Gift_UI.transform.childCount; i++)
        {
            Button button1=Gift_UI.transform.GetChild(i).GetComponent<Button>();
            //存储礼物界面二级画面底图，该底图是该二级界面的基本父物体
            GameObject BaseImage = button1.transform.GetChild(3).gameObject;
            if(Current_Button==button1)
            {
                BaseImage.GetComponent<CanvasGroup>().alpha = 1;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
                BaseImage.GetComponent<CanvasGroup>().interactable = true;

            }
            else
            {
                BaseImage.GetComponent<CanvasGroup>().alpha=0;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                BaseImage.GetComponent<CanvasGroup>().interactable = false;
            }
        }
    }
    /// <summary>
    /// 注册礼物界面二级界面的事件
    /// </summary>
    public void Grade_Two_EventSignIn(GameObject BaseImage, Gift data)
    {
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//获取二级界面中的好感度显示文本
            Gift giftdata = button.GetComponent<Gift>();//获取按钮上的商品数据

            giftdata.Data = data.Data;//获取数据
            button.gameObject.name = npcs[i % npcs.Length].name;//将按钮的名字全部转为现有的NPC的名字
            button.image.sprite = ResourceManager.GetInstance().Load<Sprite>(npcs[i % npcs.Length].GetComponent<CharaController>().Template_data.Sprite_Res);//按钮Image全部转为NPC的Sprite
            
            //为二级界面按钮注册送礼事件
            button.onClick.AddListener(() =>
            {
                if (ObjectKeeper_Singleton.Instance.gamerData.Money< giftdata.Data.Cost)//玩家钱不够
                {
                    return;
                }
                else
                {
                    EventCenter.GetInstance().EventTrigger("Gifted", button);
                    favorability.text = npcs[i % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//更新送礼的NPC的好感度
                }
            } );
        }
    }
    /// <summary>
    /// 二级界面信息更新方法，被引用于信息更新事件
    /// </summary>
    public void Grade_Two_Info_Update()
    {
        for (int i = 0; i < Gift_UI.transform.childCount; i++)
        {
            Button button1 = Gift_UI.transform.GetChild(i).GetComponent<Button>();
            //存储礼物界面二级画面底图，该底图是该二级界面的基本父物体
            GameObject BaseImage = button1.transform.GetChild(3).gameObject;
            Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
            GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
            for (int a = 0; a < buttons.Length; a++)
            {
                Button button = buttons[a];
                TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//获取二级界面中的好感度

                favorability.text = npcs[a % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//更新送礼的NPC的好感度
            }
        }
    }
    #endregion

    #region 商城 衣物页面
    /// <summary>
    /// 根据读表动态生成商品按钮
    /// </summary>
    public void ClothItem_Create()
    {
        for (int i = 0; i < Cloth_list.Count; i++)
        {
            //创建按钮
            Button Cloth_Button = Instantiate(Cloth_Buttons_Prefab[i % Cloth_Buttons_Prefab.Count], Gift_UI.transform);//生成按钮
            Cloth_Button.GetComponent<Item_Gift_Cloth>().Data = Cloth_list[i];//给按钮赋值/该值来自于Json数据文件
            ClothItem_Data data = Cloth_Button.GetComponent<Item_Gift_Cloth>().Data;
            //更改按钮细节
            Cloth_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Description;//Gift的描述
            Cloth_Button.transform.GetChild(1).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(data.ResPath);//Gift的Sprite
            Cloth_Button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = data.Name;//衣物的名称
            //为二级界面注册事件
            GameObject Gift_Grade_Two_UI = Cloth_Button.transform.GetChild(3).gameObject;
            //小动物头像
            for (int a=0;a<ObjectKeeper_Singleton.Instance.NPCs.Length;a++)
            {
                if(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.ID==data.Target_Animal_ID)
                {
                    Sprite Target_Animal_Sprite =ResourceManager.GetInstance().Load<Sprite>(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.Sprite_Res);
                    Cloth_Button.GetComponent<Item_Gift_Cloth>().HeadImage.sprite = Target_Animal_Sprite;
                }
            }
            //
            Item_Gift_Cloth ClothData = Cloth_Button.GetComponent<Item_Gift_Cloth>();

            Cloth_GradeTwo_EventSignIn(Gift_Grade_Two_UI, ClothData);
            //为按钮注册点击事件
            Cloth_Button.onClick.AddListener(() => Open_Cloth_GradeTwo_UI(Cloth_Button));
        }
    }
    /// <summary>
    /// 注册礼物界面二级界面的事件
    /// </summary>
    /// <param name="BaseImage">二级界面底图</param>
    /// <param name="data">二级界面所属的按钮的数据脚本</param>
    public void Cloth_GradeTwo_EventSignIn(GameObject BaseImage, Item_Gift_Cloth data)
    {
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
        for (int i = 0; i < buttons.Length; i++)
        {
            //按钮
            Button button = buttons[i];
            //获取二级界面中的好感度显示文本
            TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

            Item_Gift_Cloth Clothdata = button.GetComponent<Item_Gift_Cloth>();//获取按钮上的商品数据
            Clothdata.Data = data.Data;//注入数据

            for(int a = 0; a < ObjectKeeper_Singleton.Instance.NPCs.Length; a++)
            {
                if(Clothdata.Data.ID == ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.ID)
                {
                    button.gameObject.name = ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().npc_Information.Name;
                    //按钮Image全部转为NPC的Sprite
                    button.image.sprite = ResourceManager.GetInstance().Load<Sprite>(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.Sprite_Res);
                }
            }
            //为二级界面按钮注册送礼事件
            button.onClick.AddListener(() =>
            {
                if (ObjectKeeper_Singleton.Instance.gamerData.Money < Clothdata.Data.Cost)//玩家钱不够
                {
                    return;
                }
                else
                {
                    //
                    data.Bought();
                    
                    EventCenter.GetInstance().EventTrigger("Gifted", button);
                }
            });
        }
    }
    /// <summary>
    /// 打开礼物界面的二级界面
    /// </summary>
    private void Open_Cloth_GradeTwo_UI(Button button)
    {
        Current_Button = button;

        for (int i = 0; i < Gift_UI.transform.childCount; i++)
        {
            //
            Button button1 = Gift_UI.transform.GetChild(i).GetComponent<Button>();
            //存储礼物界面二级画面底图，该底图是该二级界面的基本父物体
            GameObject BaseImage = button1.transform.GetChild(3).gameObject;
            if (Current_Button == button1)
            {
                BaseImage.GetComponent<CanvasGroup>().alpha = 1;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = true;
                BaseImage.GetComponent<CanvasGroup>().interactable = true;

            }
            else
            {
                BaseImage.GetComponent<CanvasGroup>().alpha = 0;
                BaseImage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                BaseImage.GetComponent<CanvasGroup>().interactable = false;
            }
        }
    }
    /// <summary>
    /// 衣物二级界面信息更新方法，被引用于信息更新事件
    /// </summary>
    //public void GradeTwo_Cloth_Info_Update()
    //{
    //    for (int i = 0; i < Gift_UI.transform.childCount; i++)
    //    {
    //        Button button1 = Gift_UI.transform.GetChild(i).GetComponent<Button>();
    //        //存储礼物界面二级画面底图，该底图是该二级界面的基本父物体
    //        GameObject BaseImage = button1.transform.GetChild(3).gameObject;
    //        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
    //        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
    //        for (int a = 0; a < buttons.Length; a++)
    //        {
    //            Button button = buttons[a];
    //            TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//获取二级界面中的好感度

    //            favorability.text = npcs[a % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//更新送礼的NPC的好感度
    //        }
    //    }
    //}
    #endregion
    /// <summary>
    /// 清空礼物二级界面所有的按钮事件
    /// </summary>
    public void Clear_All_Button()
    {
        Button[] buttons = Gift_UI.GetComponentsInChildren<Button>();
        if(buttons.Length>0)
        {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
                Destroy(button.gameObject);
            }
        }
        else
        {
            return;
        }
    }
}
