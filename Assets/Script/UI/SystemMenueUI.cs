using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMenueUI : UIBase
{
    public RectTransform Grade_Two_CDKey_UI;

    [Header("——————Buttons——————")]
    public GameObject Buttons_Group;
    public Button Quit_Button;
    public Button CDKey_Button;
    public Button Notice_Button;
    public Button Update_Daily_Button;

    public Sprite Start_Button_Sprite;
    public Sprite Choosen_Button_Sprite;

    private Button CurrentButton;
    private void Start()
    {
        InitClick();
    }
    private void InitClick()
    {
        Quit_Button.onClick.AddListener(Quit_SystemMenueUI);
        CDKey_Button.onClick.AddListener(Open_CDKey);
        Notice_Button.onClick.AddListener(OPen_Notice_UI);
        Update_Daily_Button.onClick.AddListener(Open_Updtae_Daily_UI);
    }

    private void Open_Updtae_Daily_UI()
    {
        CurrentButton = Update_Daily_Button;
        Button_Color_Change();
    }

    private void OPen_Notice_UI()
    {
        CurrentButton = Notice_Button;
        Button_Color_Change();
    }

    private void Open_CDKey()
    {
        CurrentButton = CDKey_Button;
        Grade_Two_CDKey_UI.localScale = Vector3.one;
        Button_Color_Change();
    }
    /// <summary>
    /// 修改系统菜单中的按钮颜色
    /// </summary>
    public void Button_Color_Change()
    {
        Button[] buttons = Buttons_Group.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if(button!=CurrentButton)
            {
                button.image.color = Color.white;
            }
            else
            {
                button.image.color = new Color(248/255f, 204/255f, 0);
            }
        }
    }
    /// <summary>
    /// 退出系统菜单
    /// </summary>
    private void Quit_SystemMenueUI()
    {
        Button[] buttons = Buttons_Group.GetComponentsInChildren<Button>();
        for(int i=0;i<buttons.Length; i++)
        {
            if (buttons[i].image.color!=Color.white)
            {
                buttons[i].image.color = Color.white;
            }
            
        }
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        CloseUI(canvasGroup);
    }
}
