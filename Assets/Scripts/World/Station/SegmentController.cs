using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //I want the visual feedback from modifying the closed values in-editor too.
public class SegmentController : MonoBehaviour {

    [SerializeField] private GameObject[] blades;
    [SerializeField] private GameObject attachedRing;
    [SerializeField] [Range(0,1)] private float closedAmount;
    [SerializeField] [Range(0,1)] private float apertureSpeed;
    [SerializeField] private float maxClosed;
    [SerializeField] private Vector3 ringMountPosition;

    private float closedTarget;

    private float closedLast = 0;

    void Update() {
        if (!Utilities.CheckFloatsEqual(closedAmount, closedLast)) {
            float open = maxClosed * closedAmount * -1;
            for (int i = 0; i < blades.Length; i++) {
                Quaternion rot = blades[i].transform.localRotation;
                blades[i].transform.localRotation = Quaternion.Euler(rot.x, open, rot.z);
            }
            closedLast = closedAmount;
        }
    }

    private void FixedUpdate() {
        if (!Utilities.CheckFloatsEqual(closedAmount, closedTarget)) {
            if (closedAmount < closedTarget) {
                Debug.Log($"Closing! {closedAmount} to {closedTarget}");
                closedAmount += apertureSpeed;
                if (closedAmount >= closedTarget) {
                    closedTarget = closedAmount; //Stop any jittering from float rounding errors
                }
            }
            else if (closedAmount > closedTarget) {
                Debug.Log($"Opening! {closedAmount} to {closedTarget}");
                closedAmount -= apertureSpeed;
                if (closedAmount <= closedTarget) {
                    closedTarget = closedAmount;
                }
            }
        }
    }

    public void SetClosedAmount(float closed) {
        closed = Mathf.Clamp(closed, 0.0f, 1.0f);
        closedTarget = closed;
    }

    public void AddRing(GameObject ring) {
        Transform t = transform;
        attachedRing = Instantiate(ring, t.position + ringMountPosition, Quaternion.identity, t);
    }
}
