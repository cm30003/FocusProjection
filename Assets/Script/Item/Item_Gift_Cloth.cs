using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂载在商城衣物按钮上的数据脚本
/// </summary>
public class Item_Gift_Cloth : MonoBehaviour
{
    [Tooltip("小动物头像")]
    public Image HeadImage;
    [Tooltip("已购买遮罩")]
    public CanvasGroup Mask;
    public GameObject DressButton;
    [Header("————数据————")]
    public ClothItem_Data Data;
    /// <summary>
    /// 商品进入已购买状态，显示已购买遮罩，遮挡点击
    /// </summary>
    public void Bought()
    {
        Mask.alpha = 0.7f;
        Mask.interactable = true;
        Mask.blocksRaycasts = true;
    }
    public void ChangeTo_Dress()
    {
        Destroy(transform.GetChild(4).gameObject);
        GameObject dressButton= Instantiate(DressButton,transform);
        dressButton.transform.SetSiblingIndex(4);
    }
}
