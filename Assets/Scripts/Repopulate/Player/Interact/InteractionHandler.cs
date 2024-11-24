using UnityEngine;

namespace Repopulate.Player {
    public class InteractionHandler : MonoBehaviour {

        [SerializeField] private float _interactDistance;

        public void InteractPrimary(PlayerControllable controllable, Camera cam) {
            Interact(controllable, cam, InteractLevel.PRIMARY);
        }

        public void InteractSecondary(PlayerControllable controllable, Camera cam) {
            Interact(controllable, cam, InteractLevel.SECONDARY);
        }

        public void InteractTertiary(PlayerControllable controllable, Camera cam) {
            Interact(controllable, cam, InteractLevel.TERTIARY);
        }

        
        //TODO! This currently assumes the base object has a big collider the size of the whole object.
        //Instead we should have a script which can be attached to any collider on an object, that points to the base object, so interactions always work.
        //ALSO we call this raycast TWICE! Once here, and once for the UI element to show the interact prompt. Can we just do once?
        private void Interact(PlayerControllable controllable, Camera cam, InteractLevel interactLevel) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, _interactDistance, Constants.MASK_PLACEABLE_BASE)) {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null) {
                    interactable.OnInteract(controllable, interactLevel);
                    Debug.Log($"Interacting with {hit.transform.name}");
                }
            }
        }
    }
}