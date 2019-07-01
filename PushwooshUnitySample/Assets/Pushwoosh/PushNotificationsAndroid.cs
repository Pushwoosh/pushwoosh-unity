using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PushNotificationsAndroid : Pushwoosh
{
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject pushwoosh = null;

	private Queue<GetTagsHandler> tagsHandlers = new Queue<GetTagsHandler>();
	
	protected override void Initialize() 
	{
		if(pushwoosh != null)
			return;
		
		using(var pluginClass = new AndroidJavaClass("com.pushwoosh.unityplugin.PushwooshProxy")) {
			pluginClass.CallStatic("initialize", Pushwoosh.ApplicationCode, Pushwoosh.FcmProjectNumber);
			pushwoosh = pluginClass.CallStatic<AndroidJavaObject>("instance");
		}
		
		pushwoosh.Call("setListenerName", this.gameObject.name);
    }
 
	public override void RegisterForPushNotifications()
	{
		pushwoosh.Call("registerForPushNotifications");
	}

	public override void UnregisterForPushNotifications()
	{
		pushwoosh.Call("unregisterFromPushNotifications");
	}

	public override void SetIntTag(string tagName, int tagValue)
	{
		pushwoosh.Call("setIntTag", tagName, tagValue);
	}

	public override void SetStringTag(string tagName, string tagValue)
	{
		pushwoosh.Call("setStringTag", tagName, tagValue);
	}

	public override void SetListTag(string tagName, List<object> tagValues)
	{
		AndroidJavaObject tags = new AndroidJavaObject ("com.pushwoosh.unityplugin.TagValues");

		foreach( var tagValue in tagValues ) {
			tags.Call ("addValue", tagValue);
		}

		pushwoosh.Call ("setListTag", tagName, tags);
	}

	public override void GetTags(GetTagsHandler handler)
	{
		tagsHandlers.Enqueue(handler);
		pushwoosh.Call("getTags");
	}

    public override void SetNotificationChannelDelegate(NotificationChannelDelegate notificationChannelDelegate)
    {
        pushwoosh.Call("setNotificationChannelDelegate", notificationChannelDelegate);
    }

    public override string GetLaunchNotification()
	{
        return ReturnStringFromNative(pushwoosh.Call<string>("getLaunchNotification"));
	}

	public override void ClearLaunchNotification()
	{
		pushwoosh.Call("clearLaunchNotification");
	}

    public override NotificationSettings GetRemoteNotificationStatus()
    {
        string jsonSettings = ReturnStringFromNative(pushwoosh.Call<string>("getRemoteNotificationStatus"));
        return JsonUtility.FromJson<NotificationSettings>(jsonSettings);
    }

	public override void StartTrackingGeoPushes()
	{
		pushwoosh.Call("startTrackingGeoPushes");
	}

	public override void StopTrackingGeoPushes()
	{
		pushwoosh.Call("stopTrackingGeoPushes");
	}

	public override void ClearNotificationCenter()
	{
		pushwoosh.Call("clearNotificationCenter");
	}

	public override void SetBadgeNumber(int number)
	{
		pushwoosh.Call("setBadgeNumber", number);
	}
	
	public override void AddBadgeNumber(int deltaBadge)
	{
		pushwoosh.Call("addBadgeNumber", deltaBadge);
	}

	public override string HWID
	{
        get { return ReturnStringFromNative(pushwoosh.Call<string>("getPushwooshHWID")); }
	}

	public override string PushToken
	{
        get { return ReturnStringFromNative(pushwoosh.Call<string>("getPushToken")); }
	}

	public override void SetUserId(string userId)
	{
		pushwoosh.Call("setUserId", userId);
	}

	protected override void PostEventInternal(string eventId, string attributes)
	{
		pushwoosh.Call("postEvent", eventId, attributes);
	}

	public override void SendPurchase(string productId, double price, string currency)
	{
		pushwoosh.Call("sendPurchase", productId, price, currency);
	}

	void onRegisteredForPushNotifications(string token)
	{
		RegisteredForPushNotifications (token);
	}

	void onFailedToRegisteredForPushNotifications(string error)
	{
		FailedToRegisteredForPushNotifications (error);
	}

	void onPushNotificationsReceived(string payload)
	{
		PushNotificationsReceived (payload);
	}

	void onPushNotificationsOpened(string payload)
	{
		PushNotificationsOpened (payload);
	}

	void onTagsReceived(string json)
	{
		GetTagsHandler handler = tagsHandlers.Dequeue();
		if (handler != null) {
			try {
				IDictionary<string, object> tags = PushwooshUtils.JsonToDictionary(json);
				handler(tags, null);
			}
			catch(Exception e) {
				Debug.Log ("Invalid tags: " + e.ToString());
				handler (null, new PushwooshException(e.Message));
			}
		}
	}

	void onFailedToReceiveTags(string error)
	{
		GetTagsHandler handler = tagsHandlers.Dequeue();
		if (handler != null) {
			handler(null, new PushwooshException(error));
		}
	}

	void OnApplicationPause(bool paused)
	{
		//make sure everything runs smoothly even if pushwoosh is not initialized yet
		if (pushwoosh == null)
			Initialize();

		if(paused) {
			pushwoosh.Call("onPause");
		}
		else {
			pushwoosh.Call("onResume");
		}
	}

    //GDPR

    public override void ShowGDPRConsentUI()
    {
        pushwoosh.Call("showGDPRConsentUI");
    }

    public override void ShowGDPRDeletionUI()
    {
        pushwoosh.Call("showGDPRDeletionUI");
    }

    public override bool IsCommunicationEnabled()
    {
        return pushwoosh.Call<Boolean>("isCommunicationEnabled"); 
    }

    public override bool isDeviceDataRemoved()
    {
        return pushwoosh.Call<Boolean>("isDeviceDataRemoved");
    }

    public override bool IsGDPRAvailable()
    {
        return pushwoosh.Call<Boolean>("isAvailable");
    }

    public override void SetCommunicationEnabled(bool enable)
    {
    pushwoosh.Call("setCommunicationEnabled", enable);
    }

    void OnSetCommunicationEnabled(string success)
    {
        SetCommunicationEnableCallBack();
    }

    void OnFailedSetCommunicationEnabled(string error){
        FailedSetCommunicationEnableCallback(error);
    }

    public override void RemoveAllDeviceData()
    {
        pushwoosh.Call("removeAllDeviceData");
    }

    void OnRemoveAllDeviceData(string success)
    {
        RemoveAllDataCallBack();
    }

    void OnFailedRemoveAllDeviceData(string error)
    {
        FailedRemoveAllDataCallback(error);
    }

    private string ReturnStringFromNative(string fromNative) 
    {
        if (fromNative != null && fromNative.Length > 0) {
            return fromNative;
        }
        return null;
    }

#endif

    //Android specific methods

    public string[] GetPushHistory()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject history = pushwoosh.Call<AndroidJavaObject>("getPushHistory");
        if (history.GetRawObject().ToInt32() == 0)
        {
            return new string[0];
        }

        string[] result = AndroidJNIHelper.ConvertFromJNIArray<string[]>(history.GetRawObject());
        history.Dispose();

        return result;
#else
        return new string[0];
#endif
    }

    public void ClearPushHistory()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("clearPushHistory");
