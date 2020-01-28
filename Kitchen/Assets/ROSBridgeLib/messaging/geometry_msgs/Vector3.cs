// Generated by gencs from geometry_msgs/Vector3.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace geometry_msgs {

    [System.Serializable]
    public class Vector3 : ROSMessage
    {
      public double x;
      public double y;
      public double z;


      public Vector3()
      {
        x = 0.0;
        y = 0.0;
        z = 0.0;

      }

      public Vector3(double _x, double _y, double _z)
      {
        x = _x;
        y = _y;
        z = _z;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/Vector3";
      }

      new public static string GetMD5Hash()
      {
        return "4a842b65f413084dc2b10fb484ea7f17";
      }

    } // class Vector3

  } // namespace geometry_msgs

} // namespace ROSBridgeLib