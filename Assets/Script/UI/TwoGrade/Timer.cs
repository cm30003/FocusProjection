using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : UIBase
{
    private float timer=1f;//标准的一秒
    private int second=0;//储存秒数
    private int Start_Second=0;//储存初始专注秒数

    private int is_pause=0;

    public TextMeshProUGUI Time_Text;
    public TextMeshProUGUI Month_Day;
    public TextMeshProUGUI Focus_Time;
    [Header("————GradeTwo_SetFoucusTimeUI————")]
    public GameObject SetFoucusTimeUI;

    public Button Open_SetFoucusTimeUI_Button;
    public Button Cancle_SetFoucusTimeUI_Button;
    public Button Ensure_Foucus_Time;
    [Header("————Buttons————")]
    public Button Pause_Button;
    public Button Reset_Button;
    private void Start()
    {
        //日期初始化，并每秒更新日期和月份
        InvokeRepeating("Month_Day_Update", 0f,1f);
        InitClick();
    }
    private void InitClick()
    {
        Open_SetFoucusTimeUI_Button.onClick.AddListener(Open_SetFoucusTimeUI);
        Cancle_SetFoucusTimeUI_Button.onClick.AddListener(Cancle_SetFoucusTimeUI);
        Ensure_Foucus_Time.onClick.AddListener(Ensure_Foucus_Time_Method);
        Pause_Button.onClick.AddListener(Pause_Timer);
        Reset_Button.onClick.AddListener(Reset_Timer);
    }
    private void Update()
    {
        Time_Text.text = DateTime.Now.ToLongTimeString().ToString();
        //
        if (ObjectKeeper_Singleton.Instance.Is_Set && is_pause == 0)
        {
            Focus_Time_Method();
        }
    }
    /// <summary>
    /// 更新月份和日期
    /// </summary>
    private void Month_Day_Update()
    {
        string Month = DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("en-us")).Substring(0, 3);
        string Day = DateTime.Now.Day.ToString();
        Month_Day.text = Month + "   " + Day;
    }
    /// <summary>
    /// 重置专注时间
    /// </summary>
    private void Reset_Timer()
    {
        second = Start_Second;
    }
    /// <summary>
    /// 暂停专注时间
    /// </summary>
    private void Pause_Timer()
    {
        if(ObjectKeeper_Singleton.Instance.Is_Set)
        {
            if (is_pause == 0)
            {
                is_pause++;
            }
            else
            {
                is_pause--;
            }
        }
    }
    /// <summary>
    /// 打开专注时间设置UI
    /// </summary>
    private void Open_SetFoucusTimeUI()
    {
        OpenUI(SetFoucusTimeUI.GetComponent<CanvasGroup>());
    }
    /// <summary>
    /// 取消专注时间设置
    /// </summary>
    private void Cancle_SetFoucusTimeUI()
    {
        CloseUI(SetFoucusTimeUI.GetComponent<CanvasGroup>());
    }
    /// <summary>
    /// 确认并设置专注时间/该事件绑定在专注时间设置界面的确认按钮上
    /// </summary>
    private void Ensure_Foucus_Time_Method()
    {
        //专注时间确认后，关闭二级菜单
        CloseUI(SetFoucusTimeUI.GetComponent<CanvasGroup>());
        //获取玩家输入的专注时间文本
        string time_text = SetFoucusTimeUI.GetComponentInChildren<TMP_InputField>().text;
        //文本转化为整数并进行计算
        int time = int.Parse(time_text);

        int hour = time / 60;
        int minute= time % 60;
        //设置总秒数
        second = time * 60;
        Start_Second = time * 60;
        Focus_Time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, 0);

        ObjectKeeper_Singleton.Instance.Is_Set = true;
        //当专注时间设置后，触发NPC工作事件
        EventCenter.GetInstance().EventTrigger("FocusTime_Set");
    }

    /// <summary>
    /// 设置专注时间
    /// </summary>
    public void Focus_Time_Method()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1;
            second--;

            int Hour = second / 3600;
            int Minute = second % 3600 / 60;
            int Second = second % 3600 % 60;

            Focus_Time.text = string.Format("{0:D2}:{1:D2}:{2:D2}", Hour, Minute, Second);
        }
        if (second <= 0)
        {
            ObjectKeeper_Singleton.Instance.Is_Set = false;
            calculate_FocusTime();
        }
    }
    /// <summary>
    /// 在游戏结束后计算出今日真实的专注时间,并进行存储
    /// </summary>
    public void calculate_FocusTime()
    {
        int Last_focustime=0;

        Mission_ADay mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");
        string Day = DateTime.Now.ToString("yyyy/M/d");//存储当前日期
        if (mission_ADay!= null)
        {
            //当存储日期存在于存储文件中时，更新该日期的专注时间
            if(mission_ADay.ADay_FocusTime_Dic.ContainsKey(Day))
            {
                Last_focustime = mission_ADay.ADay_FocusTime_Dic[Day];
                Last_focustime += Start_Second - second;
                mission_ADay.ADay_FocusTime_Dic[Day] = Last_focustime;
            }
            else
            {
                Last_focustime = Start_Second - second;
                mission_ADay.ADay_FocusTime_Dic.Add(Day, Last_focustime);
            }
        }
        JsonManager.Instance.SaveData(mission_ADay, "ADay");
    }
}

