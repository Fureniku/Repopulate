using UnityEditor;
using UnityEngine;

namespace Repopulate.Utils {

    [CustomEditor(typeof(CompoundColliderCreator))]
    public class ComponentButtonCompoundCollider : Editor {

        public override void OnInspectorGUI() {
            CompoundColliderCreator colliderCreator = (CompoundColliderCreator)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Import Colliders")) {
                colliderCreator.CombineColliders();
            }

            if (GUILayout.Button("Delete Colliders")) {
                colliderCreator.DeleteColliders();
            }
        }
    }
}