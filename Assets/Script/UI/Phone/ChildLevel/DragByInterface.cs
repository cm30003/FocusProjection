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
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
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
    /// <summary>
    /// 拖拽结束
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //如果被拖入了小动物选择框。
        if(eventData.pointerCurrentRaycast.gameObject.name== "NPCChoosen")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);

            GetComponent<RectTransform>().localPosition = new Vector3(-5, -25, 0);
            GetComponent<RectTransform>().localScale = new Vector3(0.88f, 0.88f, 0.88f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(145, 205);

            CanvasGroup.blocksRaycasts = true;

            //玩家选中事件
            EventCenter.GetInstance().EventTrigger<string>("Is_Choosen",GetComponent<NPCData_Data>().data.Name);
        }
        else
        {
            CanvasGroup.blocksRaycasts = true;
            this.transform.SetParent(Originalparent);
            this.transform.localPosition = new Vector3(-5, -13, 0);
            GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(900, 1289);
            //待命事件
            EventCenter.GetInstance().EventTrigger<string>("Is_Waiting", GetComponent<NPCData_Data>().data.Name);
        }

    }
}
