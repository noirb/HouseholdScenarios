using UnityEngine;
using UnityEngine.SceneManagement;


public class resetGUI : MonoBehaviour {

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void ResetScene()
    {
        if (ScenarioLogManager.Instance != null && ScenarioLogManager.Instance.recording)
        {
            ScenarioLogManager.Instance.StopRecording();
            ScenarioLogManager.Instance.Reset();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
