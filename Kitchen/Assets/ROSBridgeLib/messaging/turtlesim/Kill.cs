// Generated by gencs from file turtlesim/Kill.srv
// DO NOT EDIT THIS FILE BY HAND


using ROSBridgeLib.turtlesim;

namespace ROSBridgeLib {
  namespace turtlesim {

    public class Kill : ROSBridgeServiceProvider<turtlesim.KillRequest>
    {
      public Kill(string serviceName) : base(serviceName)
      {
        _type = "turtlesim/Kill";
      }

      public Kill(string serviceName, string serviceType = "turtlesim/Kill") : base(serviceName, serviceType) {}
    }

  } // namespace turtlesim
} // namespace ROSBridgeLib

