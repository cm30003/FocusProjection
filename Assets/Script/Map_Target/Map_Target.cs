using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Target:MonoBehaviour
{
    public PlantData Freight;//货物

    public GameObject npc;

    public Map_Target_Kind kind;

    public bool Is_Empty=true;
    private void Start()
    {

        Freight = new PlantData(null);
        //print(Freight.Name);
        if (kind==Map_Target_Kind.Freight_Target)
        {
            GetComponent<SpriteRenderer>().enabled=false;
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
            if (/*distance<=EndDistance&&*/charaController.Target.target== this.transform)
            {
                //print(collision.gameObject.name);
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
                    //为NPC赋值
                    npc.GetComponent<CharaController>().freight = Freight;
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
