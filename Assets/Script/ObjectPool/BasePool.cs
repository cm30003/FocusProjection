using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BasePool<T> : MonoBehaviour where T:Component
{
    [SerializeField] protected T Prefab;
    [SerializeField] int defalutSize=31;

    ObjectPool<T> pool;

    public int ActiveCount => pool.CountActive;
    public int InactiveCount => pool.CountInactive;
    public int TotalCount => pool.CountAll;
    /// <summary>
    /// 对象池初始化
    /// </summary>
    public void initialize(bool collectionCheck=true)=>
        pool = new ObjectPool<T>(OnCreatePool, OnGetPool, OnReleasePool, OnDestroyPool, collectionCheck, defalutSize);
    //当元素由于池达到最大大小而无法返回到池时调用
    protected virtual void OnDestroyPool(T OBJ)
    {
        Destroy(OBJ.gameObject);
    }
    //返回池
    protected virtual void OnReleasePool(T OBJ)
    {
        OBJ.gameObject.SetActive(false);
    }
    //获取池
    protected virtual void OnGetPool(T OBJ)
    {
        OBJ.gameObject.SetActive(true);
    }
    /// <summary>
    /// 创建对象池中的元素
    /// </summary>
    /// <returns></returns>
    protected virtual T OnCreatePool()
    {
        var prefab = Instantiate(Prefab,transform);

        return prefab;
    }

    public T Get() => pool.Get();
    public void Release(T OBJ)
    {
        pool.Release(OBJ);
    }
    public void Clear()
    {
        pool.Clear();
    }
}
