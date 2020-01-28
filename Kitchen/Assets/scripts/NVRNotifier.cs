using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Notifies NVRHand of trigger events from other objects
/// Allows non-child colliders to be used for grasping.
/// </summary>
public class NVRNotifier : MonoBehaviour {
    private List<NewtonVR.NVRHand> hands;

    public NVRNotifier()
    {
        hands = new List<NewtonVR.NVRHand>();
    }

    public void Subscribe(NewtonVR.NVRHand hand)
    {
        hands.Add(hand);
    }

    public void Unsubscribe(NewtonVR.NVRHand hand)
    {
        hands.Remove(hand);
    }

    void OnTriggerEnter(Collider collider)
    {
        foreach (var hand in hands)
        {
            hand.OnOtherTriggerEnter(ref collider);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        foreach (var hand in hands)
        {
            hand.OnOtherTriggerStay(ref collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        foreach (var hand in hands)
        {
            hand.OnOtherTriggerExit(ref collider);
        }
    }
}
