# class PushNotificationsAndroid #

Provides specific features for Android platform.

## Methods

[public void registerForPushNotifications()](#registerforpushnotifications)  
[public void unregisterForPushNotifications()](#unregisterforpushnotifications)  
[public void setIntTag(string tagName, int tagValue)](#setinttag)  
[public void setStringTag(string tagName, string tagValue)](#setstringtag)  
[public void setListTag(string tagName, List<object> tagValues)](#setlisttag)  
[public String[] getPushHistory()](#getpushhistory)  
[public void clearPushHistory()](#clearpushhistory)  
[public void startTrackingGeoPushes()](#starttrackinggeopushes)  
[public void stopTrackingGeoPushes()](#stoptrackinggeopushes)  
[public void startTrackingBeaconPushes()](#starttrackingbeaconpushes)  
[public void stopTrackingBeaconPushes()](#stoptrackingbeaconpushes)  
[public void clearLocalNotifications()](#clearlocalnotifications)  
[public void clearNotificationCenter()](#clearnotificationcenter)  
[public int scheduleLocalNotification(string message, int seconds)](#schedulelocalnotification)  
[public void clearLocalNotification(int id)](#clearlocalnotification)  
[public void setMultiNotificationMode()](#setmultinotificationmode)  
[public void setSimpleNotificationMode()](#setsimplenotificationmode)  
[public void setSoundNotificationType(int soundNotificationType)](#setsoundnotificationtype)  
[public void setVibrateNotificationType(int vibrateNotificationType)](#setsoundnotificationtype)  
[public void setLightScreenOnNotification(bool lightsOn)](#setlightscreenonnotification)  
[public void setEnableLED(bool ledOn)](#setenableled)  
[public string getPushToken()](#getpushtoken)  
[public string getPushwooshHWID()](#getpushwooshhwid)  


### registerForPushNotifications

Registers for push notifications. Called automatically in `Start` function.

```csharp
public void registerForPushNotifications()
```

---
### unregisterForPushNotifications

Unregisters from push notifications.

```csharp
public void unregisterForPushNotifications()
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
### getPushHistory

Gets push history for the device (since the last `clearPushHistory` call).

```csharp
public String[] getPushHistory()
```

--
### clearPushHistory

Clears push history for the device.

```csharp
public void clearPushHistory()
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
### startTrackingBeaconPushes

Starts tracking iBeacon Push notifications

```csharp
public void startTrackingBeaconPushes()
```

--
### stopTrackingBeaconPushes

Stops tracking iBeacon Push notifications

```csharp
public void stopTrackingBeaconPushes()
```

--
### clearLocalNotifications

Clears all scheduled local notifications.

```csharp
public void clearLocalNotifications()
```

--
### clearNotificationCenter

Clears all notifications from the system tray.

```csharp
public void clearNotificationCenter()
```

--
### scheduleLocalNotification

Schedules local notification.

```csharp
public int scheduleLocalNotification(string message, int seconds)
public int scheduleLocalNotification(string message, int seconds, string userdata)
```

* **message** - notification title
* **seconds** - timeout in seconds to display the message
* **userdata** - optional

--
### clearLocalNotification

Clears specific local notification

```csharp
public void clearLocalNotification(int id)
```

* **id** - notification id returned from `scheduleLocalNotification` function

--
### setMultiNotificationMode

Allows multiple notifications in the tray.

```csharp
public void setMultiNotificationMode()
```

--
### setSimpleNotificationMode

Allows single notifications in the tray.

```csharp
public void setSimpleNotificationMode()
```

--
### setSoundNotificationType

Sets default sound notification type. Could be overriden from Pushwoosh Control Panel.
* 0 - default mode
* 1 - no sound
* 2 - always

```csharp
public void setSoundNotificationType(int soundNotificationType)
```

--
### setVibrateNotificationType

Sets default vibration notification type. Could be overriden from Pushwoosh Control Panel.
* 0 - default mode
* 1 - no vibration
* 2 - always

```csharp
public void setVibrateNotificationType(int vibrateNotificationType)
```

--
### setLightScreenOnNotification

Should the screen lit up itself when push notification arrives. Could be overriden from Pushwoosh Control Panel.

```csharp
public void setLightScreenOnNotification(bool lightsOn)
```

--
### setEnableLED

Enables LED blinking. Could be overriden from Pushwoosh Control Panel.

```csharp
public void setEnableLED(bool ledOn)
```

--
### getPushToken

Gets push notifications token. Could be empty if no push notifications token has been received.

```csharp
public string getPushToken()
```

--
### getPushwooshHWID

Gets device unique identifier.

```csharp
public string getPushwooshHWID()
```
