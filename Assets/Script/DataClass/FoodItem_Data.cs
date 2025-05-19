using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 食物的数据类
/// </summary>
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
/// <summary>
/// 植物的数据额类
/// </summary>
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
    public float Grown_Time1;
    public float Grown_Time2;
    [Tooltip("成熟时间")]
    public float Mature_Time;
    [Tooltip("播种时间")]
    public float Plant_Time;//播种时间
    [Tooltip("浇水时间")]
    public float Water_Time1;//浇水时间
    public float Water_Time2;
    [Tooltip("施肥时间")]
    public float fertilize_Time;//施肥时间
    [Tooltip("除虫时间")]
    public float BugControl_Time;//除虫时间
    [Tooltip("收获时间")]
    public float Harvest_Time;//收获时间

    [Tooltip("收获数量")]
    public int Harvest_Num;//收获数量
    public int Gamer_Num;//玩家拥有该植物的数量

    public string Germinate_SpriteResPath;//发芽图片路径
    public string Grown_SpriteResPath;//成长图片路径
    public string Mature_SpriteResPath;//成熟图片路径
    public PlantItem_Data(PlantItem_Data other)
    {
        if (other == null)
        {
            ID = 0;
            Name =null;
            Description =null;
            Sell_Price = 0;
            Cost = 0;

            Germinate_Time = 0;
            Grown_Time1 = 0;
            Grown_Time2 = 0;
            Mature_Time = 0;
            Plant_Time = 0;
            Water_Time1 = 0;
            Water_Time2=0;

            fertilize_Time = 0;
            BugControl_Time = 0;
            Harvest_Time = 0;

            Harvest_Num = 0;

            Germinate_SpriteResPath=null;
            Grown_SpriteResPath=null;
            Mature_SpriteResPath=null;
        }
        else
        {
            ID = other.ID;
            Name = other.Name;
            Description = other.Description;
            Sell_Price = other.Sell_Price;
            Cost = other.Cost;

            Germinate_Time = other.Germinate_Time;
            Grown_Time1 = other.Grown_Time1;
            Grown_Time2 = other.Grown_Time2;
            Mature_Time = other.Mature_Time;
            Plant_Time = other.Plant_Time;
            Water_Time1 = other.Water_Time1;
            Water_Time2 = other.Water_Time2;
            fertilize_Time = other.fertilize_Time;
            BugControl_Time = other.BugControl_Time;
            Harvest_Time = other.Harvest_Time;

            Harvest_Num = other.Harvest_Num;

            Germinate_SpriteResPath = other.Germinate_SpriteResPath;
            Grown_SpriteResPath = other.Grown_SpriteResPath;
            Mature_SpriteResPath = other.Mature_SpriteResPath;
        }

    }
    public override string ToString()
    {
        return
            $"<color=white>ID:</color> <color=green>{ID}</color>, " +
            $"<color=white>Name:</color> <color=green>{Name}</color>, " +
            $"<color=white>Description:</color> <color=green>{Description}</color>, " +
            $"<color=white>Sell_Price:</color> <color=green>{Sell_Price}</color>, " +
            $"<color=white>Cost:</color> <color=green>{Cost}</color>\n" +

            $"<color=white>Germinate_Time:</color> <color=green>{Germinate_Time}</color>, " +
            $"<color=white>Grown_Time1:</color> <color=green>{Grown_Time1}</color>, " +
            $"<color=white>Grown_Time2:</color> <color=green>{Grown_Time2}</color>, " +
            $"<color=white>Mature_Time:</color> <color=green>{Mature_Time}</color>\n" +

            $"<color=white>Plant_Time:</color> <color=green>{Plant_Time}</color>, " +
            $"<color=white>Water_Time1:</color> <color=green>{Water_Time1}</color>, " +
            $"<color=white>Water_Time2:</color> <color=green>{Water_Time2}</color>\n" +

            $"<color=white>fertilize_Time:</color> <color=green>{fertilize_Time}</color>, " +
            $"<color=white>BugControl_Time:</color> <color=green>{BugControl_Time}</color>, " +
            $"<color=white>Harvest_Time:</color> <color=green>{Harvest_Time}</color>\n" +

            $"<color=white>Harvest_Num:</color> <color=green>{Harvest_Num}</color>, " +
            $"<color=white>Gamer_Num:</color> <color=green>{Gamer_Num}</color>\n" +

            $"<color=white>Germinate_SpriteResPath:</color> <color=green>{Germinate_SpriteResPath}</color>\n" +
            $"<color=white>Grown_SpriteResPath:</color> <color=green>{Grown_SpriteResPath}</color>\n" +
            $"<color=white>Mature_SpriteResPath:</color> <color=green>{Mature_SpriteResPath}</color>";
    }
    public PlantItem_Data()//公共无参构造函数
    {
        Germinate_Time = 0;
        Grown_Time1 = 0;
        Grown_Time2 = 0;
        Mature_Time = 0;
        Plant_Time = 0;
        Water_Time1 = 0;
        Water_Time2 = 0;    
        fertilize_Time = 0;
        BugControl_Time = 0;
        Harvest_Time = 0;
        Harvest_Num = 0;
        Germinate_SpriteResPath = null;
        Grown_SpriteResPath = null;
        Mature_SpriteResPath = null;
    }
}
/// <summary>
/// 商城衣物数据类
/// </summary>
[Serializable]
public class ClothItem_Data
{
    public int ID;

