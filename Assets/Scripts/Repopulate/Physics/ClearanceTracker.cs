using System.Collections;
using System.Collections.Generic;
using Repopulate.World.Constructs;
using UnityEngine;

public class ClearanceTracker : MonoBehaviour {
    
    [SerializeField] private PlaceableConstruct _construct;
    [SerializeField] private GameObject _softClearance;
    [SerializeField] private GameObject _hardClearance;

    public bool HasSoftClearance { get; private set; }
    public bool HasHardClearance { get; private set; }
    
    void Start() {
        HasSoftClearance = _softClearance != null;
        HasHardClearance = _hardClearance != null;
    }
}
