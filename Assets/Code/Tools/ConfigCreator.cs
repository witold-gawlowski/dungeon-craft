using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfigCreator : MonoBehaviour
{

    [SerializeField]
    private string folderPath;

    [SerializeField]
    private GameObject levelPrefab;

    [SerializeField]
    private Texture2D levelTexture;

    [SerializeField]
    private MapConfig createdAsset;

    public void SelectPath()
    {
        folderPath = EditorUtility.OpenFolderPanel("Select save path", "", "");
    }

    public void LoadData()
    {
        MapPainter painter = GetComponent<MapPainter>();
        PrefabSaver prefabSaver = GetComponent<PrefabSaver>();
        
        if(painter != null && prefabSaver != null)
        {
            levelPrefab = prefabSaver.GetCreatedPreafabAsset();
            levelTexture = painter.GetCreatedAsset();
        }
    }

    public void CreateAndSaveConfigFile()
    {
        createdAsset = ScriptableObject.CreateInstance<MapConfig>();
        createdAsset.texture = levelTexture;
        createdAsset.levelPrefab = levelPrefab;

        long timestamp = Helpers.GetTimestamp();

        string relativeFolderPath = Helpers.GetProjectRelativePath(folderPath);
        string assetPath = AssetDatabase.GenerateUniqueAssetPath(relativeFolderPath + "/"+ timestamp + ".asset");


        AssetDatabase.CreateAsset(createdAsset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
