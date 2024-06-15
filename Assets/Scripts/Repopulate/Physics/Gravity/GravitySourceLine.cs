using UnityEngine;

namespace Repopulate.Physics.Gravity {
	[RequireComponent(typeof(BoxCollider))]
	public class GravitySourceLine : GravityBase {
    
		[Tooltip("The first point. Gravity's centre is a line between Gravity Source B and this.")]
		[SerializeField] private Vector3 gravitySourceA = new Vector3(0, 0, 0);
		[Tooltip("The second point. Gravity's centre is a line between Gravity Source A and this.")]
		[SerializeField] private Vector3 gravitySourceB;
		[Tooltip("If the object is closer than this, gravity has no effect")]
		[SerializeField] private float minEffectDistance;
		[Tooltip("If the object is further than this, gravity has no effect")]
		[SerializeField] private float maxEffectDistance;
    
		private BoxCollider cldr;
    
		void Awake() {
			gravityDistance = Vector3.Distance(gravitySourceA, gravitySourceB);
			cldr = GetComponent<BoxCollider>();
			cldr.isTrigger = true;
		}

		//Adjust gravitational positions to move with the gravitational object
		private Vector3 GravitySourceA() {
			return gravitySourceA + transform.position;
		}
	
		private Vector3 GravitySourceB() {
			return gravitySourceB + transform.position;
		}
    
		//The direction of pull, without strength. Overriden in plane
		public override Vector3 GetPullDirection(Vector3 pulledObject) {
			return Vector3.Normalize(GetGravitationalLineOrigin(GetScalarProjection(pulledObject)) - pulledObject) * (_inverseGravity ? -1 : 1);
		}

		public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
			float scalarProjection = GetScalarProjection(pulledObject);

			//Limit the gravitational position to within the source line
			if (scalarProjection <= 0 || scalarProjection >= gravityDistance) {
				gizmoCol = Color.red;
				return false;
			}

			float distanceToGravityCentre = Vector3.Distance(pulledObject, GetGravitationalLineOrigin(scalarProjection));
			bool inRange = distanceToGravityCentre >= minEffectDistance && distanceToGravityCentre <= maxEffectDistance;

			gizmoCol = inRange ? Color.green : Color.red;
		
			return inRange;
		}
    
		private float GetScalarProjection(Vector3 pulledObject) {
			Vector3 d = (GravitySourceB() - GravitySourceA()).normalized;
			Vector3 v = pulledObject - GravitySourceA();
			return Vector3.Dot(v, d);
		}
    
		private Vector3 GetGravitationalLineOrigin(float scalarProjection) {
			//Limit the gravitational position to within the source line
			if (scalarProjection <= 0) { return GravitySourceA(); }
			if (scalarProjection >= gravityDistance) { return GravitySourceB(); }

			closestPoint = GravitySourceA() + scalarProjection * (GravitySourceB() - GravitySourceA()).normalized;

			return closestPoint;
		}
    
		private void OnDrawGizmos() {
			Gizmos.color = gizmoCol;
			Gizmos.DrawLine(GravitySourceA(), GravitySourceB());
			Gizmos.DrawSphere(closestPoint, 0.25f);
		
			DrawGravitationalArea();
		}

		private void DrawGravitationalArea() {
			Vector3 directionAB = (GravitySourceB() - GravitySourceA()).normalized;
			Vector3 perpendicular = Vector3.Cross(directionAB, Vector3.up).normalized;

			float angleIncrement = 2 * Mathf.PI / 24;

			Vector3[] last = null;
	    
			for (int i = 0; i < 25; i++) { //25; 24 for the loop and one extra to make sure its closed
				float angle = i * angleIncrement;
				Vector3 minPos = minEffectDistance * perpendicular;
				Vector3 maxPos = maxEffectDistance * perpendicular;
				Vector3 point1 = GravitySourceA() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos;
				Vector3 point2 = GravitySourceA() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos;
				DrawGizmoArrow(point1, point2);
			
				Vector3 pointB1 = GravitySourceB() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos;
				Vector3 pointB2 = GravitySourceB() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos;
				DrawGizmoArrow(pointB1, pointB2);
			
				Gizmos.DrawLine(point1, pointB1);
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
