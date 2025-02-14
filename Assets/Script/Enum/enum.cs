using UnityEngine;

public enum NPC_status
{
    Rest,//回休息室休息
    TouchFish,//摸鱼
    GoToWork,//去工作的路上
    Work,//工作中
    GoToEat,//去吃东西的路上
    Eat,//吃东西
    Transport//搬运
}
public enum Map_Target_Kind
{
    TouchFish_Area,//摸鱼的地方
    Rest_Area,//休息室
    Farm_Machine,//种田机器
    Eat_Area,//吃东西的地方
    WareHouse_Area,//仓库
    Freight_Target//货运的地方
}
public enum Plant_State
{
    Empty,//空的,啥也没有，等待玩家种植
    [Tooltip("发芽阶段")]
    Germinate,//发芽阶段

    plant,//播种

    Grown,//成长阶段

    water,//浇水
    [Tooltip("成熟阶段")]
    Mature,//成熟阶段
    [Tooltip("施肥阶段")]
    fertilize,//施肥
    bug_control,//除虫

    harvest//收获
}




