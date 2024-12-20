using System;
using UnityEngine;

namespace Repopulate.Player {
	public abstract class PlayerControllable : MonoBehaviour {
	
		[SerializeField] protected Camera _camera;
	
		private InteractionHandler _interactionHandler;

		protected abstract void ControllableAwake();
		protected abstract void ControllableUpdate();
		protected abstract void ControllableFixedUpdate();

		void Start() {
			ControllableAwake();
			_interactionHandler = GetComponent<InteractionHandler>();
		}

		void Update() {
			ControllableUpdate();
		}

		void FixedUpdate() {
			ControllableFixedUpdate();
		}

		#region General Controls
		public virtual void InteractPrimary() {
			_interactionHandler.Interact(this, InteractLevel.PRIMARY);
		}
		
		public virtual void InteractSecondary() {
			_interactionHandler.Interact(this, InteractLevel.SECONDARY);
		}
		
		public virtual void InteractTertiary() {
			_interactionHandler.Interact(this, InteractLevel.TERTIARY);
		}
		#endregion
	
		public void SetCameraStatus(bool camOn) {
			_camera.enabled = camOn;
		}
	
		public void ResetCamera() {
			_camera.transform.localRotation = Quaternion.identity;
		}
	}
}
