using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

public class PushwooshBuildManager : MonoBehaviour 
{
	[PostProcessBuild]
	private static void onPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject) {
		if (target == BuildTarget.iOS || target == BuildTarget.iOS) {
			var scriptPath = Path.Combine (Application.dataPath, "Editor/PushwooshPostProcessoriOS.py");
			var args = string.Format ("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", scriptPath, pathToBuiltProject, target.ToString (), Pushwoosh.APP_CODE);
			runScript(scriptPath, args, "python");
		}
		if (target == BuildTarget.WP8Player) {
			postProcessWP8Build(pathToBuiltProject);
		}
	}

	private static void postProcessWP8Build(string pathToBuiltProject) {
		string manifestFilePath = Path.Combine( Path.Combine (pathToBuiltProject, Application.productName), "Properties/WMAppManifest.xml");

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

		UnityEngine.Debug.Log ("Windows Phone manifest sucessfully patched");
	}

	private static void runScript(string scriptPath, string args, string executor) {
		if (!File.Exists (scriptPath)) {
			UnityEngine.Debug.LogError ("Editor script not found: " + scriptPath + ". Did you accidentally delete it?");
			return;
		}

		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = executor,
				Arguments = args,
				UseShellExecute = false,
				RedirectStandardOutput = true
			}
		};
		
		proc.Start ();
		
		string output = proc.StandardOutput.ReadToEnd ();
		proc.WaitForExit ();
		
		UnityEngine.Debug.Log (scriptPath + ": " + output);
	}
}
