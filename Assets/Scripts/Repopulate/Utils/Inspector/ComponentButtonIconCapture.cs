using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IconCapture))]
public class ComponentButtonIconCapture : Editor {
    
    public override void OnInspectorGUI() {
        IconCapture iconCapture = (IconCapture)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Current Item")) {
            iconCapture.GenerateSingleImage();
        }
        
        if (GUILayout.Button("Generate All Items")) {
            iconCapture.GenerateAllImages();
        }
    }
}
