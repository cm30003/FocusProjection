using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DevelopModeManager : SingletonMono<DevelopModeManager>
{
    public RectTransform BaseImage;

    public TextMeshProUGUI Text;

    public GameObject ChangePage;

    public string[] Texts;

    public Button PlantButton;
    public Button AnimalButton;

    public CanvasGroup ChangePage_Plant;
    public CanvasGroup ChangePage_Animal;

    private int a;

    private void Start()
    {
        PlantButton.onClick.AddListener(PlantDataShow_ButtonClick);
        AnimalButton.onClick.AddListener(AnimalDataShow_ButtonClick);
    }
    private void Update()
    {
        CheckButton();
    }
    public void CheckButton()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(a==0)
            {
                a++;
                BaseImage.DOLocalMoveX(-460, 1).SetRelative(false); 
            }
            else if(a==1) 
            {
                a = 0;
                BaseImage.DOLocalMoveX(-1460, 1).SetRelative(false);
            }
        };
    }
    public void PlantDataShow_ButtonClick()
    {
        ChangePage_Plant.alpha = 1f;
        ChangePage_Plant.interactable = true;
        ChangePage_Plant.blocksRaycasts = true;

        ChangePage_Animal.alpha = 0f;
        ChangePage_Animal.interactable = false;
        ChangePage_Animal.blocksRaycasts = false;
        MonoManager.GetInstance().AddUpdateLisener(PlantDataShow);
        MonoManager.GetInstance().RemoveUpdateLisener(AnimalDataShow);
    }
    public void PlantDataShow()
    {
        Texts = new string[6];

        for (int i = 0; i < ObjectKeeper_Singleton.Instance.Farm_Group.transform.childCount; i++)
        {
            Texts[i] = ObjectKeeper_Singleton.Instance.Farm_Group.transform.GetChild(i).GetComponent<plant_State>().data.ToString();
        }

        SetText(
            Texts[0] + "\n" + "————————" + "\n"
            + Texts[1] + "\n" + "————————" + "\n"
            + Texts[2] + "\n" + "————————" + "\n"
            + Texts[3] + "\n" + "————————" + "\n"
            + Texts[4] + "\n" + "————————" + "\n"
            + Texts[5] + "\n" + "————————" + "\n"
            );


    }
    public void AnimalDataShow_ButtonClick()
    {
        ChangePage_Plant.alpha = 0f;
        ChangePage_Plant.interactable = false;
        ChangePage_Plant.blocksRaycasts = false;

        ChangePage_Animal.alpha = 1f;
        ChangePage_Animal.interactable = true;
        ChangePage_Animal.blocksRaycasts = true;
        MonoManager.GetInstance().AddUpdateLisener(AnimalDataShow);
        MonoManager.GetInstance().RemoveUpdateLisener(PlantDataShow);
    }
    public void AnimalDataShow()
    {
        Texts = new string[2];

        for (int i = 0; i < ObjectKeeper_Singleton.Instance.NPCs.Length; i++)
        {
            Texts[i] = ObjectKeeper_Singleton.Instance.NPCs[i].GetComponent<CharaController>().data.ToString();
        }
        SetText(
        Texts[0] + "\n" + "————————" + "\n"
        + Texts[1] + "\n" + "————————" + "\n"
         );
    }

    public void SetText(string text)
    {
        Text.text = text;
    }
}
public static class InputFieldExtensions
{
    public static void SetTo<T>(this TMP_InputField input, ref T field, TMP_Text warningLabel = null)
    {
        if (string.IsNullOrEmpty(input.text))
        {
            warningLabel.SetTextIfNotNull("输入为空");
            return;
        }

        try
        {
            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(input.text, out int result))
                {
                    field = (T)(object)result;
                    warningLabel.SetTextIfNotNull(string.Empty);
                }
                else
                {
                    warningLabel.SetTextIfNotNull("请输入有效的整数");
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(input.text, out float result))
                {
                    field = (T)(object)result;
                    warningLabel.SetTextIfNotNull(string.Empty);
                }
                else
                {
                    warningLabel.SetTextIfNotNull("请输入有效的数字");
                }
            }
            else if (typeof(T) == typeof(string))
            {
                field = (T)(object)input.text;
                warningLabel.SetTextIfNotNull(string.Empty);
            }
            else
            {
                warningLabel.SetTextIfNotNull("不支持的类型");
                Debug.LogError($"不支持的类型 {typeof(T)}");
            }
        }
        catch (Exception ex)
        {
            warningLabel.SetTextIfNotNull("发生异常");
            Debug.LogError($"设置字段时出错: {ex.Message}");
        }
    }
    // 扩展方法：安全设置 TMP_Text 内容
    public static void SetTextIfNotNull(this TMP_Text textComponent, string message)
    {
        if (textComponent != null)
        {
            textComponent.text = message;
        }
    }
}
