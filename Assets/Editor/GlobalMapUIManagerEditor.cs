using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlobalMapUIManager))]
public class GlobalMapUIEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GlobalMapUIManager myScript = (GlobalMapUIManager)target;
        if (GUILayout.Button("Update Button Icons"))
        {
            myScript.UpdateEditorRunButtonVisuals();
        }
    }
}
