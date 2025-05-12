using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// UI层级
/// </summary>
public enum UILevel
{
    Bot,
    Mid,
    Top,
    System
}
/// <summary>
/// UI管理器
/// 管理所有显示的面板
/// 提供给外部 显示和隐藏 等等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string,NewUIBase> LoadedNewUIDic=new Dictionary<string, NewUIBase>();
    public Dictionary<string,UIBase> LoadedOldUIDic=new Dictionary<string, UIBase>();
    //记录UICanvas父对象，方便外部使用
    public RectTransform canvas;
    /*Canavs的各个层级（选用）*/
    private Transform Bot;
    private Transform Mid;
    private Transform Top;
    private Transform System;

    public UIManager()
    {
    //    //创建存储好的Canvas预制体，UI将在该Cnavas下生成
    //    GameObject gameObject=ResourceManager.GetInstance().Load<GameObject>("Prefab/UI");
        canvas = GameObject.FindGameObjectWithTag("Phone_Base").transform as RectTransform;
    //    //跨场景时不销毁
    //    GameObject.DontDestroyOnLoad(gameObject);

    //    //找到各层级
    //    Bot = canvas.Find("Bot");
    //    Mid = canvas.Find("Mid");
    //    Top = canvas.Find("Top");
    //    System = canvas.Find("System");



    //    //加载事件系统
    //    gameObject = ResourceManager.GetInstance().Load<GameObject>("UI/EventSystem");
    //    //跨场景时不销毁
    //    GameObject.DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 通过对应枚举，获取对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(UILevel layer)
    {
        switch(layer)
        {
            case UILevel.Bot:
                return Bot;
            case UILevel.Mid:
                return Mid;
            case UILevel.Top:
                return Top;
            case UILevel.System:
                return System;
        }
        return null;
    }
    /// <summary>
    /// ShowUI的重载，用于显示继承自老UIBase的UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName">UI名字</param>
    /// <param name="callBack"></param>
    public void ShowOldUI<T>(string uiName, UnityAction<T> callBack = null) where T : UIBase 
    {
        if (LoadedOldUIDic.ContainsKey(uiName))
        {
            Debug.Log("UI已经存在");
            //LoadedOldUIDic[uiName].ShowMe();
            if (callBack != null)
            {
                callBack(LoadedOldUIDic[uiName] as T);
            }
            //避免面板重复加载 如果存在该面板 直接显示 调用回调函数后 直接Return 不再处理后面的异步加载逻辑
            return;

        }

        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/" + uiName, (obj) =>
        {

            //加载完毕后，修正UI位置
            //obj.transform.SetParent(canvas);

            //obj.transform.localPosition = Vector3.zero;
            //obj.transform.localScale = Vector3.one;

            //得到UI身上的面板脚本
            T panel = obj.GetComponent<T>();
            //UI创建完成后的逻辑
            if (callBack != null)
            {
                callBack(panel);
            }
            //把UI存起来
            LoadedOldUIDic.Add(uiName, panel);
            //把他作为Cnavas的子对象
            //并且要设置它的相对位置
        });
        //Debug.Log("UIShow");
    }
    /// <summary>
    /// 显示面板，用于显示继承NewBaseUI的UI
    /// </summary>
    /// <typeparam name="T">UI脚本类型</typeparam>
    /// <param name="uiName">UI名</param>
    /// <param name="layer">在Canvas中的显示层级</param>
    /// <param name="callBack">UI加载/显示完成后的逻辑</param>
    public void ShowNewUI<T>(string uiName,UnityAction<T> callBack=null)where T:NewUIBase
    {
        if(LoadedNewUIDic.ContainsKey(uiName))
        {
            LoadedNewUIDic[uiName].ShowMe();
            if(callBack!=null)
            {
                callBack(LoadedNewUIDic[uiName] as T);
            }
            //避免面板重复加载 如果存在该面板 直接显示 调用回调函数后 直接Return 不再处理后面的异步加载逻辑
            return;
        }

        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/"+uiName, (obj)=>
        {
            #region 弃用
            //Transform father=Bot;
            //switch(layer)
            //{
            //    case UILevel.Mid:
            //        father=Mid;
            //        break;
            //    case UILevel.Top:
            //        father=Top;
            //        break;
            //    case UILevel.System:
            //        father=System;
            //        break;
            //}
            //设置父对象 设置相对位置和大小
            #endregion
            obj.transform.SetParent(canvas);

            obj.transform.localPosition=Vector3.zero;
            obj.transform.localScale=Vector3.one;

            //(obj.transform as RectTransform).offsetMax = Vector2.zero;
            //(obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到UI身上的面板脚本
            T panel=obj.GetComponent<T>();
            //UI创建完成后的逻辑
            if(callBack!=null)
            {
                callBack(panel);
            }
            panel.ShowMe();
            //把UI存起来
            LoadedNewUIDic.Add(uiName,panel);
            //把他作为Cnavas的子对象
            //并且要设置它的相对位置
        });
    }
    /// <summary>
    /// 隐藏UI
    /// </summary>
    /// <param name="uiName">UI名称</param>
    public void HideUI(string uiName)
    {
        //如果已加载UI字典中存在该UI，则证明该UI已经加载
        if(LoadedNewUIDic.ContainsKey(uiName))
        {
            LoadedNewUIDic[uiName].HideMe();
            //销毁UI
            GameObject.Destroy(LoadedNewUIDic[uiName].gameObject);
            LoadedNewUIDic.Remove(uiName);
        }
        else if(LoadedOldUIDic.ContainsKey(uiName))
        {
            //销毁UI
            GameObject.Destroy(LoadedOldUIDic[uiName].gameObject);
            LoadedOldUIDic.Remove(uiName);
        }
    }
    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public T GetUI<T>(string uiName)where T:NewUIBase
    {
        if(LoadedNewUIDic.ContainsKey(uiName))
        {
            return LoadedNewUIDic[uiName] as T;
        }
        return null;
    }
    /// <summary>
    /// 给控件添加自定义事件触发器
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        //假如挂载对象上存在EventTrigger组件，则获取
        EventTrigger trigger=control.GetComponent<EventTrigger>();
        //若没有，则为其添加EventTrigger
        if(trigger==null)
        {
            trigger=control.gameObject.AddComponent<EventTrigger>();
        }
        //自定义事件类型
        EventTrigger.Entry entry=new EventTrigger.Entry();
        entry.eventID=type;
        //自定义回调函数
        entry.callback.AddListener(callBack);
        //添加到EventTrigger中
        trigger.triggers.Add(entry);
    }
}
