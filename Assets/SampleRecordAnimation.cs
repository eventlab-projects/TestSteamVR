using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickVR.Samples.RecordAnimation
{

    public class SampleRecordAnimation : MonoBehaviour
    {

        #region PUBLIC ATTRIBUTES

        public bool _showTrackers = true;

        public SampleRecordAnimationUI _gui = null;

        #endregion

        #region PROTECTED ATTRIBUTES

        protected bool _prevShowTrackers = true;

        protected QuickHumanBodyBones[] debugBones =
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

        protected QuickVRPlayArea _vrPlayArea = null;
        protected QuickVRInteractionManager _interactionManager = null;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            _vrPlayArea = QuickSingletonManager.GetInstance<QuickVRPlayArea>();
            ShowTrackers();
        }

        protected virtual void ShowTrackers()
        {
            foreach (QuickHumanBodyBones boneID in debugBones)
            {
                _vrPlayArea.GetVRNode(boneID)._showModel = _showTrackers;
            }

            _prevShowTrackers = _showTrackers;
        }

        // Update is called once per frame
        void Update()
        {
            if (_showTrackers != _prevShowTrackers)
            {
                ShowTrackers();
            }

            if (InputManager.GetButtonDown("ShowGUI"))
            {
                _gui.gameObject.SetActive(!_gui.gameObject.activeSelf);
                _interactionManager.GetVRInteractorHandRight().SetInteractorEnabled(InteractorType.UI, _gui.gameObject.activeSelf);
            }
        }
    }

}


