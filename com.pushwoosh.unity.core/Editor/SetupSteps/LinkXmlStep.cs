#if UNITY_EDITOR
using System.IO;

namespace PushwooshSDK.Editor
{
    public class LinkXmlStep : PushwooshSetupStep
    {
        public override string Summary => "Generate link.xml";

        public override string Details =>
            $"Generates {_destinationPath} to prevent Pushwoosh classes from being removed by Unity's IL2CPP code stripping.";

        public override bool IsRequired => true;

        protected override bool _getIsStepCompleted() => File.Exists(_destinationPath);

        protected override void _runStep()
        {
            if (!Directory.Exists(_destinationDir))
                Directory.CreateDirectory(_destinationDir);

            if (!File.Exists(_destinationPath))
            {
                string content =
                    "<linker>\n" +
                    "  <assembly fullname=\"Pushwoosh.Core.Runtime\" preserve=\"all\" />\n" +
                    "  <assembly fullname=\"Pushwoosh.Android.Runtime\" preserve=\"all\" />\n" +
                    "  <assembly fullname=\"Pushwoosh.iOS.Runtime\" preserve=\"all\" />\n" +
                    "  <assembly fullname=\"Pushwoosh.Windows.Runtime\" preserve=\"all\" />\n" +
                    "</linker>\n";
                File.WriteAllText(_destinationPath, content);
            }
        }

        private static readonly string _destinationDir = Path.Combine("Assets", "Pushwoosh");
        private static readonly string _destinationPath = Path.Combine(_destinationDir, "link.xml");
    }
}
#endif
