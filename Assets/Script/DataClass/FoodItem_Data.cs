using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FoodData_List", menuName = "Data/FoodData")]
[System.Serializable]
public class FoodData : ScriptableObject
{
    public List<FoodItem_Data> Food_List=new List<FoodItem_Data>();
}
[System.Serializable]
public class FoodItem_Data
{
    public int ID;

    public string Name;//名字
    public string Description;//描述
    public int Cost;//购买/售卖价格
    public float Buff;//增益
    public string ResPath;//资源路径

    public Sprite Sprite;//图像
}

