using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantData Data;
    private void Update()
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " 种植时间：" + Data.Plant_Time.ToString() + " 发芽时间：" + Data.Germinate_Time.ToString()
            + " 生长时间：" + (Data.Grown_Time * 2).ToString() + " 浇水时间：" + (Data.Water_Time * 2).ToString() + " 施肥时间:" + Data.fertilize_Time.ToString() +
            " 成熟时间：" + Data.Mature_Time.ToString() +" 除虫时间："+ Data.BugControl_Time .ToString()+ " 收获时间:" + Data.Harvest_Time.ToString(); 
    }
}
