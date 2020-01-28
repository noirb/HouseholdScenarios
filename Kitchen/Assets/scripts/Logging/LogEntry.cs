using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EventData
{
    public string name;     // event name
    public string property; // values that go along with this event

    public EventData(string EventName)
    {
        name = EventName;
    }

    public EventData(string EventName, string EventArgs)
    {
        name = EventName;
        property = EventArgs;
    }
}

[System.Serializable]
public class LogData
{
    public string name;         // object name
    public float[] position;    // vec3 position
    public float[] orientation; // quaternion orientation
    public float[] velocity;    // vec3 velocity
    public EventData[] events;  // events fired by this object

    public LogData(string ObjectName, Vector3 pos, Quaternion rot)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { 0.0f, 0.0f, 0.0f };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, Vector3 vel)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { vel.x, vel.y, vel.z };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, string Event)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { 0.0f, 0.0f, 0.0f };
        events = new EventData[] { new EventData(Event) };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, Vector3 vel, string Event)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { vel.x, vel.y, vel.z };
        events = new EventData[] { new EventData(Event) };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, string Event, string EventArgs)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { 0.0f, 0.0f, 0.0f };
        events = new EventData[] { new EventData(Event, EventArgs) };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, Vector3 vel, string Event, string EventArgs)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { vel.x, vel.y, vel.z };
        events = new EventData[] { new EventData(Event, EventArgs) };
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, EventData[] Events)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { 0.0f, 0.0f, 0.0f };
        events = Events;
    }

    public LogData(string ObjectName, Vector3 pos, Quaternion rot, Vector3 vel, EventData[] Events)
    {
        name = ObjectName;
        position = new float[] { pos.x, pos.y, pos.z };
        orientation = new float[] { rot.w, rot.x, rot.y, rot.z };
        velocity = new float[] { vel.x, vel.y, vel.z };
        events = Events;
    }
};

[System.Serializable]
public class LogEntry
{
    public float time;            // time at which log entry was created
    public List<LogData> logstep; // object data for this timestep

    public LogEntry(float Time)
    {
        time = Time;
        logstep = new List<LogData>();
    }

    public void Add(string Name, Vector3 pos, Quaternion rot)
    {
        Add(new LogData(Name, pos, rot));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, Vector3 vel)
    {
        Add(new LogData(Name, pos, rot, vel));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, string Event)
    {
        Add(new LogData(Name, pos, rot, Event));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, Vector3 vel, string Event)
    {
        Add(new LogData(Name, pos, rot, vel, Event));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, string Event, string EventArgs)
    {
        Add(new LogData(Name, pos, rot, Event, EventArgs));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, Vector3 vel, string Event, string EventArgs)
    {
        Add(new LogData(Name, pos, rot, vel, Event, EventArgs));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, EventData[] Events)
    {
        Add(new LogData(Name, pos, rot, Events));
    }

    public void Add(string Name, Vector3 pos, Quaternion rot, Vector3 vel, EventData[] Events)
    {
        Add(new LogData(Name, pos, rot, vel, Events));
    }

    public void Add(LogData data)
    {
        logstep.Add(data);
    }
    public string Serialize()
    {
        return JsonUtility.ToJson(this, true);
    }
};

// just a wrapper to make serialization easier
[System.Serializable]
public class ScenarioLog
{
    /// TODO: Provide foreach-ability to loop through log automatically

    [SerializeField]
    public List<LogEntry> log;

    public ScenarioLog()
    {
        log = new List<LogEntry>();
    }

    public void Add(LogEntry newEntry)
    {
        log.Add(newEntry);
    }

    public void Clear()
    {
        log.Clear();
    }

    public int Count
    {
        get { return log.Count; }
    }
}
