// Generated by gencs from std_msgs/UInt64MultiArray.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.std_msgs;

namespace ROSBridgeLib {
  namespace std_msgs {

    [System.Serializable]
    public class UInt64MultiArray : ROSMessage
    {
      public std_msgs.MultiArrayLayout layout;
      public System.Collections.Generic.List<System.UInt64>  data;


      public UInt64MultiArray()
      {
        layout = new std_msgs.MultiArrayLayout();
        data = new System.Collections.Generic.List<System.UInt64>();

      }

      public UInt64MultiArray(std_msgs.MultiArrayLayout _layout, System.Collections.Generic.List<System.UInt64>  _data)
      {
        layout = _layout;
        data = _data;
      }

      new public static string GetMessageType()
      {
        return "std_msgs/UInt64MultiArray";
      }

      new public static string GetMD5Hash()
      {
        return "6088f127afb1d6c72927aa1247e945af";
      }

    } // class UInt64MultiArray

  } // namespace std_msgs

} // namespace ROSBridgeLib
