using UnityEngine;

namespace Repopulate.Player {
    public class InteractionHandler : MonoBehaviour {

        [SerializeField] private float _interactDistance;

        //TODO! This currently assumes the base object has a big collider the size of the whole object.
        //Instead we should have a script which can be attached to any collider on an object, that points to the base object, so interactions always work.
        public void Interact(PlayerControllable controllable, InteractLevel interactLevel, GameObject hitObject) {
            IInteractable interactable = hitObject.GetComponent<IInteractable>();
            if (interactable != null) {
                interactable.OnInteract(controllable, interactLevel);
            }
        }
    }
}