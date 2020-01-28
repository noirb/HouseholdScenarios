// Generated by gencs from ontology_msgs/PropertyChanged.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace ontology_msgs {

    [System.Serializable]
    public class PropertyChanged : ROSMessage
    {
      public ROSBridgeLib.msg_helpers.Time timestamp;
      public string instanceName;
      public string propertyName;
      public string propertyValue;


      public PropertyChanged()
      {
        timestamp = new ROSBridgeLib.msg_helpers.Time();
        instanceName = "";
        propertyName = "";
        propertyValue = "";

      }

      public PropertyChanged(ROSBridgeLib.msg_helpers.Time _timestamp, string _instanceName, string _propertyName, string _propertyValue)
      {
        timestamp = _timestamp;
        instanceName = _instanceName;
        propertyName = _propertyName;
        propertyValue = _propertyValue;
      }

      new public static string GetMessageType()
      {
        return "ontology_msgs/PropertyChanged";
      }

      new public static string GetMD5Hash()
      {
        return "556f0042f58b548dea4c1ba83f756b82";
      }

    } // class PropertyChanged

  } // namespace ontology_msgs

} // namespace ROSBridgeLib
