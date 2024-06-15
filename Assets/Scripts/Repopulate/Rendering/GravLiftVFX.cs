using Repopulate.Physics.Gravity;
using UnityEngine;

namespace Repopulate.Rendering {
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
            float radius = lift.GetRadius();
            float height = lift.GetHeight();

            Transform tsfm = transform;
            tsfm.localPosition = new Vector3(0, height / 2, 0);
            tsfm.localScale = new Vector3(radius * 1.2f, height / 2, radius * 1.2f);
        
            heightLight.color = liftCol;
            baseLight.color = liftCol;
            gravLiftLines.color = liftCol;
        
            ParticleSystem.MainModule psMain = particles.main;
            ParticleSystem.ShapeModule psShape = particles.shape;
            ParticleSystem.EmissionModule psEmission = particles.emission;
        
            psMain.startLifetime = lift.GetHeight() / psMain.startSpeed.constant;
            psShape.radius = lift.GetRadius() * 0.625f;
            psEmission.rateOverTime = lift.GetRadius() * 3.75f;

            baseLight.areaSize = new Vector2(lift.GetRadius(), lift.GetRadius());
            float scale = radius * emitterScale;
            emitterBase.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
