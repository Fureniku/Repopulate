using Repopulate.Player;
using UnityEngine;
using CharacterController = Repopulate.Player.CharacterController;

public class GravityAffectedObjectDroid : GravityAffectedObject {
	
	[SerializeField] private DroidController _droidController;
	[SerializeField] private CharacterController characterController;
	
	protected override bool CurrentlyHasExternalForce() {
		return _droidController.MoveDir == Vector3.zero;
	}

	protected override void EnterGravity() {
		characterController.ResetCamera();
		transform.localScale = Vector3.one;
	}
}