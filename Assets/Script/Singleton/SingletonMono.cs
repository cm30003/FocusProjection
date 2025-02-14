using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if(instance==null)//当泛型单例未被实例化时
        {
            instance = this as T;//将泛型单例实例化
        }
        else//当泛型已经被实例化，则摧毁掉（方便确保单例的唯一性）
        {
            Destroy(gameObject);
        }
    }
    
}
