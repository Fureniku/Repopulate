using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RingCollider))]
public class ComponentButtonRingCollider : Editor {
    
    public override void OnInspectorGUI() {
        RingCollider ringCollider = (RingCollider)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Create Colliders")) {
            ringCollider.CreateColliders();
        }
        
        if (GUILayout.Button("Destroy Colliders")) {
            ringCollider.DestroyColliders();
        }
    }
}
