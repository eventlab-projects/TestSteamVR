using UnityEngine;

using QuickVR;

public class TestCopyPose : MonoBehaviour
{

    protected QuickCopyPoseBase _copyPose = null;

    void Start()
    {
        //Get or create a QuickCopyBase component in this object. 
        _copyPose = gameObject.GetOrCreateComponent<QuickCopyPoseBase>();

        //The source animator is the one that has the QuickUnityVR component, as we 
        //are copying the avatar that has the tracking. 
        _copyPose.SetAnimatorSource(FindObjectOfType<QuickUnityVR>().GetComponent<Animator>());

        //The target avatar is this animator. 
        _copyPose.SetAnimatorDest(GetComponent<Animator>());
    }

    void Update()
    {
        //Update the copy pose
        _copyPose.CopyPose();
    }

}