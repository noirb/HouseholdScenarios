using UnityEngine;
using System.Collections;

public class IroningBoard : MonoBehaviour {
    public NewtonVR.NVRInteractableRotator rotator;
    public Rigidbody rigidBody;
    public float pinThreshold = 10.0f; // gravity is disabled at (pinAngle +/- pinThreshold)
    public float pinAngle = 180.0f;    // target angle to "latch" onto

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Mathf.Abs(pinAngle - rotator.CurrentAngle) < pinThreshold)
        {
            rigidBody.useGravity = false;
            rigidBody.velocity = Vector3.zero;
        }
        else
        {
            rigidBody.useGravity = true;
        }
	}
}
