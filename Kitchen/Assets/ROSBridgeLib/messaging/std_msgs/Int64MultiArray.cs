// Generated by gencs from std_msgs/Int64MultiArray.msg
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
    public class Int64MultiArray : ROSMessage
    {
      public std_msgs.MultiArrayLayout layout;
      public System.Collections.Generic.List<System.Int64>  data;


      public Int64MultiArray()
      {
        layout = new std_msgs.MultiArrayLayout();
        data = new System.Collections.Generic.List<System.Int64>();

      }

      public Int64MultiArray(std_msgs.MultiArrayLayout _layout, System.Collections.Generic.List<System.Int64>  _data)
      {
        layout = _layout;
        data = _data;
      }

      new public static string GetMessageType()
      {
        return "std_msgs/Int64MultiArray";
      }

      new public static string GetMD5Hash()
      {
        return "54865aa6c65be0448113a2afc6a49270";
      }

    } // class Int64MultiArray

  } // namespace std_msgs

} // namespace ROSBridgeLib
