using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanBroadUI : NewUIBase
{
    [Header("————UI————")]
    public GameObject Note;

    public GameObject PlanText_Group;

    public Image Choose_Bar;

    public GameObject PlaceHolder;
    [Header("————需要保存的数据————")]
    public List<string> Mission_UnFinish;//未完成的任务
    public Mission_ADay mission_ADay;//日期/专注时间/完成的任务

    public TextMeshProUGUI CurrentText;
    private void Start()
    {
        Plan_Panel_Button();
    }
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                Close_Note();
                break;
        }
    }
    protected override void OnEscapePressed()
    {
        Close_Note();
    }
    /// <summary>
    /// 为计划板添加按钮事件-文本框点击事件
    /// </summary>
    private void Plan_Panel_Button()
    {
        //获取完成按钮
        Button[] Finish_buttons = PlanText_Group.GetComponentsInChildren<Button>();
        TextMeshProUGUI[] texts  = PlanText_Group.GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < Finish_buttons.Length; i++)
        {
            int index  = i;
            Button button = Finish_buttons[i];
            TextMeshProUGUI text = texts[i];
            UIManager.AddCustomEventListener(text, EventTriggerType.PointerClick, (data) =>
            {
                Open_Note(text, data);
                Open_Choosen_Bar(text,data);
                CurrentText  = text;
            });
            UIManager.AddCustomEventListener(Choose_Bar, EventTriggerType.PointerClick, (data) =>
            {
                Open_Note(PlanText_Group.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), data);
            });
            button.onClick.AddListener(() => Finish_Button(ref text));
        }
    }
    public void Open_Note(TextMeshProUGUI text,BaseEventData data)
    {
        if(text.text==string.Empty)
        {
            Note.GetComponentInChildren<TMP_InputField>().text = text.transform.GetSiblingIndex().ToString() + ".";
        }
        else
        {
            Note.GetComponentInChildren<TMP_InputField>().text = text.text;
        }
        Note.GetComponent<CanvasGroup>().alpha = 1;
        Note.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Note.GetComponent<CanvasGroup>().interactable = true;
    }
    public void Close_Note()
    {
        if (CurrentText != null&&Note.GetComponentInChildren<TMP_InputField>().text != string.Empty)
        {
            CurrentText.text = Note.GetComponentInChildren<TextMeshProUGUI>().text;
            Open_Choosen_Bar(CurrentText);
        }
        Note.GetComponent<CanvasGroup>().alpha = 0;
        Note.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Note.GetComponent<CanvasGroup>().interactable = false;
    }

    /// <summary>
    /// 打开当前计划框
    /// </summary>
    /// <param name="text"></param>
    private void Open_Choosen_Bar(TextMeshProUGUI text,BaseEventData data=null)
    {
        #region 弃用
        ////创建占位符
        //GameObject placeHolder = Instantiate(PlaceHolder);

        //placeHolder.transform.SetParent(PlanText_Group.transform);

        //placeHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //Choose_Bar.transform.localScale = new Vector3(1.25f,1.25f,1.25f);

        ////更改文本父物体
        //text.transform.SetParent(Choose_Bar.transform);
        ////更改文本RectTranform
        //RectTransform textRect = text.GetComponent<RectTransform>();
        ////拉伸填充父物体
        //textRect.anchorMin = Vector2.zero;
        //textRect.anchorMax = Vector2.one;
        //textRect.offsetMin = new Vector2(15,60);
        //textRect.offsetMax = new Vector2(-15,-45.5f);
        #endregion
        text.transform.SetSiblingIndex(0);
        Choose_Bar.GetComponentInChildren<TextMeshProUGUI>().text = text.text;
    }
    #region 关闭选择框功能（已弃用）
    /// <summary>
    /// 关闭当前计划框
    /// </summary>
    //public void Close_Choosen_Bar()
    //{
    //    Choose_Bar.transform.localScale = Vector3.zero;

    //    GameObject placeHolder = PlanText_Group.transform.GetChild(0).gameObject;
    //    Destroy(placeHolder);
    //}
    /// <summary>
    /// 完成项目事件,当完成该项目，则将其保存到本地
    /// </summary>
    /// <param name="Option">对应的项目</param>
    #endregion
    private void Finish_Button(ref TextMeshProUGUI Option)
    {
        mission_ADay.Day = DateTime.Now.ToString("yyyy/M/d");//存储当前日期
        if (Option != null && Option.text != "")//option变量不为空，且存在文本
        {
            //当字典中存在此键/日期时，说明此前曾经存储过同一天的内容，更新字典
            if (mission_ADay.ADay_Options_Dic.ContainsKey(mission_ADay.Day))
            {
                mission_ADay.Finished_Options = mission_ADay.ADay_Options_Dic[mission_ADay.Day] = mission_ADay.Finished_Options;
                mission_ADay.Finished_Options.Add(Option.text);
            }
            else//不存在此键/日期时，添加此键/日期及其相对应的完成项目列表
            {
                mission_ADay.Finished_Options.Clear();
                mission_ADay.Finished_Options.Add(Option.text);
                mission_ADay.ADay_Options_Dic.Add(mission_ADay.Day, mission_ADay.Finished_Options);
            }
        }
        //存储今日数据到本地
        JsonManager.Instance.SaveData(mission_ADay, "ADay");
        Option.text = "";
    }
}
