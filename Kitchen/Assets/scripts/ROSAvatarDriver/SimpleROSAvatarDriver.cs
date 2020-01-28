using UnityEngine;
using System.Collections;
using NewtonVR;
using System.Linq;
using System;

using ROSBridgeLib.simple_unity_bot;

/// <summary>
/// Example ROS<-->Unity avatar control
/// Receives hand/head pose data from ROS and passes it straight through to the avatar
/// </summary>
public class SimpleROSAvatarDriver : NVRDriver {

    [Space]

    public string ROSHost = "136.187.35.85";
    public int ROSPort = 9090;

    [Space]

    public NVRPlayer target;
    public float leftGrasp = 0.0f;
    public float leftUse = 0.0f;
    public float rightGrasp = 0.0f;
    public float rightUse = 0.0f;

    private ROSBridgeLib.ROSBridgeWebSocketConnection ros;
    ROSBridgeLib.ROSBridgePublisher  pose_pub;
    ROSBridgeLib.ROSBridgeSubscriber pose_sub;
    ROSBridgeLib.ROSBridgePublisher<ROSBridgeLib.simple_unity_bot.HandState>  hand_pub;
    ROSBridgeLib.ROSBridgeSubscriber<ROSBridgeLib.simple_unity_bot.HandState> hand_sub;


    public override NVRButtonInputs GetButtonState(NVRHand hand, NVRButtonID button)
    {
        throw new NotImplementedException();
    }

    public override string GetDeviceName(NVRHead head)
    {
        return "ROS Simple Robot Head";
    }

    public override string GetDeviceName(NVRHand hand)
    {
        return "ROS Simple Robot Hand";
    }

    public override Vector3 GetVelocity(NVRHand hand)
    {
        throw new NotImplementedException();
    }

    public override void LongHapticPulse(NVRHand hand, float seconds)
    {
    }

    void Awake()
    {
        // hands MUST be initialized before we can use them!
        foreach (var hand in Hands)
        {
            hand.DoInitialize();
        }
    }

    public override void SetButtonStates(NVRHand hand)
    {

        bool grasping = (hand == Player.LeftHand && leftGrasp > 0.9f) || (hand == Player.RightHand && rightGrasp > 0.9f);
        bool isUsing = (hand == Player.LeftHand && leftUse > 0.9f) || (hand == Player.RightHand && rightUse > 0.9f);

        foreach (var button in hand.Inputs.Keys.ToList())
        {
            NVRButtonInputs buttonState = new NVRButtonInputs();
            switch (button)
            {
                case NVRButtonID.HoldButton:
                    if (grasping && !hand.Inputs[button].IsPressed) // first frame after grasp start
                    {
                        buttonState.Axis = Vector2.one;
                        buttonState.SingleAxis = 1;
                        buttonState.PressDown = true;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = true;
                        buttonState.TouchDown = true;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = true;
                    }
                    else if (grasping && hand.Inputs[button].IsPressed) // continuing to grasp after first frame
                    {
                        buttonState.Axis = Vector2.one;
                        buttonState.SingleAxis = 1;
                        buttonState.PressDown = false;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = true;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = true;
                    }
                    else if (!grasping && !hand.Inputs[button].IsPressed) // not grasping at all
                    {
                        buttonState.Axis = hand == Player.LeftHand ? new Vector2(leftGrasp, leftGrasp) : new Vector2(rightGrasp, rightGrasp);
                        buttonState.SingleAxis = hand == Player.LeftHand ? leftGrasp : rightGrasp;
                        buttonState.PressDown = false;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = false;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = false;
                    }
                    else if (!grasping && hand.Inputs[button].IsPressed) // first frame of not grasping after a grasp
                    {
                        buttonState.Axis = hand == Player.LeftHand ? new Vector2(leftGrasp, leftGrasp) : new Vector2(rightGrasp, rightGrasp);
                        buttonState.SingleAxis = hand == Player.LeftHand ? leftGrasp : rightGrasp;
                        buttonState.PressDown = false;
                        buttonState.PressUp = true;
                        buttonState.IsPressed = false;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = true;
                        buttonState.IsTouched = false;
                    }
                    break;
                case NVRButtonID.UseButton:
                    if (isUsing && !hand.Inputs[button].IsPressed) // first frame after use start
                    {
                        buttonState.Axis = Vector2.one;
                        buttonState.SingleAxis = 1;
                        buttonState.PressDown = true;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = true;
                        buttonState.TouchDown = true;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = true;
                    }
                    else if (isUsing && hand.Inputs[button].IsPressed) // continuing to use after first frame
                    {
                        buttonState.Axis = Vector2.one;
                        buttonState.SingleAxis = 1;
                        buttonState.PressDown = false;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = true;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = true;
                    }
                    else if (!isUsing && !hand.Inputs[button].IsPressed) // not using at all
                    {
                        buttonState.Axis = hand == Player.LeftHand ? new Vector2(leftUse, leftUse) : new Vector2(rightUse, rightUse);
                        buttonState.SingleAxis = hand == Player.LeftHand ? leftUse : rightUse;
                        buttonState.PressDown = false;
                        buttonState.PressUp = false;
                        buttonState.IsPressed = false;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = false;
                        buttonState.IsTouched = false;
                    }
                    else if (!isUsing && hand.Inputs[button].IsPressed) // first frame of not using after a grasp
                    {
                        buttonState.Axis = hand == Player.LeftHand ? new Vector2(leftUse, leftUse) : new Vector2(rightUse, rightUse);
                        buttonState.SingleAxis = hand == Player.LeftHand ? leftUse : rightUse;
                        buttonState.PressDown = false;
                        buttonState.PressUp = true;
                        buttonState.IsPressed = false;
                        buttonState.TouchDown = false;
                        buttonState.TouchUp = true;
                        buttonState.IsTouched = false;
                    }
                    break;
                default:
                    break;
            }
            hand.Inputs[button] = buttonState;
        }

    }

