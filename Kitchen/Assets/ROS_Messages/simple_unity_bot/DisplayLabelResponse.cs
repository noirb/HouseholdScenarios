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
    public class DisplayLabelResponse : ServiceResponse
    {
      public System.UInt32 lbl_id;


      public DisplayLabelResponse()
      {
        lbl_id = 0;

      }

      public DisplayLabelResponse(System.UInt32 _lbl_id)
      {
        lbl_id = _lbl_id;
      }

      new public static string GetMessageType()
      {
        return "simple_unity_bot/DisplayLabelResponse";
      }

      new public static string GetMD5Hash()
      {
        return "92122ced4d99f15516245f1c3c5fd055";
      }

    } // class DisplayLabelResponse

  } // namespace simple_unity_bot

} // namespace ROSBridgeLib
