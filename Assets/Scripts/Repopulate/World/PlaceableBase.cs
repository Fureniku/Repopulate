using Repopulate.ScriptableObjects;
using UnityEngine;

public class PlaceableBase<T> : MonoBehaviour where T : SizedObject {
    
    protected Material lastMat;
    protected BoxCollider _collider;
    [SerializeField] protected bool showDebugSizeBox;
    [SerializeField] protected T _placeable;
    
    public T GetPlaceable() {
        return _placeable;
    }
    
    private void OnDrawGizmos() {
        if (showDebugSizeBox) {
            Matrix4x4 originalMatrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            Vector3Int size = _placeable.GetSize();
            float x = Mathf.Max(size.x / 2.0f, 0.5f);
            float y = Mathf.Max(size.y / 2.0f, 0.5f);
            float z = Mathf.Max(size.z / 2.0f, 0.5f);
            Gizmos.color = new Color(0.5f, 0.5f, 0.0f, 0.3f);

            Gizmos.DrawCube(new Vector3(x, y, z), _placeable.GetSize());

            Gizmos.matrix = originalMatrix;
        }
    }
}
