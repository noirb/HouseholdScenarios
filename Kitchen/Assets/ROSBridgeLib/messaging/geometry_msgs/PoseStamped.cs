// Generated by gencs from geometry_msgs/PoseStamped.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.std_msgs;
using ROSBridgeLib.geometry_msgs;

namespace ROSBridgeLib {
  namespace geometry_msgs {

    [System.Serializable]
    public class PoseStamped : ROSMessage
    {
      public std_msgs.Header header;
      public geometry_msgs.Pose pose;


      public PoseStamped()
      {
        header = new std_msgs.Header();
        pose = new geometry_msgs.Pose();

      }

      public PoseStamped(std_msgs.Header _header, geometry_msgs.Pose _pose)
      {
        header = _header;
        pose = _pose;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/PoseStamped";
      }

      new public static string GetMD5Hash()
      {
        return "d3812c3cbc69362b77dc0b19b345f8f5";
      }

    } // class PoseStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib