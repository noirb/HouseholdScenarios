using UnityEngine;
using UnityEngine.UI;

public class RecordingUI : MonoBehaviour {
    public Button       recordButton;
    public InputField   logFileField;
    public Slider       logIntervalSlider;
    public Text         logIntervalTextValue;
    public Text         recordingTime;

    void Start()
    {
        if (ScenarioLogManager.Instance != null)
        {
            logIntervalSlider.value = ScenarioLogManager.Instance.log_interval;
            logIntervalTextValue.text = ScenarioLogManager.Instance.log_interval + "s";
            logFileField.text = ScenarioLogManager.Instance.log_filename;
        }
    }

    void Update()
    {
        if (ScenarioLogManager.Instance != null)
        {
            recordingTime.text = ScenarioLogManager.Instance.GetLogTime().ToString() + "s";
        }
    }

    public void ToggleRecording()
    {
        if (!ScenarioLogManager.Instance)
            return;

        if (ScenarioLogManager.Instance.recording)
        {
            recordButton.transform.GetChild(0).GetComponent<Text>().text = "Start Recording";
            ScenarioLogManager.Instance.StopRecording();
            logIntervalSlider.enabled = true;
            logFileField.enabled = true;
        }
        else
        {
            recordButton.transform.GetChild(0).GetComponent<Text>().text = "Stop Recording";
            ScenarioLogManager.Instance.StartRecording();
            logIntervalSlider.enabled = false;
            logFileField.enabled = false;
        }
    }

    public void OnIntervalSliderValueChanged()
    {
        if (ScenarioLogManager.Instance)
        {
            ScenarioLogManager.Instance.log_interval = logIntervalSlider.value;
            logIntervalTextValue.text = logIntervalSlider.value + "s";
        }
    }

    public void OnLogFileLocationChanged()
    {
        if (ScenarioLogManager.Instance)
        {
            ScenarioLogManager.Instance.log_filename = logFileField.text; /// TODO: Validate filename
        }
    }
}
