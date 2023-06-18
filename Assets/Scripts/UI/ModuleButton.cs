using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleButton : MonoBehaviour {
    
    [SerializeField] private GameObject moduleType;

    public GameObject GetModuleType() {
        return moduleType;
    }
}
