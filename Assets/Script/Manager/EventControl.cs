using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventControl:BaseManager<EventControl>
{
    //Key:事件名
    //Value:事件/对应的委托函数
    private Dictionary<string,UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件名称</param>
    /// <param name="action">事件的委托函数</param>
    public void AddEventListener(string name, UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name] += action;
        }
        else
        {
            eventDic.Add(name, action);
        }
    }
    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name,UnityAction<object> action) 
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name]-=action;
        }
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">事件名称</param>
    public void EventTrigger(string name,object info)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// 清空事件中心
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
