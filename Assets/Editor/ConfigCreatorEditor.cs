using Codice.LogWrapper;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ConfigCreator))]
    public class ConfigCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Select Path"))
            {
                var configCreator = target as ConfigCreator
                    ;
                if (configCreator)
                {
                    configCreator.SelectPath();
                }
            }

            if (GUILayout.Button("Load Data"))
            {
                var configCreator = target as ConfigCreator;
                if (configCreator)
                {
                    configCreator.LoadData();
                }
            }

            if (GUILayout.Button("Create and save config file"))
            {
                var configCreator = target as ConfigCreator;
                if (configCreator)
                {
                    configCreator.CreateAndSaveConfigFile();
                }
            }
        }
    }
}