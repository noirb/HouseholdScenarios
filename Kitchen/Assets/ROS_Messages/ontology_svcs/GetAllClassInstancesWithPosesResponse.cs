// Generated by gencs from ontology_svcs/GetAllClassInstancesWithPoses.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.ontology_msgs;

namespace ROSBridgeLib {
  namespace ontology_svcs {

    [System.Serializable]
    public class GetAllClassInstancesWithPosesResponse : ServiceResponse
    {
      public System.Collections.Generic.List<string>  classes;
      public System.Collections.Generic.List<string>  entityNames;
      public System.Collections.Generic.List<UnityEngine.Vector3>  positions;
      public System.Collections.Generic.List<UnityEngine.Quaternion>  orientations;
      public System.Collections.Generic.List<ontology_msgs.OntoPropertyList>  properties;


      public GetAllClassInstancesWithPosesResponse()
      {
        classes = new System.Collections.Generic.List<string>();
        entityNames = new System.Collections.Generic.List<string>();
        positions = new System.Collections.Generic.List<UnityEngine.Vector3>();
        orientations = new System.Collections.Generic.List<UnityEngine.Quaternion>();
        properties = new System.Collections.Generic.List<ontology_msgs.OntoPropertyList>();

      }

      public GetAllClassInstancesWithPosesResponse(System.Collections.Generic.List<string>  _classes, System.Collections.Generic.List<string>  _entityNames, System.Collections.Generic.List<UnityEngine.Vector3>  _positions, System.Collections.Generic.List<UnityEngine.Quaternion>  _orientations, System.Collections.Generic.List<ontology_msgs.OntoPropertyList>  _properties)
      {
        classes = _classes;
        entityNames = _entityNames;
        positions = _positions;
        orientations = _orientations;
        properties = _properties;
      }

      new public static string GetMessageType()
      {
        return "ontology_svcs/GetAllClassInstancesWithPosesResponse";
      }

      new public static string GetMD5Hash()
      {
        return "ef176046c2937f589a7cf952cc06e608";
      }

    } // class GetAllClassInstancesWithPosesResponse

  } // namespace ontology_svcs

} // namespace ROSBridgeLib
