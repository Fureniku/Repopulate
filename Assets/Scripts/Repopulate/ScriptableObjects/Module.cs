using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Repopulate.ScriptableObjects {
    [CreateAssetMenu]
    public class Module : WorldPlaceableBase<PlaceableModule> {
        
        [SerializeField] private ModuleType _type;
        [SerializeField] private bool _requiresDoor = true;
    }

    public enum ModuleType {
        MAIN,
        SUB
    }
}
