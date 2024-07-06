using System.Collections.Generic;
using Repopulate.ScriptableObjects;
using UnityEngine;

namespace Repopulate.Utils.Registries {

    [CreateAssetMenu]
    public class ModuleRegistry : RegistryBase<Module> {
        private static ModuleRegistry _instance;
    
        public static ModuleRegistry Instance {
            get {
                if (_instance == null) {
                    _instance = Resources.Load<ModuleRegistry>("Module_Registry");
                    if (_instance != null) {
                        _instance.Initialize();
                    } else {
                        Debug.LogError("No instance of Module_Registry found in Resources folder.");
                    }
                }
                return _instance;
            }
        }

        public Module MAIN_WAREHOUSE_FULL_XYZ;

        public Module SUB_BUILDABLE_8_4_12;

        protected override List<Module> GetList() {
            List<Module> moduleList = new();

            return moduleList;
        }
        
        public override Module GetFromName(string moduleName) {
            throw new System.NotImplementedException();
        }
    }
}
