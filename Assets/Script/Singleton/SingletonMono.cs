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
        if(instance==null)//�����͵���δ��ʵ����ʱ
        {
            instance = this as T;//�����͵���ʵ����
        }
        else//�������Ѿ���ʵ��������ݻٵ�������ȷ��������Ψһ�ԣ�
        {
            Destroy(gameObject);
        }
    }
    
}
