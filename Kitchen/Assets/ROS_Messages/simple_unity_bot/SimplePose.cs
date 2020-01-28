// Generated by gencs from simple_unity_bot/SimplePose.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.geometry_msgs;

namespace ROSBridgeLib {
  namespace simple_unity_bot {

    [System.Serializable]
    public class SimplePose : ROSMessage
    {
      public string name;
      public UnityEngine.Vector3 position;
      public UnityEngine.Quaternion orientation;


      public SimplePose()
      {
        name = "";
        position = new UnityEngine.Vector3();
        orientation = new UnityEngine.Quaternion();

      }

      public SimplePose(string _name, UnityEngine.Vector3 _position, UnityEngine.Quaternion _orientation)
      {
        name = _name;
        position = _position;
        orientation = _orientation;
      }

      new public static string GetMessageType()
      {
        return "simple_unity_bot/SimplePose";
      }

      new public static string GetMD5Hash()
      {
        return "65b1250e41281e50a63b24196f1ad619";
      }

    } // class SimplePose

  } // namespace simple_unity_bot

} // namespace ROSBridgeLib