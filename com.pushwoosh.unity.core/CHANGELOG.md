# Changelog

All notable changes to Pushwoosh Unity SDK will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 6.2.9

### Native SDK Updates
- Android SDK: 6.7.65
- iOS SDK: 7.0.39

### Bug Fixes
- Fixed `Pushwoosh.Instance.HWID` returning "Unsupported platform" on Android IL2CPP builds. The platform class was stripped because it's only reachable through reflection. SDK now auto-generates `Assets/Pushwoosh/link.xml` on every domain reload and before each build, and adds `[Preserve]` to `PushNotificationsAndroid/IOS/Windows` as defense-in-depth.
- Fixed `Window → Pushwoosh SDK Setup` showing an empty step list. Replaced the broken reflection-based wizard with a single window for entering the Application Code.

---

## 6.2.8

### Native SDK Updates
- Android SDK: 6.7.62
- iOS SDK: 7.0.34

### New Features
- Added `PushwooshSettings` ScriptableObject and a Setup Wizard step for configuring the Application Code from the Unity Editor. Set the Application Code via `Window → Pushwoosh SDK Setup` — no code changes required.

### Bug Fixes
- Rich Media popups now display correctly when the push arrives while the app is in the foreground on Android.

### Improvements
- `ScheduleLocalNotification` is now available on the base `Pushwoosh` class on all platforms, no platform cast required:
  ```csharp
  Pushwoosh.Instance.ScheduleLocalNotification("Hello", 5);
  ```

### Breaking Changes
- `Pushwoosh.FcmProjectNumber` is deprecated. The Firebase project is now read from `google-services.json` automatically — remove any assignments to this property.

---

## [6.2.7] - 2026-04-02

### Native SDK Updates
- Android SDK: 6.7.60
- iOS SDK: 7.0.33

## [6.2.7] - 2026-04-02

### Native SDK Updates
- Android SDK: 6.7.60
- iOS SDK: 7.0.33

## [6.2.7]

### Native SDK Updates
- Android SDK: 6.7.60
- iOS SDK: 7.0.33

### Breaking Changes
- SDK packages renamed: `com.pushwoosh.unity.core`, `com.pushwoosh.unity.android`, `com.pushwoosh.unity.ios`, `com.pushwoosh.unity.windows`
- Distribution changed from `.unitypackage` to modular UPM packages

### New Features
- Added Setup Wizard for guided SDK configuration in Unity Editor
- Added modular UPM architecture — install only the platforms you need
- Added Windows (UPM/WSA) platform as a separate package
- Added auto-generated `link.xml` to prevent IL2CPP code stripping
- Added automatic iOS Background Modes configuration for remote notifications

## [6.2.6] - 2025-09-11

### Updated
- `registerExistingToken` method now only executes when network constraints are met

## [6.2.5] - 2025-06-11

### Added
- Added `registerExistingToken` method for Android platform

## [6.2.4] - 2025-05-27

### Changed
- Added SetCommunicationEnabled() and IsCommunicationEnabled() methods
- Pushwoosh Android SDK updated to 6.7.25
- Pushwoosh iOS SDK updated to 6.7.18

## [6.2.3] - 2025-02-12

### Fixed
- Windows build issues

## [6.2.2] - 2024-12-24

### Fixed
- Issue with Android devices not registering for pushes with `com.google.firebase:firebase-messaging:24.+`
- Issue with emails not being properly set on Android

## [6.2.1] - 2024-11-20

### Added
- Support for WSA (Windows Store Apps)
- Method `SetLanguage` added

### Fixed
- Fixed receiving the push token

### Updated
- Pushwoosh Android SDK updated to 6.7.12
- Pushwoosh iOS SDK updated to 6.7.9

## [6.2.0] - 2024-07-11

### Fixed
- Conflict with the latest Firebase libraries

### Updated
- AGP updated to 7.4.2, Gradle wrapper updated to 7.5
- EDM4U updated to 1.2.181 version
- Removed references to now disabled Windows and Windows Phone platforms
- Pushwoosh Android SDK updated to 6.7.10
- Pushwoosh iOS SDK updated to 6.5.14

## [6.1.10] - 2023-09-28

### Updated
- Pushwoosh Android SDK to 6.6.16 version
- Pushwoosh iOS SDK to 6.5.3 version

## [6.1.9] - 2023-06-09

### Added
- `PW_GENERATE_SUMMARY` AndroidManifest.xml boolean flag

