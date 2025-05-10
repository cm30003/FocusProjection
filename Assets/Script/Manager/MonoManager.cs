using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : BaseManager<MonoManager>
{
    public MonoContorl contorl;

    public MonoManager()
    {
        GameObject gameObject = new GameObject("MonoContorl");//在场景中创建一个gameObject对象
        contorl = gameObject.AddComponent<MonoContorl>();//给gameObject添加一个MonoContorl组件
    }
    /// <summary>
    /// 用于为不继承mono的类添加Update事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateLisener(UnityAction action)
    {
        contorl.AddUpdateLisener(action);
    }
    public void RemoveUpdateLisener(UnityAction action)
    {
        contorl.RemoveUpdateLisener(action);
    }


    //实现携程
    public Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return contorl.StartCoroutine(coroutine);
    }
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return contorl.StartCoroutine(methodName, value);
    }
    public Coroutine StartCoroutine(string methodName)
    {
        return contorl.StartCoroutine(methodName);
    }
}
