#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;

class PushwooshBuildProcessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        string assetsUrl = Directory.GetCurrentDirectory() + "/Assets/";
        string sourcePath = assetsUrl + "google-services.json";
        string amazonKeyUrl = assetsUrl + "api_key.txt";
        string libUrl = assetsUrl + "/Plugins/Android/pushwoosh-resources.androidlib/";

        CreateIfNeeded(libUrl + "/res");
        CreateIfNeeded(libUrl + "/res/values");

        string manifestPath = libUrl + "AndroidManifest.xml";
        if (!File.Exists(manifestPath))
        {
            WriteFile("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\"\n    package=\"com.pushwoosh.unitypluginresources\" >\n</manifest>", manifestPath);
        }

        string propsPath = libUrl + "project.properties";
        if (!File.Exists(propsPath))
        {
            WriteFile("target=android-21\nandroid.library=true", propsPath);
        }

        string destinationPath = assetsUrl + "/Plugins/Android/pushwoosh-resources.androidlib/res/values/googleservices.xml";
       
        if (File.Exists(sourcePath))
        {
            string json = ReadJson(sourcePath);

            var root = PushwooshJSON.JSON.Parse(json);
            var projectNumber = root["project_info"]["project_number"].Value;
            var projectId = root["project_info"]["project_id"].Value;

            var element = root["client"][0];
            var oauthClient = element["oauth_client"];
            var clientId = oauthClient.Count > 0 ? oauthClient[0]["client_id"].Value : "";

            var appId = element["client_info"]["mobilesdk_app_id"].Value;
            var googleApiKey = element["api_key"][0]["current_key"].Value;

            var xml = CreateXml(projectNumber, clientId, appId, projectId, googleApiKey);
            WriteFile(xml, destinationPath);
        }

        if (File.Exists(amazonKeyUrl))
        {
            CreateIfNeeded(libUrl + "/assets");
            FileUtil.CopyFileOrDirectory(amazonKeyUrl, libUrl + "assets/api_key.txt");
        }
    }

    private void CreateIfNeeded(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    string ReadJson(string path) {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();
        return json;
    }

    void WriteFile(string content, string path)
    {
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(content);
        writer.Close();
    }

    private string CreateXml(string projectNumber, string clientId, object appId, object projectId, object googleApiKey)
    {
        return "<?xml version='1.0' encoding='utf-8'?>\n<resources tools:keep=\"@string/gcm_defaultSenderId,@string/project_id,@string/google_api_key,@string/default_web_client_id,@string/google_app_id\" xmlns:tools=\"http://schemas.android.com/tools\">\n "
            + "<string name=\"gcm_defaultSenderId\" translatable=\"false\">" + projectNumber + "</string>\n"
            + "<string name=\"default_web_client_id\" translatable=\"false\">" + clientId + "</string>\n"
            + "<string name=\"google_app_id\" translatable=\"false\">" + appId + "</string>\n"
            + "<string name=\"project_id\" translatable=\"false\">" + projectId + "</string>\n"
            + "<string name=\"google_api_key\" translatable=\"false\">" + googleApiKey + "</string>\n"
            + "</resources>";
    }
}
#endif
