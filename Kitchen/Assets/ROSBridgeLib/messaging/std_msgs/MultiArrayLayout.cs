// Generated by gencs from std_msgs/MultiArrayLayout.msg
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
    public class MultiArrayLayout : ROSMessage
    {
      public System.Collections.Generic.List<std_msgs.MultiArrayDimension>  dim;
      public System.UInt32 data_offset;


      public MultiArrayLayout()
      {
        dim = new System.Collections.Generic.List<std_msgs.MultiArrayDimension>();
        data_offset = 0;

      }

      public MultiArrayLayout(System.Collections.Generic.List<std_msgs.MultiArrayDimension>  _dim, System.UInt32 _data_offset)
      {
        dim = _dim;
        data_offset = _data_offset;
      }

      new public static string GetMessageType()
      {
        return "std_msgs/MultiArrayLayout";
      }

      new public static string GetMD5Hash()
      {
        return "0fed2a11c13e11c5571b4e2a995a91a3";
      }

    } // class MultiArrayLayout

  } // namespace std_msgs

} // namespace ROSBridgeLib
