using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {

        eventData.pointerDrag.transform.SetParent(this.transform);
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(-5, -25, 0);
        eventData.pointerDrag.GetComponent<RectTransform>().localScale = new Vector3(0.88f, 0.88f, 0.88f);
        eventData.pointerDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(145, 205);
    }
}
