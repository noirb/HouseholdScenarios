// Generated by gencs from ontology_svcs/RelabelActivity.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace ontology_svcs {

    [System.Serializable]
    public class RelabelActivityResponse : ServiceResponse
    {
      public bool success;


      public RelabelActivityResponse()
      {
        success = false;

      }

      public RelabelActivityResponse(bool _success)
      {
        success = _success;
      }

      new public static string GetMessageType()
      {
        return "ontology_svcs/RelabelActivityResponse";
      }

      new public static string GetMD5Hash()
      {
        return "358e233cde0c8a8bcfea4ce193f8fc15";
      }

    } // class RelabelActivityResponse

  } // namespace ontology_svcs

} // namespace ROSBridgeLib
