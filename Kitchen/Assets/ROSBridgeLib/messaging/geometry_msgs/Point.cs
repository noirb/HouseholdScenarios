// Generated by gencs from geometry_msgs/Point.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace geometry_msgs {

    [System.Serializable]
    public class Point : ROSMessage
    {
      public double x;
      public double y;
      public double z;


      public Point()
      {
        x = 0.0;
        y = 0.0;
        z = 0.0;

      }

      public Point(double _x, double _y, double _z)
      {
        x = _x;
        y = _y;
        z = _z;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/Point";
      }

      new public static string GetMD5Hash()
      {
        return "4a842b65f413084dc2b10fb484ea7f17";
      }

    } // class Point

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
