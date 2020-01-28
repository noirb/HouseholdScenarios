using UnityEngine;
using System.Collections;

/// <summary>
/// Base class to be used by any object which needs to receive
/// information about the current log recording state or be notified
/// of the log entries themselves.
/// 
/// The ScenarioLogManager will call NotifyNewLogEntry as each entry is
/// created, which could allow other components to process, save, or send
/// them as needed.
/// </summary>
public abstract class ScenarioLogSubscriber {
    public abstract void NotifyLogStart(float time);
    public abstract void NotifyLogEnd(float time);
    public abstract void NotifyNewLogEntry(LogEntry entry);
}
