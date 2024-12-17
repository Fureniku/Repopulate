using System;
using Repopulate.ScriptableObjects;
using UnityEngine;

namespace Repopulate.Player {
	public abstract class PlayerControllable : MonoBehaviour {
	
		[SerializeField] protected Camera _camera;
		[SerializeField] protected float _maxInteractRange = 5;
	
		private InteractionHandler _interactionHandler;
		
		protected GameObject _lastObject;
		public static event Action<GameObject, PlayerControllable> OnAimedObjectChanged;
	
		protected abstract void ControllableAwake();
		protected abstract void ControllableUpdate();
		protected abstract void ControllableFixedUpdate();

		void Awake() {
			ControllableAwake();
			_interactionHandler = GetComponent<InteractionHandler>();
			OnAimedObjectChanged?.Invoke(null, this);
		}

		void Update() {
			ControllableUpdate();
			LookAtEvent();
		}

		void FixedUpdate() {
			ControllableFixedUpdate();
		}

		#region General Controls
		public virtual void InteractPrimary() {
			_interactionHandler.Interact(this, InteractLevel.PRIMARY, _lastObject);
		}
		
		public virtual void InteractSecondary() {
			_interactionHandler.Interact(this, InteractLevel.SECONDARY, _lastObject);
		}
		
		public virtual void InteractTertiary() {
			_interactionHandler.Interact(this, InteractLevel.TERTIARY, _lastObject);
		}
		#endregion
	
		public void SetCameraStatus(bool camOn) {
			_camera.enabled = camOn;
		}
	
		public void ResetCamera() {
			_camera.transform.localRotation = Quaternion.identity;
		}

		private void LookAtEvent() { //aim targets: construct, celestial
			if (_camera.enabled) {
				Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
				if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, _maxInteractRange, Constants.MASK_INTERACTABLE))
				{
					GameObject hitObject = hit.collider.gameObject;
					if (hitObject != _lastObject)
					{
						_lastObject = hitObject;
						OnAimedObjectChanged?.Invoke(hitObject, this);
					}
				}
				else
				{
					if (_lastObject != null) {
						_lastObject = null;
						OnAimedObjectChanged?.Invoke(null, this);
					}
				}
			}
		}
	}
}
