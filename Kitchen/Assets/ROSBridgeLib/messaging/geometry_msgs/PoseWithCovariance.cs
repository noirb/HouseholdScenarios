// Generated by gencs from geometry_msgs/PoseWithCovariance.msg
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
    public class PoseWithCovariance : ROSMessage
    {
      public geometry_msgs.Pose pose;
      public double[]  covariance;


      public PoseWithCovariance()
      {
        pose = new geometry_msgs.Pose();
        covariance = new double[36];

      }

      public PoseWithCovariance(geometry_msgs.Pose _pose, double[]  _covariance)
      {
        pose = _pose;
        covariance = _covariance;
      }

      new public static string GetMessageType()
      {
        return "geometry_msgs/PoseWithCovariance";
      }

      new public static string GetMD5Hash()
      {
        return "c23e848cf1b7533a8d7c259073a97e6f";
      }

    } // class PoseWithCovariance

  } // namespace geometry_msgs

} // namespace ROSBridgeLib
