using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseManager<InputManager>
{
    public InputManager()
    {
        MonoManager.GetInstance().AddUpdateLisener(InputManager_Update);
    }

    private void InputManager_Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //
        }
    }
}
