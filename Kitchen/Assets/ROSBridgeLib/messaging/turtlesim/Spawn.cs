// Generated by gencs from file turtlesim/Spawn.srv
// DO NOT EDIT THIS FILE BY HAND


using ROSBridgeLib.turtlesim;

namespace ROSBridgeLib {
  namespace turtlesim {

    public class Spawn : ROSBridgeServiceProvider<turtlesim.SpawnRequest>
    {
      public Spawn(string serviceName) : base(serviceName)
      {
        _type = "turtlesim/Spawn";
      }

      public Spawn(string serviceName, string serviceType = "turtlesim/Spawn") : base(serviceName, serviceType) {}
    }

  } // namespace turtlesim
} // namespace ROSBridgeLib

