#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace PushwooshSDK.Editor
{
    public sealed class PushwooshSettingsWindow : EditorWindow
    {
        [MenuItem("Window/Pushwoosh SDK Setup")]
        public static void ShowWindow()
        {
            var window = GetWindow<PushwooshSettingsWindow>(false, _title);
            window.minSize = new Vector2(420, 220);
            window.Show();
        }

        private const string _title = "Pushwoosh SDK Setup";
        private static readonly string _resourcesDir = Path.Combine("Assets", "Pushwoosh", "Resources");
        private static readonly string _assetPath = Path.Combine(_resourcesDir, "PushwooshSettings.asset");
        private static readonly Regex _codePattern = new Regex(@"^[A-Z0-9]{5}-[A-Z0-9]{5}$");

        private string _code = "";
        private string _error = "";
        private bool _editing = false;

        private void OnEnable()
        {
            var settings = LoadSettings();
            _code = settings != null ? settings.ApplicationCode ?? "" : "";
            _editing = string.IsNullOrEmpty(_code);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pushwoosh Application Code", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (!_editing && _codePattern.IsMatch(_code))
            {
                EditorGUILayout.HelpBox("Configured: " + _code, MessageType.Info);
                EditorGUILayout.Space();

                if (GUILayout.Button("Edit"))
                {
                    _editing = true;
                    _error = "";
                }
                return;
            }

            EditorGUILayout.LabelField("Format: XXXXX-XXXXX (find it in your Pushwoosh Control Panel)");
            _code = EditorGUILayout.TextField("Application Code", _code).Trim().ToUpperInvariant();

            if (!string.IsNullOrEmpty(_error))
                EditorGUILayout.HelpBox(_error, MessageType.Error);

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
                Save();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "Alternatively, set the code from code before Pushwoosh.Instance is used:\n" +
                "Pushwoosh.ApplicationCode = \"XXXXX-XXXXX\";",
                MessageType.Info);
        }

        private void Save()
        {
            if (!_codePattern.IsMatch(_code))
            {
                _error = "Invalid format. Expected: XXXXX-XXXXX (letters and digits).";
                return;
            }

            if (!Directory.Exists(_resourcesDir))
                Directory.CreateDirectory(_resourcesDir);

            var settings = LoadSettings();
            if (settings == null)
            {
                settings = CreateInstance<PushwooshSettings>();
                AssetDatabase.CreateAsset(settings, _assetPath);
            }

            var so = new SerializedObject(settings);
            so.FindProperty("_applicationCode").stringValue = _code;
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            _error = "";
            _editing = false;
        }

        private static PushwooshSettings LoadSettings()
        {
            return AssetDatabase.LoadAssetAtPath<PushwooshSettings>(_assetPath);
        }
    }
}
#endif
