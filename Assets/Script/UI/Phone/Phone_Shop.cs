using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Shop : NewUIBase
{
    public List<Image> Gift_Image_Prefab;
    public List<Image> Cloth_Buttons_Prefab;
    [Header("————用于更换的图片素材————")]
    public Sprite Start_Button_Image;
    public Sprite After_Button_Image;

    [Header("————组————")]
    public GameObject Top_Button_Group;
    public GameObject Items_Group;
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Quit":
                UIManager.GetInstance().HideUI("Phone_Shop");
                //EventCenter.GetInstance().RemoveEventListener("Info_Update", Grade_Two_Info_Update);
                break;
            case "Gitf_Button":
                Open_Gift_UI(button);
                break;
            case "Cloth_Button":
                Open_Cloth_UI(button);
                break;
            case "Building_Button":
                Open_Building_UI(button);
                break;
        }
    }

    /// <summary>
    /// 打开礼物界面
    /// </summary>
    private void Open_Gift_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        GiftItem_Create();
    }
    /// <summary>
    /// 打开建筑UI
    /// </summary>
    private void Open_Building_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        BuildingItem_Create();
    }
    /// <summary>
    /// 打开装扮界面
    /// </summary>
    private void Open_Cloth_UI(Button button)
    {
        Clear_All_Button();
        Current_Button = button;
        Button_Image_Change(Top_Button_Group, After_Button_Image, Start_Button_Image);
        ClothItem_Create();
    }

    #region 商城 礼物界面
    /// <summary>
    /// 根据读表动态生成商品按钮
    /// </summary>
    public void GiftItem_Create()
    {
        List<GiftData> gift_list = ObjectKeeper_Singleton.Instance.Gift_list;
        for (int i = 0; i < gift_list.Count; i++)
        {
            //创建按钮
            Image Gift = Instantiate(Gift_Image_Prefab[i % Gift_Image_Prefab.Count], Items_Group.transform);//生成礼物
            Gift.GetComponent<Gift>().Data = gift_list[i];//给按钮赋值
            GiftData data = Gift.GetComponent<Gift>().Data;//获取已经赋值的按钮礼物数据
            //更改按钮细节
            Gift.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Description;//Gift的描述
            Gift.transform.GetChild(1).GetComponent<Image>().sprite = ResourceManager.GetInstance().Load<Sprite>(data.Sprite_ResPath);//Gift的Sprite
            Gift.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
            Gift.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = data.Name;//Gift的名称
            //为按钮注册点击事件
            Button Buy_Gift_Button=Gift.transform.GetChild(3).GetComponent<Button>();
            Buy_Gift_Button.onClick.AddListener(() => Buy_Gift_Event(data));
        }
    }
    /// <summary>
    /// 礼物购买事件
    /// </summary>
    public void Buy_Gift_Event(GiftData giftData)
    {
        ObjectKeeper_Singleton.Instance.Buy(giftData);
    }
    #endregion

    #region 商城 服装页面
    /// <summary>
    /// 根据读表动态生成商品按钮
    /// </summary>
    public void ClothItem_Create()
    {
        List<ClothItem_Data> cloth_list = ObjectKeeper_Singleton.Instance.Cloth_list;
        for (int i = 0; i < cloth_list.Count; i++)
        {
            //创建按钮
            Image Cloth_Button = Instantiate(Cloth_Buttons_Prefab[i % Cloth_Buttons_Prefab.Count], Items_Group.transform);//生成按钮
            Cloth_Button.GetComponent<Item_Gift_Cloth>().Data = cloth_list[i];//给按钮赋值/该值来自于Json数据文件
            ClothItem_Data data = Cloth_Button.GetComponent<Item_Gift_Cloth>().Data;
            /*————更改按钮细节————*/
            //衣服的描述
            Cloth_Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Description;
            //服装的Sprite
            Image image = Cloth_Button.transform.GetChild(1).GetComponent<Image>();
            image.sprite = ResourceManager.GetInstance().Load<Sprite>(data.ResPath);//Gift的Sprite
            image.SetNativeSize();
            image.transform.localScale=new Vector2(0.4F,0.4F);
            //衣物的名称
            Cloth_Button.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = data.Name;

            //小动物头像
            for (int a=0;a<ObjectKeeper_Singleton.Instance.NPCs.Length;a++)
            {
                if(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.ID==data.Target_Animal_ID)
                {
                    Sprite Target_Animal_Sprite =ResourceManager.GetInstance().Load<Sprite>(ObjectKeeper_Singleton.Instance.NPCs[a].GetComponent<CharaController>().Template_data.Sprite_Res);
                    Cloth_Button.GetComponent<Item_Gift_Cloth>().HeadImage.sprite = Target_Animal_Sprite;
                }
            }
            /*————注册按钮事件————*/
            //如果已经被购买，则添加已购入遮罩/将购买按钮更换为换装按钮
            if (ObjectKeeper_Singleton.Instance.gamerData.Player_ClothBought.Contains(data.ID))
            {
                Cloth_Button.GetComponent<Item_Gift_Cloth>().ChangeTo_Dress();
                //Debug.Log(data.Name);
                //Cloth_Button.GetComponent<Item_Gift_Cloth>().Bought();
            }
            else//如果没有被购买则照常注册购买事件
            {
                //获取购买按钮
                Button Buy_Button = Cloth_Button.transform.GetChild(4).GetComponent<Button>();

                //为购买按钮注册点击事件
                Buy_Button.onClick.AddListener(() =>
                {
                    ObjectKeeper_Singleton.Instance.Buy(data/*() => Cloth_Button.GetComponent<Item_Gift_Cloth>().Bought()*/);//完成购买的运算
                    Cloth_Button.GetComponent<Item_Gift_Cloth>().ChangeTo_Dress();//更换按钮为换装按钮
                    DressButton_Event_SignIn(Cloth_Button);
                });
            }
        }
    }
    public void DressButton_Event_SignIn(Image Item)
    {
        ClothItem_Data data = Item.GetComponent<Item_Gift_Cloth>().Data;
        Button DressButton=Item.transform.GetChild(4).GetComponent<Button>();
        DressButton.onClick.AddListener(() =>
        {
            Debug.Log(data.Name+" 穿在 "+data.Target_Animal_ID);
        });

    }
    #endregion
    #region 商城 地图界面
    private void BuildingItem_Create()
    {
        return;
    }
    #endregion
    /// <summary>
    /// 清空礼物二级界面所有的按钮事件
    /// </summary>
    public void Clear_All_Button()
    {
        for (int i = 0; i < Items_Group.transform.childCount; i++)
        {
            Destroy(Items_Group.transform.GetChild(i).gameObject);
        }
    }
}
