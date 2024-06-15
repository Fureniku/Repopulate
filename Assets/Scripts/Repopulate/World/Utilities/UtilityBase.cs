using UnityEngine;

namespace Repopulate.World.Utilities {
    public class ResourceBase : MonoBehaviour {
    
        [SerializeField] private EnumUtility _utilityType;
    
        private void Start() {
            UtilityEventManager.OnResourceObjectCreated(_utilityType, this);
            ResourceStart();
        }

        private void OnDestroy() {
            UtilityEventManager.OnResourceObjectDestroyed(_utilityType, this);
            ResourceDestroy();
        }

        protected void ResourceStart() { }
        protected void ResourceDestroy() { }
    }
}
