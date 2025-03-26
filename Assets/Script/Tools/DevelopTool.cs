using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevelopTool : MonoBehaviour
{
    //public List<PlantData> plantDatas;

    //public GameObject[] CharaController;

    //public TextMeshProUGUI Text;
    //private void Start()
    //{
    //    plantDatas = ObjectKeeper_Singleton.Instance.gamerData.Items;

    //    CharaController=ObjectKeeper_Singleton.Instance.NPCs;
    //}
    private void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 检测是否点击在 UI 元素上
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // 获取当前点击的 UI 对象
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    Debug.Log("Clicked UI Object: " + result.gameObject.name);
                }
            }
            else
            {
                // 检测是否点击在 3D 对象上
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Clicked 3D Object: " + hit.collider.gameObject.name);
                }

                // 检测是否点击在 2D 对象上
                RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

                if (hit2D.collider != null)
                {
                    Debug.Log("Clicked 2D Object: " + hit2D.collider.gameObject.name);
                }
            }
        }
    }
}
