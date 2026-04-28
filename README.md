<p align="center">
  <img src="pushwoosh.png" alt="Pushwoosh Unity SDK">
</p>

<p align="center">
  <a href="https://github.com/Pushwoosh/pushwoosh-unity/releases"><img src="https://img.shields.io/github/release/Pushwoosh/pushwoosh-unity.svg?style=flat-square" alt="GitHub release"></a>
  <a href="https://unity.com"><img src="https://img.shields.io/badge/Unity-2021.3+-black.svg?style=flat-square&logo=unity" alt="Unity"></a>
  <a href="https://img.shields.io/badge/platforms-Android%20%7C%20iOS%20%7C%20Windows-yellowgreen.svg"><img src="https://img.shields.io/badge/platforms-Android%20%7C%20iOS%20%7C%20Windows-yellowgreen.svg?style=flat-square" alt="Platforms"></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square" alt="License"></a>
</p>

<p align="center">
  Push notifications for Android, iOS, and Windows (UWP) applications built with Unity.
</p>

## Table of Contents

- [Documentation](#documentation)
- [Features](#features)
- [Installation](#installation)
  - [UPM via Git URL](#upm-via-git-url-recommended)
  - [UPM via npm](#upm-via-npm)
  - [.unitypackage](#unitypackage)
- [Quick Start](#quick-start)
- [Packages](#packages)
- [Requirements](#requirements)
- [Support](#support)
- [License](#license)

## Documentation

[Pushwoosh Unity SDK Documentation](https://docs.pushwoosh.com/developer/pushwoosh-sdk/cross-platform-frameworks/unity/)

## Features

- **Push Notifications** - Cross-platform push notifications with rich media support
- **Tags & Segmentation** - User targeting and segmentation
- **In-App Messages** - Triggered in-app messaging
- **Analytics** - Delivery and conversion tracking
- **Setup Wizard** - Guided configuration in Unity Editor
- **Modular Architecture** - Install only the platforms you need

## Installation

### UPM via Git URL (recommended)

In Unity: **Window > Package Manager > + > Add package from git URL**

Add the core package first, then the platform packages you need:

```
https://github.com/Pushwoosh/pushwoosh-unity.git?path=com.pushwoosh.unity.core
```

```
https://github.com/Pushwoosh/pushwoosh-unity.git?path=com.pushwoosh.unity.android
```

```
https://github.com/Pushwoosh/pushwoosh-unity.git?path=com.pushwoosh.unity.ios
```

```
https://github.com/Pushwoosh/pushwoosh-unity.git?path=com.pushwoosh.unity.windows
```

To pin a specific version, append `#X.Y.Z`:

```
https://github.com/Pushwoosh/pushwoosh-unity.git?path=com.pushwoosh.unity.core#6.2.10
```

### UPM via npm

Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.pushwoosh.unity.core": "6.2.10",
    "com.pushwoosh.unity.android": "6.2.10",
    "com.pushwoosh.unity.ios": "6.2.10"
  },
  "scopedRegistries": [
    {
      "name": "npmjs",
      "url": "https://registry.npmjs.org",
      "scopes": ["com.pushwoosh"]
    }
  ]
}
```

### .unitypackage

Download `Pushwoosh.unitypackage` from [Releases](https://github.com/Pushwoosh/pushwoosh-unity/releases) and import via **Assets > Import Package > Custom Package**.

## Quick Start

```csharp
using UnityEngine;

public class PushwooshExample : MonoBehaviour
{
    void Start()
    {
        Pushwoosh.ApplicationCode = "XXXXX-XXXXX";

        Pushwoosh.Instance.OnRegisteredForPushNotifications += (token) => {
            Debug.Log("Push token: " + token);
        };

        Pushwoosh.Instance.OnPushNotificationsReceived += (payload) => {
            Debug.Log("Push received: " + payload);
        };

        Pushwoosh.Instance.RegisterForPushNotifications();
    }
}
```

## Packages

| Package | Description |
|---------|-------------|
| `com.pushwoosh.unity.core` | Cross-platform API **(required)** |
| `com.pushwoosh.unity.android` | Android platform support *(optional)* |
| `com.pushwoosh.unity.ios` | iOS platform support *(optional)* |
| `com.pushwoosh.unity.windows` | Windows UWP/WSA support *(optional)* |

## Requirements

- **Unity** 2021.3+
- **Android**: [External Dependency Manager (EDM4U)](https://github.com/googlesamples/unity-jar-resolver) for native dependency resolution
- **iOS**: CocoaPods (auto-configured by the SDK)
- **Windows**: No additional dependencies

## Support

- [Documentation](https://docs.pushwoosh.com/developer/pushwoosh-sdk/cross-platform-frameworks/unity/)
- [GitHub Issues](https://github.com/Pushwoosh/pushwoosh-unity/issues)

## License

Pushwoosh Unity SDK is released under the MIT license. See [LICENSE](LICENSE) for details.