#endif
    }

    public void StartTrackingBeaconPushes()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("startTrackingBeaconPushes");
#endif
    }

    public void StopTrackingBeaconPushes()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("stopTrackingBeaconPushes");
#endif
    }

    public void SetBeaconBackgroundMode(bool backgroundMode)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setBeaconBackgroundMode", backgroundMode);
#endif
    }

    public void ClearLocalNotifications()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("clearLocalNotifications");
#endif
    }

    public void ClearLocalNotification(int id)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("clearLocalNotification", id);
#endif
    }

    public int ScheduleLocalNotification(string message, int seconds)
    {
        return ScheduleLocalNotification(message, seconds, null, null);
    }

    public int ScheduleLocalNotification(string message, int seconds, string userdata)
    {
        IDictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("u", userdata);
        return ScheduleLocalNotification(message, seconds, parameters, null);
    }

    public int ScheduleLocalNotification(string message, int seconds, IDictionary<string, string> parameters)
    {
        return ScheduleLocalNotification(message, seconds, parameters, null);
    }

    public int ScheduleLocalNotification(string message, int seconds, IDictionary<string, string> parameters, string largeIcon)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject extras = null;
        if (parameters != null)
        {
            extras = new AndroidJavaObject("android.os.Bundle");
            foreach (var item in parameters)
            {
                extras.Call("putString", item.Key, item.Value);
            }
        }

        return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds, extras, largeIcon);
#else
        return 0;
#endif
    }

    public void SetMultiNotificationMode()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setMultiNotificationMode");
#endif
    }

    public void SetSimpleNotificationMode()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setSimpleNotificationMode");
#endif
    }

    /* 
	 * Sound notification types:
	 * 0 - default mode
	 * 1 - no sound
	 * 2 - always
	 */
    public void SetSoundNotificationType(int soundNotificationType)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setSoundNotificationType", soundNotificationType);
#endif
    }

    /* 
	 * Vibrate notification types:
	 * 0 - default mode
	 * 1 - no vibrate
	 * 2 - always
	 */
    public void SetVibrateNotificationType(int vibrateNotificationType)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setVibrateNotificationType", vibrateNotificationType);
#endif
    }

    public void SetLightScreenOnNotification(bool lightsOn)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setLightScreenOnNotification", lightsOn);
#endif
    }

    public void SetEnableLED(bool ledOn)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        pushwoosh.Call("setEnableLED", ledOn);
#endif
    }
}
