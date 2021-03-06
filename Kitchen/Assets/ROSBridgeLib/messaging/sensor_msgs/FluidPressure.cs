// Generated by gencs from sensor_msgs/FluidPressure.msg
// DO NOT EDIT THIS FILE BY HAND!

using System;
using System.Collections;
using System.Collections.Generic;
using ROSBridgeLib;
using UnityEngine;

using ROSBridgeLib.std_msgs;

namespace ROSBridgeLib {
  namespace sensor_msgs {

    [System.Serializable]
    public class FluidPressure : ROSMessage
    {
      public std_msgs.Header header;
      public double fluid_pressure;
      public double variance;


      public FluidPressure()
      {
        header = new std_msgs.Header();
        fluid_pressure = 0.0;
        variance = 0.0;

      }

      public FluidPressure(std_msgs.Header _header, double _fluid_pressure, double _variance)
      {
        header = _header;
        fluid_pressure = _fluid_pressure;
        variance = _variance;
      }

      new public static string GetMessageType()
      {
        return "sensor_msgs/FluidPressure";
      }

      new public static string GetMD5Hash()
      {
        return "804dc5cea1c5306d6a2eb80b9833befe";
      }

    } // class FluidPressure

  } // namespace sensor_msgs

} // namespace ROSBridgeLib
