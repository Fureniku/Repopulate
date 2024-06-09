using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GravitySourceCube : GravityBase {

	[SerializeField] private BoxCollider box;
	
	[SerializeField] private Vector3 size = new Vector3(5,5,5);
	
	private BoxCollider cldr;
	
	void Awake() {
		cldr = GetComponent<BoxCollider>();
		cldr.isTrigger = true;
	}
	
	private void OnValidate() {
		cldr = GetComponent<BoxCollider>();
		cldr.center = new Vector3(0, 0, 0);
		cldr.size = new Vector3(size.x, size.y, size.z);
	}
	
	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		Vector3 gravityDirection = transform.up; // Use the plane's normal as gravity direction
		return (inverseGravity ? gravityDirection : -gravityDirection).normalized; //Swapped compared to other gravity systems
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		Vector3 closestPointInCollider = cldr.ClosestPoint(pulledObject);
		if (closestPointInCollider == pulledObject) {
			gizmoCol = Color.green;
			return true;
		}
		
		gizmoCol = Color.red;
		return false;
	}

	private void OnDrawGizmos() {
		Gizmos.color = gizmoCol;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		
	    Gizmos.DrawWireCube(cldr.center, cldr.size);
	    
	    Gizmos.matrix = oldMatrix;
	}
}
