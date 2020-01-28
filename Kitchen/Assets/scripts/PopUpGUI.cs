using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpGUI : MonoBehaviour {
    public delegate void OnTextSubmit(string result);

    public OnTextSubmit onTextSubmitted;
    public Text labelText;
    public InputField inputField;
    public Text descriptionText;
    [Space]
    public GameObject UIRoot;

    public string description
    {
        get { return descriptionText.text; }
        set { descriptionText.text = value; }
    }

    public string inputLabel
    {
        get { return labelText.text; }
        set { labelText.text = value; }
    }

    public string value
    {
        get { return inputField.text; }
        set { inputField.text = value; }
    }

    public void Show()
    {
        UIRoot.SetActive(true);
    }

    public void Hide()
    {
        UIRoot.SetActive(false);
    }

    public void OnTextChanged()
    {

    }

    public void OnTextSubmitted()
    {
        if (this.onTextSubmitted != null)
        {
            onTextSubmitted(inputField.text);
        }
    }
}
