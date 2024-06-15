using System.Collections.Generic;
using Repopulate.Utils;
using Repopulate.World.Station;
using UnityEngine;

namespace Repopulate.Physics.Gravity {
    public class GravityAffectedObject : MonoBehaviour {
        [SerializeField] private GravityBase _gravitySource;
        [SerializeField] private Rigidbody _rb;
    
        [Tooltip("The speed at which the object rotates to correct orientation when entering gravity")]
        [Range(1f, 10f)]
        [SerializeField] private float _gravitationalCorrectionSpeed = 5f;
        [Tooltip("The gravitational terminal velocity of the object in standard earth-level gravity")]
        [SerializeField] private float _terminalVelocity = 5f;
    
        private List<GravityBase> _currentGravities = new();

        public GravityBase GravitySource => _gravitySource;
        public bool IsInGravity { get; private set; }
        public bool IsInElevator { get; private set; } = false;
    
        private Vector3 lastPosition; //The last known position when in gravity, used for transitioning velocity to out-of-gravity
    
        void Awake()
        {
            IsInGravity = _gravitySource != null;
        }

        public void UpdateGravity() {
            Vector3 pos = transform.position;
            if (_currentGravities.Count > 0) {
                if (!IsInGravity) {
                    EnterGravity();
                }

                GravityBase priorityGravity = _currentGravities[0];
		
                foreach (GravityBase grav in _currentGravities) {
                    if (grav.GetPriority() > priorityGravity.GetPriority()) {
                        if (grav.IsWithinGravitationalEffect(pos)) {
                            priorityGravity = grav;
                        }
                    }
                }

                _gravitySource = priorityGravity;
                IsInGravity = true;
            } else {
                if (IsInGravity) {
                    _gravitySource = null;
                    transform.parent = StationController.Instance.transform;
                    _rb.ClampVelocity(_terminalVelocity);
                }
                IsInGravity = false;
            }

            if (_gravitySource != null && _gravitySource.IsWithinGravitationalEffect(pos)) {
                Vector3 direction = _gravitySource.GetPullDirection(pos);
                Vector3 gravDirection = _gravitySource.GetPull(pos);
	
                _rb.AddForce(gravDirection);

                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
                Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _gravitationalCorrectionSpeed);

                transform.rotation = slerp;
            }
        }
    
        private void LateUpdate() {
            if (IsInGravity) {
                lastPosition = transform.position;
            }
        }
    
        private void OnTriggerEnter(Collider other) {
            GravityBase gravity = other.GetComponent<GravityBase>();

            if (gravity != null) {
                IsInGravity = true;
                _currentGravities.Add(gravity);
            }
        }

        private void OnTriggerExit(Collider other) {
            GravityBase gravity = other.GetComponent<GravityBase>();

            if (gravity != null) {
                _currentGravities.Remove(gravity);
            }
        }

        private void OnTriggerStay(Collider other) {
            GravityLift gravLift = other.GetComponent<GravityLift>();

            if (gravLift != null) {
                IsInElevator = true;
                if (CurrentlyHasExternalForce()) {
                    gravLift.HandleForces(_rb);
                }
            } else {
                IsInElevator = false;
            }
        }

        protected virtual bool CurrentlyHasExternalForce() {
            return false;
        }

        protected virtual void EnterGravity() {
            transform.localScale = Vector3.one;
        }
    }
}
