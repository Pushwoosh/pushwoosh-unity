using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

class BuildScript {
	static string[] SCENES = FindEnabledEditorScenes();

	static string APP_NAME = "UnityPushwoosh-Android";

	[MenuItem("Custom/Build/Build iOS")]
	static void PerformIOSBuild() {
		string buildPath = System.IO.Directory.GetCurrentDirectory() + "/build-ios";
		CreateDirectory (buildPath);
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
		GenericBuild(SCENES, buildPath, BuildTarget.iPhone, BuildOptions.AcceptExternalModificationsToPlayer);
	}

	[MenuItem("Custom/Build/Build Android")]
	static void PerformAndroidBuild() {
		string target_dir = APP_NAME + ".apk";
		string buildPath = System.IO.Directory.GetCurrentDirectory() + "/build-android";
		CreateDirectory(buildPath);
		GenericBuild(SCENES, buildPath + "/" + target_dir, BuildTarget.Android, BuildOptions.None);
	}

	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if(!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options) {
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
		if(res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}

	static private void CreateDirectory(string path){
		if(!System.IO.Directory.Exists (path))
			System.IO.Directory.CreateDirectory (path);
	}
}
