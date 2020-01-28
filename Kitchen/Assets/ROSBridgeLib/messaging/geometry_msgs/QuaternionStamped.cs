// Generated by gencs from geometry_msgs/QuaternionStamped.msg
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
    public class QuaternionStamped : ROSMessage
    {
      public std_msgs.Header header;
      public UnityEngine.Quaternion quaternion;


      public QuaternionStamped()
      {
        header = new std_msgs.Header();
        quaternion = new UnityEngine.Quaternion();

      }

      public QuaternionStamped(std_msgs.Header _header, UnityEngine.Quaternion _quaternion)
      {
        header = _header;
        quaternion = _quaternion;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/QuaternionStamped";
      }

      new public static string GetMD5Hash()
      {
        return "e57f1e547e0e1fd13504588ffc8334e2";
      }

    } // class QuaternionStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib