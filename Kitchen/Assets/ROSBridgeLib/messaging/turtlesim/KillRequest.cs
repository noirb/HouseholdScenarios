// Generated by gencs from turtlesim/Kill.srv
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;


namespace ROSBridgeLib {
  namespace turtlesim {

    [System.Serializable]
    public class KillRequest : ServiceArgs
    {
      public string name;


      public KillRequest()
      {
        name = "";

      }

      public KillRequest(string _name)
      {
        name = _name;
      }

      new public static string GetMessageType()
      {
        return "turtlesim/KillRequest";
      }

      new public static string GetMD5Hash()
      {
        return "c1f3d28f1b044c871e6eff2e9fc3c667";
      }

    } // class KillRequest

  } // namespace turtlesim

} // namespace ROSBridgeLib
