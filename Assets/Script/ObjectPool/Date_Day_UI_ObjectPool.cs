using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Date_Day_UI_ObjectPool : BasePool<TextMeshProUGUI>
{
    private void Awake()
    {
        initialize();
    }
}
