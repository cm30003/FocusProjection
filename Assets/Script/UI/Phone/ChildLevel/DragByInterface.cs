using UnityEngine;
using UnityEngine.EventSystems;

public class DragByInterface : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private RectTransform RectTransform;
    private CanvasGroup CanvasGroup;
    private Transform Originalparent;

    private Canvas canvas;
    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();

        canvas = this.transform.root.GetComponent<Canvas>();
        Originalparent = this.transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("BeginDrag");

        this.transform.SetParent(this.transform.parent.parent.parent);
        CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        //定义：eventData.delta 是 eventData 中的一个属性，表示事件的增量变化。具体类型取决于 eventData 的子类。
        //类型：通常为 Vector2，表示在某个事件（如拖动）中位置的变化量。
        //用途：在拖动事件中，eventData.delta 表示鼠标或触摸点在当前帧和上一帧之间的移动距离。
        //本语句意即将 RectTransform 的 anchoredPosition 属性增加 eventData.delta 的值。
        RectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.name== "NPCChoosen")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);

            GetComponent<RectTransform>().localPosition = new Vector3(-5, -25, 0);
            GetComponent<RectTransform>().localScale = new Vector3(0.88f, 0.88f, 0.88f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(145, 205);

            CanvasGroup.blocksRaycasts = true;
        }
        else
        {
            CanvasGroup.blocksRaycasts = true;
            this.transform.SetParent(Originalparent);
            this.transform.localPosition = new Vector3(-5, -13, 0);
            GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(900, 1289);
        }

    }
}
