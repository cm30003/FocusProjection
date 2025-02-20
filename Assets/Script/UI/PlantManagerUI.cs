using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantManagerUI : UIBase
{
    [Header("���������б�������")]

    public List<PlantData> PlantItem_List;

    public List<GameObject> PlantItem_Prefab_List;
    [Header("���������顪������")]
    public GameObject PlantItem_Group;

    [Header("����������ť��������")]
    public Button Quit_Button;

    private void Start()
    {
        InitClick();
    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_UI);

        PlantItem_Creat();
    }
    /// <summary>
    /// ��̬����ֲ�ﰴť
    /// </summary>
    public void PlantItem_Creat()
    {
        for(int i=0;i<PlantItem_List.Count;i++)
        {
            GameObject Plant_Button=Instantiate(PlantItem_Prefab_List[i% PlantItem_Prefab_List.Count],PlantItem_Group.transform);//���ɰ�ť
            Plant_Button.GetComponent<Plant>().Data = PlantItem_List[i];//����ť��ֵ
            Button_SignIn(Plant_Button);
        }
    }
    /// <summary>
    /// ע�ᰴť�����ֲ�¼�
    /// </summary>
    public void Button_SignIn(GameObject Plant_Button)
    {
        //�ݴ水ť
        Button button = Plant_Button.GetComponent<Button>();
        PlantData plant_Data = button.GetComponent<Plant>().Data;
        //
        button.GetComponent<Image>().sprite = plant_Data.Sprite;
        button.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = plant_Data.Name;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = plant_Data.Description;

        button.onClick.AddListener(() => Field(button));
    }
    /// <summary>
    ///���/��ֲ�¼�
    /// </summary>
    public void Field(Button button)
    {
        //�ݴ��������
        GameObject[] fileds= ObjectKeeper_Singleton.Instance.Farm_Field;
        //��������
        ObjectKeeper_Singleton.Instance.gamerData.Money+=button.GetComponent<Plant>().Data.Money_Cost_reward;
        EventCenter.GetInstance().EventTrigger("Info_Update");
        //�������״̬
        for(int i=0;i<fileds.Length;i++)
        {
            //
            GameObject field= fileds[i];
            plant_State plant_State = field.GetComponent<plant_State>();
            if (plant_State.State== Plant_State.Empty)
            {
                plant_State.State = Plant_State.plant;
                Plant(field,button);
                break;
            }
        }
    }
    /// <summary>
    /// ֲ�� ��ֲ�¼�
    /// </summary>
    /// <param name="field">���</param>
    /// <param name="button">ֲ������б�����İ�ť</param>
    public void Plant(GameObject field,Button button)
    {
        for(int i=0; i< field.transform.childCount; i++)
        {
            field.GetComponent<plant_State>().data= button.GetComponent<Plant>().Data;
            field.GetComponent<plant_State>().Temple_Data = new PlantData(button.GetComponent<Plant>().Data);
        }
    }
    private void Quit_UI()
    {
        CloseUI(GetComponent<CanvasGroup>());
    }
}
