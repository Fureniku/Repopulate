using Repopulate.Player;
using UnityEngine;
using CharacterController = Repopulate.Player.CharacterController;

namespace Repopulate.Physics.Gravity {
	public class GravityAffectedObjectDroid : GravityAffectedObject {
	
		[SerializeField] private DroidControllerBase _droidController;
		[SerializeField] private CharacterController characterController;
	
		protected override bool CurrentlyHasExternalForce() {
			return _droidController.MoveDir == Vector3.zero;
		}

		protected override void EnterGravity() {
			characterController.ResetCamera();
			transform.localScale = Vector3.one;
		}
	}
}
