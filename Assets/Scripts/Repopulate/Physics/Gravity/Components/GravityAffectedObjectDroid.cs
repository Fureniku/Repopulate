using Repopulate.Player;
using UnityEngine;

namespace Repopulate.Physics.Gravity {
	public class GravityAffectedObjectDroid : GravityAffectedObject {
	
		[SerializeField] private DroidControllerBase _droidController;

		protected override bool CurrentlyHasExternalForce() {
			return _droidController.MoveDir == Vector3.zero;
		}

		protected override void EnterGravity() {
			_droidController.ResetCamera();
			transform.localScale = Vector3.one;
		}
	}
}
