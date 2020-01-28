using UnityEngine;
using System.Collections.Generic;

public class LogPlayer : MonoBehaviour {
    public enum LogPlaybackState
    {
        NoContent,
        Playing,
        Paused,
        Stopped
    }
    public delegate void PlaybackStateChanged(LogPlaybackState newState);

    public PlaybackStateChanged onStateChanged;

    [Tooltip("Path to log file to load for playbak")]
    public string log_filename;



    [Tooltip("If true, all objects with a RigidBody will be made Kinematic during playback")]
    public bool makeKinematic = false;

    float playback_startTime = 0;   // time at which playback started
    float playback_time = 0;        // time since playback started
    int next_entry = 0;             // next log entry/timestep to process
    ScenarioLog log;
    Dictionary<string, Transform> tracked_objs;
    Dictionary<string, string> events_to_clear; // list of events for each object that need to be cleared at the end of a frame


    private LogPlaybackState _playbackState = LogPlaybackState.NoContent;
    private LogPlaybackState playbackState
    {
        get { return _playbackState; }
        set
        {
            _playbackState = value;
            if (onStateChanged != null)
            {
                onStateChanged(_playbackState);
            }
        }
    }
    public LogPlaybackState currentState
    {
        get { return playbackState; }
    }

    public bool playing
    {
        get { return currentState == LogPlaybackState.Playing; }
    }

    public float time
    {
        get { return playback_time; }
    }

    public float logTime
    {
        get { return log.Count > 0 && next_entry < log.Count ? log.log[next_entry].time : 0.0f; }
    }

    public int length
    {
        get { return log.Count; }
    }

    public int current_entry
    {
        get { return next_entry; }
    }

    Transform FindTransform(string name)
    {
        var go = GameObject.Find(name);
        if (go != null)
        {
            return go.transform;
        }
        else
        {
            return null;
        }
    }

    void Start() {
        log = new ScenarioLog();
        tracked_objs = new Dictionary<string, Transform>();
        events_to_clear = new Dictionary<string, string>();
    }

    public bool LoadLog(string filename)
    {
        // do not allow loading new logs during playback
        if (playing)
            return false;

        log_filename = filename.Replace("\"", "");

        // clear any old log data
        log = new ScenarioLog();
        tracked_objs.Clear();
        events_to_clear.Clear();

        try
        {
            string log_data = System.IO.File.ReadAllText(log_filename);
            log = JsonUtility.FromJson<ScenarioLog>(log_data);
        }
        catch ( System.IO.FileNotFoundException ex )
        {
            Debug.LogWarning("Could not find log file: '" + log_filename + "'\n" + ex.Message);
            return false;
        }
        catch ( System.Exception ex )
        {
            Debug.LogWarning("An exception was thrown while trying to read log file '" + log_filename + "':\n" + ex.Message);
            return false;
        }

        Debug.Log("Playback found " + log.Count + " log entries~!");
        playbackState = LogPlaybackState.Stopped;

        // make playback player copy
        var players = GameObject.FindObjectsOfType<NewtonVR.NVRPlayer>();
        GameObject dupe;
        NullAvatarDriver nullDriver = FindObjectOfType<NullAvatarDriver>();


        dupe = nullDriver.gameObject;

        foreach (var player in players)
        {
            if (player != dupe)
            {
                ScenarioLogManager.Instance.UnTrack(player.LeftHand);
                ScenarioLogManager.Instance.UnTrack(player.LeftHand.gameObject);
                ScenarioLogManager.Instance.UnTrack(player.RightHand);
                ScenarioLogManager.Instance.UnTrack(player.RightHand.gameObject);

                player.gameObject.SetActive(false); // disable for now
            }
        }
        dupe.SetActive(true);
        ScenarioLogManager.Instance.Track(nullDriver.LeftHand);
        ScenarioLogManager.Instance.Track(nullDriver.RightHand);
        ScenarioLogManager.Instance.Track(nullDriver.LeftHand.gameObject);
        ScenarioLogManager.Instance.Track(nullDriver.RightHand.gameObject);

        if (OntologyManager.Instance != null)
        {
            OntologyManager.Instance.LeftHand = nullDriver.LeftHand;
            OntologyManager.Instance.RightHand = nullDriver.RightHand;
        }

        // scan log entries & get references to all relevant transforms where possible
        // save 'null' for objects that don't exist in case they can be found later
        foreach (var entry in log.log)
        {
            foreach (var obj in entry.logstep)
            {
                if (!tracked_objs.ContainsKey(obj.name))
                {
                    Transform transform = FindTransform(obj.name);
                    if (transform == null)
                    {
                        Debug.Log("Could not find transform for: " + obj.name);
                    }

                    tracked_objs.Add(obj.name, transform);
                }
            }
        }

        // must do this AFTER gathering transforms to ensure we don't pick a real player obj up by mistake
        foreach (var player in players)
        {
            player.gameObject.SetActive(true); // re-enable objs we disabled
            foreach (var hand in player.Hands)
            {
                hand.gameObject.SetActive(true); // for some reason Steam disables hands when we disable the parent object above...
            }
        }

        return true;
    }

