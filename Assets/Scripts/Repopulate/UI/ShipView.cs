using UnityEngine;

namespace Repopulate.UI {
    public class ShipView : MonoBehaviour {
    
        [SerializeField] private Camera _overlayCamera;
        [SerializeField] private SolarSystemManager _solarSystem;
        [SerializeField] private RectTransform _celestialTargetter;

        private RectTransform _rect;
        public Transform ClosestVisibleObject { get; private set; }
    
        Plane[] planes;

        void Awake() {
            _rect = GetComponent<RectTransform>();
        }

        private void Update() {
            int count = _solarSystem.GetCelestialBodies().Count;
            Transform currentClosest = null;
            Vector3 rectPos = _rect.position;
            
            for (int i = 0; i < count; i++) {
                Transform bodyTransform = _solarSystem.GetCelestialBodies()[i].CelestialController.CelestialBody.transform;

                if (currentClosest == null) {
                    currentClosest = bodyTransform;
                    continue;
                }
                
                Vector3 screenPoint = _overlayCamera.WorldToScreenPoint(bodyTransform.position);

                if (RectTransformUtility.RectangleContainsScreenPoint(_rect, screenPoint)) {
                    currentClosest = Vector3.Distance(rectPos, currentClosest.position) > Vector3.Distance(rectPos, bodyTransform.position) ? bodyTransform : currentClosest;
                }
            }
        
            /*planes = GeometryUtility.CalculateFrustumPlanes(_overlayCamera);
            if (currentClosest != null && GeometryUtility.TestPlanesAABB(planes, currentClosest.gameObject.GetComponent<Collider>().bounds)) {
                Debug.Log(currentClosest.gameObject.name + " has been detected!");
            } else {
                Debug.Log("Nothing has been detected");
            }*/

            if (currentClosest != null) {
                PositionTooltip(currentClosest);
            } else {
                _celestialTargetter.gameObject.SetActive(false);
            }
        
            ClosestVisibleObject = currentClosest;
        }

        void PositionTooltip(Transform target) {
            _celestialTargetter.gameObject.SetActive(true);
            Vector3 targetScreenPosition = _overlayCamera.WorldToScreenPoint(target.position);
            _celestialTargetter.position = new Vector3(targetScreenPosition.x, targetScreenPosition.y, 0);
        }
    }
}
