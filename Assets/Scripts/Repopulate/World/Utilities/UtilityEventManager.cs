using System;

namespace Repopulate.World.Utilities {
    public static class UtilityEventManager {
        public static event Action<EnumUtility, object> ResourceObjectCreated;
        public static event Action<EnumUtility, object> ResourceObjectDestroyed;

        public static void OnResourceObjectCreated(EnumUtility type, object energyObject) {
            ResourceObjectCreated?.Invoke(type, energyObject);
        }

        public static void OnResourceObjectDestroyed(EnumUtility type, object energyObject) {
            ResourceObjectDestroyed?.Invoke(type, energyObject);
        }
    }
}