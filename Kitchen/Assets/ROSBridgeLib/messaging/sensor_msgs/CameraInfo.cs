// Generated by gencs from sensor_msgs/CameraInfo.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;

namespace ROSBridgeLib {
  namespace sensor_msgs {

    [System.Serializable]
    public class CameraInfo : ROSMessage
    {
      public std_msgs.Header header;
      public System.UInt32 height;
      public System.UInt32 width;
      public string distortion_model;
      public System.Collections.Generic.List<double>  D;
      public double[]  K;
      public double[]  R;
      public double[]  P;
      public System.UInt32 binning_x;
      public System.UInt32 binning_y;
      public sensor_msgs.RegionOfInterest roi;


      public CameraInfo()
      {
        header = new std_msgs.Header();
        height = 0;
        width = 0;
        distortion_model = "";
        D = new System.Collections.Generic.List<double>();
        K = new double[9];
        R = new double[9];
        P = new double[12];
        binning_x = 0;
        binning_y = 0;
        roi = new sensor_msgs.RegionOfInterest();

      }

      public CameraInfo(std_msgs.Header _header, System.UInt32 _height, System.UInt32 _width, string _distortion_model, System.Collections.Generic.List<double>  _D, double[]  _K, double[]  _R, double[]  _P, System.UInt32 _binning_x, System.UInt32 _binning_y, sensor_msgs.RegionOfInterest _roi)
      {
        header = _header;
        height = _height;
        width = _width;
        distortion_model = _distortion_model;
        D = _D;
        K = _K;
        R = _R;
        P = _P;
        binning_x = _binning_x;
        binning_y = _binning_y;
        roi = _roi;
      }

      new public static string GetMessageType()
      {
        return "sensor_msgs/CameraInfo";
      }

      new public static string GetMD5Hash()
      {
        return "c9a58c1b0b154e0e6da7578cb991d214";
      }

    } // class CameraInfo

  } // namespace sensor_msgs

} // namespace ROSBridgeLib