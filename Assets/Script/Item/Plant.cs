using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantData Data;
    private void Update()
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = " ��ֲʱ�䣺" + Data.Plant_Time.ToString() + " ��ѿʱ�䣺" + Data.Germinate_Time.ToString()
            + " ����ʱ�䣺" + (Data.Grown_Time * 2).ToString() + " ��ˮʱ�䣺" + (Data.Water_Time * 2).ToString() + " ʩ��ʱ��:" + Data.fertilize_Time.ToString() +
            " ����ʱ�䣺" + Data.Mature_Time.ToString() +" ����ʱ�䣺"+ Data.BugControl_Time .ToString()+ " �ջ�ʱ��:" + Data.Harvest_Time.ToString(); 
    }
}
