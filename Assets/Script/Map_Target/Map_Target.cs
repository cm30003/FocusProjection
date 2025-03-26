using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Target:MonoBehaviour
{
    [Header("————种地机器/精灵图————")]
    public Sprite Water_Sprite;
    public Sprite Fertilize_Sprite;
    public Sprite BugControl_Sprite;
    [Header("————货物数据————")]
    public PlantData Freight;//货物
    [Header("————动态数据————")]
    public GameObject npc;//当前占据该点的npc

    public Map_Target_Kind kind;//点位种类

    public bool Is_Empty=true;//是不是空的
    private void Start()
    {
        Kind_SignIn(gameObject.tag);

        Freight_SignIn();
    }
    public void Kind_SignIn(string tag)
    {
        switch (tag)
        {
            case "Farm_Machine":
                kind = Map_Target_Kind.Farm_Machine;
                break;
            case "Eat_Area":
                kind = Map_Target_Kind.Eat_Area;
                break;
            case "Freight_Target":
                kind = Map_Target_Kind.Freight_Target;
                break;
            case "Rest_Area":
                kind = Map_Target_Kind.Rest_Area;
                break;
            case "WareHouse_Area":
                kind = Map_Target_Kind.WareHouse_Area;
                break;
            case "TouchFish_Area":
                kind = Map_Target_Kind.TouchFish_Area;
                break;
        }

    }
    /// <summary>
    /// 货物初始化
    /// </summary>
    public void Freight_SignIn()
    {
        //货物初始化
        Freight = new PlantData(null);
        //如果是货物目标，则隐藏其Sprite
        if (kind == Map_Target_Kind.Freight_Target)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC")
        {
            CharaController charaController=collision.gameObject.GetComponent<CharaController>();
            #region 废码，暂时的
            //float distance = Vector2.Distance(collision.transform.position, transform.position);

            //float EndDistance = collision.GetComponent<AIPath>().endReachedDistance;
            //print(gameObject.name);
            //print(charaController.Target.target);
            #endregion
            if (charaController.Target.target== this.transform)
            {
                npc = collision.gameObject;

                Switch(charaController);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == npc)
        {
            npc=null;
            Is_Empty = true;
        }
    }
    public void Switch(CharaController charaController)
    {
        switch (kind)
        {
            case Map_Target_Kind.TouchFish_Area:
                break;

            case Map_Target_Kind.Rest_Area:
                break;

            case Map_Target_Kind.Farm_Machine:

                if(Is_Empty)
                {
                    charaController.NPC_Status = NPC_status.Work;
                    if(charaController.Work_Field !=null&& charaController.Work_Field.State==Plant_State.water)
                    {
                        GetComponent<SpriteRenderer>().sprite = Water_Sprite;
                    }
                    if(charaController.Work_Field != null&&charaController.Work_Field.State == Plant_State.fertilize)
                    {
                        GetComponent<SpriteRenderer>().sprite = Fertilize_Sprite;
                    }
                    if(charaController.Work_Field != null&&charaController.Work_Field.State == Plant_State.bug_control)
                    {
                        GetComponent<SpriteRenderer>().sprite = BugControl_Sprite;
                    }
                    Is_Empty = false;
                }


                break;

            case Map_Target_Kind.Eat_Area:
                
                if (Is_Empty)
                {
                    charaController.NPC_Status = NPC_status.Eat;
                    Is_Empty = false;
                }

                break;

            

            case Map_Target_Kind.Freight_Target:

                if (Is_Empty)
                {
                    //为NPC赋值，使其获得货物状态
                    npc.GetComponent<CharaController>().freight = Freight;
                    //切换NPC状态
                    charaController.NPC_Status = NPC_status.Transport;
                    //清空自身
                    Freight = new PlantData(null);
                    GetComponent<SpriteRenderer>().enabled = false;

                    Is_Empty = false;
                }
                break;

            case Map_Target_Kind.WareHouse_Area:

                PlantData plantData = npc.GetComponent<CharaController>().freight;
                GamerData gamerData = ObjectKeeper_Singleton.Instance.gamerData;

                //print(gamerData);
                print(gamerData.Items == null);
                if(plantData.Name!=string.Empty&&plantData.Name!=null)
                {
                    if (gamerData.Items == null || gamerData.Items.Count == 0)
                    {
                        gamerData.Items.Add(plantData);
                    }
                    else
                    {
                        PlantData foundItem = gamerData.Items.Find(item => item.Name == plantData.Name);
                        if (foundItem != null)
                        {
                            foundItem.Num += plantData.Num;
                        }
                        else
                        {
                            gamerData.Items.Add(plantData);
                        }
                    }
                }
                
                EventCenter.GetInstance().EventTrigger("ItemList_Update");
                npc.GetComponent<CharaController>().freight = new PlantData(null);
                npc.GetComponent<CharaController>().NPC_Status = NPC_status.GoToWork;

                break;
        }

    }
}
