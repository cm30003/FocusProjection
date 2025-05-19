using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevelopMode_FixPanel_Animal : MonoBehaviour
{
    [Header("————按钮————")]
    public Button[] FixButton;

    public TMP_Text WarnningText;
    [Header("————输入文本框————")]
    public TMP_InputField Favorability_Input;
    public TMP_InputField MoveSpeed_Input;
    public TMP_InputField WorkSpeed_Input;
    public TMP_InputField WorkTime_Input;
    public TMP_InputField EatTime_Input;
    public TMP_InputField Hungry_Input;
    private void Start()
    {
        ButtonEventSignIn();
    }
    public void ButtonEventSignIn()
    {
        for (int i = 0; i < FixButton.Length; i++)
        {
            int index = i;
            FixButton[index].onClick.AddListener(() => ChangeField(index));
        }
    }
    public void ChangeField(int index)
    {
        NPCData Data = ObjectKeeper_Singleton.Instance.NPCs[index].GetComponent<CharaController>().data;
        // 整数类型字段
        Favorability_Input.SetTo(ref Data.Favorability, WarnningText);
        MoveSpeed_Input.SetTo(ref Data.MoveSpeed, WarnningText);
        WorkSpeed_Input.SetTo(ref Data.Work_Speed, WarnningText);
        WorkTime_Input.SetTo(ref Data.Work_Speed, WarnningText);
        EatTime_Input.SetTo(ref Data.Eat_Time, WarnningText);
        Hungry_Input.SetTo(ref Data.Hungry_Time, WarnningText);
        // 如果需要更新 UI 或保存数据，可以在这里调用相关方法
        WarnningText.text = "数据已更新";
    }
}
