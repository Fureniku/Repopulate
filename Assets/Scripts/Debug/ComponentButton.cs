using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IconCapture))]
public class ComponentButton : Editor
{
    public override void OnInspectorGUI()
    {
        IconCapture yourComponent = (IconCapture)target;

        // Draw default properties
        DrawDefaultInspector();

        // Add a button
        if (GUILayout.Button("Generate Current Item")) {
            yourComponent.GenerateSingleImage();
        }
        
        if (GUILayout.Button("Generate All Items")) {
            yourComponent.GenerateAllImages();
        }
    }
}
