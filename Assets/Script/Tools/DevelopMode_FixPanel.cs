using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevelopMode_FixPanel : MonoBehaviour
{
    [Header("————按钮————")]
    public Button[] FixButton;

    public TMP_Text WarnningText;
    [Header("————输入文本框————")]
    public TMP_InputField SellPrice_Input;
    public TMP_InputField Cost_Input;
    public TMP_InputField Germinate_Input;
    public TMP_InputField Growm1_Input;
    public TMP_InputField Growm2_Input;
    public TMP_InputField Matrue_Input;
    public TMP_InputField Plant_Input;
    public TMP_InputField Water1_Input;
    public TMP_InputField Water2_Input;
    public TMP_InputField Fertilize_Input;
    public TMP_InputField BugControl_Input;
    public TMP_InputField Harvest_Input;
    public TMP_InputField HarvestNum_Input;
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
        PlantItem_Data Data = ObjectKeeper_Singleton.Instance.Farm_Field[index].GetComponent<plant_State>().data;
        // 整数类型字段
        SellPrice_Input.SetTo(ref Data.Sell_Price,WarnningText);
        Cost_Input.SetTo(ref Data.Cost,WarnningText);
        HarvestNum_Input.SetTo(ref Data.Harvest_Num, WarnningText);

        // 浮点类型字段
        Germinate_Input.SetTo(ref Data.Germinate_Time, WarnningText);
        Growm1_Input.SetTo(ref Data.Grown_Time1,WarnningText);
        Growm2_Input.SetTo(ref Data.Grown_Time2, WarnningText);
        Matrue_Input.SetTo(ref Data.Mature_Time, WarnningText);
        Plant_Input.SetTo(ref Data.Plant_Time, WarnningText);
        Water1_Input.SetTo(ref Data.Water_Time1,WarnningText);
        Water2_Input.SetTo(ref Data.Water_Time2, WarnningText);
        Fertilize_Input.SetTo(ref Data.fertilize_Time,  WarnningText);
        BugControl_Input.SetTo(ref Data.BugControl_Time, WarnningText);
        Harvest_Input.SetTo(ref Data.Harvest_Time,WarnningText);

        // 如果需要更新 UI 或保存数据，可以在这里调用相关方法
        WarnningText.text = "数据已更新";
    }
}
