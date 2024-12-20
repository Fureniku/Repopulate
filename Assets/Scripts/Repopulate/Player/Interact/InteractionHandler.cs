using System;
using Repopulate.Inventory;
using UnityEngine;

namespace Repopulate.Player {
    public class InteractionHandler : MonoBehaviour {

        [SerializeField] private Camera _droidCam;
        [SerializeField] private PlayerControllable _controllable;
        [SerializeField] private float _interactDistance;
        
        protected GameObject _lastObject;
        private PreviewConstruct _previewConstruct;

        public GameObject LastAimedObject => _lastObject;
        
        public static event Action<GameObject, PlayerControllable> OnAimedObjectChanged;

        void Start() {
            OnAimedObjectChanged?.Invoke(null, _controllable);
            _previewConstruct = GameManager.Instance.PreviewConstruct;
        }
        
        private void LookAtEvent() { //aim targets: construct, celestial
            if (_droidCam.enabled) {
                Ray ray = new Ray(_droidCam.transform.position, _droidCam.transform.forward);
                if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, _interactDistance, Constants.MASK_INTERACTABLE))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject != _lastObject)
                    {
                        _lastObject = hitObject;
                        OnAimedObjectChanged?.Invoke(hitObject, _controllable);
                    }
                }
                else
                {
                    if (_lastObject != null) {
                        _lastObject = null;
                        OnAimedObjectChanged?.Invoke(null, _controllable);
                    }
                }
            }
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
            if (_previewConstruct != null) {
                _previewConstruct.UpdatePreview(cam);
            }
            else {
                Debug.Log("Preview construct was null while updating camera, skipping.");
            }
        }
    }
}