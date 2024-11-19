using System.Collections.Generic;
using UnityEngine;
using Repopulate.ScriptableObjects;

namespace Repopulate.Utils.Registries {
    
    public class ConstructRegistry : RegistryBase<Construct> {
    
        private static ConstructRegistry _instance;
    
        public static ConstructRegistry Instance {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<ConstructRegistry>("Construct_Registry");
                    if (_instance != null) {
                        _instance.Initialize();
                    } else {
                        Debug.LogError("No instance of Construct_Registry found in Resources folder.");
                    }
                }
                return _instance;
            }
        }

        public Construct EMPTY;
        public Construct ALGAE_FARM_1;
        public Construct PILLAR_1;
        public Construct BATTERY_1;
        public Construct CUBE_PANELS;
        public Construct CUBE_METAL;
        public Construct WALL_LIGHT_1;
        public Construct PLANET_MINER_1;

        protected override List<Construct> GetList() {
            List<Construct> _constructList = new();

            //These MUST be in ID order.
            _constructList.Add(EMPTY);
            _constructList.Add(ALGAE_FARM_1);
            _constructList.Add(PILLAR_1);
            _constructList.Add(BATTERY_1);
            _constructList.Add(CUBE_PANELS);
            _constructList.Add(CUBE_METAL);
            _constructList.Add(WALL_LIGHT_1);
            _constructList.Add(PLANET_MINER_1);
        
            return _constructList;
        }

        public override Construct GetFromName(string constructName) {
            throw new System.NotImplementedException();
        }
    }
}
