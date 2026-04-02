
#define UNITY_3_PLUS
#define UNITY_4_PLUS
#define UNITY_5_PLUS
#if UNITY_2_6
#define UNITY_2_X
#undef UNITY_3_PLUS
#undef UNITY_4_PLUS
#undef UNITY_5_PLUS
#elif UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#define UNITY_3_X
#undef UNITY_4_PLUS
#undef UNITY_5_PLUS
#elif UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#define UNITY_4_X
#undef UNITY_5_PLUS
#elif UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
#define UNITY_5_X
#endif

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

#if UNITY_EDITOR_OSX && UNITY_5_PLUS
using UnityEditor.iOS.Xcode;
#endif

public class PushwooshBuildManager 
{

	#if UNITY_IOS
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
		if (buildTarget == BuildTarget.iOS) {
        	string preprocessorPath = path + "/Classes/Preprocessor.h";
        	string text = File.ReadAllText(preprocessorPath);
        	text = text.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
        	File.WriteAllText(preprocessorPath, text);
		}
    }
    #endif

	[PostProcessBuild]
	private static void onPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject) {
#if UNITY_EDITOR_OSX && UNITY_5_PLUS
		if (target == BuildTarget.iOS) {
			UnityEngine.Debug.Log ("Path to built project: " + pathToBuiltProject);

			string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
			UnityEngine.Debug.Log ("Project Path: " + projPath);

			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));
			string projTarget = proj.GetUnityMainTargetGuid();
			string frameworkTarget = proj.GetUnityFrameworkTargetGuid();
			UnityEngine.Debug.Log ("Project Target: " + projTarget);

			proj.AddFrameworkToProject(projTarget, "Security.framework", false);
			proj.AddFrameworkToProject(frameworkTarget, "UserNotifications.framework", false);
			proj.AddBuildProperty(projTarget, "OTHER_LDFLAGS", "-ObjC -lz -lstdc++");

            string entitlementsSource = FindEntitlementsPlist();
            if (entitlementsSource != null)
            {
                File.Copy(entitlementsSource, pathToBuiltProject + "/Entitlements.plist");
                proj.AddBuildProperty(projTarget, "CODE_SIGN_ENTITLEMENTS", "Entitlements.plist");
            }

			File.WriteAllText(projPath, proj.WriteToString());

			// Add Background Modes -> Remote notifications
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);
			PlistElementArray bgModes = plist.root["UIBackgroundModes"] as PlistElementArray;
			if (bgModes == null)
				bgModes = plist.root.CreateArray("UIBackgroundModes");
			bool hasRemote = false;
			foreach (var mode in bgModes.values)
			{
				if (mode.AsString() == "remote-notification")
				{
					hasRemote = true;
					break;
				}
			}
			if (!hasRemote)
				bgModes.AddString("remote-notification");
			plist.WriteToFile(plistPath);

			GeneratePodfile(pathToBuiltProject);
			RunPodInstall(pathToBuiltProject);
		}
