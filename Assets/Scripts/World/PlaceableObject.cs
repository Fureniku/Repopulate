using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour {
    private Material lastMat;

    public void UpdateMaterials(Material mat) {
        if (mat == lastMat) {
            return;
        }
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = mat;
        }

        lastMat = mat;
    }
}
