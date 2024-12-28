using System;
using UnityEngine;

namespace Repopulate.Player {
    public class InteractionHandler : MonoBehaviour {

        [SerializeField] private Camera _droidCam;
        [SerializeField] private PlayerControllable _controllable;
        [SerializeField] private DroidControllerBase _droid;
        [SerializeField] private float _interactDistance;
        
        protected GameObject _lastObject;

        public GameObject LastAimedObject => _lastObject;
        
        public static event Action<IInteractable, PlayerControllable> OnAimedObjectChanged;

        void Start() {
            OnAimedObjectChanged?.Invoke(null, _controllable);
        }

        //TODO! This currently assumes the base object has a big collider the size of the whole object.
        //Instead we should have a script which can be attached to any collider on an object, that points to the base object, so interactions always work.
        public void Interact(PlayerControllable controllable, InteractLevel interactLevel) {
            IInteractable interactable = _lastObject.GetComponent<IInteractable>();
            if (interactable != null) {
                interactable.OnInteract(controllable, interactLevel);
            }
        }

        public void UpdatePreview(Camera cam) {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, _interactDistance, Constants.MASK_BUILDABLE)) {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject != _lastObject)
                {
                    _lastObject = hitObject;
                    if (hitObject.TryGetComponent(out IInteractable interactable)) {
			            interactable.OnLookAt(_controllable);
                        OnAimedObjectChanged?.Invoke(interactable, _controllable);
                    }
                    else {
                        OnAimedObjectChanged?.Invoke(null, _controllable);
                    }
                }
                _droid.PreviewConstruct.UpdatePreview(hit, hitObject);
            }
            else {
                _droid.PreviewConstruct.HidePreview();
                if (_lastObject != null) { //Don't repeat call if we already invoked null
                    _lastObject = null;
                    OnAimedObjectChanged?.Invoke(null, _controllable);
                }
            }
        }
    }
}