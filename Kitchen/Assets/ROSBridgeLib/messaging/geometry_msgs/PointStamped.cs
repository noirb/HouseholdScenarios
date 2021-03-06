// Generated by gencs from geometry_msgs/PointStamped.msg
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
    public class PointStamped : ROSMessage
    {
      public std_msgs.Header header;
      public UnityEngine.Vector3 point;


      public PointStamped()
      {
        header = new std_msgs.Header();
        point = new UnityEngine.Vector3();

      }

      public PointStamped(std_msgs.Header _header, UnityEngine.Vector3 _point)
      {
        header = _header;
        point = _point;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/PointStamped";
      }

      new public static string GetMD5Hash()
      {
        return "c63aecb41bfdfd6b7e1fac37c7cbe7bf";
      }

    } // class PointStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
