using UnityEngine;

namespace Repopulate.ScriptableObjects {
    public class WorldPlaceableBase<T> : SizedObject where T : MonoBehaviour
    {
        [Header("Item Information")]
        [SerializeField] private string _name;

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
    }
    
    //Split class so we can use circular generics
    public class SizedObject : ScriptableObject
    {
        [Header("Placement Data")]
        [SerializeField] private Vector3Int _size = Vector3Int.one;
        [SerializeField] private Vector3 _mountPoint;
        [SerializeField] private bool _placeable = true;
        [SerializeField] private bool _destroyable = true;
        
        public Vector3Int GetSize() {
            return _size;
        }

        public int GetX() { return _size.x; }
        public int GetY() { return _size.y; }
        public int GetZ() { return _size.z; }
        
        public bool IsPlaceable => _placeable;
        public bool IsDestroyable => _destroyable;
    }
}
