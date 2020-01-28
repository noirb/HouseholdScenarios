using UnityEngine;
using System.Collections;

/// <summary>
/// Animation controller for disembodied/floating hands
/// </summary>
public class FloatingHandAnimationController : MonoBehaviour
{
    public enum Hand
    {
        LeftHand,
        RightHand
    };

    public Hand hand;

    private Animator animator;
    private NewtonVR.NVRPlayer nvrPlayer;
    private NewtonVR.NVRHand nvrHand;

    private float grasping = 0;
    private float pointing = 0;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        nvrPlayer = GetComponentInParent<NewtonVR.NVRPlayer>();

        if (nvrPlayer != null)
            nvrHand = hand == Hand.LeftHand ? nvrPlayer.LeftHand : nvrPlayer.RightHand;
    }

    // Update is called once per frame
    void Update()
    {
        if (nvrHand == null)
            return;

        grasping = nvrHand.HoldButtonAxis;
        pointing = nvrHand.UseButtonPressed ? 1.0f : 0.0f;

        animator.SetFloat("grasping", grasping);
        animator.SetFloat("pointing", pointing);
    }
}
