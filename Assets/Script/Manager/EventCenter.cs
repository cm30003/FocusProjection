using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
// 事件响应空接口，用于支持可有可无的参数类型
public interface IEventInfo
{

}

// 带参数事件响应
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

// 无参数事件响应
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
/// <summary>
/// 事件中心 负责注册（监听）事件、分发（触发）事件 事件支持 带参数 和 无参数 两种 带参数事件使用 EventInfo<T> 数据类型
/// </summary>
public class EventCenter: BaseManager<EventCenter>
{
    /// <summary>
    /// 用于存储所有已发生/订阅事件的字典
    /// </summary>
    private Dictionary<string, IEventInfo> _eventDic = new Dictionary<string, IEventInfo>();

    // 添加带参数事件的监听
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // 判断事件是否存在于字典中
        if (_eventDic.ContainsKey(name))
            //存在，则+该事件
            (_eventDic[name] as EventInfo<T>).actions += action;
        // 新事件
        else
            _eventDic.Add(name, new EventInfo<T>(action));
    }

    // 添加无参数事件的监听
    public void AddEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo).actions += action;
        else
            _eventDic.Add(name, new EventInfo(action));
    }

    // 移除带参数事件的监听
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo<T>).actions -= action;
    }

    // 移除无参数事件的监听
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
            (_eventDic[name] as EventInfo).actions -= action;
    }

    // 分发带参数的事件
    public void EventTrigger<T>(string name, T info)
    {
        if (_eventDic.ContainsKey(name))
            if ((_eventDic[name] as EventInfo<T>).actions != null)
                (_eventDic[name] as EventInfo<T>).actions.Invoke(info);
    }

    // 分发无参数的事件
    public void EventTrigger(string name)
    {
        if (_eventDic.ContainsKey(name))
            if ((_eventDic[name] as EventInfo).actions != null)
                (_eventDic[name] as EventInfo).actions.Invoke();
    }

    // 清空事件监听
    public void Clear()
    {
        _eventDic.Clear();
    }
}
