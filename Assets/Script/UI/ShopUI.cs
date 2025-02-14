using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIBase
{
    [Header("————表————")]
    public List<GiftData> product_list;
    public List<Button> Gift_Buttons_Prefab;
    //[Header("————组————")]
    //public List<TextMeshProUGUI> TextMeshProUGUIs;
    [Header("————临时变量————")]
    public float x;
    public float y;
    [Header("————父物体Groups————")]
    public GameObject Top_Button_Group;
    [Header("————顶部按钮————")]
    public Button Building_Button;
    public Button Cloth_Button;
    public Button Gift_Button;
    [Header("——退出按钮——")]
    public Button Quit_Button;
    [Header("————用于更换的图片素材————")]
    public Sprite Start_Button_Image;
    public Sprite After_Button_Image;
    [Header("————商店UI界面————")]
    public CanvasGroup Gift_UI;
    public GameObject Gift_Grade_Two_UI;
    private void Start()
    {
        InitClick();
    }
    /// <summary>
    /// 注册按钮事件
    /// </summary>
    private void InitClick()
    {
        Building_Button.onClick.AddListener(Open_Building_UI);//商店—建筑界面

        Cloth_Button.onClick.AddListener(Open_Cloth_UI);//商店—装扮界面

        Gift_Button.onClick.AddListener(Open_Gift_UI);//商店—礼物界面

        Quit_Button.onClick.AddListener(Quit_ShopUI);//退出按钮

        GiftItem_Creat();

        EventCenter.GetInstance().AddEventListener("Info_Update", Grade_Two_Info_Update);//这个是用于送礼物事件后实时更新好感度显示的，虽然确实是有点不美观但就先这样吧
    }

    /// <summary>
    /// 打开建筑UI
    /// </summary>
    private void Open_Building_UI()
    {
        Current_Button = Building_Button;
        Button_Image_Change(Top_Button_Group,After_Button_Image,Start_Button_Image);
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
    }

    /// <summary>
    /// 打开装扮界面
    /// </summary>
    private void Open_Cloth_UI()
    {
        Current_Button = Cloth_Button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
    }
    #region 商城 礼物界面

    /// <summary>
    /// 根据读表动态生成商品按钮
    /// </summary>
    public void GiftItem_Creat()
    {
        for (int i = 0; i < product_list.Count; i++)
        {
            Button Gift_Button = Instantiate(Gift_Buttons_Prefab[i % Gift_Buttons_Prefab.Count], Gift_UI.transform);//生成按钮
            Gift_Button.GetComponent<Gift>().Data = product_list[i];//给按钮赋值
            Gift data = Gift_Button.GetComponent<Gift>();
            //更改按钮细节
            Gift_Button.GetComponent<Image>().sprite = data.Data.Sprite;
            Gift_Button.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = data.Data.Name;
            Gift_Button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Data.Description;

            Gift_Button.onClick.AddListener(() => Open_GradeTwo_UI(Gift_Button));
        }
    }
    /// <summary>
    /// 打开礼物界面
    /// </summary>
    private void Open_Gift_UI()
    {
        Current_Button = Gift_Button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        OpenUI(Gift_UI);
    }
    /// <summary>
    /// 打开礼物界面的二级界面
    /// </summary>
    private void Open_GradeTwo_UI(Button button)
    {
        Gift data = button.GetComponent<Gift>();
        Transform transform = button.transform;

        //清空礼物二级界面所有的按钮事件
        Clear_All_Button();
        //存储礼物界面二级画面底图，该底图是该二级界面的基本父物体
        GameObject BaseImage = Gift_Grade_Two_UI.transform.GetChild(0).gameObject;
        //为二级界面注册事件
        Grade_Two_EventSignIn(BaseImage, data);
        //显示二级界面
        BaseImage.transform.localScale=Vector3.one;
        //锚定二级界面在画面中所处的位置
        BaseImage.transform.position = new Vector2(transform.position.x-x,transform.position.y-y);
    }
    /// <summary>
    /// 注册礼物界面二级界面的事件
    /// </summary>
    public void Grade_Two_EventSignIn(GameObject BaseImage,Gift data)
    {
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            TextMeshProUGUI favorability=button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//获取二级界面中的好感度
            Gift giftdata = button.GetComponent<Gift>();//获取按钮上的商品数据

            giftdata.Data = data.Data;
            button.gameObject.name = npcs[i%npcs.Length].name;//将按钮的名字全部转为现有的NPC的名字
            button.image.sprite = npcs[i % npcs.Length].GetComponentInChildren<SpriteRenderer>().sprite;//按钮Image全部转为NPC的Sprite
            favorability.text= npcs[i % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//更新送礼的NPC的好感度

            //注册送礼事件
            button.onClick.AddListener(() => EventCenter.GetInstance().EventTrigger("Gifted", button));
            button.onClick.AddListener(() => EventCenter.GetInstance().EventTrigger("Info_Update"));
        }
    }
    /// <summary>
    /// 二级界面信息更新方法
    /// </summary>
    public void Grade_Two_Info_Update()
    {
        GameObject BaseImage = Gift_Grade_Two_UI.transform.GetChild(0).gameObject;
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//暂存二级界面的所有Button组件/
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//暂存Npc数组
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//获取二级界面中的好感度

            favorability.text = npcs[i % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//更新送礼的NPC的好感度
        }
    }
    /// <summary>
    /// 清空礼物二级界面所有的按钮事件
    /// </summary>
    public void Clear_All_Button()
    {
        Button[] buttons=Gift_Grade_Two_UI.GetComponentsInChildren<Button>();
        foreach (Button button in buttons) 
        {
            button.onClick.RemoveAllListeners();
        }
    }
#endregion
    /// <summary>
    /// 退出商店
    /// </summary>
    private void Quit_ShopUI()
    {
        //关闭商店主界面
        CloseUI(GetComponent<CanvasGroup>());
        //关闭商店二级界面
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
        //将当前按钮，弃置于此
        if(Current_Button!=null)
        {
            Current_Button.GetComponent<Button>().image.sprite = Start_Button_Image;
            Current_Button = null;
        }
    }


}