    public override void TriggerHapticPulse(NVRHand hand, ushort durationMicroSec)
    {
    }

    void Start () {
        ROSHost = "ws://" + ROSHost;

        ros = new ROSBridgeLib.ROSBridgeWebSocketConnection(ROSHost, ROSPort);
        pose_sub = ros.Subscribe<SimplePoseArray>("/unity/simple_bot/pose", OnNewPoseMsg);
        hand_sub = ros.Subscribe<ROSBridgeLib.simple_unity_bot.HandState>("/unity/simple_bot/hands", OnNewHandMsg);
        pose_pub = ros.Advertise<SimplePoseArray>("/unity/simple_bot/target_pose");
        hand_pub = ros.Advertise<ROSBridgeLib.simple_unity_bot.HandState>("/unity/simple_bot/hand_targets");

        ros.Connect();
    }

    void onDestroy()
    {
        ros.Disconnect();
    }

    protected override void Update()
    {
        // do nothing
    }

    void FixedUpdate () {
        // send new poses for target's hands & head
        SimplePoseArray poses = new SimplePoseArray(new ROSBridgeLib.std_msgs.Header(42, new ROSBridgeLib.msg_helpers.Time(1, 2), "body"), null);
        poses.poses = new System.Collections.Generic.List<SimplePose>
        {
            new SimplePose("LeftHand",  target.LeftHand.transform.localPosition,  target.LeftHand.transform.localRotation),
            new SimplePose("RightHand", target.RightHand.transform.localPosition, target.RightHand.transform.localRotation),
            new SimplePose("Head",      target.Head.transform.localPosition,      target.Head.transform.localRotation)
        };
        ros.Publish(pose_pub, poses);

        // send hand input states for target
        ROSBridgeLib.simple_unity_bot.HandState LeftHandMsg = new ROSBridgeLib.simple_unity_bot.HandState("LeftHand", target.LeftHand.Inputs[NVRButtonID.HoldButton].SingleAxis, target.LeftHand.Inputs[NVRButtonID.UseButton].IsPressed ? 1.0f : 0.0f);
        ROSBridgeLib.simple_unity_bot.HandState RightHandMsg = new ROSBridgeLib.simple_unity_bot.HandState("RightHand", target.RightHand.Inputs[NVRButtonID.HoldButton].SingleAxis, target.RightHand.Inputs[NVRButtonID.UseButton].IsPressed ? 1.0f : 0.0f);
        ros.Publish(hand_pub, LeftHandMsg);
        ros.Publish(hand_pub, RightHandMsg);
        ros.Render();
    }

    void OnNewHandMsg(ROSBridgeLib.simple_unity_bot.HandState handMsg)
    {
        if (handMsg.name == "LeftHand")
        {
            leftGrasp = handMsg.grasp;
            leftUse = handMsg.use;
            SetButtonStates(Player.LeftHand);
        }
        else if (handMsg.name == "RightHand")
        {
            rightGrasp = handMsg.grasp;
            rightUse = handMsg.use;
            SetButtonStates(Player.RightHand);
        }
        else
        {
            Debug.LogWarning("Received hand state for unknown object: '" + handMsg.name + "' [" + handMsg.grasp + ", " + handMsg.use + "]");
        }
    }

    void OnNewPoseMsg(ROSBridgeLib.simple_unity_bot.SimplePoseArray poseMsg)
    {
        if (poseMsg.header.frame_id == "body")
        {
            foreach (var pose in poseMsg.poses)
            {
                switch (pose.name)
                {
                    case "LeftHand":
                        Player.LeftHand.transform.localPosition = pose.position;
                        Player.LeftHand.transform.localRotation = pose.orientation;
                        break;
                    case "RightHand":
                        Player.RightHand.transform.localPosition = pose.position;
                        Player.RightHand.transform.localRotation = pose.orientation;
                        break;
                    case "Head":
                        Player.Head.transform.localPosition = pose.position;
                        Player.Head.transform.localRotation = pose.orientation;
                        break;
                    default:
                        Debug.LogWarning("Received pose for unexpected object '" + pose.name + "'");
                        break;
                }
            }
        }
        else if (poseMsg.header.frame_id == "world")
        {
            foreach (var pose in poseMsg.poses)
            {
                switch (pose.name)
                {
                    case "LeftHand":
                        Player.LeftHand.transform.position = pose.position;
                        Player.LeftHand.transform.rotation = pose.orientation;
                        break;
                    case "RightHand":
                        Player.RightHand.transform.position = pose.position;
                        Player.RightHand.transform.rotation = pose.orientation;
                        break;
                    case "Head":
                        Player.Head.transform.position = pose.position;
                        Player.Head.transform.rotation = pose.orientation;
                        break;
                    default:
                        Debug.LogWarning("Received pose for unexpected object '" + "'");
                        break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Received poses for unknown frame '" + poseMsg.header.frame_id + "'");
        }
    }

}
