using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingCollider : MonoBehaviour {

    [Tooltip("The number of segments to create. Must be divisible by 4!")]
    [SerializeField] private int _segmentCount = 96;
    [SerializeField] private float _radius = 10;
    [SerializeField] private float _depth = 20;
    [SerializeField] private bool _showGizmo = true;

    public void CreateColliders() {
        DestroyColliders();
        if (_segmentCount % 4 == 0) {
            int objectCount = _segmentCount / 4;
            float width = (float) ((_radius / _segmentCount) * (Math.PI * 2));
            Debug.Log($"Creating {objectCount} objects with width {width}");
            for (int i = 0; i < objectCount; i++) {
                GameObject obj = new("ColliderSet_" + i) {
                    transform = {
                        rotation = transform.rotation * Quaternion.Euler(0, i * (90.0f / objectCount), 0),
                        position = transform.position,
                        parent = transform
                    }
                };

                obj.layer = gameObject.layer;

                BoxCollider boxUp = obj.AddComponent<BoxCollider>();
                BoxCollider boxRight = obj.AddComponent<BoxCollider>();
                BoxCollider boxDown = obj.AddComponent<BoxCollider>();
                BoxCollider boxLeft = obj.AddComponent<BoxCollider>();

                boxUp.center = new Vector3(0, 0, _radius);
                boxRight.center = new Vector3(_radius, 0, 0);
                boxDown.center = new Vector3(0, 0, -_radius);
                boxLeft.center = new Vector3(-_radius, 0, 0);

                boxUp.size = new Vector3(width, _depth, 0);
                boxRight.size = new Vector3(0, _depth, width);
                boxDown.size = new Vector3(width, _depth, 0);
                boxLeft.size = new Vector3(0, _depth, width);
            }
        }
        else {
            Debug.LogError($"Could not create ring colliders! {_segmentCount} is not divisible by 4.");
        }
    }

    public void DestroyColliders() {
        int objectCount = transform.childCount;
        List<Transform> children = transform.Cast<Transform>().ToList();

        foreach (Transform child in children)
        {
	        DestroyImmediate(child.gameObject);
        }

        children.Clear();
        Debug.Log($"Destroyed {objectCount} objects");
    }
    
    
    private void OnDrawGizmos() {
	    if (!_showGizmo) {
		    return;
	    }
	    Vector3 pos = transform.position;

	    // Calculate positions in local coordinates
	    Vector3 localBelowPoint = Vector3.up * -(_depth/2);
	    Vector3 localAbovePoint = Vector3.up * (_depth/2);
		Gizmos.color = Color.green;
	    Vector3 directionAB = localBelowPoint - localAbovePoint;

	    float angleIncrement = 2 * Mathf.PI / _segmentCount;

	    Vector3[] last = null;

	    for (int i = 0; i < _segmentCount + 1; i++) {
	        float angle = i * angleIncrement;
	        Vector3 minPos = Vector3.zero;
	        Vector3 maxPos = new Vector3(0, 0, _radius);

	        // Apply rotation to the points in local space
	        Vector3 point1 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
	        Vector3 point2 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
	        //Gizmos.DrawLine(point1, point2);

	        Vector3 pointB1 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
	        Vector3 pointB2 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
	        //Gizmos.DrawLine(pointB1, pointB2);

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
