using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Central class for collecting and storing log data.
/// Attach a RecordThis script to any object which should be included in the log.
/// If another class should be notified of any new log entries, it can use a ScenarioLogSubscriber
/// to receive any new entries as they're created (useful if you want to send log entries over the
/// network somewhere, analyze them before saving them, stop the recording under certain conditions, etc).
/// </summary>
public class ScenarioLogManager : Singleton<ScenarioLogManager> {

    // time between log entries
    public float log_interval = 0.01f;
    // file to store logs in. Should be set BEFORE beginning a recording!
    public string log_filename = @"C:\temp\kitchen_";

    bool _recording = false;
    bool _injectedPlayback = false;
    public bool recording
    {
        get { return _recording; }
    }

    float logWait = 0; // time to wait until capturing next log entry
    float record_start_time = 0;
    List<GameObject> tracked_objs;
    List<NewtonVR.NVRHand> tracked_hands;
    ScenarioLog log;

    List<ScenarioLogSubscriber> subscribers;

    protected ScenarioLogManager()
    {
        tracked_objs = new List<GameObject>();
        tracked_hands = new List<NewtonVR.NVRHand>();
        subscribers = new List<ScenarioLogSubscriber>();
        log = new ScenarioLog();
        _recording = false;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        Reset();
    }

    void Awake()
    {
    }

    // clears all existing log & subscriber data
    public void Reset()
    {
        tracked_objs.Clear();
        tracked_hands.Clear();
        subscribers.Clear();
        log.Clear();
    }

