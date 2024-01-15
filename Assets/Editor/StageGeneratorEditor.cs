using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(StageGenerator))]
    public class StageGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Select stage data path"))
            {
                var mapPainter = target as StageGenerator;
                if (mapPainter)
                {
                    mapPainter.SelectPath();
                }
            }

            if (GUILayout.Button("Generate!"))
            {
                var mapPainter = target as StageGenerator;
                if (mapPainter)
                {
                    mapPainter.FillSingleStage();
                }
            }
        }
    }
}