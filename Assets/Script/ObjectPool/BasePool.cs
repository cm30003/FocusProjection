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
    /// ����س�ʼ��
    /// </summary>
    public void initialize(bool collectionCheck=true)=>
        pool = new ObjectPool<T>(OnCreatePool, OnGetPool, OnReleasePool, OnDestroyPool, collectionCheck, defalutSize);
    //��Ԫ�����ڳشﵽ����С���޷����ص���ʱ����
    protected virtual void OnDestroyPool(T OBJ)
    {
        Destroy(OBJ.gameObject);
    }
    //���س�
    protected virtual void OnReleasePool(T OBJ)
    {
        OBJ.gameObject.SetActive(false);
    }
    //��ȡ��
    protected virtual void OnGetPool(T OBJ)
    {
        OBJ.gameObject.SetActive(true);
    }
    /// <summary>
    /// ����������е�Ԫ��
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
