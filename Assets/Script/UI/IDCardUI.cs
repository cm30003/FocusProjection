using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDCardUI : UIBase
{
    //致未来的我或者谁：
    //个人名片的关闭事件绑定在个人名片的黑底上，怕你忘了
    [Header("————玩家自定义输入部分————")]
    public TMP_InputField Motto;//座右铭
    public TMP_InputField Name;
    public TMP_InputField Title;//称号
    public TMP_InputField BirthDay;
    [Header("————文本————")]
    public TextMeshProUGUI Harvest_Num;
    public TextMeshProUGUI Plant_Time;
    public TextMeshProUGUI SignIn_Day;
    private void Start()
    {
        Event_SignIn();
    }
    private void OnEnable()
    {
        GamerData gamerData = JsonManager.Instance.LoadData<GamerData>("GamerData");
        if (gamerData!=null)
        {
            //座右铭
            Motto.text = gamerData.PlayerMotto;
            //玩家名
            Name.text = gamerData.PlayerName;
            //称号
            Title.text = gamerData.PlayerTitle;
            //生日
            BirthDay.text = gamerData.PlayerBirthDay;

            //收获数量
            Harvest_Num.text = "收获" + gamerData.HarvestNum.ToString() + "株植物";
            //种植数量
            Plant_Time.text = "种地时长" + gamerData.PlantTime.ToString() + "小时";
            //登录日期
            SignIn_Day.text = "登陆日期:" + gamerData.First_SignIn_Date;
        }
        
        
    }
    public void Event_SignIn()
    {
        Motto.onEndEdit.AddListener(Motto_EndEdit);
        Name.onEndEdit.AddListener(Name_EndEdit);
        Title.onEndEdit.AddListener(Title_EndEdit);
        BirthDay.onEndEdit.AddListener(BirthDay_EndEdit);
    }
    public void Motto_EndEdit(string text)
    {
        text=Motto.text;
        if (Motto.text != "")
        {
            ObjectKeeper_Singleton.Instance.gamerData.PlayerMotto = text;
        }
    }
    public void Name_EndEdit(string text)
    {
        text = Name.text;
        if (Name.text != "")
        {
            ObjectKeeper_Singleton.Instance.gamerData.PlayerName = text;
        }
    }
    public void Title_EndEdit(string text)
    {
        text = Title.text;
        if (Title.text != "")
        {
            ObjectKeeper_Singleton.Instance.gamerData.PlayerTitle = text;
        }
    }
    public void BirthDay_EndEdit(string text)
    {
        text = BirthDay.text;
        if (BirthDay.text != "")
        {
            ObjectKeeper_Singleton.Instance.gamerData.PlayerBirthDay = text;
        }
    }
    public void Close()
    {
        CloseUI(GetComponent<CanvasGroup>());
        //当前的UI关闭时，在ObjectKeeper脚本处保存数据
        EventCenter.GetInstance().EventTrigger("SaveGamerData");
        EventCenter.GetInstance().EventTrigger("Info_Update");
        this.enabled = false;
    }
}
