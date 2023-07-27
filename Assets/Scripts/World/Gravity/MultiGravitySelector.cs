using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGravitySelector : MonoBehaviour {

    [SerializeField] private GravityBase[] gravityZones;

    public GravityBase GetClosestGravity(Vector3 pulledObject) {
        if (gravityZones.Length == 1) {
            return gravityZones[0];
        }
        GravityBase closest = gravityZones[0];
        float distance = Vector3.Distance(gravityZones[0].transform.position, pulledObject);
        for (int i = 0; i < gravityZones.Length; i++) {
            float currentDistance = Vector3.Distance(gravityZones[i].transform.position, pulledObject);
            if (currentDistance < distance) {
                closest = gravityZones[i];
                distance = currentDistance;
            }
        }

        return closest;
    }
}
