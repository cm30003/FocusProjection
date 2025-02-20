using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class plant_State : MonoBehaviour
{
    public NPCData npc_Data;

    public PlantData Temple_Data;
    public PlantData data;                           
    public Plant_State State;

    private int Water_Num=2;
    private void Start()
    {
        npc_Data=new NPCData(null);
    }
    private void Update()
    {

        //print(npc_Data.Name);
        Switch_Sate();
    }
    public void Switch_Sate()
    {
        switch (State)
        {
            case Plant_State.Empty://空的
                break;


            case Plant_State.plant://播種
                //变更所有图片
                Sprit_Change(data.Germinate_Image);
                //计算时间
                Time_calculation(ref data.Plant_Time);
                //时间走完，切换到发芽状态
                if (data.Plant_Time<=0)
                {
                    State= Plant_State.Germinate;
                }
                break;


            case Plant_State.Germinate://发芽阶段
                //自然生长
                GameManager.GetInstance().Update_Timer(ref data.Germinate_Time,1f);

                if(data.Germinate_Time<=0)
                {
                    State = Plant_State.Grown;
                }
                break;


            case Plant_State.Grown://成长阶段

                Sprit_Change(data.Grown_Image);

                GameManager.GetInstance().Update_Timer(ref data.Grown_Time, 1f);

                if(data.Grown_Time<= 0)
                {
                    State = Plant_State.water;
                }

                break;


            case Plant_State.water://浇水

                Time_calculation(ref data.Water_Time);

                if (data.Plant_Time <= 0)
                {
                    State = Plant_State.water;
                    
                    if(Water_Num==0)
                    {
                        State = Plant_State.Mature;
                    }
                    else
                    {
                        Water_Num--;
                        data.Water_Time = Temple_Data.Water_Time;
                        data.Grown_Time = Temple_Data.Grown_Time;
                        State= Plant_State.Grown;
                    }
                }
                break;


            case Plant_State.Mature://成熟阶段
                GameManager.GetInstance().Update_Timer(ref data.Mature_Time, 1f);

                if(data.Mature_Time<=0)
                {
                    State= Plant_State.fertilize;
                }
                break;


            case Plant_State.fertilize://施肥

                Time_calculation(ref data.fertilize_Time);

                if (data.fertilize_Time <= 0)
                {
                    State = Plant_State.bug_control;
                }
                break;


            case Plant_State.bug_control://除虫

                Time_calculation(ref data.BugControl_Time);

                if (data.BugControl_Time <= 0)
                {
                    State = Plant_State.harvest;
                }
                break;


            case Plant_State.harvest://收获

                Time_calculation(ref data.Harvest_Time);

                if(data.Harvest_Time<=0&&data.Name!=null)
                {
                    GameObject[] gameObjects = ObjectKeeper_Singleton.Instance.Freight_Target;

                    for(int i=0;i<gameObjects.Length;i++)
                    {
                        GameObject gameObject = gameObjects[i];
                        Map_Target target = gameObject.GetComponent<Map_Target>();
                        if(target.Freight.Name==null)
                        {
                            target.GetComponent<SpriteRenderer>().enabled = true;

                            target.Freight = data;
                            
                            //print(target.name);
                            break;
                        }
                    }
                    Reset_AllTime();
                    data =new PlantData(null);

                    Sprit_Change(null);
                    State = Plant_State.Empty;
                }

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
            Image image = transform.GetChild(i).GetComponent<Image>();
            image.sprite = sprite;
        }
    }
    /// <summary>
    /// 时间计算，用以在有小动物工作下的植物时间的计算
    /// </summary>
    /// <param name="time">本阶段时间</param>
    public void Time_calculation(ref float time)
    {
        if(npc_Data.Name!=null)
        {
            GameManager.GetInstance().Update_Timer(ref time, npc_Data.Work_Speed);
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
        data.Water_Time = Temple_Data.Water_Time;
        data.Grown_Time = Temple_Data.Grown_Time;
        data.Mature_Time = Temple_Data.Mature_Time;
        data.fertilize_Time = Temple_Data.fertilize_Time;
        data.BugControl_Time = Temple_Data.BugControl_Time;
        data.Harvest_Time = Temple_Data.Harvest_Time;
    }
}
