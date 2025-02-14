using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseUI : UIBase
{
    public GameObject Object_Group;
    [Header("��������Button��������")]
    public GameObject Item_Button;
    public Button Quit_Button;
    [Header("��������Sprite��������")]
    public Sprite Start_Image;
    public Sprite Choosen_Image;
    
    private void Start()
    {
        InitClick();


        EventCenter.GetInstance().AddEventListener("ItemList_Update",Update_Evnet);
    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_UI);//�˳�

        //Item_Sign_In();
    }

    public void Update_Evnet()
    {
        Clear_ObjectGroup();
        Item_Sign_In();
    }
    /// <summary>
    /// ��Ʒ��ťע�Ტ���ɵ���¼�
    /// </summary>
    public void Item_Sign_In()
    {
        if(ObjectKeeper_Singleton.Instance.gamerData.Items != null && ObjectKeeper_Singleton.Instance.gamerData.Items.Count > 0)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.gamerData.Items.Count; i++)
            {
                Button button = Instantiate(Item_Button, Object_Group.transform).GetComponent<Button>();
                Button Sell_Button = button.transform.GetChild(0).GetComponent<Button>();
                button.transform.GetChild(1).GetComponent<Image>().sprite = ObjectKeeper_Singleton.Instance.gamerData.Items[i].Sprite;
                button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ObjectKeeper_Singleton.Instance.gamerData.Items[i].Num.ToString();
                button.onClick.AddListener(() => Click(Sell_Button));
                Sell_Button.onClick.AddListener(() => Sell(button));
            }
        }

    }
    public void Clear_ObjectGroup()
    {
        for (int i = 0; i < Object_Group.transform.childCount; i++)
        {
            Destroy(Object_Group.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="current">��ǰ������İ�ť/Item</param>
    public void Click(Button current)
    {
        Current_Button = current;
        for (int i = 0;i < Object_Group.transform.childCount;i++)
        {
            Button button= Object_Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>();
            if(button==Current_Button)
            {

                button.transform.localScale = Vector3.one;
                button.GetComponent<Image>().sprite = Choosen_Image;
            }
            else
            {
                button.transform.localScale = Vector3.zero;
                button.GetComponent<Image>().sprite = Start_Image;
            }
        }
    }
    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="button"></param>
    public void Sell(Button button)
    {
        PlantData plantData=button.GetComponent<Plant>().Data;
        foreach(PlantData item in ObjectKeeper_Singleton.Instance.gamerData.Items)
        {
            if(item==plantData&&item.Num>0)
            {
                item.Num--;
                ObjectKeeper_Singleton.Instance.gamerData.Money++;
            }
        }
        
        EventCenter.GetInstance().EventTrigger("Info_Update");//�������Ͻ������Ϣ������Ϣ
    }
    /// <summary>
    /// ����б��Ƿ���ڴ�����Ʒ
    /// </summary>
    //public void CheckList(GameObject gameObject)
    //{
    //    PlantData plantData=gameObject.GetComponent<PlantData>();
    //    //����б��д��ڸ�Item�������������
    //    if(ObjectKeeper_Singleton.Instance.gamerData.Items.Contains(plantData))
    //    {
    //        int Index= ObjectKeeper_Singleton.Instance.gamerData.Items.IndexOf(plantData);
    //        ObjectKeeper_Singleton.Instance.gamerData.Items[Index].Num++;
    //    }
    //    //�������ڸ�Item������Ӹ�Item
    //    else
    //    {
    //        ObjectKeeper_Singleton.Instance.gamerData.Items.Add(plantData);
    //        //�ڿ��� ����һ��Item��ť
    //        Button button = Instantiate(Item_Button, Object_Group.transform).GetComponent<Button>();
    //        button.onClick.AddListener(Quit_UI);
    //    }
    //}
    private void Quit_UI()
    {
        CloseUI(GetComponent<CanvasGroup>());

        if(Current_Button!=null)
        {
            Current_Button.transform.localScale = Vector3.zero;
            Current_Button.GetComponent<Image>().sprite = Start_Image;
            Current_Button = null;
        }
        
    }
}
