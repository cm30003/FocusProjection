using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailUI : UIBase
{
    public GameObject Receive_UI;
    public GameObject Write_UI;

    private GameObject Current_UI;
    
    [Header("��������Buttons��������")]
    public Button Receive_Button;
    public Button Write_Button;

    public Button Quit_Button;
    [Header("��������Button_Group��������")]
    public GameObject Top_Button_Group;
    [Header("��������Sprite_For_Change��������")]
    public Sprite Button_Image_Start;
    public Sprite Button_Image_Choosen;
    private void Start()
    {
        InitClick();
    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_MailUI);
        Receive_Button.onClick.AddListener(Open_Receive_UI);
        Write_Button.onClick.AddListener(Open_Write_UI);
    }
    /// <summary>
    /// ���ʼ�ҳ��Ĵμ����桪���ռ�
    /// </summary>
    private void Open_Receive_UI()
    {
        Receive_UI.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        Write_UI.transform.localScale = Vector3.zero;

        Current_UI = Receive_UI;
        Current_Button = Receive_Button;

        Button_Image_Change(Top_Button_Group, Button_Image_Choosen, Button_Image_Start);

        Button quit_button= Receive_UI.transform.GetChild(1).GetComponent<Button>();
        quit_button.onClick.AddListener(Close_Mail_Main_UI);
    }
    /// <summary>
    /// ���ʼ�ҳ��Ĵμ����桪��д�ʼ�
    /// </summary>
    private void Open_Write_UI()
    {
        Write_UI.transform.localScale = new Vector3(0.75f,0.75f,0.75f);

        Receive_UI.transform.localScale = Vector3.zero;

        Current_UI = Write_UI;
        Current_Button = Write_Button;

        Button_Image_Change(Top_Button_Group, Button_Image_Choosen, Button_Image_Start);

        Button quit_button= Write_UI.transform.GetChild(1).GetComponent<Button>();
        quit_button.onClick.AddListener(Close_Mail_Main_UI);
    }
    /// <summary>
    /// �ر��ʼ�����Ĵ�һ������
    /// </summary>
    private void Close_Mail_Main_UI()
    {
        Current_UI.transform.localScale = Vector3.zero;
        Current_UI = null;
    }
    private void Quit_MailUI()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        CloseUI(canvasGroup);
    }
}
