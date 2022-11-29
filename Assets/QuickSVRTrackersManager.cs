//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using Valve.VR;

//namespace QuickVR.SVR
//{

//    public class QuickSVRTrackersManager : MonoBehaviour
//    {

//        #region PROTECTED ATTRIBUTES

//        /// <summary>
//        /// Relates a SteamVR_TrackedObject with the corresponding QuickVRNode
//        /// </summary>
//        protected class QuickSVRTracker
//        {
//            public SteamVR_TrackedObject _device;
//            public QuickVRNode _vrNode;

//            public virtual void Update()
//            {
//                if (_vrNode)
//                {
//                    _vrNode.SetTracked(true);
//                    _vrNode.transform.localPosition = _device.transform.localPosition;
//                    _vrNode.transform.localRotation = _device.transform.localRotation;
//                }
//            }
//        }

//        protected Dictionary<SteamVR_TrackedObject.EIndex, QuickSVRTracker> _connectedTrackers = new Dictionary<SteamVR_TrackedObject.EIndex, QuickSVRTracker>();
//        protected static Transform _svrManagerRoot = null;

//        #endregion

//        #region CREATION AND DESTRUCTION

//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//        protected static void Init()
//        {
//            QuickVRManager vrManager = QuickSingletonManager.GetInstance<QuickVRManager>();
//            _svrManagerRoot = vrManager.transform.CreateChild("__QuickSVRManager__");
//            _svrManagerRoot.GetOrCreateComponent<QuickSVRTrackersManager>();
//        }

//        protected virtual void OnEnable()
//        {
//            QuickVRManager.OnPostCalibrate += CalibrateVRNodesTrackers;
//            QuickVRManager.OnPostUpdateVRNodes += UpdateVRNodesTrackers;
//        }

//        protected virtual void OnDisable()
//        {
//            QuickVRManager.OnPostCalibrate -= CalibrateVRNodesTrackers;
//            QuickVRManager.OnPostUpdateVRNodes -= UpdateVRNodesTrackers;
//        }

//        protected virtual void TrackerConnected(SteamVR_TrackedObject.EIndex trackerIndex)
//        {
//            Transform t = transform.CreateChild("Tracker_" + trackerIndex.ToString());
//            SteamVR_TrackedObject tObject = t.GetOrCreateComponent<SteamVR_TrackedObject>();
//            tObject.index = trackerIndex;

//            QuickSVRTracker tracker = new QuickSVRTracker();
//            tracker._device = tObject;
//            _connectedTrackers[trackerIndex] = tracker;

//            //For debug purposes only
//            Transform tDebug = t.CreateChild("__Debug__");
//            MeshFilter mFilter = tDebug.GetOrCreateComponent<MeshFilter>();
//            mFilter.sharedMesh = QuickUtils.GetUnityPrimitiveMesh(PrimitiveType.Cube);
//            tDebug.GetOrCreateComponent<MeshRenderer>();
//            tDebug.localScale = Vector3.one * 0.1f;
//        }

//        protected virtual void TrackerDisconnected(SteamVR_TrackedObject.EIndex trackerIndex)
//        {
//            QuickSVRTracker tracker = _connectedTrackers[trackerIndex];
//            DestroyImmediate(tracker._device.gameObject);
//            if (tracker._vrNode)
//            {
//                tracker._vrNode.SetTracked(false);
//            }

//            _connectedTrackers.Remove(trackerIndex);
//        }

//        #endregion

//        #region GET AND SET

//        protected virtual void CalibrateVRNodesTrackers()
//        {
//            List<SteamVR_TrackedObject.EIndex> keys = new List<SteamVR_TrackedObject.EIndex>(_connectedTrackers.Keys);
//            List<QuickSVRTracker> values = new List<QuickSVRTracker>(_connectedTrackers.Values);

//            QuickVRPlayArea playArea = QuickSingletonManager.GetInstance<QuickVRPlayArea>();
//            int numTrackers = _connectedTrackers.Count;

//            Debug.Log("NUM SVR TRACKERS = " + numTrackers);

//            if (numTrackers == 1)
//            {
//                //This tracker is the hips. 
//                values[0]._vrNode = playArea.GetVRNode(HumanBodyBones.Hips);
//            }
//            else if (numTrackers == 3)
//            {
//                //We have one tracker for the hips and two for the feet. 
//                //The hips is the one that has a higher Y coordinate. 
//                List<int> ids = new List<int>{ 0, 1, 2 };
//                int hipsID = ids[0];
//                for (int i = 1; i < ids.Count; i++)
//                {
//                    if (values[ids[i]]._device.transform.position.y > values[hipsID]._device.transform.position.y)
//                    {
//                        hipsID = ids[i];
//                    }
//                }

//                //At the end of the loop, hipsID contains the index of the hipsTracker. 
//                //The remaining two are the feet. Remove it from the ids list. 
//                values[hipsID]._vrNode = playArea.GetVRNode(HumanBodyBones.Hips);
//                ids.Remove(hipsID);

//                QuickVRNode vrNodeHead = playArea.GetVRNode(HumanBodyBones.Head);
//                Vector3 right = Vector3.ProjectOnPlane(vrNodeHead.transform.right, Vector3.up);
//                Vector3 v = Vector3.ProjectOnPlane(values[ids[0]]._device.transform.position - vrNodeHead.transform.position, Vector3.up);
//                if (Vector3.Dot(v, right) >= 0)
//                {
//                    //ids[0] is the right foot; ids[1] is the left foot. 
//                    values[ids[0]]._vrNode = playArea.GetVRNode(HumanBodyBones.RightFoot);
//                    values[ids[1]]._vrNode = playArea.GetVRNode(HumanBodyBones.LeftFoot);
//                }
//                else
//                {
//                    //the other way around. 
//                    values[ids[0]]._vrNode = playArea.GetVRNode(HumanBodyBones.LeftFoot);
//                    values[ids[1]]._vrNode = playArea.GetVRNode(HumanBodyBones.RightFoot);
//                }
//            }
//        }

//        public virtual bool IsTrackerConnected(SteamVR_TrackedObject.EIndex trackerIndex)
//        {
//            return _connectedTrackers.ContainsKey(trackerIndex);
//        }

//        #endregion

//        #region UPDATE

//        protected virtual void Update()
//        {
//            if (OpenVR.System != null)
//            {
//                for (SteamVR_TrackedObject.EIndex i = SteamVR_TrackedObject.EIndex.Device1; i <= SteamVR_TrackedObject.EIndex.Device16; i++)
//                {
//                    if (OpenVR.System.GetTrackedDeviceClass((uint)i) == ETrackedDeviceClass.GenericTracker)
//                    {
//                        if (IsTrackerConnected(i))
//                        {
//                            if (!OpenVR.System.IsTrackedDeviceConnected((uint)i))
//                            {
//                                //The tracker has been disconnected. 
//                                TrackerDisconnected(i);
//                            }
//                        }
//                        else
//                        {
//                            if (OpenVR.System.IsTrackedDeviceConnected((uint)i))
//                            {
//                                //A new tracker has been connected. Register it. 
//                                TrackerConnected(i);
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        protected virtual void UpdateVRNodesTrackers()
//        {
//            foreach (var pair in _connectedTrackers)
//            {
//                pair.Value.Update();
//            }
//        }

//        #endregion

//    }

//}


