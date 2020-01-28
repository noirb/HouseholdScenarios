using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Renders a text label floating above some object
/// </summary>
public class FloatingLabel : MonoBehaviour
{
    private GameObject labelGUI;
    private Text labelText;
    private Renderer targetRenderer;

    public string text
    {
        get { return labelText.text; }
        set { labelText.text = value; }
    }

    // Use this for initialization
    void Start()
    {
        // subscribe to pre-render event so we can billboard to multiple cameras
        CameraPreRender.AddToAllCameras();
        CameraPreRender.onPreCull += PreRender;

        labelGUI = new GameObject(this.gameObject.name + " ROSLabel", typeof(CanvasRenderer), typeof(Canvas), typeof(CanvasScaler));
        labelGUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        var rt = labelGUI.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0.25f, 0.25f);
        rt.position = this.transform.position;
        labelGUI.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 50;
        labelGUI.GetComponent<CanvasScaler>().referencePixelsPerUnit = 1;
        labelGUI.transform.position = this.transform.position;

        var child = new GameObject(this.gameObject.name + " TextLabel", typeof(Canvas), typeof(Text));
        child.transform.parent = labelGUI.transform;
        child.transform.localPosition = Vector3.zero;

        var child_rt = child.GetComponent<RectTransform>();
        child_rt.sizeDelta = labelGUI.GetComponent<RectTransform>().sizeDelta;
        child_rt.anchorMin = new Vector2(0.5f, 1.0f);
        child_rt.anchorMax = new Vector2(0.5f, 1.0f);
        child_rt.anchoredPosition = new Vector2(0.0f, 0.0f);
        child_rt.localPosition = Vector3.zero;
        child_rt.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        labelText = child.GetComponent<Text>();
        labelText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        labelText.fontSize = 1;
        labelText.horizontalOverflow = HorizontalWrapMode.Overflow;
        labelText.verticalOverflow = VerticalWrapMode.Overflow;

        targetRenderer = this.gameObject.GetComponent<Renderer>();
    }

    void OnDestroy()
    {
        CameraPreRender.onPreCull -= PreRender;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetRenderer != null)
        {
            labelGUI.transform.position = targetRenderer.bounds.center + (1.2f * Vector3.up * targetRenderer.bounds.extents.y);
        }
        else
        {
            labelGUI.transform.position = this.transform.position;
        }
    }

    void PreRender()
    {
        Vector3 difference = Camera.current.transform.position - labelGUI.transform.position;
        labelGUI.transform.LookAt(labelGUI.transform.position - difference, Camera.current.transform.up);
    }
}
