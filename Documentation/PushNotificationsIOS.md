# class PushNotificationsIOS #

Provides specific features for iOS platform.

## Methods

[public void registerForPushNotifications()](#registerforremotenotifications)  
[public void unregisterForPushNotifications()](#unregisterforremotenotifications)  
[public void setIntTag(string tagName, int tagValue)](#setinttag)  
[public void setStringTag(string tagName, string tagValue)](#setstringtag)  
[public void setListTag(string tagName, List<object> tagValues)](#setlisttag)  
[public void startTrackingGeoPushes()](#starttrackinggeopushes)  
[public void stopTrackingGeoPushes()](#stoptrackinggeopushes)  
[public void clearNotificationCenter()](#clearnotificationcenter)  
[static public void setBadge(int number)](#setbadge)  
[static public void addBadge(int deltaBadge)](#addbadge)  
[public string getPushToken()](#getpushtoken)  
[public string getPushwooshHWID()](#getpushwooshhwid)  


### registerForRemoteNotifications

Registers for push notifications. Called automatically in `Start` function.

```csharp
public void registerForRemoteNotifications()
```

---
### unregisterForRemoteNotifications

Unregisters from push notifications.

```csharp
public void unregisterForRemoteNotifications()
```

---
### setIntTag

Sets Integer Tag for the device.

```csharp
public void setIntTag(string tagName, int tagValue)
```

--
### setStringTag

Sets String Tag for the device.

```csharp
public void setStringTag(string tagName, string tagValue)
```

--
### setListTag

Sets List Tag for the device.

```csharp
public void setListTag(string tagName, List<object> tagValues)
```

--
### startTrackingGeoPushes

Starts tracking Geo Push notifications

```csharp
public void startTrackingGeoPushes()
```

--
### stopTrackingGeoPushes

Stops tracking Geo Push notifications

```csharp
public void stopTrackingGeoPushes()
```

--
### clearNotificationCenter

Clears all notifications from the system tray.

```csharp
public void clearNotificationCenter()
```

--
### setBadge

Sets iOS badge number icon.

```csharp
static public void setBadge(int number)
```

--
### addBadge

Adds badge number to the current icon number.

```csharp
static public void addBadge(int deltaBadge)
```

---
### getPushToken

Gets push notifications token. Could be empty if no push notifications token has been received.

```csharp
static public string getPushToken()
```

--
### getPushwooshHWID

Gets device unique identifier.

```csharp
static public string getPushwooshHWID()
```
