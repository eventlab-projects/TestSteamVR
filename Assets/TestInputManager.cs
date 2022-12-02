using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using QuickVR;
using Valve.VR;

public class TestInputManager : MonoBehaviour
{

    protected virtual void Update()
    {
        if (InputManager.GetButtonDown(InputManager.DEFAULT_BUTTON_CONTINUE))
        {
            Debug.Log("HOLA!!!");
        }
    }

}
