# class Pushwoosh #

Provides push notifications registration and handling and uniform access to the platform specific features.
Access it by calling `Pushwoosh.Instance` method.

```csharp
public class Pushwoosh : MonoBehaviour
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

---
### HWID

Returns Device Unique Identifier.

```csharp
public virtual string HWID
```

---
### PushToken

Returns push notifications token. Could be empty if push notifications token has not been received yet.

```csharp
public virtual string PushToken
```

## Events

### OnRegisteredForPushNotifications

Push registration success delegate.

```csharp
public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
```

---
### OnFailedToRegisteredForPushNotifications

Push registration error delegate.

```csharp
public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
```

---
### OnPushNotificationsReceived

Push notification handler delegate.

```csharp
public event NotificationHandler OnPushNotificationsReceived = delegate {};
```

---
### OnInitialized

Push initialization handler delegate.

```csharp
public event InitializationHandler OnInitialized = delegate {};
```

## Methods

### SetIntTag

Sets Integer Tag for the device.

```csharp
public virtual void SetIntTag(string tagName, int tagValue)
```

---
### SetStringTag

Sets String Tag for the device.

```csharp
public virtual void SetStringTag(string tagName, string tagValue)
```

---
### SetListTag

Sets List Tag for the device.

```csharp
public virtual void SetListTag(string tagName, List<object> tagValues)
```

---
### StartTrackingGeoPushes

Starts tracking Geo Push notifications.

```csharp
public virtual void StartTrackingGeoPushes()
```

---
### StopTrackingGeoPushes

Stops tracking Geo Push notifications.

```csharp
public virtual void StartTrackingGeoPushes()
```

---
### ClearNotificationCenter

Clears all notifications from the system tray. iOS and Android only.

```csharp
public virtual void ClearNotificationCenter()
```

---
### SetBadgeNumber

Sets application icon badge number. iOS and Android only.

```csharp
public virtual void SetBadgeNumber(int number)
```

---
### AddBadgeNumber

Increments application icon badge number. iOS and Android only.

```csharp
public virtual void AddBadgeNumber(int deltaBadge)
```
