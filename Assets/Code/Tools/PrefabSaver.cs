using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabSaver : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private string _folderPath;

    [SerializeField]
    private GameObject _savedPrefab;

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public void SelectPath()
    {
        _folderPath = EditorUtility.OpenFolderPanel("Select save path", "", "");
    }

    public GameObject GetCreatedPreafabAsset()
    {
        return _savedPrefab;
    }

    public void Save()
    {
        if (_target != null)
        {
            if (_folderPath.Length != 0)
            {
                long timestamp = Helpers.GetTimestamp();

                string filePath = _folderPath + "/" + timestamp.ToString() + ".prefab";

                if (PrefabUtility.GetPrefabAssetType(gameObject) != PrefabAssetType.NotAPrefab)
                {
                    PrefabUtility.UnpackPrefabInstance(_target, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }

                _savedPrefab = PrefabUtility.SaveAsPrefabAsset(_target, filePath);
                if (_savedPrefab != null)
                {
                    Debug.Log("Prefab saved at: " + filePath);
                }
                else
                {
                    Debug.LogError("Failed to save the prefab!");
                }
            }
        }
    }
}
