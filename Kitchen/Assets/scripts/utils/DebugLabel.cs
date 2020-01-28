using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// This adds world-space billboarding text labels to any object it's attached to.
/// The properties field should specify a property or field name accessible from the GameObject. If it can be found, its value
/// will be printed on the label and updated each frame.
/// 
/// Property names should be of the form:
///     Component.Property.Sub_Property  --> Will effectively call this.GetComponent(Component).Property.Sub_Property.ToString()
///     Property.Sub_Property            --> Will effectively call this.GameObject.Property.Sub_Property.ToString()
/// 
/// Examples:
///     transform.position               --> Prints the current GameObject position
///     transform.position.x             --> Prints just the x coordinate of the position
///     NVRHand.HoldButtonPressed        --> If an NVRHand component is attached to the GameObject, will print GetComponent("NVRHand").HoldButtonPressed
///
/// Because values are retrieved through Reflection, it's also possible to specify private or protected members.
/// </summary>
public class DebugLabel : MonoBehaviour {
    [Tooltip("List of properties to display in label, of the form 'Property.Sub_Property'. Will search components of the root GameObject as well.")]
    public string[] properties;

    private GameObject labelGUI;
    private Dictionary<string, Text> propertyMap;
    private Renderer targetRenderer;

    object GetPropertyValueInComponents(GameObject obj, string propertyName)
    {
        if (obj == null) { return null; }

        // does propertyName reference a component?
        Component component = null;
        if (propertyName.IndexOf('.') > 0)
        {
            component = obj.GetComponent(propertyName.Substring(0, propertyName.IndexOf('.')));
        }

        if (component != null)
        {
            return GetNestedPropertyValue(component, propertyName.Substring(propertyName.IndexOf('.')+1));
        }
        else
        {
            return GetNestedPropertyValue(obj, propertyName);
        }
    }

    /// <summary>
    /// Searches obj (and all sub-objects of obj) for the given property and returns its value
    /// </summary>
    /// <param name="obj">The root object to search for the property</param>
    /// <param name="propertyName">
    ///     A property name of the form: "property.sub_property.sub_sub_property".
    ///     Root property MUST exist on obj!
    /// </param>
    /// <returns>The property if it exists, otherwise null</returns>
    object GetNestedPropertyValue(object obj, string propertyName)
    {
        if (obj == null) { return null; }

        var props = propertyName.Split('.');
        foreach (var prop in props)
        {
            PropertyInfo pinfo = obj.GetType().GetProperty(prop, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo finfo = obj.GetType().GetField(prop, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (pinfo != null)
            {
                obj = pinfo.GetValue(obj, null);
            }
            else if (finfo != null)
            {
                obj = finfo.GetValue(obj);
            }
            else
            {
                Debug.LogWarning("Could not find " + propertyName + ". " + obj.GetType().Name + " has no field or property named '" + prop + "'");

                return null; // hit an object in the tree with a missing property
            }
        }

        return obj;
    }

    // Use this for initialization
    void Start () {

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

        // subscribe to pre-render event so we can billboard to multiple cameras
        CameraPreRender.onPreCull += PreRender;

        propertyMap = new Dictionary<string, Text>();

        labelGUI = new GameObject(this.gameObject.name + " DebugLabel", typeof(CanvasRenderer), typeof(Canvas), typeof(CanvasScaler));
        labelGUI.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        var rt = labelGUI.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0.25f, 0.25f);
        rt.position = this.transform.position;
        labelGUI.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 50;
        labelGUI.GetComponent<CanvasScaler>().referencePixelsPerUnit = 1;
        labelGUI.transform.position = this.transform.position;

        float offset = 0;
        float offset_inc = 0.055f;
        foreach (var prop in properties)
        {
            var child = new GameObject(this.gameObject.name + " PropertyLabel", typeof(Canvas), typeof(Text));
            child.transform.parent = labelGUI.transform;
            child.transform.localPosition = Vector3.zero;

            var child_rt = child.GetComponent<RectTransform>();
            child_rt.sizeDelta = labelGUI.GetComponent<RectTransform>().sizeDelta;
            child_rt.anchorMin = new Vector2(0.5f, 1.0f);
            child_rt.anchorMax = new Vector2(0.5f, 1.0f);
            child_rt.localPosition = Vector3.zero;
            child_rt.anchoredPosition = new Vector2(0, offset * offset_inc);
            offset += 1.0f;
            child_rt.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            var child_text = child.GetComponent<Text>();
            child_text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            child_text.fontSize = 1;
            child_text.text = GetPropertyValueInComponents(this.gameObject, prop).ToString();
            child_text.horizontalOverflow = HorizontalWrapMode.Overflow;
            child_text.verticalOverflow = VerticalWrapMode.Overflow;

            propertyMap.Add(prop, child_text);
        }

        targetRenderer = this.gameObject.GetComponent<Renderer>();
    }

    void OnDestroy()
    {
        CameraPreRender.onPreCull -= PreRender;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (targetRenderer != null)
        {
            labelGUI.transform.position = this.transform.position + Vector3.up * targetRenderer.bounds.extents.y;
        }
        else
        {
            labelGUI.transform.position = this.transform.position;
        }

        foreach (var property in propertyMap)
        {
            var val = GetPropertyValueInComponents(this.gameObject, property.Key);
            if (val != null)
            {
                string propName = "";
                var ind = property.Key.LastIndexOf('.');
                if (ind > 0)
                {
                    propName += property.Key.Substring(ind, property.Key.Length - ind);
                }
                else
                {
                    propName += property.Key;
                }
                property.Value.text = propName + " : " + val.ToString();
            }
        }
    }

    void PreRender()
    {
        Vector3 difference = Camera.current.transform.position - labelGUI.transform.position;
        labelGUI.transform.LookAt(labelGUI.transform.position - difference, Camera.current.transform.up);
    }
}
