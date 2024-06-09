using UnityEngine;

namespace Repopulate.Utils {
	
	public static class RigidbodyExtensions {
		public static void ClampVelocity(this Rigidbody rigidbody, float maxSpeed) {
			Vector3 velocityIn = rigidbody.velocity;
			float currentSpeed = velocityIn.magnitude;
		
			if (currentSpeed > maxSpeed) {
				Vector3 desiredVelocity = rigidbody.velocity.normalized * maxSpeed;
				rigidbody.velocity = desiredVelocity;
			}
		}
	}
}
