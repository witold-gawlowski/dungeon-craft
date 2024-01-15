using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapPainter))]
    [CanEditMultipleObjects]
    public class MapPainterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Draw"))
            {
                var mapPainter = target as MapPainter;
                if (mapPainter)
                {
                    mapPainter.Draw();
                }
            }

            if (GUILayout.Button("Select Path"))
            {
                var mapPainter = target as MapPainter;
                if (mapPainter)
                {
                    mapPainter.SelectPath();
                }
            }

            if (GUILayout.Button("Save"))
            {
                var mapPainter = target as MapPainter;
                if (mapPainter)
                {
                    mapPainter.Save();
                }
            }

            if (GUILayout.Button("Load"))
            {
                var mapPainter = target as MapPainter;
                if (mapPainter)
                {
                    mapPainter.LoadLastAssetPathCreated();
                }
            }
            
        }
    }
}