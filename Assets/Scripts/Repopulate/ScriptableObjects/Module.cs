using UnityEngine;

namespace Repopulate.ScriptableObjects {
    [CreateAssetMenu]
    public class Module : WorldPlaceableBase<PlaceableModule> {
        
        [Header("Module Information")]
        [SerializeField] private ModuleType _type;
        [SerializeField][Tooltip("Whether there needs to be a door for this mount")] private bool _requiresDoor = true;

        public ModuleType ModuleType => _type;
        public bool RequiresDoor => _requiresDoor;
    }

    public enum ModuleType {
        MAIN,
        SUB
    }
}
