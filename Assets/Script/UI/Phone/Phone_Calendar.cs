using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Calendar : NewUIBase
{

    private int[] Big_Yue = { 1, 3, 5, 7, 8, 10, 12 };//闰月
    private int[] Small_Yue = { 4, 6, 9, 11 };//平月

    private int Current_Month;
    private int Current_Year;
    private int Current_Day;
    [Header("——————文本——————")]
    public TextMeshProUGUI This_Year_Text;//当前年份 文本
    public TextMeshProUGUI This_Month_Text;//当前月份 文本

    public string Choosen_Date;//日期 文本
    public TextMeshProUGUI FocusTime_Text;//专注时间文本

    public TextMeshProUGUI Finished_Mission_Text;//详情文本
    [Header("——————对象池——————")]
    public Phone_Date_UI_ObjectPool pool;
    [Header("————位置信息————")]
    public Transform Date_Day_Group;//日期组
    public Transform Finished_Mission_Group;//右侧已完成任务组
    [Header("————素材————")]
    public GameObject Place_Holder;
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_Calendar");
                break;
            case "Next_Year":
                Next_Year();
                break;
            case "Last_Year":
                Last_Year();
                break;
            case "Next_Month":
                Next_Month();
                break;
            case "Last_Month":
                Last_Month();
                break;
            case "Phone_Data(Clone)":
                print("111111");
                Date_Select(button);
                break;
        }
    }
    private void Start()
    {
        Current_Month = DateTime.Now.Month;
        Current_Year = DateTime.Now.Year;
        Current_Day = DateTime.Now.Day;

        Update_Calendar();
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

        if (Current_Month > 12)
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
        // 从本地Json文件中获取数据
        Mission_ADay mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");

        string current_year = Current_Year.ToString();
        string current_month = Current_Month.ToString();
        string current_Day = Current_Day.ToString();

        This_Year_Text.text = current_year + "年";
        This_Month_Text.text = current_month + "月";

        GetDayOfWeek(Current_Year,Current_Month);//生成星期占位符

        if (Big_Yue.Contains(Current_Month))//当月份为闰月
        {
            for (int i = 1; i <= 31; i++)
            {
                //从对象池中获取一个日期预制体
                Button Date_Daily = pool.Get();
                //如果按钮不存在点击事件
                if (Date_Daily.onClick.GetPersistentEventCount() == 0)
                {
                    //则给按钮添加点击事件
                    Date_Daily.onClick.AddListener(() => Date_Select(Date_Daily));

                }
                //设置日期文本
                Date_Daily.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                //设置 存在已完成任务 的 日期 下的 下标
                if(mission_ADay.ADay_Options_Dic.ContainsKey(current_year + "/" + current_month + "/" + i.ToString()))
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color= new Color(255f, 255f, 255f, 255f);
                }
                else
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                }

                //设置位置
                Date_Daily.transform.SetParent(Date_Day_Group);
                #region 设置文本颜色 弃用
                ////当选择的当前年份小于真实的当前年份，则证明该年一整年都已经被经历过了
                //if (Current_Year < DateTime.Now.Year)
                //{
                //    //则将日期设定为黄色
                //    Date_Daily.color = Color.yellow;
                //}
                //else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                //{
                //    Date_Daily.color = Color.yellow;
                //}
                //else
                //{
                //    Date_Daily.color = Color.white;
                //}
                #endregion
            }
        }
        else if (Small_Yue.Contains(Current_Month))//当月份为平月
        {
            for (int i = 1; i <= 30; i++)
            {
                Button Date_Daily = pool.Get();

                if (Date_Daily.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Daily.onClick.AddListener(() => Date_Select(Date_Daily));
                }

                Date_Daily.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

                //设置 存在已完成任务 的 日期 下的 下标
                if (mission_ADay.ADay_Options_Dic.ContainsKey(current_year + "/" + current_month + "/" + i.ToString()))
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                }
                else
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                }

                Date_Daily.transform.SetParent(Date_Day_Group);
                #region 设置文本颜色 弃用
                ////设置文本颜色
                //if (Current_Year < DateTime.Now.Year)
                //{
                //    //则将日期设定为黄色
                //    Date_Daily.color = Color.yellow;
                //}
                //else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                //{
                //    Date_Daily.color = Color.yellow;
                //}
                //else
                //{
                //    Date_Daily.color = Color.white;
                //}
                #endregion
            }
        }
        else if (Current_Month == 2 && Current_Year % 4 == 0)//2月
        {
            for (int i = 1; i <= 29; i++)
            {
                Button Date_Daily = pool.Get();

                if (Date_Daily.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Daily.onClick.AddListener(() => Date_Select(Date_Daily));
                }

                Date_Daily.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

                //设置 存在已完成任务 的 日期 下的 下标
                if (mission_ADay.ADay_Options_Dic.ContainsKey(current_year + "/" + current_month + "/" + i.ToString()))
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                }
                else
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                }

                Date_Daily.transform.SetParent(Date_Day_Group);
                #region 设置文本颜色 弃用
                //if (Current_Year < DateTime.Now.Year)
                //{
                //    //则将日期设定为黄色
                //    Date_Daily.color = Color.yellow;
                //}
                //else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                //{
                //    Date_Daily.color = Color.yellow;
                //}
                //else
                //{
                //    Date_Daily.color = Color.white;
                //}
                #endregion
            }
        }
        else if (Current_Month == 2 && Current_Year % 4 != 0)
        {
            for (int i = 1; i <= 28; i++)
            {
                Button Date_Daily = pool.Get();

                if (Date_Daily.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Daily.onClick.AddListener(() => Date_Select(Date_Daily));
                }

                Date_Daily.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();

                //设置 存在已完成任务 的 日期 下的 下标
                if (mission_ADay.ADay_Options_Dic.ContainsKey(current_year + "/" + current_month + "/" + i.ToString()))
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                }
                else
                {
                    Date_Daily.transform.GetChild(2).GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                }
                //设置位置
                Date_Daily.transform.SetParent(Date_Day_Group);
                #region 设置文本颜色 弃用
                //if (Current_Year < DateTime.Now.Year)
                //{
                //    //则将日期设定为黄色
                //    Date_Daily.color = Color.yellow;
                //}
                //else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//当选择的当前年份大于等于现实年份时，用月份和日期决断
                //{
                //    Date_Daily.color = Color.yellow;
                //}
                //else
                //{
                //    Date_Daily.color = Color.white;
                //}
                #endregion
            }
        }
        Choosen_Date = current_year + "/" + current_month + "/" + current_Day;
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
        string Choosen_Day = button.GetComponentInChildren<TextMeshProUGUI>().text;
        Choosen_Date = current_year + "/" + current_month + "/" + Choosen_Day;
        Current_Button = button;

        Update_RightSide_Information();

        //显示选择框
        for (int i = 0; i < Date_Day_Group.childCount; i++)
        {
            Button Date = Date_Day_Group.GetChild(i).GetComponent<Button>();
            if(Date!=null)
            {
                Image Kuang = Date.GetComponentInChildren<Image>();
                if (Date == Current_Button)
                {
                    Kuang.color = new Color(255f, 255f, 255f, 255f);
                }
                else
                {
                    Kuang.color = new Color(255f, 255f, 255f, 0f);
                }
            }
        }
    }
    /// <summary>
    /// 判断当前年月的一日是星期几
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    public void GetDayOfWeek(int year,int month)
    {
        DateTime dateTime = new DateTime(year,month,1);
        DayOfWeek dayOfWeek=dateTime.DayOfWeek;
        switch (dayOfWeek)
        {
            case DayOfWeek.Sunday://星期天
                break;
            case DayOfWeek.Monday://星期一
                PlaceHolder_Instantiate(1);
                break;
            case DayOfWeek.Tuesday://星期二
                PlaceHolder_Instantiate(2);
                break;
            case DayOfWeek.Wednesday://星期三
                PlaceHolder_Instantiate(3);
                break;
            case DayOfWeek.Thursday://星期四
                PlaceHolder_Instantiate(4);
                break;
            case DayOfWeek.Friday://星期五
                PlaceHolder_Instantiate(5);
                break;
            case DayOfWeek.Saturday://星期六
                PlaceHolder_Instantiate(6);
                break;
        }
    }
    /// <summary>
    /// 占位符创建
    /// </summary>
    /// <param name="num">占位符数量</param>
    public void PlaceHolder_Instantiate(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject placeHolder = Instantiate(Place_Holder, Date_Day_Group);
            placeHolder.transform.SetSiblingIndex(i);
        }
    }
    /// <summary>
    /// 更新日历的详细信息
    /// </summary>
    public void Update_RightSide_Information()
    {
        // 从本地Json文件中获取数据
        Mission_ADay mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");
        Update_FocusTime(mission_ADay);
        // 不为空，生成右侧的详细信息
        if (mission_ADay != null)
        {
            //在每次更新信息的时候，先删除之前的信息
            for (int i = 0; i < Finished_Mission_Group.childCount; i++)
            {
                Destroy(Finished_Mission_Group.GetChild(i).gameObject);
            }
            // 判断日期是否匹配，若匹配，说明该日期已有相应存储数据
            if (mission_ADay.ADay_Options_Dic.ContainsKey(Choosen_Date))
            {
                //更新本地文件中存储的
                for (int i = 0; i < mission_ADay.ADay_Options_Dic[Choosen_Date].Count; i++)
                {
                    Updat_Mission_Text(mission_ADay.ADay_Options_Dic[Choosen_Date][i]);
                }
            }
            else
            {
                Debug.Log("未匹配");
                Updat_Mission_Text("无");
            }
        }
        else
        {
            Updat_Mission_Text("无");
        }
    }
    /// <summary>
    /// 生成底部的该日期完成任务文本
    /// </summary>
    /// <param name="text">对应文本</param>
    private void Updat_Mission_Text(string text)
    {
        // 实例化TextMeshProUGUI
        TextMeshProUGUI rightside_Information_text = Instantiate(Finished_Mission_Text, Finished_Mission_Group);
        rightside_Information_text.text = text;
    }
    /// <summary>
    /// 更新底部的该日期专注时间文本
    /// </summary>
    /// <param name="mission_ADay">专注时间</param>
    private void Update_FocusTime(Mission_ADay mission_ADay)
    {
        //当该日期被存入专注时间字典中时，更新该日期的专注时间文本
        if (mission_ADay.ADay_FocusTime_Dic.ContainsKey(Choosen_Date))
        {
            int Minute = mission_ADay.ADay_FocusTime_Dic[Choosen_Date] % 3600 / 60;
            FocusTime_Text.text = Minute.ToString() + "分钟";
        }
        else
        {
            FocusTime_Text.text = "0分钟";
        }
    }
    #endregion
    /// <summary>
    /// 重置 日历 将生成的日期释放回对象池中
    /// </summary>
    public void Clear()
    {
        for (int i = Date_Day_Group.childCount; i >= 0; i--)
        {
            if (i < Date_Day_Group.childCount)
            {
                GameObject gameObject=Date_Day_Group.GetChild(i).gameObject;
                if (gameObject.gameObject.activeSelf&& gameObject != null)
                {
                    Button Object = gameObject.GetComponent<Button>();
                    if (Object != null)
                    {
                        pool.Release(Object);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
