// Generated by gencs from std_msgs/Int16.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace std_msgs {

    [System.Serializable]
    public class Int16 : ROSMessage
    {
      public System.Int16 data;


      public Int16()
      {
        data = 0;

      }

      public Int16(System.Int16 _data)
      {
        data = _data;
      }

      new public static string GetMessageType()
      {
        return "std_msgs/Int16";
      }

      new public static string GetMD5Hash()
      {
        return "8524586e34fbd7cb1c08c5f5f1ca0e57";
      }

    } // class Int16

  } // namespace std_msgs

} // namespace ROSBridgeLib
