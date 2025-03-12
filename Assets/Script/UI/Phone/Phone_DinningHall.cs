using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_DinningHall : NewUIBase
{
    [Header("————食物————")]
    public FoodData Current_Food;

    public List<FoodData> Food_List;//要读表的数据列表

    public Transform Buttons_Group;//按钮生成的位置
    [Header("————素材————")]
    public List<Button> Gift_Buttons_Prefab;
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
            button.GetComponent<Item_Food>().Data = Food_List[i];//给按钮赋值

            Button Buy_Button = button.transform.GetChild(3).GetComponent<Button>();

            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Food_List[i].Description;
            button.transform.GetChild(1).GetComponent<Image>().sprite = Food_List[i].Sprite;
            button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = Food_List[i].Name;
            button.GetComponent<Item_Food>().Data = Food_List[i];
            //button.onClick.AddListener(() => Click(button));
            //Buy_Button.onClick.AddListener(() => Buy(button));
        }
    }
    ///// <summary>
    ///// 点击事件
    ///// </summary>
    ///// <param name="current">当前被点击的按钮/Item</param>
    //public void Click(Button parent)
    //{
    //    Current_Button = parent;
    //    for (int i = 0; i < Buttons_Group.childCount; i++)
    //    {
    //        Button button = Buttons_Group.GetChild(i).GetComponent<Button>();
    //        Button Buy_Button = button.transform.GetChild(0).GetComponent<Button>();
    //        if (button == Current_Button)
    //        {
    //            Buy_Button.transform.localScale = Vector3.one;
    //            button.GetComponent<Image>().sprite = Clicked_Sprite;
    //        }
    //        else
    //        {
    //            Buy_Button.transform.localScale = Vector3.zero;
    //            button.GetComponent<Image>().sprite = Stat_Sprite;
    //        }
    //    }
    //}
    ///// <summary>
    ///// 购买事件
    ///// </summary>
    ///// <param name="button">当前Item</param>
    //public void Buy(Button button)
    //{
    //    Current_Food = button.GetComponent<food>().Data;

    //    ObjectKeeper_Singleton.Instance.foodData = Current_Food;
    //    ObjectKeeper_Singleton.Instance.gamerData.Money += Current_Food.Money_Cost_reward;
    //    EventCenter.GetInstance().EventTrigger("Info_Update");
    //}
    ///// <summary>
    ///// 退出UI
    ///// </summary>
    //private void Quit_DiningHall()
    //{
    //    CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
    //    CloseUI(canvasGroup);
    //    if (Current_Button != null)
    //    {
    //        Current_Button.transform.GetChild(0).localScale = Vector3.zero;
    //        Current_Button.GetComponent<Image>().sprite = Stat_Sprite;
    //        Current_Button = null;
    //    }

    //}
}
