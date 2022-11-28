using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using QuickVR;
using Valve.VR;

public class TestInputManager : MonoBehaviour
{

    // a reference to the action
    //public SteamVR_Action_Boolean _triggerPressed = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    // a reference to the hand
    public SteamVR_Input_Sources _handType;

    //public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    Debug.Log("Trigger is up");
    //    //Sphere.GetComponent<MeshRenderer>().enabled = false;
    //}
    //public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    Debug.Log("Trigger is down");
    //    //Sphere.GetComponent<MeshRenderer>().enabled = true;
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _triggerPressed.AddOnStateDownListener(TriggerDown, _handType);
    //    _triggerPressed.AddOnStateUpListener(TriggerUp, _handType);
    //}

    //public virtual void Start()
    //{
    //    SteamVR_Actions.default_GrabPinch.AddOnStateDownListener(TriggerPressed, SteamVR_Input_Sources.Any);
    //}

    //private void TriggerPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    //{
    //    //Do something
    //    Debug.Log("PRESSED!!!");
    //}

    public virtual void Test()
    {
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("PEPITO 1!!!");
        }
        if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("PEPITO 2!!!");
        }
    }

    void Update()
    {
        Test();
        //SteamVR_Actions._default.
        //Debug.Log(InputManagerVR.GetKeyDown(InputManagerVR.ButtonCodes.LeftTriggerPress));
    }
}
