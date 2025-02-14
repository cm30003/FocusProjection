using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class NPCManagerUI : UIBase
{
    [Header("��������Ԥ���塪������")]
    public GameObject NPCUI_Prefab;
    [Header("����������ť��������")]
    public Button Quit_Button;
    [Header("���������顪������")]
    public GameObject NPC_Group;
    private void Start()
    {
        InitClick();

    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_UI);
        NPCUI_Generate();
        NPC_SignIn();
        EventCenter.GetInstance().AddEventListener("Info_Update", Update_Info);
    }
    /// <summary>
    /// ����NPC������������NPC��UI
    /// </summary>
    public void NPCUI_Generate()
    {
        if(NPC_Group.transform.childCount==0)
        {
            GameObject[] npc=ObjectKeeper_Singleton.Instance.NPCs;
            
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.NPCs.Length; i++)
            {
                GameObject gameObject = Instantiate(NPCUI_Prefab, NPC_Group.transform);
                //��ÿ��NPC��ֵ
                gameObject.name = npc[i].name;
                gameObject.GetComponent<Image>().sprite = npc[i].GetComponentInChildren<SpriteRenderer>().sprite;
                gameObject.transform.Find("NPC_Description_Bar/Name").GetComponent<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Name;
                gameObject.transform.Find("NPC_Description_Bar/personality").GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Personality;
                gameObject.transform.Find("NPC_Description_Bar/hobby").GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Hobby;
                gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Favorability.ToString();//�øж�
            }
        }
    }
    /// <summary>
    /// ע��NPC������������NPC�ĵ���¼�
    /// </summary>
    public void NPC_SignIn()
    {
        //����NPC_Group�µ����������壬ע�����¼�
        for(int i=0;i<NPC_Group.transform.childCount;i++)
        {
            GameObject NPC = NPC_Group.transform.GetChild(i).gameObject;
            
            Button button = NPC.GetComponent<Button>();
            //print(button.gameObject.name);
            button.onClick.AddListener(() =>NPC_Order(button) );
        }
    }
    public void Update_Info()
    {
        GameObject[] npc = ObjectKeeper_Singleton.Instance.NPCs;
        for (int i=0;i<NPC_Group.transform.childCount;i++)
        {
            GameObject NPC = NPC_Group.transform.GetChild(i).gameObject;
            NPC.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Favorability.ToString();//�øж�
        }
    }
    /// <summary>
    /// ����NPCָ���ѡ���ӦNPC�����䷢��ǿ��ָ��
    /// </summary>
    /// <param name="currentbutton">��ǰ��ѡ���NPC</param>
    public void NPC_Order(Button currentbutton)
    {
        //��ȡ��ǰ��ť�����ѵ�ǰ��ť��Order�˵�
        Current_Button = currentbutton;
        for(int i = 0; i < NPC_Group.transform.childCount; i++)
        {
            //
            Button button = NPC_Group.transform.GetChild(i).GetComponent<Button>();
            //print(NPC_Group.transform.childCount);
            //print(button.gameObject.name);
            GameObject NPC_Order = button.transform.GetChild(2).gameObject;
            GameObject Button_Group= NPC_Order.transform.GetChild(1).gameObject;
            if (button==Current_Button)
            {
                //���ѵ�ǰ��ť��Order�˵�
                NPC_Order.transform.localScale = Vector3.one;
                //ע��order�˵��İ�ť����¼�
                NPC_Order_Second(Button_Group.transform.GetChild(0).GetComponent<Button>(), "NPC_Work");
                NPC_Order_Second(Button_Group.transform.GetChild(1).GetComponent<Button>(), "NPC_Rest");
                NPC_Order_Second(Button_Group.transform.GetChild(2).GetComponent<Button>(), "NPC_Eat");
            }
            else
            {
                NPC_Order.transform.localScale = Vector3.zero;
            }
        }
    }
    /// <summary>
    /// ע��NPC����˵������е���¼�
    /// </summary>
    /// <param name="button">NPC����˵����Ӱ�ť</param>
    /// <param name="action_Name">�¼�����</param>
    public void NPC_Order_Second(Button button,string action_Name)
    {
        if(button.onClick.GetPersistentEventCount()==0)
        {
            button.onClick.AddListener(()=> EventCenter.GetInstance().EventTrigger(action_Name, button));
        }
    }
    /// <summary>
    /// �˳��¼�
    /// </summary>
    private void Quit_UI()
    {
        CloseUI(GetComponent<CanvasGroup>());
        if(Current_Button!=null)
        {
            Current_Button.transform.GetChild(2).transform.localScale = Vector3.zero;
        }
    }
}
