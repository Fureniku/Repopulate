using System;
using UnityEditor;
using UnityEngine;

namespace Repopulate.Physics {
    public class CircleFloorCollider : MonoBehaviour {
    
        [SerializeField] private CircleFloorHole[] holes;
        [SerializeField] private float holeHeight = 20f;

        public void RefreshHoles() {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders) {
                if (Application.isEditor) {
                    DestroyImmediate(col);
                } else {
                    Destroy(col);
                }
            }

            foreach (CircleFloorHole hole in holes) {
                CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();

                capsule.center = hole.GetPosition();
                capsule.radius = hole.GetRadius();
                capsule.height = holeHeight;
                capsule.isTrigger = true;
            }
        }
    }


    [Serializable]
    public class CircleFloorHole {
        [SerializeField] private Vector3 position;
        [SerializeField] private float radius;

        public Vector3 GetPosition() => position;
        public float GetRadius() => radius;
    }

    [CustomEditor(typeof(CircleFloorCollider))]
    public class ObjectBuilderEditor : Editor {
    
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            CircleFloorCollider cfc = (CircleFloorCollider)target;

            if (GUILayout.Button("Refresh Collider")) {
                cfc.RefreshHoles();
            }
        }
    }
}