#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEditor;
using UnityEditor.Build;

class CustomBuildProcessor : IPreprocessBuild
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

        string destinationPath = assetsUrl + "/Plugins/Android/pushwoosh-resources.androidlib/res/values/googleservices.xml";
       
        if (File.Exists(sourcePath))
        {
            string json = ReadJson(sourcePath);

            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            var projectInfo = dict["project_info"] as Dictionary<string, object>;
            var projectNumber = projectInfo["project_number"] as string;
            var projectId = projectInfo["project_id"] as string;

            var client = dict["client"] as List<object>;
            var element = client[0] as Dictionary<string, object>;
            var oauthClient = element["oauth_client"] as List<object>;
            var elementOathClient = oauthClient.Count > 0 ? oauthClient[0] as Dictionary<string, object> : null;
            var clientId = elementOathClient != null ? elementOathClient["client_id"] as string : "";

            var clientInfo = element["client_info"] as Dictionary<string, object>;
            var appId = clientInfo["mobilesdk_app_id"];
            
            var apiKey = element["api_key"] as List<object>;
            var elementApiKey = apiKey[0] as Dictionary<string, object>;
            var googleApiKey = elementApiKey["current_key"] as string;


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
