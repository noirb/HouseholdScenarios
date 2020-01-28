using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.ontology_svcs;

/// <summary>
/// Handles communication for ontology-related information between Unity and ROS
/// </summary>
public class OntologyManager : Singleton<OntologyManager>
{
    public string ROSHost = "ws://136.187.35.70";
    public int ROSPort = 9090;
    [Space]
    public GameObject popupGUI;

    // TODO: Move these
    [Space]
    public NewtonVR.NVRHand LeftHand;
    public NewtonVR.NVRHand RightHand;

    public bool nohands = false;
    private bool _needInput = false;
    private RosLogger _rosLogger;
    private ROSBridgeWebSocketConnection ros;
    private ROSBridgeServiceProvider onto_list_srv;
    private ROSBridgeServiceProvider onto_list_with_pose_srv;
    private ROSBridgeServiceProvider relabelObject_srv;

    private ROSBridgePublisher<ROSBridgeLib.ontology_msgs.HandUpdate> l_hand_pub;
    private ROSBridgePublisher<ROSBridgeLib.ontology_msgs.HandUpdate> r_hand_pub;

    bool _ready = false;
    bool Ready()
    {
        return (ros != null) && (ros.connected) && _ready;
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("ros-ip") && PlayerPrefs.HasKey("ros-port"))
        {
            ROSHost = PlayerPrefs.GetString("ros-ip");
            ROSPort = PlayerPrefs.GetInt("ros-port");
        }

