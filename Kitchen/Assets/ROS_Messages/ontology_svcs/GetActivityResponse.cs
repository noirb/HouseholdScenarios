// Generated by gencs from ontology_svcs/GetActivity.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.ontology_msgs;

namespace ROSBridgeLib {
  namespace ontology_svcs {

    [System.Serializable]
    public class GetActivityResponse : ServiceResponse
    {
      public ontology_msgs.Activity activity;


      public GetActivityResponse()
      {
        activity = new ontology_msgs.Activity();

      }

      public GetActivityResponse(ontology_msgs.Activity _activity)
      {
        activity = _activity;
      }

      new public static string GetMessageType()
      {
        return "ontology_svcs/GetActivityResponse";
      }

      new public static string GetMD5Hash()
      {
        return "b4d7ef05bad2c155cabfa0f01d26d766";
      }

    } // class GetActivityResponse

  } // namespace ontology_svcs

} // namespace ROSBridgeLib