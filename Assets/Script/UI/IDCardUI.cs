using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDCardUI : UIBase
{
    //��δ�����һ���˭��
    //������Ƭ�Ĺر��¼����ڸ�����Ƭ�ĺڵ��ϣ���������
    [Header("������������Զ������벿�֡�������")]
    public TMP_InputField Motto;//������
    public TMP_InputField Name;
    public TMP_InputField Title;//�ƺ�
    public TMP_InputField BirthDay;
    [Header("���������ı���������")]
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
            //������
            Motto.text = gamerData.PlayerMotto;
            //�����
            Name.text = gamerData.PlayerName;
            //�ƺ�
            Title.text = gamerData.PlayerTitle;
            //����
            BirthDay.text = gamerData.PlayerBirthDay;

            //�ջ�����
            Harvest_Num.text = "�ջ�" + gamerData.HarvestNum.ToString() + "��ֲ��";
            //��ֲ����
            Plant_Time.text = "�ֵ�ʱ��" + gamerData.PlantTime.ToString() + "Сʱ";
            //��¼����
            SignIn_Day.text = "��½����:" + gamerData.First_SignIn_Date;
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
        //��ǰ��UI�ر�ʱ����ObjectKeeper�ű�����������
        EventCenter.GetInstance().EventTrigger("SaveGamerData");
        EventCenter.GetInstance().EventTrigger("Info_Update");
        this.enabled = false;
    }
}
