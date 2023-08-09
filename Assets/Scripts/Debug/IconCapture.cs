using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconCapture : MonoBehaviour {

	[SerializeField] private Item targetItem;
    [SerializeField] private float distance = 1.25f;

    private Vector3 GetTargetPoint(GameObject go, Item item) {
        Vector3 v = item.GetSize();
        return go.transform.position + new Vector3(v.x / 2.0f, v.y / 2.0f, v.z / 2.0f);
    }

    //Get highest dimension to decide how much to back up by
    private float GetBonusDistance(Item item) {
        if (item.GetX() > item.GetY()) {
            return item.GetX() > item.GetZ() ? item.GetX() : item.GetZ();
        }
        return item.GetY() > item.GetZ() ? item.GetY() : item.GetZ();
    }

    public void GenerateAllImages() {
        List<Item> itemList = ItemRegistry.Instance.GetItemList();
        for (int i = 0; i < itemList.Count; i++) {
            GenerateImage(itemList[i]);
        }
    }

    [ContextMenu("Generate Image")]
    public void GenerateSingleImage() {
        GenerateImage(targetItem);
    }

    private void GenerateImage(Item item) {
        int imageSize = 430;
        GameObject go = Instantiate(item.Get());

        Camera camera = new GameObject("IconCam").AddComponent<Camera>();

        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0, 0, 0, 0); // Transparent background

        // Position the camera to capture only the targetGameObject
        camera.transform.position = new Vector3(-distance - (GetBonusDistance(item)/2), 2, -distance - (GetBonusDistance(item)/2));
        camera.transform.LookAt(GetTargetPoint(go, item));

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
        string name = item.GetItemUnlocalizedName();
        System.IO.File.WriteAllBytes(Application.dataPath + $"/Textures/Icons/{name}.png", bytes); // Save image as PNG in project directory

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
