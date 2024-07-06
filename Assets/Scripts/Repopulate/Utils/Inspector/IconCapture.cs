using System.Collections.Generic;
using Repopulate.ScriptableObjects;
using Repopulate.Utils.Registries;
using Repopulate.World.Constructs;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

//TOOL! Make it one properly.
namespace Repopulate.Utils {
    public class IconCapture : MonoBehaviour {

        [SerializeField] private Construct _targetConstruct;
        [SerializeField] private float distance = 1.25f;
        [SerializeField] private float verticalOffset = 0f;
        [SerializeField] private float objectRotation = 0f;

        private static string ICON_PATH = "/Textures/Icons/";
        private static string PRESET_PATH = "Assets" + ICON_PATH + "IconGenerator.preset";
        
        private Camera cam = null;
        private GameObject go = null;

        private Vector3 GetTargetPoint(GameObject go, Construct construct) {
            Vector3 v = _targetConstruct.GetSize();
            return go.transform.position + new Vector3(v.x / 2.0f, v.y / 2.0f, v.z / 2.0f);
        }

        //Get highest dimension to decide how much to back up by
        private float GetBonusDistance(Construct construct) {
            if (construct.GetX() > construct.GetY()) {
                return construct.GetX() > construct.GetZ() ? construct.GetX() : construct.GetZ();
            }

            return construct.GetY() > construct.GetZ() ? construct.GetY() : construct.GetZ();
        }

        public void GenerateAllImages() {
            List<Construct> constructList = ConstructRegistry.Instance.List;
            for (int i = 0; i < ConstructRegistry.Instance.Count; i++) {
                GenerateImageFull(constructList[i]);
            }
        }

        [ContextMenu("Generate Image")]
        public void GenerateSingleImage() {
            GenerateImageFull(_targetConstruct);
        }

        [ContextMenu("Debug Camera")]
        public void DebugCamera() {
            int imageSize = 430;
            CreateObject(_targetConstruct);
            go.name = "DEBUG_Construct";
            
            CreateCamera();

            cam.transform.position = new Vector3(-distance - (GetBonusDistance(_targetConstruct) / 2), 2 + verticalOffset, -distance - (GetBonusDistance(_targetConstruct) / 2));
            cam.transform.LookAt(GetTargetPoint(go, _targetConstruct));

            RenderTexture renderTexture = new RenderTexture(imageSize, imageSize, 24, RenderTextureFormat.ARGB32);
            renderTexture.useMipMap = false;
            renderTexture.autoGenerateMips = false;
            cam.targetTexture = renderTexture;
            cam.Render();
        }

        [ContextMenu("Debug Take Image")]
        public void DebugImage() {
            GenerateImage(_targetConstruct);
        }

        [ContextMenu("Destroy Debug Objects")]
        public void ClearObjects() {
            if (go != null) {
                DestroyImmediate(go);
            }
            if (cam != null) {
                DestroyImmediate(cam.gameObject);
            }
            DestroyImmediate(FindObjectOfType<PlaceableConstruct>().GameObject());
        }

        private void CreateCamera() {
            if (cam != null) {
                DestroyImmediate(cam.gameObject);
            }
            cam = new GameObject("IconCam").AddComponent<Camera>();
            
            HDAdditionalCameraData additionalCameraData = cam.GetComponent<HDAdditionalCameraData>();
            if (additionalCameraData == null) {
                additionalCameraData = cam.gameObject.AddComponent<HDAdditionalCameraData>();
            }

            additionalCameraData.backgroundColorHDR = new Color(0, 0, 0, 0);
            additionalCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;

            cam.clearFlags = CameraClearFlags.Nothing;
            cam.backgroundColor = new Color(0, 0, 0, 0); // Transparent background
        }

        private void CreateObject(Construct construct) {
            go = Instantiate(construct.Get());
            go.transform.rotation = Quaternion.Euler(0, objectRotation, 0);
        }

        private void GenerateImageFull(Construct construct) {
            CreateCamera();
            CreateObject(construct);
            GenerateImage(construct);
            ClearObjects();
        }

        private void GenerateImage(Construct construct) {
            int imageSize = 430;
            cam.transform.position = new Vector3(-distance - (GetBonusDistance(construct) / 2), 2 + verticalOffset, -distance - (GetBonusDistance(construct) / 2));
            cam.transform.LookAt(GetTargetPoint(go, construct));
            RenderTexture renderTexture = new RenderTexture(imageSize, imageSize, 24, RenderTextureFormat.ARGB32);
            renderTexture.useMipMap = false;
            renderTexture.autoGenerateMips = false;
            cam.targetTexture = renderTexture;
            cam.Render();
            
            Texture2D texture = new Texture2D(imageSize, imageSize, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, imageSize, imageSize), 0, 0);
            texture.Apply();
            RenderTexture.active = null;

            Texture2D croppedTexture = CropTexture(texture);

            byte[] bytes = croppedTexture.EncodeToPNG();
            string iconName = construct.GetUnlocalizedName;
            if (iconName.Length == 0) {
                Debug.LogError($"Unlocalized name for {_targetConstruct.name} is not set! Unable to generate image");
                return;
            }
            string path = Application.dataPath + $"{ICON_PATH}{iconName}.png";
            string localPath = $"Assets{ICON_PATH}{iconName}.png";
            System.IO.File.WriteAllBytes(path, bytes); // Save image as PNG in project directory

            cam.targetTexture = null;
            renderTexture.Release();
            
            ApplyPreset(localPath);

            Debug.Log($"Icon for {name} generated and saved to icons folder at {path}");
            AssetDatabase.Refresh();
        }
        
        private Texture2D CropTexture(Texture2D sourceTexture) {
            // Calculate the crop rectangle based on your requirements
            int x = 87; // Left cropping offset
            int y = 87; // Top cropping offset
            int width = sourceTexture.width - x * 2; // Width after cropping
            int height = sourceTexture.height - y * 2; // Height after cropping

            Color[] pixels = sourceTexture.GetPixels(x, y, width, height);
            Texture2D croppedTexture = new Texture2D(width, height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }
        
        private void ApplyPreset(string assetPath)
        {
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            // Load the preset
            Preset preset = AssetDatabase.LoadAssetAtPath<Preset>(PRESET_PATH);
            if (preset == null)
            {
                Debug.LogError("Preset not found at path: " + PRESET_PATH);
                return;
            }

            // Load the texture importer for the generated asset
            TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (textureImporter == null)
            {
                Debug.LogError("TextureImporter not found for asset: " + assetPath);
                return;
            }

            // Apply the preset to the texture importer
            if (preset.ApplyTo(textureImporter))
            {
                Debug.Log("Preset applied successfully to: " + assetPath);
            }
            else
            {
                Debug.LogError($"Failed to apply preset to: {assetPath}. You will need to fix errors or manually apply the preset.");
            }

            // Re-import the asset to apply the changes
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }
    }
}