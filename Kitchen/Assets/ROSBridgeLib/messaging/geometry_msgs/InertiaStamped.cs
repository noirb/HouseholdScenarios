// Generated by gencs from geometry_msgs/InertiaStamped.msg
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
    public class InertiaStamped : ROSMessage
    {
      public std_msgs.Header header;
      public geometry_msgs.Inertia inertia;


      public InertiaStamped()
      {
        header = new std_msgs.Header();
        inertia = new geometry_msgs.Inertia();

      }

      public InertiaStamped(std_msgs.Header _header, geometry_msgs.Inertia _inertia)
      {
        header = _header;
        inertia = _inertia;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/InertiaStamped";
      }

      new public static string GetMD5Hash()
      {
        return "ddee48caeab5a966c5e8d166654a9ac7";
      }

    } // class InertiaStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
