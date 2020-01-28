using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.ontology_msgs;
using ROSBridgeLib.ontology_svcs;
using System;

/// <summary>
/// Floating label which displays information received from ROS messages
/// </summary>
public class ROSLabel : MonoBehaviour {

    public enum ROSCommunicationStyle
    {
        Subscriber,
        ServiceProvider
    };

    public string ROSHost = "ws://136.187.35.99";
    public int ROSPort = 9090;
    public ROSCommunicationStyle CommunicationStyle = ROSCommunicationStyle.Subscriber;
    [Space]
    public string TopicName = "/Unity/Utils/Label";

    FloatingLabel label;
    ROSBridgeWebSocketConnection ros;

    void Start () {
        label = gameObject.AddComponent<FloatingLabel>();

        ros = new ROSBridgeWebSocketConnection(ROSHost, ROSPort);

        switch (CommunicationStyle)
        {
            case ROSCommunicationStyle.Subscriber:
                var lblSub = ros.Subscribe(TopicName,
                    (ROSBridgeLib.ontology_msgs.DisplayLabel msg) =>
                    {
                        OnSetLabel(msg.text, msg.lbl_id);
                    });
                break;
            case ROSCommunicationStyle.ServiceProvider:
                var lblServ = ros.Advertise<ROSBridgeLib.ontology_svcs.DisplayLabel, DisplayLabelRequest, DisplayLabelResponse>(TopicName,
                    (DisplayLabelRequest args, out DisplayLabelResponse response) =>
                    {
                        response = new DisplayLabelResponse();
                        response.lbl_id = OnSetLabel(args.text, args.lbl_id);
                        return true;
                    });
                break;
            default:
                return;  // don't even connect if we have no subscriber/service
        }

        ros.Connect();
    }

    void Update()
    {
        ros.Render();
    }

    void OnDestroy()
    {
        if (ros != null)
            ros.Disconnect();
    }

    uint OnSetLabel(string label_text, uint label_id)
    {
        label.text = label_text;

        return label_id;
    }

}
