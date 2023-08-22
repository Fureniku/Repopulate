using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravLiftVFX : MonoBehaviour {

    [SerializeField] private Color liftCol;

    [SerializeField] private GravityLift lift;
    [SerializeField] private Light heightLight;
    [SerializeField] private Light baseLight;
    [SerializeField] private Material volLightMain;
    [SerializeField] private Material volLightTop;
    [SerializeField] private Material gravLiftLines;
    [SerializeField] private GameObject emitterBase;
    [SerializeField] private float emitterScale = 0.1875f;

    private void OnValidate() {
        UpdateParameters();
    }

    public void UpdateParameters() {
        Vector3 liftPos = lift.transform.position;
        float radius = lift.GetRadius();
        float height = lift.GetHeight();
        
        transform.position = new Vector3(liftPos.x, liftPos.y + height / 2, liftPos.z);
        transform.localScale = new Vector3(radius * 1.2f, height / 2, radius * 1.2f);
        
        heightLight.color = liftCol;
        baseLight.color = liftCol;
        volLightMain.color = liftCol;
        volLightTop.color = liftCol;
        gravLiftLines.color = liftCol;

        float scale = radius * emitterScale;
        emitterBase.transform.localScale = new Vector3(scale, scale, scale);
    }
}
