using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class EventControl:BaseManager<EventControl>
{
    //Key:�¼���
    //Value:�¼�/��Ӧ��ί�к���
    private Dictionary<string,UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();
    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="name">�¼�����</param>
    /// <param name="action">�¼���ί�к���</param>
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
    /// �Ƴ��¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name,UnityAction<object> action) 
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name]-=action;
        }
    }
    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="name">�¼�����</param>
    public void EventTrigger(string name,object info)
    {
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// ����¼�����
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
