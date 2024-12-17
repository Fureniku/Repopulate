using Repopulate.Player;
using Repopulate.ScriptableObjects;
using UnityEngine;

namespace Repopulate.World.Constructs {
	public class InteractableCollider : MonoBehaviour, IInteractable {

		[SerializeField] private GameObject _interactableObject;
		[SerializeField] private IInteractable _interactable;
        
		void Awake() {
			if (_interactable == null) {
				_interactable = AttemptToFind();
			}
		}

		private IInteractable AttemptToFind() {
			IInteractable foundConstruct = _interactableObject.GetComponent<IInteractable>() 
			                               ?? GetComponent<IInteractable>() 
			                               ?? transform.parent?.GetComponent<IInteractable>() 
			                               ?? transform.GetComponentInParent<IInteractable>()
			                               ?? GetComponentInChildren<IInteractable>();

			if (foundConstruct == null) {
				Debug.LogError($"Unable to find a construct for collider {name}");
			}

			return foundConstruct;
		}

		public IInteractable GetInteractable() {
			return _interactable;
		}

		public Construct GetConstruct() {
			return _interactable.GetConstruct();
		}

		public void OnInteract(PlayerControllable controllable, InteractLevel interactLevel) {
			_interactable.OnInteract(controllable, interactLevel);
		}

		public void OnLookAt(PlayerControllable controllable) {
			_interactable.OnLookAt(controllable);
		}
	}
}