using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CellResizeScript))]
[CanEditMultipleObjects]
public class CellResizeScriptEditor : UnityEditor.Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Resize cells"))
        {
            var resizeScript = target as CellResizeScript;
            resizeScript.UpdateCellSize();
        }
    }
}