using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(LogPlayer))]
public class PlaybackUI : MonoBehaviour {
    public Button playPauseButton;
    public InputField logFileField;
    public Slider logTimeSlider;
    public Text logTimeTextValue;
    public Toggle kinematicPlayback;
    public CanvasGroup settingsGroup;

    LogPlayer logPlayer;

    // Use this for initialization
    void Start() {
        if (logPlayer == null)
        {
            logPlayer = GetComponent<LogPlayer>();
        }
        logPlayer.onStateChanged += onPlayerStateChanged;
        logPlayer.makeKinematic = kinematicPlayback.isOn;
        if (logFileField.text.Length > 0)
        {
            OnLogFileSet(); // init log player if we have a default filename set
        }
    }

    void onPlayerStateChanged(LogPlayer.LogPlaybackState newState)
    {
        switch (newState)
        {
            case LogPlayer.LogPlaybackState.NoContent:
                break;
            case LogPlayer.LogPlaybackState.Playing:
                playPauseButton.GetComponentInChildren<Text>().text = "Pause";
                settingsGroup.interactable = false;
                break;
            case LogPlayer.LogPlaybackState.Paused:
                playPauseButton.GetComponentInChildren<Text>().text = "Play";
                settingsGroup.interactable = true;
                break;
            case LogPlayer.LogPlaybackState.Stopped:
                playPauseButton.GetComponentInChildren<Text>().text = "Play";
                settingsGroup.interactable = true;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (logPlayer.currentState != LogPlayer.LogPlaybackState.NoContent)
        {
            logTimeSlider.value = logPlayer.current_entry;
            logTimeTextValue.text = logPlayer.logTime.ToString();
        }
    }


    public void OnPlayPauseClicked()
    {
        if (logPlayer.playing)
        {
            logPlayer.Pause();
        }
        else
        {
            logPlayer.Play();
        }
        
    }

    public void OnStopClicked()
    {
        if (logPlayer.currentState != LogPlayer.LogPlaybackState.Stopped)
        {
            logPlayer.Stop();
        }
    }

    public void OnNextClicked()
    {
        if (logPlayer.playing || logPlayer.currentState == LogPlayer.LogPlaybackState.NoContent)
        {
            return;
        }

        logPlayer.NextFrame();
    }

    public void OnPrevClicked()
    {
        if (logPlayer.playing || logPlayer.currentState == LogPlayer.LogPlaybackState.NoContent)
        {
            return;
        }

        logPlayer.PrevFrame();
    }


    public void OnLogFileSet()
    {

        if (!logPlayer.LoadLog(logFileField.text))
        {
            logFileField.GetComponentInChildren<Text>().color = Color.red;
            playPauseButton.interactable = false;
        }
        else
        {
            logFileField.GetComponentInChildren<Text>().color = Color.cyan;
            logTimeSlider.maxValue = logPlayer.length;
            playPauseButton.interactable = true;
        }
    }

    public void OnKinematicToggled()
    {
        logPlayer.makeKinematic = kinematicPlayback.isOn;
    }

}
