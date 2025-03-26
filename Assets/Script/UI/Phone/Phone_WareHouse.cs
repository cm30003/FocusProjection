using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_WareHouse : NewUIBase
{
    public Transform Object_Group;

    public List<GameObject> Items_Button_Prefab;

    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_WareHouse");
                break;
        }
    }
    public void Update_Evnet()
    {
        Clear_ObjectGroup();
        Item_Sign_In();
    }
    /// <summary>
    /// 物品按钮注册并生成点击事件
    /// </summary>
    public void Item_Sign_In()
    {
        if (ObjectKeeper_Singleton.Instance.gamerData.Items != null && ObjectKeeper_Singleton.Instance.gamerData.Items.Count > 0)
        {
            for (int i = 0; i < ObjectKeeper_Singleton.Instance.gamerData.Items.Count; i++)
            {
                //创建Item
                GameObject Item = Instantiate(Items_Button_Prefab[i % Items_Button_Prefab.Count], Object_Group);
                //为Item赋值
                Item.GetComponent<Plant>().Data = ObjectKeeper_Singleton.Instance.gamerData.Items[i];
                //获取卖出按钮
                Button Sell_Button = Item.transform.GetChild(2).GetComponent<Button>();
                //创建Item信息
                Item.transform.GetChild(0).GetComponent<Image>().sprite = ObjectKeeper_Singleton.Instance.gamerData.Items[i].Sprite;
                Item.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = ObjectKeeper_Singleton.Instance.gamerData.Items[i].Description;
                //创建卖出事件
                Sell_Button.onClick.AddListener(() => Sell(Item));
            }
        }
    }
    /// <summary>
    /// 清除显示物体，方便重新生成
    /// </summary>
    public void Clear_ObjectGroup()
    {
        for (int i = 0; i < Object_Group.transform.childCount; i++)
        {
            Destroy(Object_Group.transform.GetChild(i).gameObject);
        }
    }
    #region 弃用
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="current">当前被点击的按钮/Item</param>
    //public void Click(Button current)
    //{
    //    Current_Button = current;
    //    for (int i = 0; i < Object_Group.transform.childCount; i++)
    //    {
    //        Button button = Object_Group.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>();
    //        if (button == Current_Button)
    //        {

    //            button.transform.localScale = Vector3.one;
    //            button.GetComponent<Image>().sprite = Choosen_Image;
    //        }
    //        else
    //        {
    //            button.transform.localScale = Vector3.zero;
    //            button.GetComponent<Image>().sprite = Start_Image;
    //        }
    //    }
    //}
    #endregion
    /// <summary>
    /// 卖出事件
    /// </summary>
    /// <param name="Item">挂载数据的Item</param>
    public void Sell(GameObject Item)
    {
        PlantData plantData = Item.GetComponent<Plant>().Data;
        foreach (PlantData item in ObjectKeeper_Singleton.Instance.gamerData.Items)
        {
            if (item == plantData && item.Num > 0)
            {
                item.Num--;
                ObjectKeeper_Singleton.Instance.gamerData.Money++;
            }
        }

        EventCenter.GetInstance().EventTrigger("Info_Update");//画面左上角玩家信息更新信息
    }
    #region 弃用
    /// <summary>
    /// 检查列表是否存在存入物品
    /// </summary>
    //public void CheckList(GameObject gameObject)
    //{
    //    PlantData plantData=gameObject.GetComponent<PlantData>();
    //    //如果列表中存在该Item，则更新其数量
    //    if(ObjectKeeper_Singleton.Instance.gamerData.Items.Contains(plantData))
    //    {
    //        int Index= ObjectKeeper_Singleton.Instance.gamerData.Items.IndexOf(plantData);
    //        ObjectKeeper_Singleton.Instance.gamerData.Items[Index].Num++;
    //    }
    //    //若不存在该Item，则添加该Item
    //    else
    //    {
    //        ObjectKeeper_Singleton.Instance.gamerData.Items.Add(plantData);
    //        //在库中 生成一个Item按钮
    //        Button button = Instantiate(Item_Button, Object_Group.transform).GetComponent<Button>();
    //        button.onClick.AddListener(Quit_UI);
    //    }
    //}
    #endregion
}
