// Generated by gencs from ontology_msgs/OntoProperty.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace ontology_msgs {

    [System.Serializable]
    public class OntoProperty : ROSMessage
    {
      public string name;
      public string value;


      public OntoProperty()
      {
        name = "";
        value = "";

      }

      public OntoProperty(string _name, string _value)
      {
        name = _name;
        value = _value;
      }

      new public static string GetMessageType()
      {
        return "ontology_msgs/OntoProperty";
      }

      new public static string GetMD5Hash()
      {
        return "bc6ccc4a57f61779c8eaae61e9f422e0";
      }

    } // class OntoProperty

  } // namespace ontology_msgs

} // namespace ROSBridgeLib