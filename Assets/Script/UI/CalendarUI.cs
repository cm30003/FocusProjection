using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CalendarUI : UIBase
{

    public GameObject Date_DayUI_Prefab;//日期预制体
    public GameObject Date_Day_Group;//日期父物体
    [Header("——————按钮——————")]
    public Button Last_Month_Button;//上个月
    public Button Next_Month_Button;//下个月
    public Button Last_Year_Button;//去年
    public Button Next_Year_Button;//明年

    public Button Quit_Button;//退出按钮

    private int[] Big_Yue= {1,3,5,7,8,10,12};//闰月
    private int[] Small_Yue= {4,6,9,11};//平月

    private int Current_Year;//当前年份
    private int Current_Month;//当前月份
    private int Current_Day;//当前日期

    [Header("——————文本——————")]
    
    public TextMeshProUGUI This_Year_Text;//当前年份 文本
    public TextMeshProUGUI This_Month_Text;//当前月份 文本

    public TextMeshProUGUI Rightside_Date_Text;//右侧日期 文本
    public TextMeshProUGUI Rightside_FocusTime_Text;//专注时间文本
    public Transform Rightside_Finished_Mission_Group;//右侧已完成任务组
    public TextMeshProUGUI RighitSide_Information_Text;//右侧详情文本
    [Header("——————对象池——————")]
    public Date_Day_UI_ObjectPool pool;

    private void Start()
    {
        InitClick();

        Current_Month = DateTime.Now.Month;
        Current_Year = DateTime.Now.Year;
        Current_Day = DateTime.Now.Day;

        Update_Calendar();
    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_CalendarUI);
        Last_Month_Button.onClick.AddListener(Last_Month);
        Next_Month_Button.onClick.AddListener(Next_Month);
        Last_Year_Button.onClick.AddListener(Last_Year);
        Next_Year_Button.onClick.AddListener(Next_Year);
    }
    private void Next_Year()
    {
        Current_Year++;
        Clear();
        Update_Calendar();
    }

    private void Last_Year()
    {
        Current_Year--;
        Clear();
        Update_Calendar();
    }

    private void Next_Month()
    {
        Current_Month++;
        Clear();

        if(Current_Month>12)
        {
            Current_Month = 1;
        }

        Update_Calendar();
    }

    private void Last_Month()
    {
        Current_Month--;
        Clear();

        if (Current_Month <= 0)
        {
            Current_Month = 12;
        }

        Update_Calendar();
    }

    /// <summary>
    /// 更新日历
    /// </summary>
    private void Update_Calendar()
    {
        string current_year = Current_Year.ToString();
        string current_month = Current_Month.ToString();
        string current_Day = Current_Day.ToString();

        This_Year_Text.text = current_year + "年";
        This_Month_Text.text = current_month + "月";

        if (Big_Yue.Contains(Current_Month))//当月份为闰月
        {
            for (int i = 1; i <= 31; i++)
            {
                TextMeshProUGUI Date_Daily = pool.Get();//从对象池中获取一个日期预制体
                //获得日期上的脚本
                Button Date_Button = Date_Daily.GetComponent<Button>();
                if (Date_Button.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Button.onClick.AddListener(() => Date_Select(Date_Button));
                }

                Date_Daily.text = i.ToString();//设置其文本内容
                Date_Daily.transform.parent = Date_Day_Group.transform;//设置其父物体

                //当选择的当前年份小于真实的当前年份，则证明该年一整年都已经被经历过了
                if(Current_Year<DateTime.Now.Year)
                {
                    //则将日期设定为黄色
                    Date_Daily.color = Color.yellow;
                }
                else if(Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        else if (Small_Yue.Contains(Current_Month))//当月份为平月
        {
            for (int i = 1; i <= 30; i++)
            {
                TextMeshProUGUI Date_Daily = pool.Get();

                Button Date_Button = Date_Daily.GetComponent<Button>();
                if(Date_Button.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Button.onClick.AddListener(() => Date_Select(Date_Button));
                }

                Date_Daily.text = i.ToString();
                Date_Daily.transform.parent = Date_Day_Group.transform;
                if (Current_Year < DateTime.Now.Year)
                {
                    //则将日期设定为黄色
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        else if (Current_Month == 2 && Current_Year % 4 == 0)//2月
        {
            for (int i = 1; i <= 29; i++)
            {
                TextMeshProUGUI Date_Daily = pool.Get();

                Button Date_Button = Date_Daily.GetComponent<Button>();
                if (Date_Button.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Button.onClick.AddListener(() => Date_Select(Date_Button));
                }

                Date_Daily.text = i.ToString();
                Date_Daily.transform.parent = Date_Day_Group.transform;
                if (Current_Year < DateTime.Now.Year)
                {
                    //则将日期设定为黄色
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        else if (Current_Month == 2 && Current_Year % 4 != 0)
        {
            for (int i = 1; i <= 28; i++)
            {
                TextMeshProUGUI Date_Daily = pool.Get();

                Button Date_Button = Date_Daily.GetComponent<Button>();
                if (Date_Button.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Button.onClick.AddListener(() => Date_Select(Date_Button));
                }

                Date_Daily.text = i.ToString();
                Date_Daily.transform.parent = Date_Day_Group.transform;
                if (Current_Year < DateTime.Now.Year)
                {
                    //则将日期设定为黄色
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        Rightside_Date_Text.text = current_year + "/" + current_month + "/" + current_Day;
    }
    #region 日期选择事件
    /// <summary>
    /// 日期选择事件，选择该日期后，更改并显示右侧详细信息
    /// </summary>
    /// <param name="button">相对应的日期</param>
    public void Date_Select(Button button)
    {
        string current_year = Current_Year.ToString();
        string current_month = Current_Month.ToString();
        string Choosen_Day=button.GetComponent<TextMeshProUGUI>().text;
        Rightside_Date_Text.text= current_year + "/" + current_month + "/" + Choosen_Day;
        Current_Button = button;

        Update_RightSide_Information();

        //显示选择框
        for (int i = 0; i < Date_Day_Group.transform.childCount; i++)
        {
            Button Date = Date_Day_Group.transform.GetChild(i).GetComponent<Button>();
            Image Kuang = Date.GetComponentInChildren<Image>();
            if (Date==Current_Button)
            {
                Kuang.color = new Color(255f, 255f, 255f, 255f);
            }
            else
            {
                Kuang.color = new Color(255f, 255f, 255f, 0f);
            }
        }
    }
    /// <summary>
    /// 更新日历右侧的详细信息
    /// </summary>
    public void Update_RightSide_Information()
    {
        // 从本地Json文件中获取数据
        Mission_ADay mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");
        Update_Rightside_FocusTime(mission_ADay);
        // 不为空，生成右侧的详细信息
        if (mission_ADay != null)
        {
            //在每次更新信息的时候，先删除之前的信息
            for (int i = 0; i < Rightside_Finished_Mission_Group.childCount; i++)
            {
                Destroy(Rightside_Finished_Mission_Group.GetChild(i).gameObject);
            }
            // 判断日期是否匹配，若匹配，说明该日期已有相应存储数据
            if (mission_ADay.ADay_Options_Dic.ContainsKey(Rightside_Date_Text.text))
            {
                //更新本地文件中存储的
                for (int i=0;i< mission_ADay.ADay_Options_Dic[Rightside_Date_Text.text].Count;i++)
                {
                    UpdateRightSideText(mission_ADay.ADay_Options_Dic[Rightside_Date_Text.text][i]);
                }
            }
            else
            {
                Debug.Log("未匹配");
                UpdateRightSideText("无");
            }
        }
        else
        {
            UpdateRightSideText("无");
        }
    }
    /// <summary>
    /// 生成日历右侧详细信息文本的预制体
    /// </summary>
    /// <param name="text">对应文本</param>
    private void UpdateRightSideText(string text)
    {
        // 实例化TextMeshProUGUI
        TextMeshProUGUI rightside_Information_text = Instantiate(RighitSide_Information_Text, Rightside_Finished_Mission_Group);
        rightside_Information_text.text = text;
    }
    private void Update_Rightside_FocusTime(Mission_ADay mission_ADay)
    {
        if(mission_ADay.ADay_FocusTime_Dic.ContainsKey(Rightside_Date_Text.text))
        {
            int Minute = mission_ADay.ADay_FocusTime_Dic[Rightside_Date_Text.text] % 3600 / 60;
            Rightside_FocusTime_Text.text = Minute.ToString()+"分钟";
        }
        else
        {
            Rightside_FocusTime_Text.text = "0分钟";
        }
    }
    #endregion
    /// <summary>
    /// 重置 日历 将生成的日期释放回对象池中
    /// </summary>
    public void Clear()
    {
        for(int i= Date_Day_Group.transform.childCount; i>=0;i--)
        {
            if(i<Date_Day_Group.transform.childCount)
            {
                TextMeshProUGUI Object = Date_Day_Group.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                if(Object.gameObject.activeSelf)
                {
                    pool.Release(Object);
                }
            }
        }
    }
    /// <summary>
    /// 退出日历
    /// </summary>
    private void Quit_CalendarUI()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        CloseUI(canvasGroup);
    }
}
