using UnityEngine;
using System.Collections;

/// <summary>
/// IK Controller for Arms/Shoulders rig
/// </summary>
public class ArmIK : MonoBehaviour {

    protected Animator animator;

    [Tooltip("If FALSE, IK will not be applied.")]
    public bool ikActive = false;
    [Tooltip("Transform to use for target position of right hand")]
    public Transform rightHandPosTarget = null;
    [Tooltip("Transform to use for target rotation of right hand")]
    public Transform rightHandRotTarget = null;
    [Tooltip("Transform to use for target position of left hand")]
    public Transform leftHandPosTarget = null;
    [Tooltip("Transform to use for target rotation of left hand")]
    public Transform leftHandRotTarget = null;

    public float graspL = 0.0f;
    public float graspR = 0.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float grasp_L, grasp_R;
        float point_L, point_R;

        // for debugging, set graspL/graspR to non-zero values to force a certain pose
        // leave them both at 0 to take a pose from user input.
        if (graspL > 0)
        {
            grasp_L = graspL;
        }
        else
        {
            grasp_L = leftHandRotTarget.GetComponent<NewtonVR.NVRHand>().HoldButtonAxis;
        }
        
        if (graspR > 0)
        {
            grasp_R = graspR;
        }
        else
        {
            grasp_R = rightHandRotTarget.GetComponent<NewtonVR.NVRHand>().HoldButtonAxis;
        }

        point_L = leftHandRotTarget.GetComponent<NewtonVR.NVRHand>().UseButtonPressed ? 1.0f : 0.0f;
        point_R = rightHandRotTarget.GetComponent<NewtonVR.NVRHand>().UseButtonPressed ? 1.0f : 0.0f;

        animator.SetFloat("grasp_left", grasp_L);
        animator.SetFloat("grasp_right", grasp_R);
        animator.SetFloat("point_left", point_L);
        animator.SetFloat("point_right", point_R);
        animator.SetFloat("grasping", grasp_L + grasp_R);
        animator.SetFloat("pointing", point_L + point_R);
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // Set the right hand target position and rotation, if one has been assigned
                if (rightHandPosTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPosTarget.position);
                }
                if (rightHandRotTarget != null)
                {
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandRotTarget.rotation);
                }

                // Set the left hand target position and rotation, if one has been assigned
                if (leftHandPosTarget != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosTarget.position);
                }
                if (leftHandRotTarget != null)
                {
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRotTarget.rotation);
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