        initROS(ROSHost, ROSPort);
    }

    void initROS(string hostname, int port)
    {
        ros = new ROSBridgeWebSocketConnection(hostname, port);
        ros.onConnectionSuccess += () => { initROSComms(ros); };
        ros.onConnectionFailure += () => { _needInput = true; };
        ros.Connect();
    }

    void initROSComms(ROSBridgeWebSocketConnection ros)
    {
        if (!ros.connected)
            return;

        onto_list_srv = ros.Advertise<GetAllClassInstances, GetAllClassInstancesRequest, GetAllClassInstancesResponse>("/sgv/onto/GetAllClassInstances", onGetAllClassInstances);
        onto_list_with_pose_srv = ros.Advertise<GetAllClassInstancesWithPoses, GetAllClassInstancesWithPosesRequest, GetAllClassInstancesWithPosesResponse>("/sgv/onto/GetAllClassInstancesWithPoses", onGetAllClassInstancesWithPoses);
        relabelObject_srv = ros.Advertise<RelabelObject, RelabelObjectRequest, RelabelObjectResponse>("/sgv/sim/RelabelObject", onRelabelObject);
        l_hand_pub = ros.Advertise<ROSBridgeLib.ontology_msgs.HandUpdate>("/sgv/human/hands/left");
        r_hand_pub = ros.Advertise<ROSBridgeLib.ontology_msgs.HandUpdate>("/sgv/human/hands/right");

        _rosLogger = new RosLogger(ros);

        _ready = true;
    }

    void requestROSConnection()
    {
        var gui = GameObject.Instantiate(popupGUI).GetComponent<PopUpGUI>();
        gui.description = "Please enter ROS IP:Port";
        gui.inputLabel = "x.x.x.x:pppp";
        gui.value = ROSHost + ":" + ROSPort;
        gui.onTextSubmitted += (string res) =>
        {
            if (res.Contains(":"))
            {
                string host = res.Substring(0, res.LastIndexOf(':'));
                string port = res.Substring(res.LastIndexOf(':') + 1);
                ROSHost = host; // everything before last ':' is hostname
                ROSPort = int.Parse(port); // everything after is port
            }
            else
            {
                ROSHost = res; // presume port should remain unchanged
            }
            gui.Hide();
            Destroy(gui.gameObject, 2);

            // save connection details for later
            PlayerPrefs.SetString("ros-ip", ROSHost);
            PlayerPrefs.SetInt("ros-port", ROSPort);

            initROS(ROSHost, ROSPort);
        };
        gui.Show();
    }

    bool onRelabelObject(RelabelObjectRequest req, out RelabelObjectResponse res)
    {
        GameObject.Find(req.objName).GetComponentInChildren<OntologyClass>().ClassName = req.objClass;

        res = new RelabelObjectResponse();
        res.success = true;
        return true;
    }

    bool onGetAllClassInstances(GetAllClassInstancesRequest req, out GetAllClassInstancesResponse res)
    {
        Debug.Log("Got request for all Ontology classes!");
        res = new GetAllClassInstancesResponse();
        var all_classes = GameObject.FindObjectsOfType<OntologyClass>();
        foreach (var ontoclass in all_classes)
        {
            res.classes.Add(ontoclass.ClassName);
            res.entityNames.Add(ontoclass.gameObject.name);

            res.properties.Add(new ROSBridgeLib.ontology_msgs.OntoPropertyList());

            PropertyActionProvider p = ontoclass.gameObject.GetComponent<PropertyActionProvider>();
            if (p != null)
            {
                string[] properties = p.GetPropertyStatus().Split('|');
                foreach (var property in properties)
                {
                    string[] property_split = property.Split(':');
                    res.properties[res.properties.Count - 1].properties.Add(new ROSBridgeLib.ontology_msgs.OntoProperty(property_split[0], property_split[1]));
                }
            }
            else
            {
                ReactiveProperty r = ontoclass.gameObject.GetComponent<ReactiveProperty>();
                if (r != null)
                {
                    string[] properties = r.GetPropertyState().Split('|');
                    foreach (var property in properties)
                    {
                        string[] property_split = property.Split(':');
                        res.properties[res.properties.Count - 1].properties.Add(new ROSBridgeLib.ontology_msgs.OntoProperty(property_split[0], property_split[1]));
                    }
                }
            }

        }
        Debug.Log("Returning " + res.classes.Count + " class names...");
        return true;
    }

    bool onGetAllClassInstancesWithPoses(GetAllClassInstancesWithPosesRequest req, out GetAllClassInstancesWithPosesResponse res)
    {
        res = new GetAllClassInstancesWithPosesResponse();
        var all_classes = GameObject.FindObjectsOfType<OntologyClass>();
        foreach (var ontoclass in all_classes)
        {
            res.classes.Add(ontoclass.ClassName);
            res.entityNames.Add(ontoclass.gameObject.name);
            res.positions.Add(ontoclass.transform.position);
            res.orientations.Add(ontoclass.transform.rotation);

            res.properties.Add(new ROSBridgeLib.ontology_msgs.OntoPropertyList());

            PropertyActionProvider p = ontoclass.gameObject.GetComponent<PropertyActionProvider>();
            if (p != null)
            {
                string[] properties = p.GetPropertyStatus().Split('|');
                foreach (var property in properties)
                {
                    string[] property_split = property.Split(':');
                    res.properties[res.properties.Count - 1].properties.Add(new ROSBridgeLib.ontology_msgs.OntoProperty(property_split[0], property_split[1]));
                }
            }
            else
            {
                ReactiveProperty r = ontoclass.gameObject.GetComponent<ReactiveProperty>();
                if (r != null)
                {
                    string[] properties = r.GetPropertyState().Split('|');
                    foreach (var property in properties)
                    {
                        string[] property_split = property.Split(':');
                        res.properties[res.properties.Count - 1].properties.Add(new ROSBridgeLib.ontology_msgs.OntoProperty(property_split[0], property_split[1]));
                    }
                }
            }
        }

        return true;
    }

    void Update()
    {
        if (_needInput)
        {
            _needInput = false;
            requestROSConnection();
        }

        if (ros != null && ros.connected)
            ros.Render();
    }

    public void InjectHandUpdate(NewtonVR.NVRHand hand, LogData log, float time)
    {
        if (!Ready())
            return;
        ROSBridgeLib.ontology_msgs.HandUpdate msg = new ROSBridgeLib.ontology_msgs.HandUpdate();
        msg.grasp = hand.HoldButtonAxis;
        msg.use = hand.UseButtonAxis;
        if (hand.CurrentlyInteracting != null)
        {
            msg.obj_inHand = hand.CurrentlyInteracting.gameObject.name;
        }
        else
        {
            msg.obj_inHand = "NONE";
        }
        msg.handState = new ROSBridgeLib.ontology_msgs.ObjUpdate();
        msg.handState.timestamp = new ROSBridgeLib.msg_helpers.Time(time);
        msg.handState.position = hand.transform.position;
        msg.handState.orientation = hand.transform.rotation;
        msg.handState.velocity = new Vector3(log.velocity[0], log.velocity[1], log.velocity[2]);
        msg.handState.name = hand.gameObject.name;

        if (msg.handState.name.Contains("left"))
        {
            l_hand_pub.Publish(msg);
        }
        else
        {
            r_hand_pub.Publish(msg);
        }
    }

    void FixedUpdate()
    {
        if (!Ready() || nohands) //ros == null || !ros.connected)
            return;

        ROSBridgeLib.ontology_msgs.HandUpdate left = new ROSBridgeLib.ontology_msgs.HandUpdate();
        left.grasp = LeftHand.HoldButtonAxis;
        left.use = LeftHand.UseButtonAxis;
        if (LeftHand.CurrentlyInteracting != null)
        {
            left.obj_inHand = LeftHand.CurrentlyInteracting.gameObject.name;
        }
        else
        {
            left.obj_inHand = "NONE";
        }
        left.handState = new ROSBridgeLib.ontology_msgs.ObjUpdate();
        left.handState.timestamp = new ROSBridgeLib.msg_helpers.Time(ScenarioLogManager.Instance.GetLogTime());
        left.handState.position = LeftHand.transform.position;
        left.handState.orientation = LeftHand.transform.rotation;
        left.handState.velocity = LeftHand.GetVelocityEstimation();
        left.handState.name = LeftHand.gameObject.name;

        l_hand_pub.Publish(left);

        ROSBridgeLib.ontology_msgs.HandUpdate right = new ROSBridgeLib.ontology_msgs.HandUpdate();
        right.grasp = RightHand.HoldButtonAxis;
        right.use = RightHand.UseButtonAxis;
        if (RightHand.CurrentlyInteracting != null)
        {
            right.obj_inHand = RightHand.CurrentlyInteracting.gameObject.name;
        }
        else
        {
            right.obj_inHand = "NONE";
        }
        right.handState = new ROSBridgeLib.ontology_msgs.ObjUpdate();
        right.handState.timestamp = new ROSBridgeLib.msg_helpers.Time(ScenarioLogManager.Instance.GetLogTime());
        right.handState.position = RightHand.transform.position;
        right.handState.orientation = RightHand.transform.rotation;
        right.handState.velocity = RightHand.GetVelocityEstimation();
        right.handState.name = RightHand.gameObject.name;

        r_hand_pub.Publish(right);
    }
}
