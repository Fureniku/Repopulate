using Repopulate.Rendering;
using UnityEngine;

namespace Repopulate.Physics.Gravity {
	[RequireComponent(typeof(CapsuleCollider))]
	public class GravityLift : MonoBehaviour {

		[SerializeField] private float strength = 10f;
		[SerializeField] private float height = 10f;
		[SerializeField] private float radius = 5f;
    
		[SerializeField] private float centerForce = 10f;
		[SerializeField] private float maxSlowdownRangeFactor = 0.1f; // Controls how close to the center the slowdown range extends

		[SerializeField][Tooltip("Extend the capsule height a little to compensate for the roundness at the base. Will cause effect to appear slightly below lift.")]
		private bool capsuleCompensate = false;

		private CapsuleCollider cldr;

		[SerializeField] private GravLiftVFX vfxObject;
    
		private void OnValidate() {
			cldr = GetComponent<CapsuleCollider>();
			cldr.center = new Vector3(0, height / 2f, 0);
			cldr.height = height + (capsuleCompensate ? 10f : 0f);
			cldr.radius = radius;

			if (vfxObject != null) {
				vfxObject.UpdateParameters();
			}
		}

		public float GetRadius() {
			return radius;
		}

		public float GetHeight() {
			return height;
		}

		void Awake() {
			cldr = GetComponent<CapsuleCollider>();
			cldr.isTrigger = true;
		}

		public void HandleForces(Rigidbody rb) {
			float slowRadius = radius * maxSlowdownRangeFactor;
			Vector3 rbRawPosition = rb.transform.position;

			Vector3 localPosition = transform.InverseTransformPoint(rbRawPosition);
			localPosition.y = 0f; // Only get X/Z distance, so distance to horizontal center

			float distanceToCenter = localPosition.magnitude - slowRadius;

			Vector3 forceVector = Vector3.zero;

			if (distanceToCenter > 0f) {
				float slowdownFactor = Mathf.Clamp01(distanceToCenter / slowRadius);
				if (slowdownFactor > 0f) {
					float centerForceMagnitude = centerForce * slowdownFactor;

					float angleToCenter = Mathf.Atan2(localPosition.z, localPosition.x);
            
					float forceX = -Mathf.Cos(angleToCenter) * centerForceMagnitude;
					float forceZ = -Mathf.Sin(angleToCenter) * centerForceMagnitude;

					forceVector += new Vector3(forceX, 0f, forceZ);
				}
			}
	    
			Vector3 upwardForceVector = transform.up * strength;
			forceVector += upwardForceVector;

			// Apply combined forces
			rb.AddForce(forceVector, ForceMode.Force);
		}

		private void OnDrawGizmos() {
			DrawGravitationalArea(transform.position, Vector3.zero, Vector3.up * height);
		}

		private void DrawGravitationalArea(Vector3 pos, Vector3 localBelowPoint, Vector3 localAbovePoint) {
			Gizmos.color = Color.cyan;
			Vector3 directionAB = localBelowPoint - localAbovePoint;

			float angleIncrement = 2 * Mathf.PI / 24;

			Vector3[] last = null;

			for (int i = 0; i < 25; i++) { // 25; 24 for the loop and one extra to make sure it's closed
				float angle = i * angleIncrement;
				Vector3 minPos = Vector3.zero;
				Vector3 maxPos = new Vector3(0, 0, radius);

				// Apply rotation to the points in local space
				Vector3 point1 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
				Vector3 point2 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
				Gizmos.DrawLine(point1, point2);

				Vector3 pointB1 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
				Vector3 pointB2 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
				Gizmos.DrawLine(pointB1, pointB2);

				Gizmos.DrawLine(point2, pointB2);

				if (last != null) {
					Gizmos.DrawLine(point1, last[0]);
					Gizmos.DrawLine(point2, last[1]);
					Gizmos.DrawLine(pointB1, last[2]);
					Gizmos.DrawLine(pointB2, last[3]);
				}

				last = new[] { point1, point2, pointB1, pointB2 };
			}
		}
	}
}
