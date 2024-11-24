using System;
using System.Collections.Generic;
using Repopulate.Player;
using UnityEngine;

namespace Repopulate.ScriptableObjects {
    public class WorldPlaceableBase<T> : SizedObject where T : MonoBehaviour
    {
        [Header("Item Information")]
        [SerializeField] private string _name;
        [SerializeField] private InteractionSet _primaryInteraction;
        [SerializeField] private InteractionSet _secondaryInteraction;
        [SerializeField] private InteractionSet _tertiaryInteraction;
        
        [Header("Prefab Information")]
        [SerializeField] private T _prefab;
        [SerializeField] private Sprite _icon;
        
        public GameObject Get() {
            if (_prefab == null) {
                return new GameObject();
            }
            return _prefab.gameObject;
        }

        public Sprite GetIcon() {
            return _icon;
        }

        public string GetUnlocalizedName => _name;
        public InteractionSet PrimaryInteractions => _primaryInteraction;
        public InteractionSet SecondaryInteractions => _secondaryInteraction;
        public InteractionSet TertiaryInteractions => _tertiaryInteraction;
        
        public bool HasInteractions => !(_primaryInteraction.InteractionType == PlaceableInteractions.None &&
                                         _secondaryInteraction.InteractionType == PlaceableInteractions.None &&
                                         _tertiaryInteraction.InteractionType == PlaceableInteractions.None);
    }
    
    //Split class so we can use circular generics
    public class SizedObject : ScriptableObject
    {
        [Header("Placement Data")]
        [SerializeField] private Vector3Int _size = Vector3Int.one;
        [SerializeField] private Vector3 _mountPoint;
        [SerializeField] private bool _placeable = true;
        [SerializeField] private bool _destroyable = true;

        public Vector3Int Size => _size;
        public Vector3 MountPoint => _mountPoint;

        public int GetX() { return _size.x; }
        public int GetY() { return _size.y; }
        public int GetZ() { return _size.z; }
        
        public bool IsPlaceable => _placeable;
        public bool IsDestroyable => _destroyable;

        public Vector3 GetSizeMinimums() {
            float x = Mathf.Max(_size.x / 2.0f, 0.5f);
            float y = Mathf.Max(_size.y / 2.0f, 0.5f);
            float z = Mathf.Max(_size.z / 2.0f, 0.5f);
            return new Vector3(x, y, z);
        }
    }

    [Serializable]
    public struct InteractionSet {
        [SerializeField] public PlaceableInteractions InteractionType;
        [SerializeField] public string KeyCode;
        [SerializeField] public string PromptText;
    }
    public enum PlaceableInteractions {
        None,
        OpenInventory,
        Configure,
        ToggleState
    }
}
