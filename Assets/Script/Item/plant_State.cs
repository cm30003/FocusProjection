using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class plant_State : MonoBehaviour
{
    public NPCData npc_Data;

    public PlantItem_Data Temple_Data;
    public PlantItem_Data data;
    public Plant_State State;

    public float Standard_Second = 1;
    private void Start()
    {
        npc_Data = new NPCData(null);
    }
    private void Update()
    {
        Switch_Sate();
        //Debug.Log(State);
    }
    public void Switch_Sate()
    {
        switch (State)
        {
            case Plant_State.Empty://空的
                break;


            case Plant_State.plant://播種
                //变更所有图片
                Sprit_Change(ResourceManager.GetInstance().Load<Sprite>(data.Germinate_SpriteResPath) );
                //计算时间
                Standard_Second-=Time.deltaTime;
                if(Standard_Second<=0)
                {
                    data.Plant_Time-=npc_Data.Work_Speed;
                    Standard_Second = 1;
                }

                //时间走完，切换到发芽状态
                if (data.Plant_Time <= 0)
                {
                    State = Plant_State.Germinate;
                }
                break;

            case Plant_State.Germinate://发芽阶段 自然生长

                data.Germinate_Time -= Time.deltaTime;

                if (data.Germinate_Time <= 0)
                {
                    State = Plant_State.Grown1;
                }
                break;


            case Plant_State.Grown1://成长阶段

                Sprit_Change(ResourceManager.GetInstance().Load<Sprite>(data.Grown_SpriteResPath) );

                data.Grown_Time1 -= Time.deltaTime;

                if (data.Grown_Time1 <= 0)
                {
                    State = Plant_State.water1;
                    Standard_Second = 1;
                }

                break;


            case Plant_State.water1://浇水
                if(npc_Data.ID!=0)
                {
                    Standard_Second -= Time.deltaTime;

                    if (Standard_Second <= 0)
                    {
                        data.Water_Time1 -= npc_Data.Work_Speed;
                        Standard_Second = 1;
                    }
                }

                if (data.Water_Time1 <= 0)
                {
                    State = Plant_State.Grown2;
                }
                break;

            case Plant_State.Grown2://成长阶段

                data.Grown_Time2 -= Time.deltaTime; 

                if (data.Grown_Time2 <= 0)
                {
                    State = Plant_State.water2;
                    Standard_Second = 1;
                }

                break;

            case Plant_State.water2://浇水

                if (npc_Data.ID != 0)
                {
                    Standard_Second -= Time.deltaTime;

                    if (Standard_Second <= 0)
                    {
                        data.Water_Time2 -= npc_Data.Work_Speed;
                        Standard_Second = 1;
                    }
                }

                if (Standard_Second <= 0)
                {
                    data.Water_Time2 -= npc_Data.Work_Speed;
                    Standard_Second = 1;
                }

                if (data.Water_Time2 <= 0)
                {
                    State = Plant_State.Mature;
                }
                break;

            case Plant_State.Mature://成熟阶段

                data.Mature_Time-= Time.deltaTime;

                if (data.Mature_Time <= 0)
                {
                    State = Plant_State.fertilize;
                }
                break;


            case Plant_State.fertilize://施肥

                if(npc_Data.ID!=0)
                {
                    Standard_Second -= Time.deltaTime;

                    if (Standard_Second <= 0)
                    {
                        data.fertilize_Time -= npc_Data.Work_Speed;
                        Standard_Second = 1;
                    }
                }
                

                if (data.fertilize_Time <= 0)
                {
                    State = Plant_State.bug_control;
                    Standard_Second = 1;
                }
                break;


            case Plant_State.bug_control://除虫

                if (npc_Data.ID != 0)
                {
                    Standard_Second -= Time.deltaTime;

                    if (Standard_Second <= 0)
                    {
                        data.BugControl_Time -= npc_Data.Work_Speed;
                        Standard_Second = 1;
                    }
                }

                if (data.BugControl_Time <= 0)
                {
                    State = Plant_State.harvest;
                    Standard_Second = 1;
                }
                break;


            case Plant_State.harvest://收获

                if (npc_Data.ID != 0)
                {
                    Standard_Second -= Time.deltaTime;

                    if (Standard_Second <= 0)
                    {
                        data.Harvest_Time -= npc_Data.Work_Speed;
                        Standard_Second = 1;
                    }
                }

                if (data.Harvest_Time <= 0 && data.ID != 0)
                {
                    State=Plant_State.Package;
                }

                break;
            case Plant_State.Package:
                GameObject[] gameObjects = ObjectKeeper_Singleton.Instance.Freight_Target;

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    GameObject gameObject = gameObjects[i];
                    Map_Target target = gameObject.GetComponent<Map_Target>();
                    if (target.Freight.Name == null)
                    {
                        target.GetComponent<SpriteRenderer>().enabled = true;

                        target.Freight = data;

                        break;
                    }
                }
                //Reset_AllTime();
                data = null;

                Sprit_Change(null);
                State = Plant_State.Empty;
                break;
        }
    }

    /// <summary>
    /// 图片切换，用以在植物的不同状态下切换图片
    /// </summary>
    /// <param name="sprite">当前状态对应图片</param>
    public void Sprit_Change(Sprite sprite)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
    /// <summary>
    /// 时间计算，用以在有小动物工作下的植物时间的计算
    /// </summary>
    /// <param name="time">本阶段时间</param>
    public void Time_calculation(ref float time)
    {
        if (npc_Data.ID != 0)
        {
            GameManager.GetInstance().Update_Timer(ref time, npc_Data.Work_Speed,ref Standard_Second);
        }
        else
        {
            return;
        }
    }
    public void Reset_AllTime()
    {
        data.Plant_Time = Temple_Data.Plant_Time;
        data.Germinate_Time = Temple_Data.Germinate_Time;
        data.Water_Time1 = Temple_Data.Water_Time1;
        data.Water_Time2 = Temple_Data.Water_Time2;
        data.Grown_Time1 = Temple_Data.Grown_Time1;
        data.Grown_Time2 = Temple_Data.Grown_Time2;
        data.Mature_Time = Temple_Data.Mature_Time;
        data.fertilize_Time = Temple_Data.fertilize_Time;
        data.BugControl_Time = Temple_Data.BugControl_Time;
        data.Harvest_Time = Temple_Data.Harvest_Time;
    }
}

