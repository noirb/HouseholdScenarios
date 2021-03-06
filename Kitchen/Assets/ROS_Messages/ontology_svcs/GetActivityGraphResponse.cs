// Generated by gencs from ontology_svcs/GetActivityGraph.srv
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
    public class GetActivityGraphResponse : ServiceResponse
    {
      public ontology_msgs.Graph graph;


      public GetActivityGraphResponse()
      {
        graph = new ontology_msgs.Graph();

      }

      public GetActivityGraphResponse(ontology_msgs.Graph _graph)
      {
        graph = _graph;
      }

      new public static string GetMessageType()
      {
        return "ontology_svcs/GetActivityGraphResponse";
      }

      new public static string GetMD5Hash()
      {
        return "d27cf71443cd1d0607e04229b17ba4d7";
      }

    } // class GetActivityGraphResponse

  } // namespace ontology_svcs

} // namespace ROSBridgeLib
