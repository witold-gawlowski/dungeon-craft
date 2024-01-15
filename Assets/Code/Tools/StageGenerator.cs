using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] private string relativeDataPath;

    [SerializeField]
    private List<MapConfig> mapConfigs;

    public void SelectPath()
    {
        var absolutPath = EditorUtility.OpenFolderPanel("Select stage data path", "", "");
        relativeDataPath = Helpers.GetProjectRelativePath(absolutPath);
    }

    public void FillSingleStage()
    {
        if (System.IO.Directory.Exists(relativeDataPath))
        {
            mapConfigs = GetMapConfigsInDirectory(relativeDataPath);
        }
        else
        {
            Debug.Log("Given path is not a directory");
        }
    }

    private List<MapConfig> GetMapConfigsInDirectory(string path)
    {
        List<MapConfig> configs = new List<MapConfig>();

        string[] guids = AssetDatabase.FindAssets("t:MapConfig", new string[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            MapConfig config = AssetDatabase.LoadAssetAtPath<MapConfig>(assetPath);
            if (config != null)
            {
                configs.Add(config);
            }
        }

        return configs;
    }

}
