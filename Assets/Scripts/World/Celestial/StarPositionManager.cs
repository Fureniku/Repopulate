using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPositionManager : MonoBehaviour {

    void Update() {
        Vector3 directionToOrigin = Vector3.Normalize(Vector3.zero - transform.position);
        Quaternion rotation = Quaternion.LookRotation(directionToOrigin);

        transform.rotation = rotation;
    }
}
