using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Policies;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAgents.Configuration;
using MLAgents.Utilities;

namespace MLAgents.Editor
{
    public class ConfigurationGenerator : EditorWindow
    {
        [MenuItem("Window/ML Agents Configuration/Configuration Generator")]
        public static void ShowWindow()
        {
            ConfigurationGenerator wnd = GetWindow<ConfigurationGenerator>();

            wnd.InitializeConfiguration();
            wnd.InitializePath();
            wnd.ParseBehaviors();
            wnd.titleContent = new GUIContent("Configuration generator");
        }

        protected string _configPath;

        public virtual void InitializePath()
        {
            var defaultPath = ConfigurationSettings.GetOrCreateSettings().DefaultConfigsPath;
            var name = SceneManager.GetActiveScene().name;

            _configPath = System.IO.Path.Combine(defaultPath, $"{name}.yaml");
        }

        public class BehaviorsComparer : IEqualityComparer<BehaviorParameters>
        {
            public bool Equals(BehaviorParameters first, BehaviorParameters second)
            {
                if (ReferenceEquals(first, second))
                    return true;

                return first.BehaviorName == second.BehaviorName;
            }

            public int GetHashCode(BehaviorParameters behavior) => behavior.BehaviorName.GetHashCode();
        }

        protected List<string> _managedSerializedProperties = new List<string>();

        [SerializeField]
        protected List<Behavior> _behaviors;

        public virtual void ParseBehaviors()
        {
            var behaviorComponents = UnityEngine.Object.FindObjectsByType<BehaviorParameters>(FindObjectsSortMode.None)
                .Distinct(new BehaviorsComparer());

            _behaviors = behaviorComponents
                .Select(b => new Behavior { BehaviorId = b.BehaviorName })
                .ToList();

            // Bind behavior presets
            UnityEngine.Object.FindObjectsOfType<BehaviorConfig>()
                .GroupBy(c => c.BehaviorName)
                .Select(c => c.First())
                .ToList()
                .ForEach(c =>
                {
                    var behaviorIndex = _behaviors.FindIndex(b => b.BehaviorId == c.BehaviorName.Value);

                    if (behaviorIndex == -1)
                    {
                        return;
                    }

                    c.Behavior.BehaviorId = c.BehaviorName.Value;
                    _behaviors[behaviorIndex] = c.Behavior;

                    _managedSerializedProperties.Add($"_behaviors.data[{behaviorIndex}]");
                });
        }

        protected SerializedObject _serializedObject;
        [SerializeField]
        protected Config _configuration;

        public virtual void InitializeConfiguration()
        {
            var config = UnityEngine.Object.FindObjectsByType<EnvironmentConfig>(FindObjectsSortMode.None)
                .DefaultIfEmpty(null)
                .FirstOrDefault();

            if (config != null)
            {
                _configuration = config.Configuration;
                _managedSerializedProperties.Add("_configuration");

                return;
            }

            _configuration = new Config();
        }

        protected Vector2 _scrollPosition;
        protected bool _behaviorsOpen;

        public void OnGUI()
        {
            if (_serializedObject is null)
            {
                _serializedObject = new SerializedObject(this);
            }
            _serializedObject.Update();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            var behaviorsProperty = _serializedObject.FindProperty("_behaviors");

            _behaviorsOpen = EditorGUILayout.Foldout(
                _behaviorsOpen,
                "Behaviors",
                new GUIStyle(EditorStyles.foldout)
                {
                    margin = new RectOffset(2, 0, 0, 0)
                }
            );

            if (_behaviorsOpen)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < behaviorsProperty.arraySize; i++)
                {
                    var behavior = behaviorsProperty.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(behavior);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(_serializedObject.FindProperty("_configuration"));

            _configPath = EditorGUILayout.TextField("Name", _configPath);
            bool _generate = GUILayout.Button("Generate config");
            EditorGUILayout.EndScrollView();

            _serializedObject.ApplyModifiedProperties();
            if (_generate)
            {
                _configuration.Behaviors = _behaviors;
                ConfigCreator.CreateConfig(_configuration, _configPath);
            }
        }
    }
}
