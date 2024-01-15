using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PrefabSaver))]
    [CanEditMultipleObjects]
    public class PrefabSaverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Select Path"))
            {
                var generatorScript = target as PrefabSaver;
                if (generatorScript)
                {
                    generatorScript.SelectPath();
                }
            }
            if (GUILayout.Button("Save as prefab"))
            {
                var generatorScript = target as PrefabSaver;
                if (generatorScript)
                {
                    generatorScript.Save();
                }
            }
        }
    }
}