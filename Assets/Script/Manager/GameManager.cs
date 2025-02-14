using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    public float Standard_Second = 1;
    /// <summary>
    /// ��ʱ��
    /// </summary>
    /// <param name="time">��ʱ��</param>
    /// <param name="speed">�ٶ�</param>
    public void Update_Timer(ref float time,float speed)
    {
        Standard_Second -= Time.deltaTime;
        if (Standard_Second <= 0)
        {
            Standard_Second = 1;
            time-= speed;
        }
    }
    public void FixedUpdate_Timer(ref float time,float speed)
    {
        Standard_Second -= Time.fixedDeltaTime;
        if (Standard_Second <= 0)
        {
            Standard_Second = 1;
            time -= speed;
        }
    }
}
