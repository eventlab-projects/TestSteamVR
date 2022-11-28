using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

namespace QuickVR
{

    public class InputManagerSteamVR : BaseInputManager
    {

        public enum ButtonCodes
        {
            LeftTriggerPress,
            LeftGripPress,

            RightTriggerPress,
            RightGripPress,
        }

        #region CREATION AND DESTRUCTION

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        protected static void Init()
        {
            QuickSingletonManager.GetInstance<InputManager>().CreateDefaultImplementation<InputManagerSteamVR>();
        }

        protected override void Awake()
        {
            base.Awake();

            CreateSteamVRBehaviourPose(true);
            CreateSteamVRBehaviourPose(false);
        }

        protected virtual SteamVR_Behaviour_Pose CreateSteamVRBehaviourPose(bool isLeftHand)
        {
            Transform t = transform.CreateChild(isLeftHand? "__LeftHandPose__" : "__RightHandPose__");
            SteamVR_Behaviour_Pose result = t.GetOrCreateComponent<SteamVR_Behaviour_Pose>();
            result.inputSource = isLeftHand? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;

            return result;
        }

        #endregion

        #region GET AND SET

        public override string[] GetButtonCodes()
        {
            List<string> codes = new List<string>();
            foreach (ButtonCodes b in QuickUtils.GetEnumValues<ButtonCodes>())
            {
                codes.Add(b.ToString());
            }

            return GetCodes(codes);
        }

        protected override float ImpGetAxis(string axis)
        {
            return 0;
        }

        protected override bool ImpGetButton(string button)
        {
            bool result = false;

            bool isLeft = button.StartsWith("L");
            if (button.Contains("Trigger"))
            {
                result = SteamVR_Actions._default.GrabPinch.GetStateDown(isLeft ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand);
            }
            else if (button.Contains("Grip"))
            {
                result = SteamVR_Actions._default.GrabGrip.GetStateDown(isLeft ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand);
            }

            return result;
        }

        #endregion



    }

}


