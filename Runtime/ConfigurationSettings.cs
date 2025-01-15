using MLAgents.Utilities;
using UnityEditor;
using UnityEngine;

namespace MLAgents.Configuration
{
    public class ConfigurationSettings : ScriptableObject
    {
        [field: HideInInspector]
        [field: SerializeField]
        public string DefaultConfigsPath { get; private set; } = "Assets/ML-Agents/Configs";
        public void UpdateConfigsPath(string newPath)
        {
            DefaultConfigsPath = newPath;
            EditorUtility.SetDirty(this);
        }

        public const string SettingsDirectory = "MLAgentsConfiguration";
        public const string SettingsFile = "ConfigurationSettings.asset";
        public static string SettingsPath => System.IO.Path.Combine("Assets", SettingsDirectory, SettingsFile);

        public static ConfigurationSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ConfigurationSettings>(SettingsPath);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ConfigurationSettings>();
                SaveUtilities.EnsureFolderExists(SettingsDirectory);
                AssetDatabase.CreateAsset(settings, SettingsPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    [CustomEditor(typeof(ConfigurationSettings))]
    public class ConfigurationSettingsEditor : UnityEditor.Editor
    {
        private ConfigurationSettings _settings;

        private void DrawConfigsPathProperty()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Default configs path", EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth - 1),
                GUILayout.Height(EditorGUIUtility.singleLineHeight));

            EditorGUI.BeginDisabledGroup(true);
            var path = EditorGUILayout.TextField(_settings.DefaultConfigsPath);
            EditorGUI.EndDisabledGroup();

            var clicked = GUILayout.Button("Browse...", GUILayout.ExpandWidth(false), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            EditorGUILayout.EndHorizontal();

            if (clicked)
            {
                string newPath = EditorUtility.SaveFolderPanel(
                   "Select folder",
                   System.IO.Path.Combine("Assets", _settings.DefaultConfigsPath),
                   "Configs"
                );

                if (string.IsNullOrEmpty(newPath))
                {
                    return;
                }

                _settings.UpdateConfigsPath(newPath);
                path = newPath;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _settings = (ConfigurationSettings)target;

            DrawConfigsPathProperty();
        }
    }
}
