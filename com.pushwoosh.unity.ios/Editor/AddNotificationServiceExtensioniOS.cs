#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Text.RegularExpressions;

public class AddNotificationServiceExtensioniOS
{
    private static string FindNotificationServiceDir()
    {
        string[] searchPaths = {
            "Assets/NotificationService",
            "Packages/com.pushwoosh.unity.ios/NotificationService"
        };

        foreach (string path in searchPaths)
        {
            string fullPath = Path.GetFullPath(path);
            if (Directory.Exists(fullPath) && File.Exists(Path.Combine(fullPath, "NotificationService.h")))
                return fullPath;
        }

        return null;
    }

    [PostProcessBuild]
    private static void PostProcessBuild_iOS(BuildTarget target, string buildPath)
    {
        if (target != BuildTarget.iOS)
            return;

        //Copy files to xcode project dir
        Directory.CreateDirectory(buildPath + "/NotificationService");

        string nseSourceDir = FindNotificationServiceDir();
        if (nseSourceDir == null)
        {
            Debug.LogWarning("Pushwoosh: NotificationService files not found, skipping NSE setup");
            return;
        }

        File.Copy(Path.Combine(nseSourceDir, "NotificationService.h"), buildPath + "/NotificationService/NotificationService.h");
        File.Copy(Path.Combine(nseSourceDir, "NotificationService.m"), buildPath + "/NotificationService/NotificationService.m");

        string infoPlistSource = Path.Combine(nseSourceDir, "Info.plist");
        if (!File.Exists(infoPlistSource))
        {
            // Generate a minimal Info.plist for NSE
            string plistContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\n" +
                "<plist version=\"1.0\">\n<dict>\n" +
                "<key>CFBundleDisplayName</key><string>NotificationService</string>\n" +
                "<key>CFBundleExecutable</key><string>NotificationService</string>\n" +
                "<key>CFBundleIdentifier</key><string>$(PRODUCT_BUNDLE_IDENTIFIER)</string>\n" +
                "<key>CFBundleInfoDictionaryVersion</key><string>6.0</string>\n" +
                "<key>CFBundleName</key><string>NotificationService</string>\n" +
                "<key>CFBundlePackageType</key><string>XPC!</string>\n" +
                "<key>CFBundleShortVersionString</key><string>1.0</string>\n" +
                "<key>CFBundleVersion</key><string>1</string>\n" +
                "<key>NSExtension</key><dict>\n" +
                "<key>NSExtensionPointIdentifier</key><string>com.apple.usernotifications.service</string>\n" +
                "<key>NSExtensionPrincipalClass</key><string>NotificationService</string>\n" +
                "</dict>\n</dict>\n</plist>";
            File.WriteAllText(buildPath + "/NotificationService/Info.plist", plistContent);
        }
        else
        {
            File.Copy(infoPlistSource, buildPath + "/NotificationService/Info.plist");
        }

        //load xcode project
        PBXProject proj = new PBXProject();
        string projPath = PBXProject.GetPBXProjectPath(buildPath);
        proj.ReadFromFile(projPath);

        #if UNITY_2019_3_OR_NEWER
        string mainTarget = proj.GetUnityFrameworkTargetGuid();
        #else
        string mainTarget = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
        #endif

        //Try to retreive bundleId from xcode project
        string bundleId = null;

        string infoPlistPath = "Info.plist";
        try {
            infoPlistPath = proj.GetBuildPropertyForAnyConfig(mainTarget, "INFOPLIST_FILE");
        } catch (Exception e)
        {
            Debug.Log("Can't load Info.plist location: " + e);
        }

        try
        {
            PlistDocument infoPlist = new PlistDocument();
            infoPlist.ReadFromFile(buildPath + "/" + infoPlistPath);
            bundleId = infoPlist.root.values["CFBundleIdentifier"].AsString();
            Regex configParamsMatcher = new Regex(@"\$\{([\w_]*)\}");
            MatchCollection matches = configParamsMatcher.Matches(bundleId);
            bundleId = configParamsMatcher.Replace(bundleId, (match) =>
            {
                return proj.GetBuildPropertyForAnyConfig(mainTarget, match.Groups[1].Value);
            });
        }
        catch (Exception e)
        {
            Debug.Log("Can't load BundleId: " + e);
            if (String.IsNullOrEmpty(bundleId))
            {
                bundleId = "com.unity3d.product";
            }
        }

        //Try to retrieve archs
        string archs = proj.GetBuildPropertyForAnyConfig(mainTarget, "ARCHS");

        //Add extension
        string newTarget = proj.AddAppExtension(mainTarget, "NotificationService", bundleId + ".NotificationService", "NotificationService/Info.plist");
        proj.AddFileToBuild(newTarget, proj.AddFile(buildPath + "/NotificationService/NotificationService.h", "NotificationService/NotificationService.h"));
        proj.AddFileToBuild(newTarget, proj.AddFile(buildPath + "/NotificationService/NotificationService.m", "NotificationService/NotificationService.m"));

        proj.SetBuildProperty(newTarget, "IPHONEOS_DEPLOYMENT_TARGET", "13.0");
        if (!string.IsNullOrEmpty(archs))
            proj.SetBuildProperty(newTarget, "ARCHS", archs);

        proj.AddFrameworkToProject(newTarget, "UserNotifications.framework", false);
        proj.WriteToFile(projPath);
    }
}
#endif
