using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

namespace QuickVR.SVR
{

    public class QuickSVRTrackersManager : MonoBehaviour
    {

        /// <summary>
        /// Relates a SteamVR_TrackedObject with the corresponding QuickVRNode
        /// </summary>
        protected struct QuickSVRTracker
        {
            public SteamVR_TrackedObject _device;
            public QuickVRNode _vrNode;

            public void Update()
            {
                if (_vrNode)
                {
                    _vrNode.SetTracked(true);
                    _vrNode.transform.localPosition = _device.transform.localPosition;
                    _vrNode.transform.localRotation = _device.transform.localRotation;
                }
            }
        }

        #region PROTECTED ATTRIBUTES

        protected Dictionary<SteamVR_TrackedObject.EIndex, QuickSVRTracker> _connectedTrackers = new Dictionary<SteamVR_TrackedObject.EIndex, QuickSVRTracker>();

        #endregion

        #region CREATION AND DESTRUCTION

        protected virtual void OnEnable()
        {
            QuickVRManager.OnPostUpdateVRNodes += UpdateVRNodesTrackers;
        }

        protected virtual void OnDisable()
        {
            QuickVRManager.OnPostUpdateVRNodes -= UpdateVRNodesTrackers;
        }

        protected virtual void TrackerConnected(SteamVR_TrackedObject.EIndex trackerIndex)
        {
            Transform t = transform.CreateChild("Tracker_" + trackerIndex.ToString());
            SteamVR_TrackedObject tObject = t.GetOrCreateComponent<SteamVR_TrackedObject>();
            tObject.index = trackerIndex;

            QuickSVRTracker tracker = new QuickSVRTracker();
            tracker._device = tObject;
            _connectedTrackers[trackerIndex] = tracker;

            //For debug purposes only
            Transform tDebug = t.CreateChild("__Debug__");
            MeshFilter mFilter = tDebug.GetOrCreateComponent<MeshFilter>();
            mFilter.sharedMesh = QuickUtils.GetUnityPrimitiveMesh(PrimitiveType.Cube);
            tDebug.GetOrCreateComponent<MeshRenderer>();
            tDebug.localScale = Vector3.one * 0.1f;
        }

        protected virtual void TrackerDisconnected(SteamVR_TrackedObject.EIndex trackerIndex)
        {
            QuickSVRTracker tracker = _connectedTrackers[trackerIndex];
            DestroyImmediate(tracker._device.gameObject);
            if (tracker._vrNode)
            {
                tracker._vrNode.SetTracked(false);
            }

            _connectedTrackers.Remove(trackerIndex);
        }

        #endregion

        #region GET AND SET

        public virtual bool IsTrackerConnected(SteamVR_TrackedObject.EIndex trackerIndex)
        {
            return _connectedTrackers.ContainsKey(trackerIndex);
        }

        #endregion

        #region UPDATE

        protected virtual void Update()
        {
            if (OpenVR.System != null)
            {
                for (SteamVR_TrackedObject.EIndex i = SteamVR_TrackedObject.EIndex.Device1; i <= SteamVR_TrackedObject.EIndex.Device16; i++)
                {
                    if (OpenVR.System.GetTrackedDeviceClass((uint)i) == ETrackedDeviceClass.GenericTracker)
                    {
                        if (IsTrackerConnected(i))
                        {
                            if (!OpenVR.System.IsTrackedDeviceConnected((uint)i))
                            {
                                //The tracker has been disconnected. 
                                TrackerDisconnected(i);
                            }
                        }
                        else
                        {
                            if (OpenVR.System.IsTrackedDeviceConnected((uint)i))
                            {
                                //A new tracker has been connected. Register it. 
                                TrackerConnected(i);
                            }
                        }
                    }
                }

                //foreach (var pair in _connectedTrackers)
                //{
                //    Debug.Log(OpenVR.System.GetTrackedDeviceActivityLevel((uint)pair.Key));
                //}
            }
        }

        protected virtual void UpdateVRNodesTrackers()
        {
            foreach (var pair in _connectedTrackers)
            {
                pair.Value.Update();
            }
        }

        #endregion

    }

}


