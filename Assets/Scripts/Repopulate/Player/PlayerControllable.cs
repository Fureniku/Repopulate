using UnityEngine;

namespace Repopulate.Player {
	public abstract class PlayerControllable : MonoBehaviour {
	
		[SerializeField] protected Camera _camera;
	
		private InteractionHandler _interactionHandler;
	
		protected abstract void ControllableAwake();
		protected abstract void ControllableFixedUpdate();

		void Awake() {
			ControllableAwake();
			_interactionHandler = GetComponent<InteractionHandler>();
		}

		void FixedUpdate() {
			ControllableFixedUpdate();
		}

		#region General Controls
		public virtual void Interact() {
			_interactionHandler.Interact(this, _camera);
		}

		public virtual void ToggleMode() {
			
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
