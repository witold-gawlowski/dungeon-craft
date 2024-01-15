using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.SceneManagement;

class EditorScrips : EditorWindow
{
 
    [MenuItem("File/Open Level Shop _&%h")]
    public static void OpenLevelShop()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/2-LevelShop.unity");
    }
    
    [MenuItem("File/Open GlobalMap _&%j")]
    public static void OpenGlobalMap()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/8-GlobalMap.unity");
    }
}