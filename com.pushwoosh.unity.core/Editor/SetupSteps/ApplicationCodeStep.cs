#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace PushwooshSDK.Editor
{
    public class ApplicationCodeStep : PushwooshSetupStep
    {
        public override string Summary => "Set Application Code";

        public override string Details =>
            "Creates PushwooshSettings asset with your Pushwoosh Application Code (format: XXXXX-XXXXX). " +
            "The code can be found in your Pushwoosh Control Panel.";

        public override bool IsRequired => true;

        private static readonly string _resourcesDir = Path.Combine("Assets", "Pushwoosh", "Resources");
        private static readonly string _assetPath = Path.Combine(_resourcesDir, "PushwooshSettings.asset");
        private static readonly Regex _codePattern = new Regex(@"^[A-Z0-9]{5}-[A-Z0-9]{5}$");

        protected override bool _getIsStepCompleted()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PushwooshSettings>(_assetPath);
            return settings != null && !string.IsNullOrEmpty(settings.ApplicationCode);
        }

        protected override void _runStep()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PushwooshSettings>(_assetPath);

            if (settings == null)
            {
                if (!Directory.Exists(_resourcesDir))
                    Directory.CreateDirectory(_resourcesDir);

                settings = ScriptableObject.CreateInstance<PushwooshSettings>();
                AssetDatabase.CreateAsset(settings, _assetPath);
            }

            ApplicationCodeWindow.Show(settings);
        }
    }

    public class ApplicationCodeWindow : EditorWindow
    {
        private static readonly Regex _codePattern = new Regex(@"^[A-Z0-9]{5}-[A-Z0-9]{5}$");

        private PushwooshSettings _settings;
        private string _code = "";
        private string _error = "";

        public static void Show(PushwooshSettings settings)
        {
            var window = GetWindow<ApplicationCodeWindow>(true, "Pushwoosh Application Code");
            window._settings = settings;
            window._code = settings.ApplicationCode ?? "";
            window.minSize = new Vector2(350, 120);
            window.maxSize = new Vector2(450, 120);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Enter your Pushwoosh Application Code:");
            EditorGUILayout.Space();

            _code = EditorGUILayout.TextField("Application Code", _code).Trim().ToUpper();

            if (!string.IsNullOrEmpty(_error))
            {
                EditorGUILayout.HelpBox(_error, MessageType.Error);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Save"))
            {
                if (!_codePattern.IsMatch(_code))
                {
                    _error = "Invalid format. Expected: XXXXX-XXXXX (letters and digits)";
                    return;
                }

                var so = new SerializedObject(_settings);
                so.FindProperty("_applicationCode").stringValue = _code;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();

                Close();
            }
        }
    }
}
#endif
