using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FoodItem_Data
{
    public int ID;

    public string Name;//名字
    public string Description;//描述
    public int Cost;//购买/售卖价格
    public float Buff;//增益
    public string ResPath;//资源路径
}
[Serializable]
public class PlantItem_Data
{
    public int ID;
    public string Name;
    public string Description;
    public int Sell_Price;
    public int Cost;
    [Tooltip("发芽时间")]
    public float Germinate_Time;
    [Tooltip("生长时间")]
    public float Grown_Time;
    [Tooltip("成熟时间")]
    public float Mature_Time;
    [Tooltip("播种时间")]
    public float Plant_Time;//播种时间
    [Tooltip("浇水时间")]
    public float Water_Time;//浇水时间
    [Tooltip("施肥时间")]
    public float fertilize_Time;//施肥时间
    [Tooltip("除虫时间")]
    public float BugControl_Time;//除虫时间
    //[Tooltip("松土时间")]
    //public float plow_Time;//松土时间
    [Tooltip("收获时间")]
    public float Harvest_Time;//收获时间

    [Tooltip("收获数量")]
    public int Harvest_Num;//收获数量

    public string Germinate_SpriteResPath;
    public string Grown_SpriteResPath;
    public string Mature_SpriteResPath;
}
[Serializable]
public class ClothItem_Data
{
    public int ID;

    public string Name;
    public string Description;
    public int Cost;

    public string ResPath;
}

