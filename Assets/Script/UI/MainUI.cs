using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUI : UIBase
{
    [Header("————各系统————")]
    public CanvasGroup ShopUI;//商店界面
    public CanvasGroup Player_ID_Card_UI;//玩家名片
    public CanvasGroup NCP_Manager_UI;//动物管理
    public CanvasGroup Calendar_UI;//日历
    public CanvasGroup Plant_Manage_UI;//植物管理
    public CanvasGroup System_Menue_UI;//系统菜单
    public CanvasGroup Mail_UI;//邮件
    public CanvasGroup WareHouse_UI;//仓库
    public CanvasGroup DiningHall_UI;//食堂
    [Header("————底部工具栏————")]
    public Button Quit_Button;//退出
    public Button Mail_Button;//邮件
    public Button Calendar_Button;//日历
    public Button Hide_UI_Button;//隐藏主UI
    public Button System_Menue_Button;//系统菜单
    [Header("————主界面按钮————")]
    public Button Shop_Button;//商店
    public Button NPCManager_Button;//动物管理
    public Button DiningHall_Button;//食堂
    public Button WareHouse_Button;//仓库
    public Button PlantManager_Button;//植物管理
    public Button Timer;//计时器
    public Button Player_IDCard_Button;//玩家名片
    [Header("————右上计划板————")]
    public GameObject InputField_Group;

    public GameObject Choose_Bar;

    public GameObject PlaceHolder;

    public Mission_ADay mission_ADay;
    [Header("————左上玩家信息————")]
    public TextMeshProUGUI Player_Name;
    public TextMeshProUGUI Player_Money;
    public TextMeshProUGUI Player_level;
    private void Start()
    {
        if (JsonManager.Instance.LoadData<Mission_ADay>("ADay") != null)
        {
            mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");
        }
        else
        {
            mission_ADay = new Mission_ADay();
        }
        InitClick();
    }
    private void InitClick()
    {
        Shop_Button.onClick.AddListener(Open_Shop_UI);//商店
        NPCManager_Button.onClick.AddListener(Open_NCP_Manager_UI);//动物管理
        DiningHall_Button.onClick.AddListener(Open_DiningHall_UI);//食堂
        WareHouse_Button.onClick.AddListener(Open_WareHouse_UI);//仓库
        PlantManager_Button.onClick.AddListener(Open_Plant_Manage_UI);//植物管理

        Player_IDCard_Button.onClick.AddListener(Open_Player_IDCard);//打开玩家名片
        Quit_Button.onClick.AddListener(QuitGame);//退出
        Mail_Button.onClick.AddListener(Open_Mail_UI);//邮件
        Calendar_Button.onClick.AddListener(Open_Calendar_UI);//日历
        Hide_UI_Button.onClick.AddListener(Hide_Main_UI);//隐藏主UI
        System_Menue_Button.onClick.AddListener(Open_System_Menue_UI);//打开系统菜单

        EventCenter.GetInstance().AddEventListener("Info_Update", Info_Update);//信息更新事件
        Info_Update();

        Plan_Panel_Button();
    }
    /// <summary>
    /// 更新主UI中的玩家信息
    /// </summary>
    public void Info_Update()
    {
        Player_Name.text = ObjectKeeper_Singleton.Instance.gamerData.PlayerName;
        Player_Money.text = ObjectKeeper_Singleton.Instance.gamerData.Money.ToString();
        Player_level.text ="Lv."+ObjectKeeper_Singleton.Instance.gamerData.Level.ToString();
    }
    #region 计划板相关
    /// <summary>
    /// 为计划板添加按钮事件-输入文本框选择事件
    /// </summary>
    private void Plan_Panel_Button()
    {
        //获取完成按钮
        Button[] Finish_buttons = InputField_Group.GetComponentsInChildren<Button>();
        //获取输入文本框
        TMP_InputField[] inputField = InputField_Group.GetComponentsInChildren<TMP_InputField>();

        for(int i=0;i<Finish_buttons.Length;i++)
        {
            Button button = Finish_buttons[i];
            TMP_InputField field = inputField[i];
            field.onSelect.AddListener((text)=>Open_Choosen_Bar(field));
            button.onClick.AddListener(() => Finish_Button(ref field));
        }
    }
    /// <summary>
    /// 完成项目事件,当完成该项目，则将其保存到本地
    /// </summary>
    /// <param name="Option">对应的项目</param>
    private void Finish_Button(ref TMP_InputField Option)
    {
        mission_ADay.Day = DateTime.Now.ToString("yyyy/M/d");//存储当前日期
        if (Option!=null&&Option.text!="")//option变量不为空，且存在文本
        {
            //当字典中存在此键/日期时，说明此前曾经存储过同一天的内容，更新字典
            if (mission_ADay.ADay_Options_Dic.ContainsKey(mission_ADay.Day))
            {
                mission_ADay.Options=mission_ADay.ADay_Options_Dic[mission_ADay.Day] = mission_ADay.Options;
                mission_ADay.Options.Add(Option.text);
            }
            else//不存在此键/日期时，添加此键/日期及其相对应的完成项目列表
            {
                mission_ADay.Options.Clear();
                mission_ADay.Options.Add(Option.text);
                mission_ADay.ADay_Options_Dic.Add(mission_ADay.Day, mission_ADay.Options);
            }
        }
        //存储今日数据到本地
        JsonManager.Instance.SaveData(mission_ADay, "ADay");
        Option.text = "";
    }
    /// <summary>
    /// 打开当前计划框
    /// </summary>
    /// <param name="text"></param>
    private void Open_Choosen_Bar(TMP_InputField text)
    {
        //确认计划板当前是否打开，并判断输入框是否为空
        if(Choose_Bar.transform.localScale==Vector3.zero&&text.text!="")
        {
            //创建占位符
            GameObject placeHolder = Instantiate(PlaceHolder);

            placeHolder.transform.SetParent(InputField_Group.transform);

            placeHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);

            placeHolder.transform.SetSiblingIndex(0);

            text.transform.SetSiblingIndex(1);

            Choose_Bar.transform.localScale = Vector3.one;
            Choose_Bar.GetComponentInChildren<TextMeshProUGUI>().text = text.text;
        } 
    }
    /// <summary>
    /// 关闭当前计划框
    /// </summary>
    public void Close_Choosen_Bar()
    {
        Choose_Bar.transform.localScale = Vector3.zero;

        GameObject placeHolder = InputField_Group.transform.GetChild(0).gameObject;
        Destroy(placeHolder);
    }
    #endregion 计划板相关
    /// <summary>
    /// 打开主UI
    /// </summary>
    public void Open_Main_UI()
    {
        var ui = GetComponent<CanvasGroup>();
        OpenUI(ui);
    }
    /// <summary>
    /// 隐藏主UI
    /// </summary>
    private void Hide_Main_UI()
    {
        var ui = GetComponent<CanvasGroup>();
        CloseUI(ui);
    }
    /// <summary>
    /// 打开玩家名片
    /// </summary>
    private void Open_Player_IDCard()
    {
        OpenUI(Player_ID_Card_UI);
        Player_ID_Card_UI.GetComponent<IDCardUI>().enabled = true;
    }

    public void Open_Shop_UI()
    {
        OpenUI(ShopUI);
    }
    public void Open_Player_ID_Card_UI()
    {
        OpenUI(Player_ID_Card_UI);
    }
    public void Open_NCP_Manager_UI()
    {
        OpenUI(NCP_Manager_UI);
    }
    public void Open_Calendar_UI()
    {
        OpenUI(Calendar_UI);
    }
    public void Open_Plant_Manage_UI()
    {
        OpenUI(Plant_Manage_UI);
    }
    public void Open_System_Menue_UI()
    {
        OpenUI(System_Menue_UI);
    }
    public void Open_Mail_UI()
    {
        OpenUI(Mail_UI);
    }
    public void Open_WareHouse_UI()
    {
        OpenUI(WareHouse_UI);
    }
    public void Open_DiningHall_UI()
    {
        OpenUI(DiningHall_UI);
    }
    public void QuitGame()
    {
        ObjectKeeper_Singleton.Instance.Save_GamerData();
        Timer.GetComponent<Timer>().calculate_FocusTime();
        Application.Quit();
    }
}