    void MakeKinematic(Transform obj, bool kinematic)
    {
        var body = obj.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.isKinematic = kinematic;
        }
    }

    public void Play()
    {
        ScenarioLogManager.Instance.InjectStart(Time.time);
        OntologyManager.Instance.nohands = true;
        foreach (var obj in tracked_objs)
        {
            if (obj.Value != null)
            {
                MakeKinematic(obj.Value, makeKinematic);
            }
        }


        playback_startTime = Time.time;
        playback_time = log.log[next_entry].time;
        playbackState = LogPlaybackState.Playing;
    }

    public void Pause()
    {
        playbackState = LogPlaybackState.Paused;
    }

    public void Stop()
    {
        ScenarioLogManager.Instance.InjectStop(Time.time);
        OntologyManager.Instance.nohands = false;
        playbackState = LogPlaybackState.Stopped;
        playback_time = 0;
        next_entry = 0;

        var players = GameObject.FindObjectsOfType<NewtonVR.NVRPlayer>();
        GameObject dupe;
        NullAvatarDriver nullDriver = FindObjectOfType<NullAvatarDriver>();
        dupe = nullDriver.gameObject;
        ScenarioLogManager.Instance.UnTrack(nullDriver.LeftHand);
        ScenarioLogManager.Instance.UnTrack(nullDriver.RightHand);

        // find first player that is not the playback dupe and set it to broadcast hand data
        foreach (var player in players)
        {
            if (player != dupe)
            {
                if (OntologyManager.Instance != null)
                {
                    OntologyManager.Instance.LeftHand = player.LeftHand;
                    OntologyManager.Instance.RightHand = player.RightHand;
                }
                if (ScenarioLogManager.Instance != null)
                {
                    ScenarioLogManager.Instance.Track(player.LeftHand);
                    ScenarioLogManager.Instance.Track(player.RightHand);
                }

                break;
            }
        }
    }

    public bool NextFrame()
    {
        if (next_entry >= log.Count)
            return false;

        bool contained_events = false;
        var curr_time = log.log[next_entry].time;
        //while (next_entry < log.Count && curr_time == log.log[next_entry].time)
        //{
            contained_events = contained_events || UpdateObjects(log.log[next_entry]);
            ScenarioLogManager.Instance.InjectLogEntry(log.log[next_entry]);
            
            next_entry++;
        //}
        return contained_events;
    }

    public bool PrevFrame()
    {
        if (next_entry <= 0)
            return false;

        bool contained_events = false;
        var curr_time = log.log[next_entry].time;
        while (next_entry > 0 && curr_time == log.log[next_entry].time)
        {
            next_entry--;
            contained_events = contained_events || UpdateObjects(log.log[next_entry]);
        }
        return contained_events;
    }

    // Update is called once per frame
    void Update() {
        if (!playing || next_entry >= log.Count)
            return; // end of log, nothing left to do

        playback_time += Time.deltaTime;

        // wait for next timestep before applying next log data
        while (next_entry < log.Count && log.log[next_entry].time <= playback_time)
        {
            bool contained_events = NextFrame();

            // any events in the log must persist for at least one frame
            if (contained_events)
                break;
        }

        if (next_entry >= log.Count)
        {
            Pause();
        }

    }

    void LateUpdate()
    {
        if (!playing)
            return;

        // If any state from the event injection in UpdateEvents
        // needs to be cleared at the end of a frame, do it here
        foreach (var entry in events_to_clear)
        {
            switch (entry.Value)
            {
                case "HoldButtonDown":
                    tracked_objs[entry.Key].GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressDown = false;
                    break;
                case "HoldButtonUp":
                    tracked_objs[entry.Key].GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressUp = false;
                    tracked_objs[entry.Key].GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].IsPressed = false;
                    break;
                case "UseButtonDown":
                    tracked_objs[entry.Key].GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].PressDown = false;
                    break;
                case "UseButtonUp":
                    tracked_objs[entry.Key].GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].PressUp = false;
                    break;
                default:
                    break;
            }
        }
        events_to_clear.Clear();
    }

    /// <summary>
    /// Updates all objects possible for the current LogEntry.
    /// Any objects in the LogEntry that playback hasn't seen yet will (hopefully)
    /// be found so they can be updated now and on future frames.
    /// </summary>
    /// <param name="logEntry"></param>
    /// <returns>True if this logEntry contained events requiring a full update to process</returns>
    bool UpdateObjects(LogEntry logEntry)
    {
        bool contained_events = false;
        foreach (var obj in logEntry.logstep)
        {
            // if we hit a new object, try to find it & add it to the list
            if (!tracked_objs.ContainsKey(obj.name))
            {
                tracked_objs.Add(obj.name, FindTransform(obj.name));
            }

            if (tracked_objs.ContainsKey(obj.name))
            {
                // if we couldn't previously find this object, check to see if it exists, now...
                if (tracked_objs[obj.name] == null)
                {
                    tracked_objs[obj.name] = FindTransform(obj.name);
                }

                // if we have a valid reference to this object, ensure it's active and update it
                if (tracked_objs[obj.name] != null)
                {
                    if (!tracked_objs[obj.name].gameObject.activeSelf)
                    {
                        tracked_objs[obj.name].gameObject.SetActive(true);
                    }
                    tracked_objs[obj.name].position = new Vector3(obj.position[0], obj.position[1], obj.position[2]);
                    tracked_objs[obj.name].rotation = new Quaternion(obj.orientation[1], obj.orientation[2], obj.orientation[3], obj.orientation[0]);
                    Rigidbody rb = tracked_objs[obj.name].GetComponentInChildren<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = new Vector3(obj.velocity[0], obj.velocity[1], obj.velocity[2]);
                    }
                    // Perform any event injection as necessary
                    contained_events = UpdateEvents(tracked_objs[obj.name], obj.events);

                    var hand = tracked_objs[obj.name].GetComponent<NewtonVR.NVRHand>();
                    if (hand != null)
                    {
                        hand.InjectNewPoses();
                        OntologyManager.Instance.InjectHandUpdate(hand, obj, logEntry.time);
                    }
                }
            }

        }

        return contained_events;
    }

    /// <summary>
    /// Called any time the log contains event data for an object. If any even injection or
    /// special handling should be done during playback based on events triggered during the
    /// original recording, it should be handled here.
    /// 
    /// If some state from the input injection needs to be cleaned up at the end of the frame,
    /// add the event name to events_to_clear and handle it in LateUpdate()
    /// </summary>
    /// <param name="obj">The object which triggered or received the event during the original recording</param>
    /// <param name="events">The EventData for the event (event name and, possibly, parameters)</param>
    /// <returns>True if there were events which require a full update to process</returns>
    bool UpdateEvents(Transform obj, EventData[] events)
    {
        if (events.Length == 0)
            return false; // nothing to do

        try {
            for (int i = 0; i < events.Length; i++)
            {
                switch (events[i].name)
                {
                    case "HoldButtonDown":
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressUp = false;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressDown = true;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].IsPressed = true;
                        events_to_clear.Add(obj.name, "HoldButtonDown");
                        break;
                    case "HoldButtonUp":
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressDown = false;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].IsPressed = false;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].PressUp = true;
                        events_to_clear.Add(obj.name, "HoldButtonUp");
                        break;
                    case "UseButtonDown":
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].PressDown = true;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].IsPressed = true;
                        events_to_clear.Add(obj.name, "UseButtonDown");
                        break;
                    case "UseButtonUp":
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].PressUp = true;
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.UseButton].IsPressed = false;
                        events_to_clear.Add(obj.name, "UseButtonUp");
                        break;
                    case "HoldAxis":
                        obj.GetComponent<NewtonVR.NVRHand>().Inputs[NVRButtonID.HoldButton].SingleAxis = float.Parse(events[i].property);
                        break;
                    default:
                        break;
                }
            }
        }catch (System.Exception e)
        {
            Debug.Log("UpdateEvents FAILED: " + e.Message);
        }
        return events_to_clear.ContainsKey(obj.name);
    }
}