    /// <summary>
    /// Ensures the given Subscriber receives any future log entries
    /// </summary>
    /// <param name="subscriber"></param>
    public void Subscribe(ScenarioLogSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    /// <summary>
    /// Stops the given Subscriber from receiving any future log entries
    /// </summary>
    /// <param name="subscriber"></param>
    public void Unsubscribe(ScenarioLogSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }

    /// <summary>
    /// Ensures that the given GameObject will be included in any future log entries.
    /// Can be called after recording has already started.
    /// </summary>
    /// <param name="obj"></param>
    public void Track(GameObject obj)
    {
        tracked_objs.Add(obj);
    }

    /// <summary>
    /// Allows for the tracking of the state of a specific NVRHand
    /// </summary>
    /// <param name="hand"></param>
    public void Track(NewtonVR.NVRHand hand)
    {
        tracked_hands.Add(hand);
    }

    /// <summary>
    /// Ensures the given GameObject is not included in any future log entries.
    /// Can be called after recording has already started.
    /// </summary>
    /// <param name="obj"></param>
    public void UnTrack(GameObject obj)
    {
        tracked_objs.Remove(obj);
    }

    /// <summary>
    /// Removes the given NVRHand from tracking
    /// </summary>
    /// <param name="hand"></param>
    public void UnTrack(NewtonVR.NVRHand hand)
    {
        tracked_hands.Remove(hand);
    }

    /// <summary>
    /// Adds a log entry for the given GameObject mentioning that the given event has happened.
    /// Useful for recording object-specific events, like button presses or collisions.
    /// Intended to be called asynchronously during recording by other classes. Events are not
    /// automatically tracked by the LogManager.
    /// 
    /// Should only be called after recording has already started.
    /// </summary>
    /// <param name="obj">The object which received or triggered the event</param>
    /// <param name="eventName">The event name</param>
    public void LogEvent(GameObject obj, string eventName)
    {
        if (!recording)
        {
            return;
        }

        LogEntry newEntry = new LogEntry(GetLogTime());
        NewtonVR.NVRHand hand = obj.GetComponent<NewtonVR.NVRHand>();
        Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();

        if (hand != null)
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, hand.GetVelocityEstimation(), eventName);
        }
        else if (rb != null)
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, rb.velocity, eventName);
        }
        else
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, eventName);
        }
        
        AddLogEntry(newEntry);
    }

    /// <summary>
    /// Adds a log entry for the given GameObject mentioning that the given event has happened.
    /// Useful for recording object-specific events, like button presses or collisions.
    /// Intended to be called asynchronously during recording by other classes. Events are not
    /// automatically tracked by the LogManager.
    /// 
    /// Should only be called after recording has already started.
    /// </summary>
    /// <param name="obj">The object which received or triggered the event</param>
    /// <param name="eventName">The event name</param>
    /// <param name="property">Any related event parameters</param>
    public void LogEvent(GameObject obj, string eventName, string property)
    {
        if (!recording)
        {
            return;
        }
        LogEntry newEntry = new LogEntry(GetLogTime());
        NewtonVR.NVRHand hand = obj.GetComponent<NewtonVR.NVRHand>();
        Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();

        if (hand != null)
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, hand.GetVelocityEstimation(), eventName, property);
        }
        else if (rb != null)
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, rb.velocity, eventName, property);
        }
        else
        {
            newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, eventName, property);
        }
        
        AddLogEntry(newEntry);
    }

    /// <summary>
    /// Begins a new recording
    /// </summary>
    public void StartRecording()
    {
        if (_recording) // do not start over if we're already recording
            return;

        record_start_time = Time.time; // NOW(-ish) is "zero" for the recording
        log.Clear();
        _recording = true;

        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyLogStart(GetLogTime());
        }
    }

    /// <summary>
    /// Ends a recording.
    /// Ensures any collected log data is saved to a file.
    /// </summary>
    public void StopRecording()
    {
        if (!_recording) // Do nothing if we're not already recording
            return;

        var last_time = GetLogTime();
        _recording = false;

        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyLogEnd(last_time);
        }

        SaveLogToFile(log_filename);

    }

    /// <summary>
    /// Adds a new log entry to the total log.
    /// Each entry contains all object data for one timestep.
    /// </summary>
    /// <param name="entry"></param>
    void AddLogEntry(LogEntry entry)
    {
        log.Add(entry);
        NotifySubscribers(entry);
    }

    /// <summary>
    /// Passes a new LogEntry to any subscribers listening for log data
    /// </summary>
    /// <param name="entry"></param>
    void NotifySubscribers(LogEntry entry)
    {
        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyNewLogEntry(entry);
        }
    }

    public void InjectStart(float time)
    {
        record_start_time = time;
        _injectedPlayback = true;
        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyLogStart(time);
        }
    }
    public void InjectStop(float time)
    {
        _injectedPlayback = false;
        foreach (var subscriber in subscribers)
        {
            subscriber.NotifyLogEnd(time);
        }
    }
    public void InjectLogEntry(LogEntry entry)
    {
        NotifySubscribers(entry);
    }

    /// <summary>
    /// Creates and saves a LogEntry for all currently tracked objects
    /// </summary>
    /// <param name="time">Current time to save with log entry</param>
    void LogObjects(float time)
    {
        LogEntry newEntry = new LogEntry(time);
        foreach (var obj in tracked_objs)
        {
            // hands do not report velocity through their RigidBody
            NewtonVR.NVRHand hand = obj.GetComponent<NewtonVR.NVRHand>();
            if (hand != null)
            {
                newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, hand.GetVelocityEstimation());
                continue;
            }

            // all other objects should have a normal velocity
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation, rb.velocity);
            }
            else
            {
                newEntry.Add(obj.name, obj.transform.position, obj.transform.rotation);
            }
        }
        foreach (var hand in tracked_hands)
        {
            LogEvent(hand.gameObject, "HoldAxis", hand.HoldButtonAxis.ToString());
        }

        AddLogEntry(newEntry);
    }

    /// <summary>
    /// Writes the current log data to a file.
    /// Will overwrite the file if it exists, and will create any
    /// non-existent directories in the given path as necessary.
    /// </summary>
    /// <param name="filename"></param>
    void SaveLogToFile(string filename)
    {
        filename += "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".txt";
        string total_log = JsonUtility.ToJson(log, true);
        string dirName = filename.Substring(0, filename.LastIndexOf('\\'));
        if (!System.IO.Directory.Exists(dirName))
        {
            Debug.Log("Creating directory: " + dirName);
            System.IO.Directory.CreateDirectory(dirName);
        }
        System.IO.File.WriteAllText(filename, total_log);
    }

    /// <summary>
    /// Gets "current" log time
    /// </summary>
    /// <returns>"Current" time in log. If no logs are being recorded, time returned is always zero</returns>
    public float GetLogTime()
    {
        if (!recording && !_injectedPlayback)
        {
            return 0;
        }

        return Time.time - record_start_time; /// TODO: Replace with experiment time when integrating with SIGVERSE proper
    }

    void Update ()
    {
        if (!recording)
            return;

        // wait for next timestep before saving new log data
        if (logWait <= 0)
        {
            LogObjects(GetLogTime());
            logWait = log_interval;
        }

        logWait -= Time.deltaTime;
    }

    new void OnDestroy()
    {
        base.OnDestroy();

        // make sure to clean up and save any logs we've captured
        if (_recording)
        {
            StopRecording();
        }
    }
}
