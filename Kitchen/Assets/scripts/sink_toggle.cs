using UnityEngine;
using System.Collections;

/// <summary>
/// Used to turn sink particle effects on and off
/// </summary>
public class sink_toggle : MonoBehaviour {
    [Tooltip("Objects whose rotation we should track for measuring on/off state")]
    public NewtonVR.NVRInteractableRotator handle;
    [Tooltip("Angle over which effects should be enabled")]
    public float threshold_angle = 20;

    ParticleSystem[] faucet_flow;

	// Use this for initialization
	void Start () {
        faucet_flow = GetComponentsInChildren<ParticleSystem>();
	}

    // Update is called once per frame
    void Update() {
        foreach (var fx in faucet_flow)
        {
            var em = fx.emission;
            if (handle.CurrentAngle > threshold_angle)
            {
                em.enabled = true;
            }
            else
            {
                em.enabled = false;
            }
        }
	}
}
