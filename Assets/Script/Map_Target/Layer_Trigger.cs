using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer_Trigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MeshRenderer meshRenderer = collision.GetComponentInChildren<MeshRenderer>();
        //如果此时小动物位于天台下方，则触发该触发器
        if (this.gameObject.name == "Bottom_Trigger" && meshRenderer.sortingOrder == 0)
        {
            meshRenderer.sortingOrder = 5;
            return;
        }
        if(this.gameObject.name == "Bottom_Trigger" && meshRenderer.sortingOrder == 5)
        {
            meshRenderer.sortingOrder = 0;
            return;
        }
        //if(this.gameObject.name == "Enter_Trigger"&& meshRenderer.sortingOrder == 5)
        //{
        //    collision.gameObject.transform.position = BottomTrigger.position;
        //    meshRenderer.sortingOrder = 0;
        //}
        if(this.gameObject.name == "Farm_Trigger"&&meshRenderer.sortingOrder == 5)
        {
            meshRenderer.sortingLayerName = "Character";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        MeshRenderer meshRenderer = collision.GetComponentInChildren<MeshRenderer>();
        if (collision.gameObject.name == "Farm_Trigger"&& meshRenderer.sortingLayerName == "Character")
        {
            print(33333);
            meshRenderer.sortingLayerName = "Map";
        }
    }
}
