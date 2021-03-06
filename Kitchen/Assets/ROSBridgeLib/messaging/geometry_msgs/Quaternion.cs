// Generated by gencs from geometry_msgs/Quaternion.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace geometry_msgs {

    [System.Serializable]
    public class Quaternion : ROSMessage
    {
      public double x;
      public double y;
      public double z;
      public double w;


      public Quaternion()
      {
        x = 0.0;
        y = 0.0;
        z = 0.0;
        w = 0.0;

      }

      public Quaternion(double _x, double _y, double _z, double _w)
      {
        x = _x;
        y = _y;
        z = _z;
        w = _w;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/Quaternion";
      }

      new public static string GetMD5Hash()
      {
        return "a779879fadf0160734f906b8c19c7004";
      }

    } // class Quaternion

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
