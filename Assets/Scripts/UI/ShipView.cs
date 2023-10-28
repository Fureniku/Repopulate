using System.Collections;
using System.Collections.Generic;
using Repopulate.World;
using UnityEngine;

public class ShipView : MonoBehaviour {
    
    [SerializeField] private Camera _overlayCamera;
    [SerializeField] private SolarSystemManager _solarSystem;
    [SerializeField] private RectTransform _celestialTargetter;

    private RectTransform _rect;
    public Transform ClosestVisibleObject { get; private set; }

    void Awake() {
        _rect = GetComponent<RectTransform>();
    }

    private void Update() {
        int count = _solarSystem.GetCelestialBodies().Count;
        Transform currentClosest = null;
        for (int i = 0; i < count; i++) {
            Transform bodyTransform = _solarSystem.GetCelestialBodies()[i].CelestialController.CelestialBody.transform;
            
            Vector3 screenPoint = _overlayCamera.WorldToScreenPoint(bodyTransform.position);

            if (RectTransformUtility.RectangleContainsScreenPoint(_rect, screenPoint)) {
                currentClosest = (currentClosest == null ||
                                  Vector3.Distance(_rect.position, currentClosest.position) > Vector3.Distance(_rect.position, bodyTransform.position)) ? bodyTransform : currentClosest;
            }
        }

        if (currentClosest != null) {
            PositionTooltip(currentClosest);
        } else {
            _celestialTargetter.gameObject.SetActive(false);
        }
        
        ClosestVisibleObject = currentClosest;
    }

    private Vector3 smoothScreenPosition;
    private const float smoothingFactor = 0.1f;

    void PositionTooltip(Transform currentClosest)
    {
        _celestialTargetter.gameObject.SetActive(true);
        Vector3 targetScreenPosition = _overlayCamera.WorldToScreenPoint(currentClosest.position);
        smoothScreenPosition = Vector3.Lerp(smoothScreenPosition, targetScreenPosition, smoothingFactor);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, smoothScreenPosition, null, out Vector2 localPosition);
        _celestialTargetter.anchoredPosition = localPosition;
    }
}