#endif

		if (target == BuildTarget.WP8Player) {
			postProcessWP8Build(pathToBuiltProject);
		}
	}

	private static string FindEntitlementsPlist() {
		string[] searchPaths = {
			"Assets/Plugins/iOS/Entitlements.plist",
			"Packages/com.pushwoosh.unity.ios/Plugins/iOS/Entitlements.plist"
		};

		foreach (string path in searchPaths) {
			string fullPath = Path.GetFullPath(path);
			if (File.Exists(fullPath))
				return fullPath;
		}

		return null;
	}

	private static bool IsEDM4UInstalled() {
		return System.Type.GetType("Google.IOSResolver, Google.IOSResolver") != null ||
		       System.Type.GetType("Google.IOSResolver, Unity.IOSResolver") != null;
	}

	private static string GetIOSSDKVersion() {
		string[] searchPaths = {
			"Assets/Pushwoosh/Editor/PushwooshIOSDependencies.xml",
			"Assets/Editor/PushwooshIOSDependencies.xml",
			"Packages/com.pushwoosh.unity.ios/Editor/PushwooshIOSDependencies.xml"
		};

		foreach (string xmlPath in searchPaths) {
			if (File.Exists(xmlPath)) {
				string content = File.ReadAllText(xmlPath);
				var match = Regex.Match(content, @"PushwooshXCFramework.*?version=""([0-9.]+)""");
				if (match.Success) {
					return match.Groups[1].Value;
				}
			}
		}
		return null;
	}

	private static void GeneratePodfile(string path) {
		if (IsEDM4UInstalled()) {
			UnityEngine.Debug.Log("Pushwoosh: EDM4U detected, skipping Podfile generation");
			return;
		}

		string podfilePath = Path.Combine(path, "Podfile");
		if (File.Exists(podfilePath)) {
			string existing = File.ReadAllText(podfilePath);
			if (existing.Contains("PushwooshXCFramework")) {
				UnityEngine.Debug.Log("Pushwoosh: Podfile already contains PushwooshXCFramework, skipping generation");
				return;
			}
		}

		string iosVersion = GetIOSSDKVersion();
		if (string.IsNullOrEmpty(iosVersion)) {
			UnityEngine.Debug.LogWarning("Pushwoosh: Could not detect iOS SDK version from PushwooshIOSDependencies.xml");
			return;
		}

		string podfileContent =
			"source 'https://cdn.cocoapods.org/'\n" +
			"platform :ios, '13.0'\n\n" +
			"target 'UnityFramework' do\n" +
			"  pod 'PushwooshXCFramework', '" + iosVersion + "'\n" +
			"end\n\n" +
			"target 'Unity-iPhone' do\n" +
			"end\n";

		File.WriteAllText(podfilePath, podfileContent);
		UnityEngine.Debug.Log("Pushwoosh: Podfile generated with PushwooshXCFramework " + iosVersion);
	}

	private static void RunPodInstall(string path) {
		try {
			ProcessStartInfo psi = new ProcessStartInfo() {
				FileName = "/bin/bash",
				Arguments = "-l -c \"cd \\\"" + path + "\\\" && pod install\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true
			};

			Process process = Process.Start(psi);
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			process.WaitForExit();

			if (process.ExitCode == 0) {
				UnityEngine.Debug.Log("Pushwoosh: pod install succeeded\n" + output);
			} else {
				UnityEngine.Debug.LogWarning("Pushwoosh: pod install failed (exit code " + process.ExitCode + ")\n" + error + "\n" + output);
				UnityEngine.Debug.LogWarning("Pushwoosh: Run 'pod install' manually in " + path);
			}
		} catch (System.Exception e) {
			UnityEngine.Debug.LogWarning("Pushwoosh: Could not run pod install: " + e.Message + "\nRun 'pod install' manually in " + path);
		}
	}

	private static void postProcessWP8Build(string pathToBuiltProject) {
		string manifestFilePath = Path.Combine( Path.Combine (pathToBuiltProject, PlayerSettings.productName), "Properties/WMAppManifest.xml");

		if (!File.Exists (manifestFilePath)) {
			UnityEngine.Debug.LogError ("Windows Phone manifest not found: " + manifestFilePath);
			return;
		}

		XmlDocument manifest = new XmlDocument ();
		manifest.Load(manifestFilePath);

		XmlNode capabilities = manifest.SelectSingleNode ("//Capabilities");
		XmlNodeList matchingCapability = manifest.SelectNodes("//Capability[@Name='ID_CAP_IDENTITY_DEVICE']");
		if (matchingCapability.Count == 0) {
			XmlElement newCapability = manifest.CreateElement("Capability");
			newCapability.SetAttribute("Name", "ID_CAP_IDENTITY_DEVICE");
			capabilities.AppendChild(newCapability);
		}

		matchingCapability = manifest.SelectNodes("//Capability[@Name='ID_CAP_PUSH_NOTIFICATION']");
		if (matchingCapability.Count == 0) {
			XmlElement newCapability = manifest.CreateElement("Capability");
			newCapability.SetAttribute("Name", "ID_CAP_PUSH_NOTIFICATION");
			capabilities.AppendChild(newCapability);
		}

		manifest.Save (manifestFilePath);

		UnityEngine.Debug.Log ("Windows Phone manifest successfully patched");
	}
}
