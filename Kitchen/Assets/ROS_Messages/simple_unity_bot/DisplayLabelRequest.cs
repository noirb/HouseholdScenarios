// Generated by gencs from simple_unity_bot/DisplayLabel.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace simple_unity_bot {

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
        return "simple_unity_bot/DisplayLabelRequest";
      }

      new public static string GetMD5Hash()
      {
        return "9c40d93b6e19d3165743dc6281bd45ce";
      }

    } // class DisplayLabelRequest

  } // namespace simple_unity_bot

} // namespace ROSBridgeLib
