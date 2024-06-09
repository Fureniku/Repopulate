using System.Collections.Generic;
using UnityEngine;

//TOOL! Make it one properly.
public class IconCapture : MonoBehaviour {

	[SerializeField] private Construct _targetConstruct;
    [SerializeField] private float distance = 1.25f;

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
        List<Construct> constructList = ConstructRegistry.Instance.ConstructList;
        for (int i = 0; i < ConstructRegistry.Instance.ConstructCount; i++) {
            GenerateImage(constructList[i]);
        }
    }

    [ContextMenu("Generate Image")]
    public void GenerateSingleImage() {
        GenerateImage(_targetConstruct);
    }

    private void GenerateImage(Construct construct) {
        int imageSize = 430;
        GameObject go = Instantiate(construct.Get());

        Camera camera = new GameObject("IconCam").AddComponent<Camera>();

        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0); // Transparent background

        // Position the camera to capture only the targetGameObject
        camera.transform.position = new Vector3(-distance - (GetBonusDistance(construct)/2), 2, -distance - (GetBonusDistance(construct)/2));
        camera.transform.LookAt(GetTargetPoint(go, construct));

        RenderTexture renderTexture = new RenderTexture(imageSize, imageSize, 24);
        camera.targetTexture = renderTexture;
        camera.Render();

        Texture2D texture = new Texture2D(imageSize, imageSize, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, imageSize, imageSize), 0, 0);
        texture.Apply();
        RenderTexture.active = null;
        
        Texture2D croppedTexture = CropTexture(texture);

        byte[] bytes = croppedTexture.EncodeToPNG();
        string iconName = construct.GetItemUnlocalizedName;
        System.IO.File.WriteAllBytes(Application.dataPath + $"/Textures/Icons/{iconName}.png", bytes); // Save image as PNG in project directory

        camera.targetTexture = null;
        renderTexture.Release();
        DestroyImmediate(camera.gameObject);
        DestroyImmediate(go);

        Debug.Log("Icon for " + name + " generated and saved to icons folder");
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
}
