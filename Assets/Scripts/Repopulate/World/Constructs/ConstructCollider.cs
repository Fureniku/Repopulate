using UnityEngine;

namespace Repopulate.World.Constructs {
    public class ConstructCollider : MonoBehaviour {

        [SerializeField] private PlaceableConstruct _construct;
        
        void Awake() {
            if (_construct == null) {
                _construct = AttemptToFind();
            }
        }

        private PlaceableConstruct AttemptToFind() {
            PlaceableConstruct foundConstruct = GetComponent<PlaceableConstruct>() 
                                           ?? transform.GetComponentInParent<PlaceableConstruct>()
                                           ?? GetComponentInChildren<PlaceableConstruct>();

            if (foundConstruct == null) {
                Debug.LogError($"Unable to find a construct for collider {name}");
            }

            return foundConstruct;
        }

        public PlaceableConstruct Construct() {
            return _construct;
        }
    }
}
