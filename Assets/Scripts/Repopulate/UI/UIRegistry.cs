using System.Collections.Generic;
using Repopulate.Player;
using Repopulate.Utils.Registries;
using UnityEngine;

namespace Repopulate.UI {

    [CreateAssetMenu]
    public class UIRegistry : RegistryBase<UIController> {
        
        private static UIRegistry _instance;
        
        public static UIRegistry Instance {
            get {
                if (ReferenceEquals(_instance, null)) {
                    _instance = Resources.Load<UIRegistry>("UI_Registry");
                    if (!ReferenceEquals(_instance, null)) {
                        _instance.Initialize();
                    } else {
                        Debug.LogError("No instance of UI_Registry found in Resources folder.");
                    }
                }
                return _instance;
            }
        }
        
        protected override List<UIController> GetList() {
            throw new System.NotImplementedException();
        }

        public override UIController GetFromName(string resourceName) {
            //should never be used for UI
            throw new System.NotImplementedException();
        }
    }
}