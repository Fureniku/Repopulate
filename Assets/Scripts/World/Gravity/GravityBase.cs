using UnityEngine;

public abstract class GravityBase : MonoBehaviour {
    
    [Tooltip("The strength and direction of the gravity")]
    [SerializeField] protected Vector3 gravity = new Vector3(0, -9.8f, 0);
    [Tooltip("If true, the force of gravity will push away from this point instead of pulling towards it. Used for gravity rings.")]
    [SerializeField] protected bool inverseGravity = false;
    [Tooltip("Priority for gravity fields. Only one gravity area can be active, with higher numbers chosen first")]
    [SerializeField] private int priority;

    protected float gravityDistance;
    
    //Debug:
    protected Vector3 closestPoint; //The last returned closest point, to update the gizmo position
    protected Color gizmoCol = Color.magenta; //Variable to change gizmo colour when player is within gravitational range

    public abstract Vector3 GetPullDirection(Vector3 pulledObject);
    public abstract bool IsWithinGravitationalEffect(Vector3 pulledObject);

    //The direction and strength of the gravitational pull
    public Vector3 GetPull(Vector3 pulledObject) {
        return GetPullDirection(pulledObject) * gravity.magnitude;
    }
    
    private void OnTriggerStay(Collider other) {
        DroidController droid = other.GetComponent<DroidController>();
        if (droid != null) {
            if (droid.CurrentGravitySource() == this) {
                gizmoCol = Color.white;
                if (IsWithinGravitationalEffect(droid.transform.position)) {
                    gizmoCol = Color.green;
                    if (droid.transform.parent != transform) {
                        droid.transform.parent = transform;
                        droid.transform.localScale = Vector3.one;
                    }
                }
            } else {
                gizmoCol = Color.red;
            }
        }
    }

    protected void DrawGizmoArrow(Vector3 from, Vector3 to) {
        Gizmos.DrawLine(from, to);
        Vector3 direction = to - from;
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 30, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 30, 0) * Vector3.forward;
        Gizmos.DrawLine(to, to - (direction + right) * 0.1f);
        Gizmos.DrawLine(to, to - (direction + left) * 0.1f);
    }

    public int GetPriority() {
        return priority;
    }
}
