using JetBrains.Annotations;
using Repopulate.Physics.Gravity;
using Repopulate.ScriptableObjects;
using Repopulate.World.Constructs;
using UnityEngine;

public class PlaceableModule : PlaceableBase<Module> {
    
    [Space(10)]
    [Header("Module Settings")]
    [SerializeField] [CanBeNull] private GameObject _meshesParent;
    [SerializeField] [CanBeNull] private GameObject _collidersParent;
    [SerializeField] [CanBeNull] private GravitySourceCube _gravityParent;
    [SerializeField] [CanBeNull] private ConstructGrid _gridParent;
    [Space(5)]
    [Tooltip("Whether the required child objects should match their position to the placeable size automatically")]
    [SerializeField] private bool _validatePositions = true;

    private void OnValidate() {
        if (_validatePositions && _placeable != null) {
            Vector3 offsetPos = _placeable.GetSizeMinimums();
            Debug.Log($"Setting positions to {offsetPos}");
            if (_meshesParent != null) _meshesParent.transform.localPosition = offsetPos;
            if (_gravityParent != null) _gravityParent.transform.localPosition = offsetPos;
            if (_collidersParent != null) _collidersParent.transform.localPosition = offsetPos;
            if (_gridParent != null) _gridParent.transform.localPosition = Vector3.zero;
        }
    }
}
