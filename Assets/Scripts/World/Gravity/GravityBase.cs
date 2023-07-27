using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GravityBase : MonoBehaviour {
    
    [Tooltip("The strength and direction of the gravity")]
    [SerializeField] protected Vector3 gravity = new Vector3(0, -9.8f, 0);
    [Tooltip("If true, the force of gravity will push away from this point instead of pulling towards it. Used for gravity rings.")]
    [SerializeField] protected bool inverseGravity = false;
    
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
}
