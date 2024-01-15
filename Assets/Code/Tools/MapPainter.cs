using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapPainter: MonoBehaviour
{
    [SerializeField]
    private GameObject _mapGOToDraw;

    [SerializeField]
    private Texture2D _texture;

    [SerializeField]
    private string FolderSavePath;

    [SerializeField]
    private Texture2D createdTextureAsset;

    [SerializeField]
    private string relativePath;

    public Texture2D GetCreatedAsset()
    {
        return createdTextureAsset;
    }

    //public void Draw()
    //{
    //    List<Vector2Int> coordinates = CreateCoordinates();

    //    if (coordinates.Count == 0)
    //    {
    //        Debug.LogError("No coordinates entered!");
    //        return;
    //    }

    //    // Find the minimum and maximum coordinates
    //    Vector2Int minCoords = coordinates[0];
    //    Vector2Int maxCoords = coordinates[0];

    //    foreach (Vector2Int coord in coordinates)
    //    {
    //        minCoords.x = Mathf.Min(minCoords.x, coord.x);
    //        minCoords.y = Mathf.Min(minCoords.y, coord.y);
    //        maxCoords.x = Mathf.Max(maxCoords.x, coord.x);
    //        maxCoords.y = Mathf.Max(maxCoords.y, coord.y);
    //    }

    //    // Calculate texture size
    //    int width = Mathf.Abs(maxCoords.x - minCoords.x) + 1;
    //    int height = Mathf.Abs(maxCoords.y - minCoords.y) + 1;

    //    _texture = new Texture2D(width, height);
    //    Color32[] pixels = new Color32[width * height];

    //    // Set all pixels to transparent
    //    for (int i = 0; i < pixels.Length; i++)
    //    {
    //        pixels[i] = new Color32(0, 0, 0, 0);
    //    }

    //    // Set pixels at coordinates to a non-transparent color
    //    foreach (Vector2Int coord in coordinates)
    //    {
    //        int x = coord.x - minCoords.x;
    //        int y = coord.y - minCoords.y;
    //        int index = x + y * width;
    //        pixels[index] = Color.white;
    //    }

    //    // Apply the pixel array to the texture
    //    _texture.SetPixels32(pixels);
    //    _texture.Apply();


    //}

    public void Draw()
    {
        List<Vector2Int> coordinates = CreateCoordinates();

        if (coordinates.Count == 0)
        {
            Debug.LogError("No coordinates entered!");
            return;
        }

        // Find the minimum and maximum coordinates
        Vector2Int minCoords = coordinates[0];
        Vector2Int maxCoords = coordinates[0];

        foreach (Vector2Int coord in coordinates)
        {
            minCoords.x = Mathf.Min(minCoords.x, coord.x);
            minCoords.y = Mathf.Min(minCoords.y, coord.y);
            maxCoords.x = Mathf.Max(maxCoords.x, coord.x);
            maxCoords.y = Mathf.Max(maxCoords.y, coord.y);
        }

        // Calculate texture size
        int width = Mathf.Abs(maxCoords.x - minCoords.x) + 1;
        int height = Mathf.Abs(maxCoords.y - minCoords.y) + 1;

        Color32[] pixels = new Color32[width * height];

        // Set all pixels to transparent
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }

        // Set pixels at coordinates to a non-transparent color
        foreach (Vector2Int coord in coordinates)
        {
            int x = coord.x - minCoords.x;
            int y = coord.y - minCoords.y;
            int index = x + y * width;
            pixels[index] = Color.white;
        }

        ResizeTexture(ref pixels, width, height, 4);
        // Apply the pixel array to the texture

        _texture = new Texture2D(width*4, height*4);
        _texture.SetPixels32(pixels);
        _texture.Apply();
    }

    void ResizeTexture(ref Color32[] inputTexture, int inputWidth, int inputHeight, int scale)
    {
        int outputWidth = inputWidth * scale;
        int outputHeight = inputHeight * scale;

        Color32[] resizedTexture = new Color32[outputWidth * outputHeight];

        for (int y = 0; y < outputHeight; y++)
        {
            for (int x = 0; x < outputWidth; x++)
            {
                int originalX = x / scale;
                int originalY = y / scale;

                int index = x + y * outputWidth;
                int originalIndex = originalX + originalY * inputWidth;

                resizedTexture[index] = inputTexture[originalIndex];
            }
        }

        inputTexture = resizedTexture;
    }

    private List<Vector2Int> CreateCoordinates()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Transform t in _mapGOToDraw.transform)
        {
            if (t != _mapGOToDraw.transform)
            {
                int x = Mathf.RoundToInt(t.localPosition.x);
                int y = Mathf.RoundToInt(t.localPosition.y);
                result.Add(new Vector2Int(x, y));
            }
        }
        return result;
    }

    public void SelectPath()
    {
        FolderSavePath = EditorUtility.OpenFolderPanel("Select save path", "", "");
    }

    public void Save()
    {
        if (FolderSavePath.Length > 0 && _texture != null)
        {
            // Save the texture to a file
            byte[] bytes = _texture.EncodeToPNG();

            long timestamp = Helpers.GetTimestamp();

            string filePath = FolderSavePath + "/" + timestamp.ToString() + ".png";

            if (filePath.Length != 0)
            {
                System.IO.File.WriteAllBytes(filePath, bytes);
                Debug.Log("Texture saved to: " + filePath);
            }

            AssetDatabase.Refresh();

            // Clean up
            DestroyImmediate(_texture);

            relativePath = Helpers.GetProjectRelativePath(filePath);
        }
    }

    public void LoadLastAssetPathCreated()
    {
        if (!string.IsNullOrEmpty(relativePath))
        {
            createdTextureAsset = AssetDatabase.LoadAssetAtPath<Texture2D>(relativePath);
            if(createdTextureAsset == null)
            {
                Debug.Log("created texture asset is null, from path: " + relativePath);
            }
        }
        else
        {
            Debug.Log("RelativePath not created correctly.");
        }
    }
}



