using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_DinningHall : NewUIBase
{
    [Header("————食物————")]
    public FoodItem_Data Current_Food;

    public List<FoodItem_Data> Food_List;//要读表的数据列表

    public Transform Buttons_Group;//按钮生成的位置
    [Header("————素材————")]
    public List<Button> Gift_Buttons_Prefab;
    public Sprite Clicked_Sprite;
    public Sprite Stat_Sprite;
    private void Start()
    {
        string FoodInfo = ResourceManager.GetInstance().Load<TextAsset>("JsonDataAsset/FoodItem_Data").text;
        //反序列化解析为对应的数据结构
        FoodItem_Data[] foodItem_Datas = JsonMapper.ToObject<FoodItem_Data[]>(FoodInfo);
        Food_List = new List<FoodItem_Data>(foodItem_Datas);
        Food_SignIn();
    }

    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_DiningHall");
                break;
        }
    }
    /// <summary>
    /// 根据读表动态生成食品按钮
    /// </summary>
    public void Food_SignIn()
    {
        for (int i = 0; i < Food_List.Count; i++)
        {
            //创建按钮
            Button button = Instantiate(Gift_Buttons_Prefab[i % Gift_Buttons_Prefab.Count], Buttons_Group.transform);//生成按钮

            Button Buy_Button = button.transform.GetChild(3).GetComponent<Button>();
            //修改按钮细节
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Food_List[i].Description;
            button.transform.GetChild(1).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(Food_List[i].ResPath);
            button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = Food_List[i].Name;
            //赋值
            button.GetComponent<Item_Food>().Data = Food_List[i];

            button.onClick.AddListener(() => Click(button));
            Buy_Button.onClick.AddListener(() => Buy(button));
        }
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="current">当前被点击的按钮/Item</param>
    public void Click(Button parent)
    {
        Current_Button = parent;

        for (int i = 0; i < Buttons_Group.childCount; i++)
        {
            Button button = Buttons_Group.GetChild(i).GetComponent<Button>();
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
    /// 购买事件
    /// </summary>
    /// <param name="button">当前Item</param>
    public void Buy(Button button)
    {
        Current_Food = button.GetComponent<Item_Food>().Data;

        ObjectKeeper_Singleton.Instance.foodData = Current_Food;
        ObjectKeeper_Singleton.Instance.gamerData.Money += Current_Food.Cost;
        EventCenter.GetInstance().EventTrigger("Info_Update");
    }
}
