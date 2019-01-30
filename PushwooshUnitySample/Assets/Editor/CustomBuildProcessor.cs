#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
class MyCustomBuildProcessor : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        string assetsUrl = System.IO.Directory.GetCurrentDirectory()+"/Assets/";
        string sourcePath = assetsUrl + "google-services.json";
        if (File.Exists(sourcePath))
        {
            FileUtil.CopyFileOrDirectory(sourcePath, assetsUrl + "/Plugins/Android/assets/google-services.json");
        }
    }
}
#endif
