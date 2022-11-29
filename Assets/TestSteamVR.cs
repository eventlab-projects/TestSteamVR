using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QuickVR;

public class TestSteamVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        QuickVRPlayArea playArea = QuickSingletonManager.GetInstance<QuickVRPlayArea>();
        QuickHumanBodyBones[] debugBones =
        {
            QuickHumanBodyBones.Head,
            QuickHumanBodyBones.Hips,
            QuickHumanBodyBones.LeftHand,
            QuickHumanBodyBones.RightHand,
            QuickHumanBodyBones.LeftFoot,
            QuickHumanBodyBones.RightFoot,
            QuickHumanBodyBones.LeftLowerArm,
            QuickHumanBodyBones.RightLowerArm,
            QuickHumanBodyBones.LeftLowerLeg,
            QuickHumanBodyBones.RightLowerLeg
        };

        foreach (QuickHumanBodyBones boneID in debugBones)
        {
            playArea.GetVRNode(boneID)._showModel = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(QuickVRManager._hmdModel);
    }
}
