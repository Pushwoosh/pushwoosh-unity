using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
using System.IO;
using UnityEditor.Callbacks;
using System;
using System.Text.RegularExpressions;

public class AddNotificationServiceExtensioniOS : MonoBehaviour
{
    [PostProcessBuild]
    private static void PostProcessBuild_iOS(BuildTarget target, string buildPath)
    {
        if (target != BuildTarget.iOS)
            return;

        //Copy files to xcode project dir
        Directory.CreateDirectory(buildPath + "/NotificationService");
        //string[] filesToCopy = ["Info.plist", "NotificationService.entitlements", ];
        File.Copy("Assets/NotificationService/NotificationService.h", buildPath + "/NotificationService/NotificationService.h");
        File.Copy("Assets/NotificationService/NotificationService.m", buildPath + "/NotificationService/NotificationService.m");
        File.Copy("Assets/NotificationService/Info.plist", buildPath + "/NotificationService/Info.plist");

        //load xcode project
        PBXProject proj = new PBXProject();
        string projPath = PBXProject.GetPBXProjectPath(buildPath);
        proj.ReadFromFile(projPath);

        #if UNITY_2019_3_OR_NEWER
        string mainTarget = proj.TargetGuidByName(proj.GetUnityFrameworkTargetGuid());
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

        proj.SetBuildProperty(newTarget, "IPHONEOS_DEPLOYMENT_TARGET", "10.0");
        proj.SetBuildProperty(newTarget, "ARCHS", archs);

        proj.AddFrameworkToProject(newTarget, "UserNotifications.framework", false);
        proj.WriteToFile(projPath);
    }
}
