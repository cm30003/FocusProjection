using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_PlantManager : NewUIBase
{
    [Header("————列表————")]

    public List<PlantData> PlantItem_List;

    public List<GameObject> PlantItem_Prefab_List;
    [Header("————组————")]
    public Transform PlantItem_Group;
    [Header("————素材————")]
    public Sprite Clicked_Sprite;
    public Sprite Stat_Sprite;

    private void Start()
    {
        PlantItem_Creat();
    }
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_PlantManager");
                break;
        }
    }
    /// <summary>
    /// 动态生成植物按钮
    /// </summary>
    public void PlantItem_Creat()
    {
        for (int i = 0; i < PlantItem_List.Count; i++)
        {
            GameObject Plant_Button = Instantiate(PlantItem_Prefab_List[i % PlantItem_Prefab_List.Count], PlantItem_Group);//生成按钮
            Plant_Button.GetComponent<Plant>().Data = PlantItem_List[i];//给按钮赋值
            Plant data = Plant_Button.GetComponent<Plant>();

            Button_SignIn(Plant_Button);
            //更改按钮细节
            Plant_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Data.Description;
            Plant_Button.transform.GetChild(1).GetComponent<Image>().sprite = data.Data.Sprite;
            Plant_Button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = data.Data.Name;
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
        Button Buy_Button = button.transform.GetChild(3).GetComponent<Button>();

        Buy_Button.onClick.AddListener(() => Field(button));
        button.onClick.AddListener(() => Click(button));
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="current">当前被点击的按钮/Item</param>
    public void Click(Button parent)
    {
        Current_Button = parent;

        for (int i = 0; i < PlantItem_Group.childCount; i++)
        {
            Button button = PlantItem_Group.GetChild(i).GetComponent<Button>();
            Button Buy_Button = button.transform.GetChild(3).GetComponent<Button>();
            if (button == Current_Button)
            {
                Buy_Button.transform.localScale = Vector3.one;
                button.GetComponent<Image>().sprite = Clicked_Sprite;
            }
            else
            {
                Buy_Button.transform.localScale = Vector3.zero;
                button.GetComponent<Image>().sprite = Stat_Sprite;
            }
        }
    }
    /// <summary>
    ///田地/种植事件
    /// </summary>
    public void Field(Button button)
    {
        //暂存所有田地
        GameObject[] fileds = ObjectKeeper_Singleton.Instance.Farm_Field;
        //更新数据
        ObjectKeeper_Singleton.Instance.gamerData.Money += button.GetComponent<Plant>().Data.Money_Cost_reward;
        EventCenter.GetInstance().EventTrigger("Info_Update");
        //更新田地状态
        for (int i = 0; i < fileds.Length; i++)
        {
            //
            GameObject field = fileds[i];
            plant_State plant_State = field.GetComponent<plant_State>();
            if (plant_State.State == Plant_State.Empty)
            {
                plant_State.State = Plant_State.plant;
                Plant(field, button);
                break;
            }
        }
    }
    /// <summary>
    /// 植物 种植事件
    /// </summary>
    /// <param name="field">田地</param>
    /// <param name="button">植物管理中被点击的按钮</param>
    public void Plant(GameObject field, Button button)
    {
        for (int i = 0; i < field.transform.childCount; i++)
        {
            field.GetComponent<plant_State>().data = button.GetComponent<Plant>().Data;
            field.GetComponent<plant_State>().Temple_Data = new PlantData(button.GetComponent<Plant>().Data);
        }
    }
}
