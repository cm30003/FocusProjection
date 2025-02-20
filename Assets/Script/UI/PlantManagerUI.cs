using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantManagerUI : UIBase
{
    [Header("————列表————")]

    public List<PlantData> PlantItem_List;

    public List<GameObject> PlantItem_Prefab_List;
    [Header("————组————")]
    public GameObject PlantItem_Group;

    [Header("————按钮————")]
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
    /// 动态生成植物按钮
    /// </summary>
    public void PlantItem_Creat()
    {
        for(int i=0;i<PlantItem_List.Count;i++)
        {
            GameObject Plant_Button=Instantiate(PlantItem_Prefab_List[i% PlantItem_Prefab_List.Count],PlantItem_Group.transform);//生成按钮
            Plant_Button.GetComponent<Plant>().Data = PlantItem_List[i];//给按钮赋值
            Button_SignIn(Plant_Button);
        }
    }
    /// <summary>
    /// 注册按钮点击种植事件
    /// </summary>
    public void Button_SignIn(GameObject Plant_Button)
    {
        //暂存按钮
        Button button = Plant_Button.GetComponent<Button>();
        PlantData plant_Data = button.GetComponent<Plant>().Data;
        //
        button.GetComponent<Image>().sprite = plant_Data.Sprite;
        button.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = plant_Data.Name;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = plant_Data.Description;

        button.onClick.AddListener(() => Field(button));
    }
    /// <summary>
    ///田地/种植事件
    /// </summary>
    public void Field(Button button)
    {
        //暂存所有田地
        GameObject[] fileds= ObjectKeeper_Singleton.Instance.Farm_Field;
        //更新数据
        ObjectKeeper_Singleton.Instance.gamerData.Money+=button.GetComponent<Plant>().Data.Money_Cost_reward;
        EventCenter.GetInstance().EventTrigger("Info_Update");
        //更新田地状态
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
    /// 植物 种植事件
    /// </summary>
    /// <param name="field">田地</param>
    /// <param name="button">植物管理中被点击的按钮</param>
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
