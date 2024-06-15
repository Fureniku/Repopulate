using System.Collections.Generic;
using UnityEngine;

namespace Repopulate.World.Station {
    public class MainThrusterControl : MonoBehaviour {

        [SerializeField] private ParticleSystem[] particles;
        [SerializeField] private float thrusterStrength = 1f;
        [SerializeField] private float currentThrust;
        [SerializeField] private List<ThrusterBurn> burnList = new();

        private bool burnLast = false;

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
                        break;
                    case BurnState.BURN:
                        SetThrusterStrength(burn.GetBurnStrength());
                        ProcessBurn();
                        burnLast = true;
                        break;
                    case BurnState.POST_BURN:
                        break;
                    case BurnState.RESET:
                        break;
                    case BurnState.FINISH:
                        burnList.Remove(burn);
                        SetThrusterStrength(0);
                        break;
                }
            }
        }

        private void LateUpdate() {
            if (!burnLast) {
                SetThrusterStrength(0);
            }
        }

        public void Burn(float thrust) {
            if (burnList.Count > 0) {
                return;
            }

            SetThrusterStrength(thrust);
            ProcessBurn();

            burnLast = true;
        }

        private void ProcessBurn() {
            if (currentThrust > 0) {
                Vector3 directionStrength = transform.right * (thrusterStrength * currentThrust * -1);

                GameManager.Instance.GetShipController().AddForce(transform.position, directionStrength);
            }
        }


        private void SetThrusterStrength(float thrust) {
            currentThrust = Mathf.Clamp(thrust, 0, 1);
            foreach (ParticleSystem particleSys in particles) {
                if (thrust == 0) {
                    particleSys.Stop();
                }
                else {
                    particleSys.Play();
                    ParticleSystem.MainModule engineMain;
                    engineMain = particleSys.main;
                    engineMain.startLifetime = currentThrust * 5;
                }
            }
        }
    }
}