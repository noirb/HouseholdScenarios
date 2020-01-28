using UnityEngine;
using System.Collections;
using NewtonVR;
using System;

/// <summary>
/// An "empty" avatar driver which does not apply anything to the attached head/hands/player
/// </summary>
public class NullAvatarDriver : NVRDriver {

    void Awake()
    {
        foreach (var hand in Hands)
        {
            hand.DoInitialize();
        }
    }
    public override NVRButtonInputs GetButtonState(NVRHand hand, NVRButtonID button)
    {
        throw new NotImplementedException();
    }

    public override string GetDeviceName(NVRHead head)
    {
        return "Head";
    }

    public override string GetDeviceName(NVRHand hand)
    {
        return "Hand";
    }

    public override Vector3 GetVelocity(NVRHand hand)
    {
        Rigidbody rb = hand.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
            return rb.velocity;
        else
            return new Vector3(0, 0, 0);
    }

    public override void LongHapticPulse(NVRHand hand, float seconds)
    {
        return; // do nothing
    }

    public override void SetButtonStates(NVRHand hand)
    {
        return; // do nothing
    }

    public override void TriggerHapticPulse(NVRHand hand, ushort durationMicroSec)
    {
        return; // do nothing
    }
}
