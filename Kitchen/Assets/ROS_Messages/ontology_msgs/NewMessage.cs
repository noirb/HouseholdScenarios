// Generated by gencs from ontology_msgs/NewMessage.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace ontology_msgs {

    [System.Serializable]
    public class NewMessage : ROSMessage
    {
      public System.Int64 id;


      public NewMessage()
      {
        id = 0;

      }

      public NewMessage(System.Int64 _id)
      {
        id = _id;
      }

      new public static string GetMessageType()
      {
        return "ontology_msgs/NewMessage";
      }

      new public static string GetMD5Hash()
      {
        return "ef7df1d34137d3879d089ad803388efa";
      }

    } // class NewMessage

  } // namespace ontology_msgs

} // namespace ROSBridgeLib
