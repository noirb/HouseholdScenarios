// Generated by gencs from ontology_svcs/DisplayLabel.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace ontology_svcs {

    [System.Serializable]
    public class DisplayLabelRequest : ServiceArgs
    {
      public string text;
      public System.UInt32 lbl_id;


      public DisplayLabelRequest()
      {
        text = "";
        lbl_id = 0;

      }

      public DisplayLabelRequest(string _text, System.UInt32 _lbl_id)
      {
        text = _text;
        lbl_id = _lbl_id;
      }

      new public static string GetMessageType()
      {
        return "ontology_svcs/DisplayLabelRequest";
      }

      new public static string GetMD5Hash()
      {
        return "9c40d93b6e19d3165743dc6281bd45ce";
      }

    } // class DisplayLabelRequest

  } // namespace ontology_svcs

} // namespace ROSBridgeLib