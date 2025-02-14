using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class NPCManagerUI : UIBase
{
    [Header("————预制体————")]
    public GameObject NPCUI_Prefab;
    [Header("————按钮————")]
    public Button Quit_Button;
    [Header("————组————")]
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
    /// 生成NPC管理器中所有NPC的UI
    /// </summary>
    public void NPCUI_Generate()
    {
        if(NPC_Group.transform.childCount==0)
        {
            GameObject[] npc=ObjectKeeper_Singleton.Instance.NPCs;
            
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.NPCs.Length; i++)
            {
                GameObject gameObject = Instantiate(NPCUI_Prefab, NPC_Group.transform);
                //给每个NPC赋值
                gameObject.name = npc[i].name;
                gameObject.GetComponent<Image>().sprite = npc[i].GetComponentInChildren<SpriteRenderer>().sprite;
                gameObject.transform.Find("NPC_Description_Bar/Name").GetComponent<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Name;
                gameObject.transform.Find("NPC_Description_Bar/personality").GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Personality;
                gameObject.transform.Find("NPC_Description_Bar/hobby").GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Hobby;
                gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Favorability.ToString();//好感度
            }
        }
    }
    /// <summary>
    /// 注册NPC管理器中所有NPC的点击事件
    /// </summary>
    public void NPC_SignIn()
    {
        //遍历NPC_Group下的所有子物体，注册点击事件
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
            NPC.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = npc[i].GetComponent<CharaController>().data.Favorability.ToString();//好感度
        }
    }
    /// <summary>
    /// 唤出NPC指令，可选择对应NPC并对其发送强制指令
    /// </summary>
    /// <param name="currentbutton">当前被选择的NPC</param>
    public void NPC_Order(Button currentbutton)
    {
        //获取当前按钮，唤醒当前按钮的Order菜单
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
                //唤醒当前按钮的Order菜单
                NPC_Order.transform.localScale = Vector3.one;
                //注册order菜单的按钮点击事件
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
    /// 注册NPC命令菜单的所有点击事件
    /// </summary>
    /// <param name="button">NPC命令菜单的子按钮</param>
    /// <param name="action_Name">事件名称</param>
    public void NPC_Order_Second(Button button,string action_Name)
    {
        if(button.onClick.GetPersistentEventCount()==0)
        {
            button.onClick.AddListener(()=> EventCenter.GetInstance().EventTrigger(action_Name, button));
        }
    }
    /// <summary>
    /// 退出事件
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
