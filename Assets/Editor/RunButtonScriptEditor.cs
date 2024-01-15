using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(RunButtonScript))]
    [CanEditMultipleObjects]
    public class RunButtonScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Random Seed"))
            {
                var runButton = target as RunButtonScript;
                if (runButton)
                {
                    runButton.RandomSeed();
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}