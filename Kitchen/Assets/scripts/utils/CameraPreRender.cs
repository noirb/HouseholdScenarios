using UnityEngine;

/// <summary>
/// Provides notification to any object subscribed to onPreCull
/// that a camera is just about to render.
/// </summary>
public class CameraPreRender : MonoBehaviour {

    public delegate void PreCullEvent();
    public static PreCullEvent onPreCull;

    // called just before each camera renders
    void OnPreCull()
    {
        if (onPreCull != null)
        {
            onPreCull();
        }
    }

    public static void AddToAllCameras()
    {
        // check to see if any cameras are missing a CameraPreRender component and add it if so
        var cameras = FindObjectsOfType<Camera>();
        foreach (var camera in cameras)
        {
            var preRenderComponent = camera.gameObject.GetComponent<CameraPreRender>();
            if (preRenderComponent == null)
            {
                camera.gameObject.AddComponent<CameraPreRender>();
            }
        }
    }
}
