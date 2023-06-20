using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour {
    
    [SerializeField] private ModuleController[] modules;
    [SerializeField] private int maxModules;

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
