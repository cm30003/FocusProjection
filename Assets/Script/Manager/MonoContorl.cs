using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoContorl : MonoBehaviour
{
    private event UnityAction UpdateEvent;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (UpdateEvent!=null)
        {
            UpdateEvent();
        }
    }
    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateLisener(UnityAction action)
    {
        UpdateEvent += action;
    }
    /// <summary>
    /// 给外部提供的 移除帧更新事件的函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateLisener(UnityAction action)
    {
        UpdateEvent -= action;
    }
}
