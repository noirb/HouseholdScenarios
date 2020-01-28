// Generated by gencs from geometry_msgs/AccelWithCovarianceStamped.msg
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
    public class AccelWithCovarianceStamped : ROSMessage
    {
      public std_msgs.Header header;
      public geometry_msgs.AccelWithCovariance accel;


      public AccelWithCovarianceStamped()
      {
        header = new std_msgs.Header();
        accel = new geometry_msgs.AccelWithCovariance();

      }

      public AccelWithCovarianceStamped(std_msgs.Header _header, geometry_msgs.AccelWithCovariance _accel)
      {
        header = _header;
        accel = _accel;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/AccelWithCovarianceStamped";
      }

      new public static string GetMD5Hash()
      {
        return "96adb295225031ec8d57fb4251b0a886";
      }

    } // class AccelWithCovarianceStamped

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
