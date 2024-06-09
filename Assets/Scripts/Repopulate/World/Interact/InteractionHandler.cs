using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    [SerializeField] private float _interactDistance;
    
    public void Interact(PlayerControllable controllable, Camera cam) {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null) {
                interactable.OnInteract(controllable);
                Debug.Log($"Interacting with {hit.transform.name}");
            }
        }
    }
}
