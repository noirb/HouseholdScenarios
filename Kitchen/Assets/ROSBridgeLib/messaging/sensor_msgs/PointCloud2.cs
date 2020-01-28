// Generated by gencs from sensor_msgs/PointCloud2.msg
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
    public class PointCloud2 : ROSMessage
    {
      public std_msgs.Header header;
      public System.UInt32 height;
      public System.UInt32 width;
      public System.Collections.Generic.List<sensor_msgs.PointField>  fields;
      public bool is_bigendian;
      public System.UInt32 point_step;
      public System.UInt32 row_step;
      public System.Collections.Generic.List<byte>  data;
      public bool is_dense;


      public PointCloud2()
      {
        header = new std_msgs.Header();
        height = 0;
        width = 0;
        fields = new System.Collections.Generic.List<sensor_msgs.PointField>();
        is_bigendian = false;
        point_step = 0;
        row_step = 0;
        data = new System.Collections.Generic.List<byte>();
        is_dense = false;

      }

      public PointCloud2(std_msgs.Header _header, System.UInt32 _height, System.UInt32 _width, System.Collections.Generic.List<sensor_msgs.PointField>  _fields, bool _is_bigendian, System.UInt32 _point_step, System.UInt32 _row_step, System.Collections.Generic.List<byte>  _data, bool _is_dense)
      {
        header = _header;
        height = _height;
        width = _width;
        fields = _fields;
        is_bigendian = _is_bigendian;
        point_step = _point_step;
        row_step = _row_step;
        data = _data;
        is_dense = _is_dense;
      }

      new public static string GetMessageType()
      {
        return "sensor_msgs/PointCloud2";
      }

      new public static string GetMD5Hash()
      {
        return "1158d486dd51d683ce2f1be655c3c181";
      }

    } // class PointCloud2

  } // namespace sensor_msgs

} // namespace ROSBridgeLib
