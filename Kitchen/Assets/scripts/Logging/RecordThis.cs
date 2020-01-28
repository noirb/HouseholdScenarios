using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this script to any object which should be included in scenario logs.
/// It will automatically register/deregister the object when it's activated
/// or destroyed.
/// </summary>
public class RecordThis : MonoBehaviour {
    public bool recordEvents = false;

    NewtonVR.NVRHand VRHand;

    void Start() {
        var logMgr = ScenarioLogManager.Instance;

        if (recordEvents)
        {
            // check for a hand if we want to record events
            VRHand = gameObject.GetComponent<NewtonVR.NVRHand>();
        }

        // must check this in case application is shutting down & manager was already destroyed
        if (logMgr != null)
        {
            logMgr.Track(this.gameObject);
            if (VRHand != null)
            {
                logMgr.Track(VRHand);
            }
        }
    }

    void Update()
    {
        if (!recordEvents || !ScenarioLogManager.Instance.recording)
            return;

        if (VRHand.HoldButtonDown)
        {
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "NVRHand_HoldButtonDown"); // record a "grasp" for this frame
        }
        else if (VRHand.HoldButtonUp)
        {
            ScenarioLogManager.Instance.LogEvent(this.gameObject, "NVRHand_HoldButtonUp"); // record a "release" for this frame
        }
    }

    void OnDestroy()
    {
        var logMgr = ScenarioLogManager.Instance;

        // must check this in case application is shutting down & manager was already destroyed
        if (logMgr != null)
        {
            ScenarioLogManager.Instance.UnTrack(this.gameObject);
            if (VRHand != null)
            {
                logMgr.Track(VRHand);
            }
        }
    }
}
