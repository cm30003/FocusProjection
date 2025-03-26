using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone_Date_UI_ObjectPool : BasePool<Button>
{
    private void Awake()
    {
        initialize();
    }
}
