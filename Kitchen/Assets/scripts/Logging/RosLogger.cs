using UnityEngine;
using System.Collections.Generic;
using System;
using ROSBridgeLib;
using ROSBridgeLib.ontology_msgs;
using ROSBridgeLib.msg_helpers;

public class RosLogger : ScenarioLogSubscriber {

    public string hostIP;
    public int hostPort;

    private ROSBridgeWebSocketConnection _ros;
    private ROSBridgePublisher<PropertyChanged> _propertypub;
    private ROSBridgePublisher<ObjUpdate> _objpub;
    private ROSBridgePublisher<SimulationStateChange> _simstatepub;

    public RosLogger(ROSBridgeWebSocketConnection ros)
    {
        _ros = ros;
        _propertypub = _ros.Advertise<PropertyChanged>("/sgv/onto/property_changed");
        _objpub = _ros.Advertise<ObjUpdate>("/sgv/onto/object_moved");
        _simstatepub = _ros.Advertise<SimulationStateChange>("/sgv/simulation_state_changed");

        ScenarioLogManager.Instance.Subscribe(this);

        _simstatepub.Publish(
            new SimulationStateChange(
                SimulationStateChange.SIMULATION_LOADED,
                new ROSBridgeLib.msg_helpers.Time(0.0f)
                )
            );
    }

    public override void NotifyLogEnd(float time)
    {
        _simstatepub.Publish(
            new SimulationStateChange(
                SimulationStateChange.SIMULATION_STOPPED,
                new ROSBridgeLib.msg_helpers.Time(time)
                )
            );
        return;
    }

    public override void NotifyLogStart(float time)
    {
        _simstatepub.Publish(
            new SimulationStateChange(
                SimulationStateChange.SIMULATION_STARTED,
                new ROSBridgeLib.msg_helpers.Time(time)
                )
            );
        return;
    }

    public override void NotifyNewLogEntry(LogEntry entry)
    {
        foreach (var e in entry.logstep)
        {
            if (e.events != null)
            {
                foreach (var evt in e.events)
                {
                    if (evt.name == "PropertyChanged")
                    {
                        Debug.Log("Sending PropertyChanged for " + e.name);
                        var prams = evt.property.Split(':');
                        PropertyChanged p = new PropertyChanged(new ROSBridgeLib.msg_helpers.Time(entry.time), e.name, prams[0], prams[1]);
                        _propertypub.Publish(p);
                    }
                }
            }

            ObjUpdate o = new ObjUpdate(
                new ROSBridgeLib.msg_helpers.Time(entry.time),
                e.name,
                new Vector3(e.position[0], e.position[1], e.position[2]),
                new Quaternion(e.orientation[0], e.orientation[1], e.orientation[2], e.orientation[3]),
                new Vector3(e.velocity[0], e.velocity[1], e.velocity[2])
                );
            _objpub.Publish(o);
        }

    }
}
