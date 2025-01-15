using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MLAgents.Configuration
{
    public class ConfigurationSettingsProvider : SettingsProvider
    {
        private ConfigurationSettings _settings;

        private static string customSettingsPath => ConfigurationSettings.SettingsPath;

        public ConfigurationSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = ConfigurationSettings.GetOrCreateSettings();
        }

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

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            DrawConfigsPathProperty();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            return new ConfigurationSettingsProvider("Project/ML Agents Configuration", SettingsScope.Project);
        }
    }
}