### Updated
- Pushwoosh Android SDK to 6.6.12 version
- Pushwoosh iOS SDK to 6.4.13 version

## [6.1.8] - 2023-04-17

### Updated
- Pushwoosh Android SDK updated to 6.6.10
- Pushwoosh iOS SDK updated to 6.4.12

## [6.1.7] - 2023-02-28

### Updated
- Pushwoosh Android SDK updated to 6.6.9
- Pushwoosh iOS SDK updated to 6.4.10

## [6.1.6] - 2022-12-22

### Updated
- Pushwoosh Android SDK updated to 6.6.7
- Pushwoosh iOS SDK updated to 6.4.8

## [6.1.5] - 2022-12-01

### Updated
- Pushwoosh Android SDK to 6.6.5
- Pushwoosh iOS SDK to 6.4.8

## [6.1.4] - 2022-09-14

### Added
- Callback methods for iOS purchases made from In-Apps

### Updated
- Pushwoosh iOS SDK version to 6.4.5

## [6.1.3] - 2022-07-29

### Fixed
- Crash in PushAmazonHandlerJob on Fire 7+ devices

### Updated
- Pushwoosh iOS SDK to 6.4.3
- Pushwoosh Android SDK to 6.6.1

## [6.1.2] - 2022-05-18

### Changed
- iOS badges now require adding a NotificationServiceExtension

### Updated
- Pushwoosh Android SDK to 6.5.2 version
- Pushwoosh iOS SDK to 6.4.2 version

## [6.1.1] - 2022-03-28

### Updated
- Pushwoosh Android SDK version to 6.4.4
- Pushwoosh iOS SDK to 6.3.5

## [6.1.0] - 2022-03-09

### Changed
- Plugin now uses `GetUnityMainTargetGuid` instead of deprecated `TargetGuidByName`
- Android resources are now provided via .androidlib to support latest Unity releases

### Updated
- Pushwoosh iOS SDK to 6.3.4
- Pushwoosh Android SDK to 6.4.3

## [6.0.11] - 2022-02-07

### Fixed
- java.lang.NullPointerException while executing doInBackground()

### Updated
- Pushwoosh Android SDK to 6.4.1
- Pushwoosh iOS SDK to 6.3.3

## [6.0.10] - 2022-01-25

### Updated
- Pushwoosh iOS SDK to 6.3.2
- Pushwoosh Android SDK to 6.4.0

## [6.0.9] - 2021-12-01

### Updated
- Pushwoosh iOS SDK to 6.3.1
- Pushwoosh Android SDK to 6.3.6

## [6.0.8] - 2021-09-21

### Updated
- Android SDK updated to 6.3.3 version
- iOS SDK updated to 6.2.5 version

## [6.0.7] - 2021-04-08

### Changed
- Removed the method that collected the list of installed packages to comply with Play Store policy

### Updated
- Android SDK updated to 6.2.7

## [6.0.6] - 2021-03-24

### Fixed
- Pushwoosh initialization process for plugins
- setUserId now properly triggers success callback
- NullPointerException in RichMediaWebActivity
- Bug with group notifications in Android 11

### Updated
- Android SDK updated to 6.2.4

## [6.0.5] - 2021-02-19

### Fixed
- Fixed UserIDs mixup using SetUserId method in Android

### Updated
- iOS SDK updated to 6.1.1
- Android SDK updated to 6.1.4

## [6.0.4] - 2020-10-29

### Updated
- iOS SDK updated to 6.0.9
- Android SDK updated to 6.0.7

## [6.0.3] - 2020-06-05

### Fixed
- Android build issues

## [6.0.2] - 2020-06-05

### Fixed
- Android build issue

## [6.0.1] - 2020-06-05

### Fixed
- Android build issue

## [6.0.0] - 2020-06-05

### Changed
- Android Support libraries replaced with AndroidX

### Updated
- iOS SDK updated to 6.0.2
- Android SDK updated to 6.0.0

## [5.22.3] - 2020-03-19

### Updated
- Android SDK updated to 5.22.2
- iOS SDK updated to 5.22.3

## [5.21.0] - 2019-12-26

### Updated
- Android SDK version updated to 5.21.4
- iOS SDK version updated to 5.21.0

## [5.19.0] - 2019-10-31

