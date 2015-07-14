# class Pushwoosh #

Provides push notifications registration and handling and uniform access to the platform specific features.
Access it by calling `Pushwoosh.Instance` method.

```csharp
public class Pushwoosh : SingletonBase<Pushwoosh>
```

Example:
```csharp
Pushwoosh.Instance.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
Pushwoosh.Instance.OnPushNotificationsReceived += onPushNotificationsReceived;
```

## Properties

### APP_CODE

Set this variable to your Pushwoosh App Id.

```csharp
public const string APP_CODE = "ENTER_PUSHWOOSH_APP_ID_HERE";
```

---
### GCM_PROJECT_NUMBER

Set this variable to your Google Project Id For Android.

```csharp
public const string GCM_PROJECT_NUMBER = "ENTER_GOOGLE_PROJECT_NUMBER_HERE";
```

## Events

### OnRegisteredForPushNotifications

Push registration success delegate.

```csharp
public event RegisteredForPushNotificationsHandler OnRegisteredForPushNotifications = delegate {};
```

---
### OnFailedToRegisteredForPushNotifications

Push registration error delegate.

```csharp
public event FailedToRegisteredForPushNotificationsHandler OnFailedToRegisteredForPushNotifications = delegate {};
```

---
### OnPushNotificationsReceived

Push notification handler delegate.

```csharp
public event PushNotificationsReceivedHandler OnPushNotificationsReceived = delegate {};
```

## Methods

### IOSPushNotificationsManager

Returns iOS push notifications manager. Works on iOS only.

```csharp
public PushNotificationsIOS IOSPushNotificationsManager
```

--
### AndroidPushNotificationsManager

Returns Android push notifications manager. Works on Android only.

```csharp
public PushNotificationsAndroid AndroidPushNotificationsManager
```

--
### WP8PushNotificationsManager

Returns Windows Phone push notifications manager. Works on Windows Phone only.

```csharp
public PushNotificationsWP8 WP8PushNotificationsManager
```
