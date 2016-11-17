# class PushNotificationsIOS #

Provides specific features for iOS platform.

## Base class methods

[public string PushToken](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#pushtoken)  
[public string HWID](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#hwid)   
[public void RegisterForPushNotifications()](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#registerforpushnotifications)  
[public void UnregisterForPushNotifications()](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#unregisterforpushnotifications)  
[public void SetIntTag(string tagName, int tagValue)](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#setinttag)  
[public void SetStringTag(string tagName, string tagValue)](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#setstringtag)  
[public void SetListTag(string tagName, List<object> tagValues)](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#setlisttag)  
[public void StartTrackingGeoPushes()](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#starttrackinggeopushes)  
[public void StopTrackingGeoPushes()](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#stoptrackinggeopushes)  
[public void ClearNotificationCenter()](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#clearnotificationcenter)  
[public void SetBadgeNumber(int number)](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#setbadgenumber)  
[public void AddBadgeNumber(int deltaBadge)](https://github.com/Pushwoosh/pushwoosh-unity/blob/master/Documentation/Pushwoosh.md#addbadgenumber)  
[public void SetUserId(string userId)](Pushwoosh.md#setuserid)  
[public void PostEvent(string eventId, IDictionary<string, object> attributes)](Pushwoosh.md#postevent)  
[public void SendPurchase(string productId, double price, string currency)](Pushwoosh.md#sendpurchase)  

## Methods

[public string GetLaunchNotification()](#getlaunchnotification)  
[public void ClearLaunchNotification()](#clearlaunchnotification)  

### GetLaunchNotification

Returns launch notification if the app was started in response to push notification or null otherwise.

```csharp
public string GetLaunchNotification()
```

---
### ClearLaunchNotification

Resets launch notifiation, [GetLaunchNotification()](#getlaunchnotification) will return null after this call.

```csharp
public void ClearLaunchNotification()
```
