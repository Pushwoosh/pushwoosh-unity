#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace PushwooshSDK.Editor
{
    public class PushwooshLinkXmlGenerator : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string Dir = "Assets/Pushwoosh";
        private const string Path = "Assets/Pushwoosh/link.xml";

        private const string Content =
            "<linker>\n" +
            "  <assembly fullname=\"Pushwoosh.Core.Runtime\" preserve=\"all\" />\n" +
            "  <assembly fullname=\"Pushwoosh.Android.Runtime\" preserve=\"all\" />\n" +
            "  <assembly fullname=\"Pushwoosh.iOS.Runtime\" preserve=\"all\" />\n" +
            "  <assembly fullname=\"Pushwoosh.Windows.Runtime\" preserve=\"all\" />\n" +
            "</linker>\n";

        public void OnPreprocessBuild(BuildReport report)
        {
            EnsureLinkXml();
        }

        [InitializeOnLoadMethod]
        private static void EnsureOnLoad()
        {
            EditorApplication.delayCall += EnsureLinkXml;
        }

        private static void EnsureLinkXml()
        {
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);

            if (File.Exists(Path))
            {
                var existing = File.ReadAllText(Path).Replace("\r\n", "\n");
                if (existing == Content)
                    return;
            }

            File.WriteAllText(Path, Content);
            AssetDatabase.ImportAsset(Path, ImportAssetOptions.ForceSynchronousImport);
        }
    }
}
#endif