    public string Name;
    public string Description;
    public int Cost;

    public int Target_Animal_ID;

    public string ResPath;
}
[Serializable]
public class NPCInformation//NPC信息类（UI显示）
{
    [Header("————基本信息————")]
    public int ID;
    public string Name;//名字
    public string Hobby;//爱好
    public string Personality;//性格
    public string Description;//描述文本
    public string BirthDay;//生日
    public NPCInformation(NPCInformation other)
    {
        if (other == null)
        {
            // 如果传入的参数为 null，则初始化为默认值
            ID = 0;
            Name = string.Empty;
            Hobby = string.Empty;
            Personality = string.Empty;
            Description = string.Empty;
            BirthDay = string.Empty;
        }
        else
        {
            ID = other.ID;
            Name = other.Name;
            Hobby = other.Hobby;
            Personality = other.Personality;
            Description = other.Description;
            BirthDay = other.BirthDay;
        }
    }
    // 公共无参构造函数
    public NPCInformation()
    {
        ID = 0;
        Name = string.Empty;
        Hobby = string.Empty;
        Personality = string.Empty;
        Description = string.Empty;
        BirthDay = string.Empty;
    }
}
[Serializable]
public class NPCData//NPC数据类
{
    public int ID;//ID（1001~1999）
    [Header("————喜好————")]
    public int Favorability;//好感度
    public int FavorvateThing_ID;//喜好物的ID
    public int CherishThing_ID;//珍视的东西的ID
    [Header("————效率————")]
    public float MoveSpeed;//移动速度
    public float Work_Speed;//工作速度
    [Header("————时间————")]
    public float Work_Time;//工作时间

    public float Eat_Time;//吃饭时间

    public float Hungry_Time;//饥饿时间
    [Header("————资源————")]
    public string Sprite_Res;
    // 拷贝构造函数
    public NPCData(NPCData other)
    {
        if (other == null)
        {
            ID  = 0;

            Favorability = 0;

            FavorvateThing_ID = 0;
            CherishThing_ID = 0;

            MoveSpeed = 0f;
            Work_Speed = 0f;

            Work_Time = 0f;
            Eat_Time = 0f;
            Hungry_Time = 0f;

            Sprite_Res=null;
        }
        else
        {
            ID=other.ID;

            Favorability = other.Favorability;

            FavorvateThing_ID = other.FavorvateThing_ID;
            CherishThing_ID = other.CherishThing_ID;

            MoveSpeed = other.MoveSpeed;
            Work_Speed = other.Work_Speed;

            Work_Time = other.Work_Time;
            Eat_Time = other.Eat_Time;
            Hungry_Time = other.Hungry_Time;

            Sprite_Res = other.Sprite_Res;
        }
    }
    // 公共无参构造函数
    public NPCData()
    {
        ID = 0;

        Favorability = 0;

        FavorvateThing_ID = 0;
        CherishThing_ID = 0;

        MoveSpeed = 0f;
        Work_Speed = 0f;

        Work_Time = 0f;
        Eat_Time = 0f;
        Hungry_Time = 0f;

        Sprite_Res = null;
    }
    public override string ToString()
    {
        return
            $"<color=white>ID:</color> <color=green>{ID}</color>\n" +

            $"<color=white>Favorability:</color> <color=green>{Favorability}</color>, " +
            $"<color=white>FavorvateThing_ID:</color> <color=green>{FavorvateThing_ID}</color>, " +
            $"<color=white>CherishThing_ID:</color> <color=green>{CherishThing_ID}</color>\n" +

            $"<color=white>MoveSpeed:</color> <color=green>{MoveSpeed}</color>, " +
            $"<color=white>Work_Speed:</color> <color=green>{Work_Speed}</color>\n" +

            $"<color=white>Work_Time:</color> <color=green>{Work_Time}</color>, " +
            $"<color=white>Eat_Time:</color> <color=green>{Eat_Time}</color>, " +
            $"<color=white>Hungry_Time:</color> <color=green>{Hungry_Time}</color>\n" +

            $"<color=white>Sprite_Res:</color> <color=green>{Sprite_Res}</color>";
    }
}
/// <summary>
/// 商城礼物数据类
/// </summary>
[Serializable]
public class GiftData
{
    public int ID;
    public string Name;//名字
    public string Description;//描述
    public int Cost;//购买/售卖价格
    [Tooltip("好感度加成")]
    public int Default_Affinity;//默认好感度
    public int Like_Affinity;//喜爱好感度
    public int Favorate_Affinity;//最高好感度

    [Tooltip("资源路径")]
    public string Sprite_ResPath;
}


