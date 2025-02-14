using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiningHall : UIBase
{
    [Header("����������ǰʳ�������")]
    public FoodData Current_Food;

    public List<FoodData> Food_List;//Ҫ����������б�

    public Transform Buttons_Group;//��ť���ɵ�λ��
    public Button Quit_Button;//�˳���ť
    [Header("���������زġ�������")]
    public Button Item_button;//��ťԤ�Ƽ�

    public Sprite Stat_Sprite;
    public Sprite Clicked_Sprite;

    private void Start()
    {
        
        InitClick();

    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_DiningHall);//�˳���ť����¼�

        Food_SignIn();
    }
    /// <summary>
    /// ������ť
    /// </summary>
    public void Food_SignIn()
    {
        for (int i = 0; i < Food_List.Count; i++)
        {
            Button button = Instantiate(Item_button, Buttons_Group).GetComponent<Button>();
            Button Buy_Button = button.transform.GetChild(0).GetComponent<Button>();


            button.transform.GetChild(1).GetComponent<Image>().sprite = Food_List[i].Sprite;
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Food_List[i].Name;
            button.GetComponent<food>().Data = Food_List[i];
            button.onClick.AddListener(() => Click(button));
            Buy_Button.onClick.AddListener(() => Buy(button));
        }
    }
    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="current">��ǰ������İ�ť/Item</param>
    public void Click(Button parent)
    {
        Current_Button = parent;
        for (int i = 0; i < Buttons_Group.childCount; i++)
        {
            Button button = Buttons_Group.GetChild(i).GetComponent<Button>();
            Button Buy_Button = button.transform.GetChild(0).GetComponent<Button>();
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
    /// �����¼�
    /// </summary>
    /// <param name="button">��ǰItem</param>
    public void Buy(Button button)
    {
        Current_Food=button.GetComponent<food>().Data;

        ObjectKeeper_Singleton.Instance.foodData = Current_Food;
        ObjectKeeper_Singleton.Instance.gamerData.Money+=Current_Food.Money_Cost_reward;
        EventCenter.GetInstance().EventTrigger("Info_Update");
    }
    /// <summary>
    /// �˳�UI
    /// </summary>
    private void Quit_DiningHall()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        CloseUI(canvasGroup);
        if(Current_Button!=null)
        {
            Current_Button.transform.GetChild(0).localScale = Vector3.zero;
            Current_Button.GetComponent<Image>().sprite = Stat_Sprite;
            Current_Button = null;
        }
        
    }
}
