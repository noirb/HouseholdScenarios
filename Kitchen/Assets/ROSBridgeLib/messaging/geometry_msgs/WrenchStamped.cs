// Generated by gencs from geometry_msgs/WrenchStamped.msg
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
    public class WrenchStamped : ROSMessage
    {
      public std_msgs.Header header;
      public geometry_msgs.Wrench wrench;


      public WrenchStamped()
      {
        header = new std_msgs.Header();
        wrench = new geometry_msgs.Wrench();

      }

      public WrenchStamped(std_msgs.Header _header, geometry_msgs.Wrench _wrench)
      {
        header = _header;
        wrench = _wrench;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/WrenchStamped";
      }

      new public static string GetMD5Hash()
      {
        return "d78d3cb249ce23087ade7e7d0c40cfa7";
      }

    } // class WrenchStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
