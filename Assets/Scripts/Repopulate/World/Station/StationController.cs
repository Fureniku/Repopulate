using System;
using System.Collections;
using System.Collections.Generic;
using Repopulate.Utils;
using UnityEngine;

public class StationController : MonoSingleton<StationController> {
    
    

    [SerializeField] private StationResources stationResources;
    [SerializeField] private SegmentController[] segments;
    [SerializeField] private int maxRings;

    protected override void Awake() {
        base.Awake();
    }
    
}
