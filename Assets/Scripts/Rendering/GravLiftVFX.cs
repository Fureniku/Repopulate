using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravLiftVFX : MonoBehaviour {

    [SerializeField] private Color liftCol;

    [SerializeField] private GravityLift lift;
    [SerializeField] private Light heightLight;
    [SerializeField] private Light baseLight;
    [SerializeField] private Material gravLiftLines;
    [SerializeField] private GameObject emitterBase;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float emitterScale = 0.1875f;

    private void OnValidate() {
        UpdateParameters();
    }

    public void UpdateParameters() {
        Vector3 liftPos = lift.transform.position;
        float radius = lift.GetRadius();
        float height = lift.GetHeight();
        
        transform.localPosition = new Vector3(0, height / 2, 0);
        transform.localScale = new Vector3(radius * 1.2f, height / 2, radius * 1.2f);
        
        heightLight.color = liftCol;
        baseLight.color = liftCol;
        gravLiftLines.color = liftCol;
        
        ParticleSystem.MainModule psMain = particles.main;
        ParticleSystem.ShapeModule psShape = particles.shape;
        ParticleSystem.EmissionModule psEmission = particles.emission;
        
        psMain.startLifetime = lift.GetHeight() / psMain.startSpeed.constant;
        psShape.radius = lift.GetRadius() * 0.625f;
        psEmission.rateOverTime = lift.GetRadius() * 7.5f;


        
        //baseLight.transform.localPosition = new Vector3(0, -0.5f, 0);
        baseLight.areaSize = new Vector2(lift.GetRadius(), lift.GetRadius());
        float scale = radius * emitterScale;
        emitterBase.transform.localScale = new Vector3(scale, scale, scale);
    }
}
