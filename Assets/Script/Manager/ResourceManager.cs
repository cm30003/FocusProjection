using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// </summary>
public class ResourceManager : BaseManager<ResourceManager>
{
    /// <summary>
    /// 同步加载资源（读取Json文件时，其类型应该为TextAsset类型）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Load<T>(string name)where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个Game0bject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset Audioclip
            return res;
    }
    /// <summary>
    /// 异步加载资源（读取Json文件时，其类型应该为TextAsset类型）
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="name">名字</param>
    /// <param name="callback">回调</param>
    public void LoadAsync<T>(string name,UnityAction<T>callback) where T :Object
    {
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync(name,callback));
    }
    
    //真正的协同程序函数 用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //加载资源
        ResourceRequest r=Resources.LoadAsync<T>(name);
        yield return r;
        //加载完毕后回调
        //为什么要回调：因为异步加载需要在加载完成后使用
        if(r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }
    
}
