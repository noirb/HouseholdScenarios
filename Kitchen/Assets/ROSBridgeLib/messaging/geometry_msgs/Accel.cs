// Generated by gencs from geometry_msgs/Accel.msg
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
    public class Accel : ROSMessage
    {
      public UnityEngine.Vector3 linear;
      public UnityEngine.Vector3 angular;


      public Accel()
      {
        linear = new UnityEngine.Vector3();
        angular = new UnityEngine.Vector3();

      }

      public Accel(UnityEngine.Vector3 _linear, UnityEngine.Vector3 _angular)
      {
        linear = _linear;
        angular = _angular;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/Accel";
      }

      new public static string GetMD5Hash()
      {
        return "9f195f881246fdfa2798d1d3eebca84a";
      }

    } // class Accel

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
