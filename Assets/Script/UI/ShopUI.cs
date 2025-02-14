using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIBase
{
    [Header("����������������")]
    public List<GiftData> product_list;
    public List<Button> Gift_Buttons_Prefab;
    //[Header("���������顪������")]
    //public List<TextMeshProUGUI> TextMeshProUGUIs;
    [Header("����������ʱ������������")]
    public float x;
    public float y;
    [Header("��������������Groups��������")]
    public GameObject Top_Button_Group;
    [Header("��������������ť��������")]
    public Button Building_Button;
    public Button Cloth_Button;
    public Button Gift_Button;
    [Header("�����˳���ť����")]
    public Button Quit_Button;
    [Header("�����������ڸ�����ͼƬ�زġ�������")]
    public Sprite Start_Button_Image;
    public Sprite After_Button_Image;
    [Header("���������̵�UI���桪������")]
    public CanvasGroup Gift_UI;
    public GameObject Gift_Grade_Two_UI;
    private void Start()
    {
        InitClick();
    }
    /// <summary>
    /// ע�ᰴť�¼�
    /// </summary>
    private void InitClick()
    {
        Building_Button.onClick.AddListener(Open_Building_UI);//�̵ꡪ��������

        Cloth_Button.onClick.AddListener(Open_Cloth_UI);//�̵ꡪװ�����

        Gift_Button.onClick.AddListener(Open_Gift_UI);//�̵ꡪ�������

        Quit_Button.onClick.AddListener(Quit_ShopUI);//�˳���ť

        GiftItem_Creat();

        EventCenter.GetInstance().AddEventListener("Info_Update", Grade_Two_Info_Update);//����������������¼���ʵʱ���ºøж���ʾ�ģ���Ȼȷʵ���е㲻���۵�����������
    }

    /// <summary>
    /// �򿪽���UI
    /// </summary>
    private void Open_Building_UI()
    {
        Current_Button = Building_Button;
        Button_Image_Change(Top_Button_Group,After_Button_Image,Start_Button_Image);
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
    }

    /// <summary>
    /// ��װ�����
    /// </summary>
    private void Open_Cloth_UI()
    {
        Current_Button = Cloth_Button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
    }
    #region �̳� �������

    /// <summary>
    /// ���ݶ���̬������Ʒ��ť
    /// </summary>
    public void GiftItem_Creat()
    {
        for (int i = 0; i < product_list.Count; i++)
        {
            Button Gift_Button = Instantiate(Gift_Buttons_Prefab[i % Gift_Buttons_Prefab.Count], Gift_UI.transform);//���ɰ�ť
            Gift_Button.GetComponent<Gift>().Data = product_list[i];//����ť��ֵ
            Gift data = Gift_Button.GetComponent<Gift>();
            //���İ�ťϸ��
            Gift_Button.GetComponent<Image>().sprite = data.Data.Sprite;
            Gift_Button.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = data.Data.Name;
            Gift_Button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Data.Description;

            Gift_Button.onClick.AddListener(() => Open_GradeTwo_UI(Gift_Button));
        }
    }
    /// <summary>
    /// ���������
    /// </summary>
    private void Open_Gift_UI()
    {
        Current_Button = Gift_Button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        OpenUI(Gift_UI);
    }
    /// <summary>
    /// ���������Ķ�������
    /// </summary>
    private void Open_GradeTwo_UI(Button button)
    {
        Gift data = button.GetComponent<Gift>();
        Transform transform = button.transform;

        //�����������������еİ�ť�¼�
        Clear_All_Button();
        //�洢���������������ͼ���õ�ͼ�Ǹö�������Ļ���������
        GameObject BaseImage = Gift_Grade_Two_UI.transform.GetChild(0).gameObject;
        //Ϊ��������ע���¼�
        Grade_Two_EventSignIn(BaseImage, data);
        //��ʾ��������
        BaseImage.transform.localScale=Vector3.one;
        //ê�����������ڻ�����������λ��
        BaseImage.transform.position = new Vector2(transform.position.x-x,transform.position.y-y);
    }
    /// <summary>
    /// ע������������������¼�
    /// </summary>
    public void Grade_Two_EventSignIn(GameObject BaseImage,Gift data)
    {
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//�ݴ�������������Button���
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//�ݴ�Npc����
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            TextMeshProUGUI favorability=button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//��ȡ���������еĺøж�
            Gift giftdata = button.GetComponent<Gift>();//��ȡ��ť�ϵ���Ʒ����

            giftdata.Data = data.Data;
            button.gameObject.name = npcs[i%npcs.Length].name;//����ť������ȫ��תΪ���е�NPC������
            button.image.sprite = npcs[i % npcs.Length].GetComponentInChildren<SpriteRenderer>().sprite;//��ťImageȫ��תΪNPC��Sprite
            favorability.text= npcs[i % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//���������NPC�ĺøж�

            //ע�������¼�
            button.onClick.AddListener(() => EventCenter.GetInstance().EventTrigger("Gifted", button));
            button.onClick.AddListener(() => EventCenter.GetInstance().EventTrigger("Info_Update"));
        }
    }
    /// <summary>
    /// ����������Ϣ���·���
    /// </summary>
    public void Grade_Two_Info_Update()
    {
        GameObject BaseImage = Gift_Grade_Two_UI.transform.GetChild(0).gameObject;
        Button[] buttons = BaseImage.GetComponentsInChildren<Button>();//�ݴ�������������Button���/
        GameObject[] npcs = ObjectKeeper_Singleton.Instance.NPCs;//�ݴ�Npc����
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            TextMeshProUGUI favorability = button.transform.parent.transform.parent.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();//��ȡ���������еĺøж�

            favorability.text = npcs[i % npcs.Length].GetComponent<CharaController>().data.Favorability.ToString();//���������NPC�ĺøж�
        }
    }
    /// <summary>
    /// �����������������еİ�ť�¼�
    /// </summary>
    public void Clear_All_Button()
    {
        Button[] buttons=Gift_Grade_Two_UI.GetComponentsInChildren<Button>();
        foreach (Button button in buttons) 
        {
            button.onClick.RemoveAllListeners();
        }
    }
#endregion
    /// <summary>
    /// �˳��̵�
    /// </summary>
    private void Quit_ShopUI()
    {
        //�ر��̵�������
        CloseUI(GetComponent<CanvasGroup>());
        //�ر��̵��������
        CloseUI(Gift_UI);
        Gift_Grade_Two_UI.transform.GetChild(0).localScale = Vector3.zero;
        //����ǰ��ť�������ڴ�
        if(Current_Button!=null)
        {
            Current_Button.GetComponent<Button>().image.sprite = Start_Button_Image;
            Current_Button = null;
        }
    }


}
