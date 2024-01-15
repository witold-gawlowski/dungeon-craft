//C# Example (LookAtPointEditor.cs)

using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GeneratorScript))]
    [CanEditMultipleObjects]
    public class GeneratorScriptEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Run"))
            {
                var generatorScript = target as GeneratorScript;
                if (generatorScript)
                {
                    generatorScript.Run();
                }
            }
            //if (GUILayout.Button("DrawTiles"))
            //{
            //    var generatorScript = target as GeneratorScript;
            //    if (generatorScript)
            //    {
            //        generatorScript.CrateTiles();
            //    }
            //}
            //if (GUILayout.Button("Clear"))
            //{
            //    var generatorScript = target as GeneratorScript;
            //    if (generatorScript)
            //    {
            //        generatorScript.ClearLevel();
            //    }
            //}
        }
    }
}