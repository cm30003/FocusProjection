using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CalendarUI : UIBase
{

    public GameObject Date_DayUI_Prefab;//����Ԥ����
    public GameObject Date_Day_Group;//���ڸ�����
    [Header("��������������ť������������")]
    public Button Last_Month_Button;//�ϸ���
    public Button Next_Month_Button;//�¸���
    public Button Last_Year_Button;//ȥ��
    public Button Next_Year_Button;//����

    public Button Quit_Button;//�˳���ť

    private int[] Big_Yue= {1,3,5,7,8,10,12};//����
    private int[] Small_Yue= {4,6,9,11};//ƽ��

    private int Current_Year;//��ǰ���
    private int Current_Month;//��ǰ�·�
    private int Current_Day;//��ǰ����

    [Header("�������������ı�������������")]
    
    public TextMeshProUGUI This_Year_Text;//��ǰ��� �ı�
    public TextMeshProUGUI This_Month_Text;//��ǰ�·� �ı�

    public TextMeshProUGUI Rightside_Date_Text;//�Ҳ����� �ı�
    public TextMeshProUGUI Rightside_FocusTime_Text;//רעʱ���ı�
    public Transform Rightside_Finished_Mission_Group;//�Ҳ������������
    public TextMeshProUGUI RighitSide_Information_Text;//�Ҳ������ı�
    [Header("����������������ء�����������")]
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
    /// ��������
    /// </summary>
    private void Update_Calendar()
    {
        string current_year = Current_Year.ToString();
        string current_month = Current_Month.ToString();
        string current_Day = Current_Day.ToString();

        This_Year_Text.text = current_year + "��";
        This_Month_Text.text = current_month + "��";

        if (Big_Yue.Contains(Current_Month))//���·�Ϊ����
        {
            for (int i = 1; i <= 31; i++)
            {
                TextMeshProUGUI Date_Daily = pool.Get();//�Ӷ�����л�ȡһ������Ԥ����
                //��������ϵĽű�
                Button Date_Button = Date_Daily.GetComponent<Button>();
                if (Date_Button.onClick.GetPersistentEventCount() == 0)
                {
                    Date_Button.onClick.AddListener(() => Date_Select(Date_Button));
                }

                Date_Daily.text = i.ToString();//�������ı�����
                Date_Daily.transform.parent = Date_Day_Group.transform;//�����丸����

                //��ѡ��ĵ�ǰ���С����ʵ�ĵ�ǰ��ݣ���֤������һ���궼�Ѿ�����������
                if(Current_Year<DateTime.Now.Year)
                {
                    //�������趨Ϊ��ɫ
                    Date_Daily.color = Color.yellow;
                }
                else if(Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//��ѡ��ĵ�ǰ��ݴ��ڵ�����ʵ���ʱ�����·ݺ����ھ���
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        else if (Small_Yue.Contains(Current_Month))//���·�Ϊƽ��
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
                    //�������趨Ϊ��ɫ
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//��ѡ��ĵ�ǰ��ݴ��ڵ�����ʵ���ʱ�����·ݺ����ھ���
                {
                    Date_Daily.color = Color.yellow;
                }
                else
                {
                    Date_Daily.color = Color.white;
                }
            }
        }
        else if (Current_Month == 2 && Current_Year % 4 == 0)//2��
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
                    //�������趨Ϊ��ɫ
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//��ѡ��ĵ�ǰ��ݴ��ڵ�����ʵ���ʱ�����·ݺ����ھ���
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
                    //�������趨Ϊ��ɫ
                    Date_Daily.color = Color.yellow;
                }
                else if (Current_Month <= DateTime.Now.Month && i <= DateTime.Now.Day)//��ѡ��ĵ�ǰ��ݴ��ڵ�����ʵ���ʱ�����·ݺ����ھ���
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
    #region ����ѡ���¼�
    /// <summary>
    /// ����ѡ���¼���ѡ������ں󣬸��Ĳ���ʾ�Ҳ���ϸ��Ϣ
    /// </summary>
    /// <param name="button">���Ӧ������</param>
    public void Date_Select(Button button)
    {
        string current_year = Current_Year.ToString();
        string current_month = Current_Month.ToString();
        string Choosen_Day=button.GetComponent<TextMeshProUGUI>().text;
        Rightside_Date_Text.text= current_year + "/" + current_month + "/" + Choosen_Day;
        Current_Button = button;

        Update_RightSide_Information();

        //��ʾѡ���
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
    /// ���������Ҳ����ϸ��Ϣ
    /// </summary>
    public void Update_RightSide_Information()
    {
        // �ӱ���Json�ļ��л�ȡ����
        Mission_ADay mission_ADay = JsonManager.Instance.LoadData<Mission_ADay>("ADay");
        Update_Rightside_FocusTime(mission_ADay);
        // ��Ϊ�գ������Ҳ����ϸ��Ϣ
        if (mission_ADay != null)
        {
            //��ÿ�θ�����Ϣ��ʱ����ɾ��֮ǰ����Ϣ
            for (int i = 0; i < Rightside_Finished_Mission_Group.childCount; i++)
            {
                Destroy(Rightside_Finished_Mission_Group.GetChild(i).gameObject);
            }
            // �ж������Ƿ�ƥ�䣬��ƥ�䣬˵��������������Ӧ�洢����
            if (mission_ADay.ADay_Options_Dic.ContainsKey(Rightside_Date_Text.text))
            {
                //���±����ļ��д洢��
                for (int i=0;i< mission_ADay.ADay_Options_Dic[Rightside_Date_Text.text].Count;i++)
                {
                    UpdateRightSideText(mission_ADay.ADay_Options_Dic[Rightside_Date_Text.text][i]);
                }
            }
            else
            {
                Debug.Log("δƥ��");
                UpdateRightSideText("��");
            }
        }
        else
        {
            UpdateRightSideText("��");
        }
    }
    /// <summary>
    /// ���������Ҳ���ϸ��Ϣ�ı���Ԥ����
    /// </summary>
    /// <param name="text">��Ӧ�ı�</param>
    private void UpdateRightSideText(string text)
    {
        // ʵ����TextMeshProUGUI
        TextMeshProUGUI rightside_Information_text = Instantiate(RighitSide_Information_Text, Rightside_Finished_Mission_Group);
        rightside_Information_text.text = text;
    }
    private void Update_Rightside_FocusTime(Mission_ADay mission_ADay)
    {
        if(mission_ADay.ADay_FocusTime_Dic.ContainsKey(Rightside_Date_Text.text))
        {
            int Minute = mission_ADay.ADay_FocusTime_Dic[Rightside_Date_Text.text] % 3600 / 60;
            Rightside_FocusTime_Text.text = Minute.ToString()+"����";
        }
        else
        {
            Rightside_FocusTime_Text.text = "0����";
        }
    }
    #endregion
    /// <summary>
    /// ���� ���� �����ɵ������ͷŻض������
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
    /// �˳�����
    /// </summary>
    private void Quit_CalendarUI()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        CloseUI(canvasGroup);
    }
}
