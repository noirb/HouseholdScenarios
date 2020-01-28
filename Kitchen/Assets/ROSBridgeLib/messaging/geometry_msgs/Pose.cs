// Generated by gencs from geometry_msgs/Pose.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.geometry_msgs;

namespace ROSBridgeLib {
  namespace geometry_msgs {

    [System.Serializable]
    public class Pose : ROSMessage
    {
      public UnityEngine.Vector3 position;
      public UnityEngine.Quaternion orientation;


      public Pose()
      {
        position = new UnityEngine.Vector3();
        orientation = new UnityEngine.Quaternion();

      }

      public Pose(UnityEngine.Vector3 _position, UnityEngine.Quaternion _orientation)
      {
        position = _position;
        orientation = _orientation;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/Pose";
      }

      new public static string GetMD5Hash()
      {
        return "e45d45a5a1ce597b249e23fb30fc871f";
      }

    } // class Pose

  } // namespace geometry_msgs

} // namespace ROSBridgeLib