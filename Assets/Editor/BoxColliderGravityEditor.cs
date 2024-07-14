using Repopulate.Physics.Gravity;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GravitySourceCube))]
public class BoxColliderGravityEditor : Editor
{
    public override void OnInspectorGUI() {
        GravitySourceCube gravity = (GravitySourceCube) target;
        BoxCollider boxCollider = gravity.GetComponent<BoxCollider>();
        
        EditorGUILayout.LabelField("Box Collider", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(true);

        if (boxCollider != null) {
            boxCollider.hideFlags = HideFlags.HideInInspector;
            EditorGUILayout.Vector3Field("Size", boxCollider.size);
        } else {
            EditorGUILayout.LabelField("Missing!", EditorStyles.boldLabel);
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(20);
        
        DrawDefaultInspector();
    }
}