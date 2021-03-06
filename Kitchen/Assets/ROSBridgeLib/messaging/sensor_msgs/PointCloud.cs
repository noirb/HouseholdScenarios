// Generated by gencs from sensor_msgs/PointCloud.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.std_msgs;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.sensor_msgs;

namespace ROSBridgeLib {
  namespace sensor_msgs {

    [System.Serializable]
    public class PointCloud : ROSMessage
    {
      public std_msgs.Header header;
      public System.Collections.Generic.List<geometry_msgs.Point32>  points;
      public System.Collections.Generic.List<sensor_msgs.ChannelFloat32>  channels;


      public PointCloud()
      {
        header = new std_msgs.Header();
        points = new System.Collections.Generic.List<geometry_msgs.Point32>();
        channels = new System.Collections.Generic.List<sensor_msgs.ChannelFloat32>();

      }

      public PointCloud(std_msgs.Header _header, System.Collections.Generic.List<geometry_msgs.Point32>  _points, System.Collections.Generic.List<sensor_msgs.ChannelFloat32>  _channels)
      {
        header = _header;
        points = _points;
        channels = _channels;
      }

      new public static string GetMessageType()
      {
        return "sensor_msgs/PointCloud";
      }

      new public static string GetMD5Hash()
      {
        return "d8e9c3f5afbdd8a130fd1d2763945fca";
      }

    } // class PointCloud

  } // namespace sensor_msgs

} // namespace ROSBridgeLib
