#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PushwooshSDK.Editor
{
    public sealed class PushwooshSetupWindow : EditorWindow
    {
        [MenuItem("Window/Pushwoosh SDK Setup")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(PushwooshSetupWindow), false, _title);
            window.minSize = _minSize;
            window.Show();
        }

        private static readonly Vector2 _minSize = new Vector2(400, 300);
        private const string _title = "Pushwoosh SDK Setup";
        private const string _description = "Setup steps for Pushwoosh Unity SDK";

        private List<PushwooshSetupStep> _setupSteps;
        private readonly Queue<PushwooshSetupStep> _stepsToRun = new Queue<PushwooshSetupStep>();

        private bool _guiSetupComplete = false;
        private GUIStyle _summaryStyle;
        private GUIStyle _runStyle;
        private GUIStyle _detailsStyle;
        private GUIStyle _requiredStyle;
        private GUIStyle _optionalStyle;
        private Texture _checkTexture;
        private Texture _boxTexture;
        private Vector2 _scrollPosition;

        private void OnEnable()
        {
            var baseType = typeof(PushwooshSetupStep);
            var steps = new List<PushwooshSetupStep>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.FullName.Contains("Pushwoosh"))
                    continue;

                foreach (var type in assembly.GetTypes())
                {
                    if (type != baseType && baseType.IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        if (Activator.CreateInstance(type) is PushwooshSetupStep step)
                            steps.Add(step);
                    }
                }
            }

            _setupSteps = steps;
        }

        private void OnGUI()
        {
            _setupGUI();

            GUILayout.Label(_description);
            EditorGUILayout.Separator();

            if (_setupSteps == null)
                return;

            var willDisableControls = _stepsToRun.Count > 0 || EditorApplication.isUpdating || EditorApplication.isCompiling;

            EditorGUI.BeginDisabledGroup(willDisableControls);
            if (GUILayout.Button("Run All Steps"))
            {
                foreach (var step in _setupSteps)
                    _stepsToRun.Enqueue(step);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            foreach (var step in _setupSteps)
            {
                EditorGUILayout.BeginHorizontal();

                var sumContent = new GUIContent(step.Summary);
                var sumRect = GUILayoutUtility.GetRect(sumContent, _summaryStyle);

                var checkRect = new Rect(sumRect.x, sumRect.y, sumRect.height, sumRect.height);
                GUI.DrawTexture(checkRect, step.IsStepCompleted ? _checkTexture : _boxTexture);

                sumRect.x += sumRect.height + EditorStyles.label.padding.left;
                GUI.Label(sumRect, sumContent);

                EditorGUI.BeginDisabledGroup(step.IsStepCompleted || willDisableControls);
                if (GUILayout.Button("Run", _runStyle) && !_stepsToRun.Contains(step))
                    _stepsToRun.Enqueue(step);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                GUILayout.Label(step.Details, _detailsStyle);

                if (step.IsRequired)
                    GUILayout.Label("Required", _requiredStyle);
                else
                    GUILayout.Label("Optional", _optionalStyle);

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }

            EditorGUILayout.EndScrollView();
        }

        private void OnInspectorUpdate()
        {
            while (_stepsToRun.Count > 0)
            {
                var step = _stepsToRun.Dequeue();
                EditorUtility.DisplayProgressBar("Pushwoosh Setup", $"Running: {step.Summary}", 0.5f);
                step.RunStep();
            }
            EditorUtility.ClearProgressBar();
        }

        private void _setupGUI()
        {
            if (_guiSetupComplete)
                return;

            _summaryStyle = EditorStyles.boldLabel;

            _detailsStyle = new GUIStyle(GUI.skin.textField);
            _detailsStyle.wordWrap = true;

            _runStyle = new GUIStyle(GUI.skin.button);
            _runStyle.fixedWidth = _minSize.x * .3f;

            _requiredStyle = new GUIStyle(EditorStyles.miniBoldLabel);
            _requiredStyle.normal.textColor = Color.red;

            _optionalStyle = new GUIStyle(EditorStyles.miniBoldLabel);
            _optionalStyle.normal.textColor = Color.yellow;
            _optionalStyle.fontStyle = FontStyle.Italic;

            _checkTexture = EditorGUIUtility.IconContent("TestPassed").image;
            _boxTexture = EditorGUIUtility.IconContent("Warning").image;

            _guiSetupComplete = true;
        }
    }
}
#endif
