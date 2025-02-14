using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUI : UIBase
{
    [Header("����������ϵͳ��������")]
    public CanvasGroup ShopUI;//�̵����
    public CanvasGroup Player_ID_Card_UI;//�����Ƭ
    public CanvasGroup NCP_Manager_UI;//�������
    public CanvasGroup Calendar_UI;//����
    public CanvasGroup Plant_Manage_UI;//ֲ�����
    public CanvasGroup System_Menue_UI;//ϵͳ�˵�
    public CanvasGroup Mail_UI;//�ʼ�
    public CanvasGroup WareHouse_UI;//�ֿ�
    public CanvasGroup DiningHall_UI;//ʳ��
    [Header("���������ײ���������������")]
    public Button Quit_Button;//�˳�
    public Button Mail_Button;//�ʼ�
    public Button Calendar_Button;//����
    public Button Hide_UI_Button;//������UI
    public Button System_Menue_Button;//ϵͳ�˵�
    [Header("�������������水ť��������")]
    public Button Shop_Button;//�̵�
    public Button NPCManager_Button;//�������
    public Button DiningHall_Button;//ʳ��
    public Button WareHouse_Button;//�ֿ�
    public Button PlantManager_Button;//ֲ�����
    public Button Timer;//��ʱ��
    public Button Player_IDCard_Button;//�����Ƭ
    [Header("�����������ϼƻ��塪������")]
    public GameObject InputField_Group;

    public GameObject Choose_Bar;

    public GameObject PlaceHolder;

    public Mission_ADay mission_ADay;
    [Header("�����������������Ϣ��������")]
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
        Shop_Button.onClick.AddListener(Open_Shop_UI);//�̵�
        NPCManager_Button.onClick.AddListener(Open_NCP_Manager_UI);//�������
        DiningHall_Button.onClick.AddListener(Open_DiningHall_UI);//ʳ��
        WareHouse_Button.onClick.AddListener(Open_WareHouse_UI);//�ֿ�
        PlantManager_Button.onClick.AddListener(Open_Plant_Manage_UI);//ֲ�����

        Player_IDCard_Button.onClick.AddListener(Open_Player_IDCard);//�������Ƭ
        Quit_Button.onClick.AddListener(QuitGame);//�˳�
        Mail_Button.onClick.AddListener(Open_Mail_UI);//�ʼ�
        Calendar_Button.onClick.AddListener(Open_Calendar_UI);//����
        Hide_UI_Button.onClick.AddListener(Hide_Main_UI);//������UI
        System_Menue_Button.onClick.AddListener(Open_System_Menue_UI);//��ϵͳ�˵�

        EventCenter.GetInstance().AddEventListener("Info_Update", Info_Update);//��Ϣ�����¼�
        Info_Update();

        Plan_Panel_Button();
    }
    /// <summary>
    /// ������UI�е������Ϣ
    /// </summary>
    public void Info_Update()
    {
        Player_Name.text = ObjectKeeper_Singleton.Instance.gamerData.PlayerName;
        Player_Money.text = ObjectKeeper_Singleton.Instance.gamerData.Money.ToString();
        Player_level.text ="Lv."+ObjectKeeper_Singleton.Instance.gamerData.Level.ToString();
    }
    #region �ƻ������
    /// <summary>
    /// Ϊ�ƻ�����Ӱ�ť�¼�-�����ı���ѡ���¼�
    /// </summary>
    private void Plan_Panel_Button()
    {
        //��ȡ��ɰ�ť
        Button[] Finish_buttons = InputField_Group.GetComponentsInChildren<Button>();
        //��ȡ�����ı���
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
    /// �����Ŀ�¼�,����ɸ���Ŀ�����䱣�浽����
    /// </summary>
    /// <param name="Option">��Ӧ����Ŀ</param>
    private void Finish_Button(ref TMP_InputField Option)
    {
        mission_ADay.Day = DateTime.Now.ToString("yyyy/M/d");//�洢��ǰ����
        if (Option!=null&&Option.text!="")//option������Ϊ�գ��Ҵ����ı�
        {
            //���ֵ��д��ڴ˼�/����ʱ��˵����ǰ�����洢��ͬһ������ݣ������ֵ�
            if (mission_ADay.ADay_Options_Dic.ContainsKey(mission_ADay.Day))
            {
                mission_ADay.Options=mission_ADay.ADay_Options_Dic[mission_ADay.Day] = mission_ADay.Options;
                mission_ADay.Options.Add(Option.text);
            }
            else//�����ڴ˼�/����ʱ����Ӵ˼�/���ڼ������Ӧ�������Ŀ�б�
            {
                mission_ADay.Options.Clear();
                mission_ADay.Options.Add(Option.text);
                mission_ADay.ADay_Options_Dic.Add(mission_ADay.Day, mission_ADay.Options);
            }
        }
        //�洢�������ݵ�����
        JsonManager.Instance.SaveData(mission_ADay, "ADay");
        Option.text = "";
    }
    /// <summary>
    /// �򿪵�ǰ�ƻ���
    /// </summary>
    /// <param name="text"></param>
    private void Open_Choosen_Bar(TMP_InputField text)
    {
        //ȷ�ϼƻ��嵱ǰ�Ƿ�򿪣����ж�������Ƿ�Ϊ��
        if(Choose_Bar.transform.localScale==Vector3.zero&&text.text!="")
        {
            //����ռλ��
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
    /// �رյ�ǰ�ƻ���
    /// </summary>
    public void Close_Choosen_Bar()
    {
        Choose_Bar.transform.localScale = Vector3.zero;

        GameObject placeHolder = InputField_Group.transform.GetChild(0).gameObject;
        Destroy(placeHolder);
    }
    #endregion �ƻ������
    /// <summary>
    /// ����UI
    /// </summary>
    public void Open_Main_UI()
    {
        var ui = GetComponent<CanvasGroup>();
        OpenUI(ui);
    }
    /// <summary>
    /// ������UI
    /// </summary>
    private void Hide_Main_UI()
    {
        var ui = GetComponent<CanvasGroup>();
        CloseUI(ui);
    }
    /// <summary>
    /// �������Ƭ
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
