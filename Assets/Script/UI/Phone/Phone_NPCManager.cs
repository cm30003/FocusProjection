using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Phone_NPCManager : NewUIBase
{
    [Header("————信息组————")]
    public GameObject Info_Group;
    [Header("————等待组————")]
    public GameObject Waitings_Group;
    public GameObject Choose_Group;

    public GameObject Npc_Waiting_Prefab;
    private void Start()
    {
        Creat_Waitings();
        Create_Choose();
    }
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_NPCManager");
                break;
        }
    }
    public void Create_Choose()
    {
        List<GameObject> Worknpc = ObjectKeeper_Singleton.Instance.Work_NPCs;

        for(int i = 0; i < Worknpc.Count; i++)
        {
            GameObject Choosen = Choose_Group.transform.GetChild(i).gameObject;
            //创建Image组件并置于Waiting下方
            GameObject child = Instantiate(Npc_Waiting_Prefab, Choosen.transform);

            Image image = child.GetComponent<Image>();

            child.transform.localPosition = new Vector3(-5, -25, 0);
            child.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            //设置waiting内容
            Button button = Choosen.AddComponent<Button>();
            button.onClick.AddListener(() => UpdateInfo(button));
            //为每个waiting添加NPCData_Data组件/添加其NPC数据
            child.AddComponent<NPCData_Data>();
            child.GetComponent<NPCData_Data>().data = Worknpc[i].GetComponent<CharaController>().Template_data;
            child.GetComponent<NPCData_Data>().npcInformation = Worknpc[i].GetComponent<CharaController>().npc_Information;
            image.sprite = ResourceManager.GetInstance().Load<Sprite>(child.GetComponent<NPCData_Data>().data.Sprite_Res);
            image.SetNativeSize();
        }
    }
    /// <summary>
    /// 创建等待组的内容
    /// </summary>
    public void Creat_Waitings()
    {
        GameObject[] npc = ObjectKeeper_Singleton.Instance.NPCs;

        for (int i = 0; i < ObjectKeeper_Singleton.Instance.Waiting_NPCs.Count; i++)
        {
            GameObject waiting = Waitings_Group.transform.GetChild(i).gameObject;
            //创建Image组件并置于Waiting下方
            GameObject child = Instantiate(Npc_Waiting_Prefab,waiting.transform);
            
            Image image = child.GetComponent<Image>();

            child.transform.localPosition = new Vector3(-5,-13,0);
            child.transform.localScale = new Vector3(0.25f,0.25f,0.25f);

            //设置waiting内容
            Button button= waiting.AddComponent<Button>();
            button.onClick.AddListener(() => UpdateInfo(button));
            //为每个waiting添加NPCData_Data组件/添加其NPC数据
            child.AddComponent<NPCData_Data>();
            child.GetComponent<NPCData_Data>().data= npc[i].GetComponent<CharaController>().Template_data;
            child.GetComponent<NPCData_Data>().npcInformation= npc[i].GetComponent<CharaController>().npc_Information;
            image.sprite = ResourceManager.GetInstance().Load<Sprite>(child.GetComponent<NPCData_Data>().data.Sprite_Res) ;
            image.SetNativeSize();
        }
    }
    public void UpdateInfo(Button button)
    {
        Info_Group.transform.GetChild(0).GetComponent<Image>().sprite=ResourceManager.GetInstance().Load<Sprite>(button.GetComponentInChildren<NPCData_Data>().data.Sprite_Res) ;
        Info_Group.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text= button.GetComponentInChildren<NPCData_Data>().npcInformation.Name;
        Info_Group.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text= button.GetComponentInChildren<NPCData_Data>().npcInformation.Description;

    }
}
