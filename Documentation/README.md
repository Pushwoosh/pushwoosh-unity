# Pushwoosh Unity SDK

Push notification SDK for iOS and Android.

## API Reference

### Pushwoosh

Main SDK interface. Singleton accessed via `Pushwoosh.Instance`.

**Properties:**
- `Pushwoosh.ApplicationCode` — Pushwoosh app code (e.g. `"XXXXX-XXXXX"`)
- `Pushwoosh.FcmProjectNumber` — Firebase Cloud Messaging sender ID (Android)
- `Pushwoosh.Instance.HWID` — Hardware ID
- `Pushwoosh.Instance.PushToken` — Push token

**Events:**
- `OnRegisteredForPushNotifications(string token)` — successful registration
- `OnFailedToRegisteredForPushNotifications(string error)` — registration failed
- `OnPushNotificationsReceived(string payload)` — push received (foreground)
- `OnPushNotificationsOpened(string payload)` — push opened by user

**Methods:**
- `RegisterForPushNotifications()` — register for push notifications
- `UnregisterForPushNotifications()` — unregister
- `SetStringTag(string name, string value)` — set string tag
- `SetIntTag(string name, int value)` — set integer tag
- `SetListTag(string name, List<object> values)` — set list tag
- `GetTags(GetTagsHandler handler)` — get tags from server
- `PostEvent(string eventId, IDictionary<string, object> attributes)` — post in-app event
- `SetUserId(string userId)` — set user ID
- `SetEmail(string email)` — set user email
- `SetEmails(List<string> emails)` — set multiple emails
- `SetUser(string userId, List<string> emails)` — set user ID and emails
- `SetLanguage(string language)` — set language
- `SendPurchase(string productId, double price, string currency)` — track purchase
- `SetBadgeNumber(int number)` — set badge number
- `AddBadgeNumber(int delta)` — increment badge number
- `ClearNotificationCenter()` — clear notification center
- `GetLaunchNotification()` — get notification that launched the app
- `ClearLaunchNotification()` — clear launch notification
- `StartTrackingGeoPushes()` — start geolocation tracking
- `StopTrackingGeoPushes()` — stop geolocation tracking
- `IsCommunicationEnabled()` — check if server communication is enabled
- `SetCommunicationEnabled(bool enabled)` — enable/disable server communication

### PushNotificationsAndroid

Android-specific methods (available on Android only):

- `ScheduleLocalNotification(string message, int seconds)` — schedule local notification
- `ClearLocalNotifications()` — clear all local notifications
- `SetMultiNotificationMode()` / `SetSimpleNotificationMode()` — notification grouping
- `SetNotificationChannelDelegate(NotificationChannelDelegate delegate)` — customize notification channels
