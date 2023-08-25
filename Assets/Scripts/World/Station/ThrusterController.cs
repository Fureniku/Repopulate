using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour {

    [SerializeField] private Vector2 minMaxAngles;
    [SerializeField] private float currentAngle;
    [SerializeField] private float thrusterStrength = 1f;
    [SerializeField] [Range(0, 1)] private float currentThrust = 0f;
    [SerializeField] private ParticleSystem engineFlare;

    private void OnValidate() {
        SetThrusterAngle();
        
    }

    private void Awake() {
    }

    // Update is called once per frame
    void Update() {
        if (currentThrust > 0) {
            GameManager.Instance.GetShipController().AddForce(transform.position, (transform.right*-1) * thrusterStrength);
            SetThrusterAngle();
        }
    }

    private void SetThrusterAngle() {
        if (currentAngle < minMaxAngles.x) {
            currentAngle = minMaxAngles.x;
        }

        if (currentAngle > minMaxAngles.y) {
            currentAngle = minMaxAngles.y;
        }

        Vector3 rot = transform.rotation.eulerAngles;

        transform.rotation = Quaternion.Euler(currentAngle, rot.y, rot.z);
        
        ParticleSystem.MainModule engineMain;
        engineMain = engineFlare.main;
        engineMain.startLifetime = currentThrust * 5;
    }

    private void ResetThruster() {
        currentAngle = (minMaxAngles.x + minMaxAngles.y) / 2f;
    }
}
