using Repopulate.Tools;
using UnityEditor;
using UnityEngine;

namespace Repopulate.Utils {
    [CustomEditor(typeof(RingColliderGenerator))]
    public class ComponentButtonRingCollider : Editor {
    
        public override void OnInspectorGUI() {
            RingColliderGenerator ringCollider = (RingColliderGenerator)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Create Colliders")) {
                ringCollider.CreateColliders();
            }
        
            if (GUILayout.Button("Destroy Colliders")) {
                ringCollider.DestroyColliders();
            }
        }
    }
}
