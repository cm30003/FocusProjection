using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_NPCManager : NewUIBase
{
    [Header("————信息组————")]
    public GameObject Info_Group;
    [Header("————等待组————")]
    public GameObject Waitings_Group;

    public GameObject Npc_Waiting_Prefab;
    private void Start()
    {
        Creat_Waitings();
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
    public void Creat_Waitings()
    {
        GameObject[] npc = ObjectKeeper_Singleton.Instance.NPCs;

        for (int i = 0; i < ObjectKeeper_Singleton.Instance.NPCs.Length; i++)
        {
            GameObject waiting = Waitings_Group.transform.GetChild(i).gameObject;
            //创建Image组件并置于Waiting下方
            GameObject child = Instantiate(Npc_Waiting_Prefab,waiting.transform);
            
            Image image = child.GetComponent<Image>();

            child.transform.localPosition = new Vector3(-5,-13,0);
            child.transform.localScale = new Vector3(0.1f,0.1f,0.1f);

            //设置waiting内容
            Button button= waiting.AddComponent<Button>();
            button.onClick.AddListener(() => UpdateInfo(button));

            waiting.AddComponent<NPCData_Data>();
            waiting.GetComponent<NPCData_Data>().data= npc[i].GetComponent<CharaController>().Template_data;
            image.sprite = waiting.GetComponent<NPCData_Data>().data.sprite;
            image.SetNativeSize();
        }
    }
    public void UpdateInfo(Button button)
    {
        print("111111");
        Info_Group.transform.GetChild(0).GetComponent<Image>().sprite= button.GetComponent<NPCData_Data>().data.sprite;
        Info_Group.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text= button.GetComponent<NPCData_Data>().data.Name;
        Info_Group.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text= button.GetComponent<NPCData_Data>().data.Description;

    }
}
