# class PushwooshForWindowsPhone #

Provides specific features for Windows Phone platform.

## Properties

### PushToken

Returns push notifications token. Could be empty if push notifications token has not been received yet.

---
### HWID

Returns Device Unique Identifier.

## Methods

### SubscribeForPushNotifications

Registers for push notifications. Called automatically in `Start` function.

```csharp
public void SubscribeForPushNotifications()
```

---
### UnsubscribeForPushNotifications

Unregisters from push notifications.

```csharp
public void UnsubscribeForPushNotifications()
```

---
### SetTags

Sets Tags for the device.

```csharp
public void SetTags(List<KeyValuePair<string, object>> tags)
```

---
### StartGeoLocation

Starts tracking Geo Push notifications

```csharp
public void StartGeoLocation()
```

---
### StartGeoLocation

Stops tracking Geo Push notifications

```csharp
public void StartGeoLocation()
```