### Fixed
- ANRs caused by push messages being processed in the main thread (Android)
- Incorrect inbox URL opening behaviour (Android)
- Background processing that caused extra battery consumption (Android)

### Changed
- Replaced UIWebView with WKWebView (iOS)

### Updated
- Android SDK version updated to 5.19.5
- iOS SDK version updated to 5.19.3

## [5.17.0] - 2019-07-09

### Fixed
- PlayServicesResolver updated to 1.2.121.0
- Preprocessor definitions for PushNotificationsAndroid class updated

## [5.15.0] - 2019-06-19

### Fixed
- Android build issue related to firebase libraries version
- Error while passing date event attributes

## [5.14.1] - 2019-05-23

### Fixed
- Zip Path Traversal Vulnerability

### Updated
- Android SDK version updated to 5.14.3
- iOS SDK version updated to 5.13.1

## [5.14.0] - 2019-04-19

### Added
- Ability to update names and descriptions of Notification Channels via SDK

### Updated
- Android SDK version updated to 5.14.0
- iOS SDK version updated to 5.13.1

## [5.13.0] - 2019-03-06

### Fixed
- Renamed GcmProjectNumber to FcmProjectNumber

### Updated
- Android SDK version updated to 5.13.2
- iOS SDK version updated to 5.13.0

## [5.12.2] - 2019-02-21

### Updated
- Dependencies versions

## [5.12.1] - 2019-02-12

### Fixed
- FCM registration error
- Crash on iOS 9

### Added
- Auto add iOS Entitlements

### Updated
- iOS SDK version updated to 5.12.2

## [5.12.0] - 2019-02-07

### Changed
- Unity plugin updated to use FCM instead of GCM

### Updated
- Android SDK version updated to 5.12.1
- iOS SDK version updated to 5.12.1

## [5.11.0] - 2018-12-07

### Fixed
- Issue with badge module resulting in a NullPointerException

### Updated
- Android SDK version updated to 5.11.0
- iOS SDK version updated to 5.11.0

## [5.10.0] - 2018-11-28

### Changed
- Moved iOS native dependencies to CocoaPods
- Geozones feature is no longer linked by default on iOS

### Updated
- Android SDK version updated to 5.9.4
- iOS SDK version updated to 5.10.0

## [5.9.3] - 2018-10-29

### Updated
- Android SDK version updated to 5.9.3

## [5.9.2] - 2018-10-23

### Added
- RemoteNotificationStatus getter
- Different iOS notification handlers support

### Updated
- Android SDK version updated to 5.9.2

## [5.9.0] - 2018-09-28

### Added
- Local notifications with large icon

### Fixed
- Unity 2018 issues

### Updated
- Android SDK version updated to 5.9.0
- iOS SDK version updated to 5.9.0

## [5.8.2] - 2018-08-30

### Improved
- SDK performance related to OOM issues

### Updated
- Android SDK version updated to 5.8.8

## [5.8.1] - 2018-08-08

### Fixed
- Occasional app crashes when using local notifications

### Updated
- Android SDK version updated to 5.8.5
- iOS SDK version updated to 5.8.3

## [5.8.0] - 2018-06-25

### Added
- Unity 2018 support

### Updated
- Android SDK version updated to 5.8.1
- iOS SDK version updated to 5.8.0

## [5.6.2] - 2018-06-04

### Updated
- Android SDK version updated to 5.7.5
- iOS SDK version updated to 5.7.2

## [5.6.1] - 2018-05-31

### Updated
- Android SDK version updated to 5.7.4
- iOS SDK version updated to 5.7.1

## [5.6.0] - 2018-05-10

### Added
- GDPR compliance solution
- In-App business solutions

### Fixed
- iOS SDK conflict with Firebase

### Updated
- Android SDK version updated to 5.7.2
- iOS SDK version updated to 5.7

## [5.5.4] - 2018-02-21

### Updated
- Android SDK version updated to 5.5.5
- iOS SDK version updated to 5.5.4

## [5.5.3] - 2018-01-15

### Updated
- Android SDK version updated to 5.5.3
- iOS SDK version updated to 5.5.3

## [5.5.1] - 2017-12-21

### Added
- Support of Java 7

## [5.5.0] - 2017-12-12

### Updated
- iOS SDK updated to 5.5.2
- Android SDK updated to 5.5.2

## [5.4.0] - 2017-10-24

### Updated
- Android SDK version updated to 5.4.1
- iOS SDK version updated to 5.4.0
