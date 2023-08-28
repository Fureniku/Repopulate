using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ThrusterController : MonoBehaviour {

    private const float minAngle = -65f;
    private const float maxAngle = 65f;
    
    [SerializeField] private GameObject rotationPoint;
    [SerializeField] [Range(minAngle, maxAngle)] private float currentAngle;
    [SerializeField] private float thrusterStrength = 1f;
    [SerializeField] [Range(0, 1)] private float currentThrust = 0f;
    [SerializeField] private ParticleSystem engineFlare;
    [SerializeField] private ThrusterController opposite;

    [SerializeField] private List<ThrusterBurn> burnList = new();

    private bool burnLast = false;

    private void OnValidate() {
        rotationPoint.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
    }

    void Awake() {
        SetThrusterStrength(0);
    }

    public void AddBurn(float angle, float time, float strength) {
        burnList.Add(new ThrusterBurn(angle, time, strength));
    }

    public void AddWait(float time) {
        burnList.Add(new ThrusterBurn(time));
    }

    void FixedUpdate() {
        burnLast = false;
        if (burnList.Count > 0) {
            ThrusterBurn burn = burnList[0];

            switch (burn.GetBurnState()) {
                case BurnState.PRE_BURN:
                    SetThrusterAngle(burn.GetThrusterAngle(), true, burn.PreBurnTimeStep());
                    break;
                case BurnState.BURN:
                    SetThrusterStrength(burn.GetBurnStrength());
                    ProcessBurn();
                    burnLast = true;
                    break;
                case BurnState.POST_BURN: //Do nothing in post burn, just let particles fizzle out
                    break;
                case BurnState.RESET:
                    SetThrusterAngle(0, true, burn.ResetTimeStep());
                    break;
                case BurnState.FINISH:
                    burnList.Remove(burn);
                    SetThrusterStrength(0);
                    SetThrusterAngle(0, false);
                    break;
            }
        }
    }

    private void LateUpdate() {
        if (!burnLast) {
            SetThrusterStrength(0);
        }
    }

    public void Burn(float angle, float thrust) {
        if (burnList.Count > 0) {
            return;
        }
        SetThrusterAngle(angle, false);
        SetThrusterStrength(thrust);
        ProcessBurn();
        burnLast = true;
    }

    private void ProcessBurn() {
        if (currentThrust > 0) {
            Quaternion rotationQuaternion = rotationPoint.transform.localRotation;
            Vector3 directionStrength = transform.forward * (thrusterStrength * currentThrust);
            
            Vector3 rotatedDirection = rotationQuaternion * directionStrength.normalized;

            Vector3 rotatedStrength = rotatedDirection * directionStrength.magnitude;

            directionStrength = rotatedStrength;

            GameManager.Instance.GetShipController().AddForce(transform.position, directionStrength);
        }
    }
    
    private void SetThrusterAngle(float rotationValue, bool slerp, float t = 0f) {
        currentAngle = Mathf.Lerp(minAngle, maxAngle, (rotationValue + 1f) / 2f);
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        if (slerp) {
            Quaternion start = rotationPoint.transform.localRotation;
            Quaternion target = Quaternion.Euler(currentAngle, 0, 0);

            rotationPoint.transform.localRotation = Quaternion.Slerp(start, target, t);
            return;
        }

        rotationPoint.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
    }

    private void SetThrusterStrength(float thrust) {
        currentThrust = Mathf.Clamp(thrust, 0, 1);
        ParticleSystem.MainModule engineMain;
        engineMain = engineFlare.main;
        engineMain.startLifetime = currentThrust * 5;
    }

    private void ResetThruster() {
        currentAngle = 0f;
    }

    public ThrusterController Opposite() => opposite;
}
