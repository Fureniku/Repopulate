using System;
using UnityEngine;

public class RingController : MonoBehaviour {
    
    [SerializeField] private ModuleController[] modules;
    [SerializeField] private int maxModules;
    [SerializeField] private float rotationSpeed;

    private float oxygenPressure;
    
    // Start is called before the first frame update
    void Start() {
        if (modules == null) {
            modules = new ModuleController[maxModules];
        } else {
            maxModules = modules.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        transform.Rotate (rotationSpeed * Time.deltaTime,0,0);
    }

    private void BalanceOxygen() {
        for (int i = 0; i < maxModules; i++) {
            ModuleController mc = modules[i];
            if (mc != null) {
                if (Utilities.CheckFloatsEqual(oxygenPressure, mc.GetProducedOxygen())) {
                    
                }
            }
        }
    }
}
