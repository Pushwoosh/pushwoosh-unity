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
			pluginClass.CallStatic("initialize", Pushwoosh.ApplicationCode, Pushwoosh.GcmProjectNumber);
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

	public string GetLaunchNotification()
	{
		return pushwoosh.Call<string>("getLaunchNotification");
	}

	public void ClearLaunchNotification()
	{
		pushwoosh.Call("clearLaunchNotification");
	}

	public String[] GetPushHistory()
	{
		AndroidJavaObject history = pushwoosh.Call<AndroidJavaObject>("getPushHistory");
		if (history.GetRawObject().ToInt32() == 0) {
			return new String[0];
		}
		
		String[] result = AndroidJNIHelper.ConvertFromJNIArray<String[]>(history.GetRawObject());
		history.Dispose();
		
		return result;
	}
	
	public void ClearPushHistory()
	{
		pushwoosh.Call("clearPushHistory");
	}

	public override void StartTrackingGeoPushes()
	{
		pushwoosh.Call("startTrackingGeoPushes");
	}

	public override void StopTrackingGeoPushes()
	{
		pushwoosh.Call("stopTrackingGeoPushes");
	}
	
	public void StartTrackingBeaconPushes()
	{
		pushwoosh.Call("startTrackingBeaconPushes");
	}

	public void StopTrackingBeaconPushes()
	{
		pushwoosh.Call("stopTrackingBeaconPushes");
	}

	public void SetBeaconBackgroundMode(bool backgroundMode)
	{
		pushwoosh.Call("setBeaconBackgroundMode", backgroundMode);
	}
	
	public void ClearLocalNotifications()
	{
		pushwoosh.Call("clearLocalNotifications");
	}

	public override void ClearNotificationCenter()
	{
		pushwoosh.Call("clearNotificationCenter");
	}

	public int ScheduleLocalNotification(string message, int seconds)
	{
		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds);
	}

	public int ScheduleLocalNotification(string message, int seconds, string userdata)
	{
		IDictionary<string,string> parameters = new Dictionary<string, string>();
		parameters.Add("u", userdata);
		return ScheduleLocalNotification(message, seconds, parameters);
	}

	public int ScheduleLocalNotification(string message, int seconds, IDictionary<string, string> parameters)
	{
		var extras = new AndroidJavaObject("android.os.Bundle");
		foreach (var item in parameters)
		{
			extras.Call("putString", item.Key, item.Value);
		}
		
		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds, extras);
	}

	public void ClearLocalNotification(int id)
	{
		pushwoosh.Call("clearLocalNotification", id);
	}
	
	public void SetMultiNotificationMode()
	{
		pushwoosh.Call("setMultiNotificationMode");
	}

	public void SetSimpleNotificationMode()
	{
		pushwoosh.Call("setSimpleNotificationMode");
	}

	/* 
	 * Sound notification types:
	 * 0 - default mode
	 * 1 - no sound
	 * 2 - always
	 */
	public void SetSoundNotificationType(int soundNotificationType)
	{
		pushwoosh.Call("setSoundNotificationType", soundNotificationType);
	}

	/* 
	 * Vibrate notification types:
	 * 0 - default mode
	 * 1 - no vibrate
	 * 2 - always
	 */
	public void SetVibrateNotificationType(int vibrateNotificationType)
	{
		pushwoosh.Call("setVibrateNotificationType", vibrateNotificationType);
	}

	public void SetLightScreenOnNotification(bool lightsOn)
	{
		pushwoosh.Call("setLightScreenOnNotification", lightsOn);
	}

	public void SetEnableLED(bool ledOn)
	{
		pushwoosh.Call("setEnableLED", ledOn);
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
		get { return pushwoosh.Call<string>("getPushwooshHWID"); }
	}

	public override string PushToken
	{
		get { return pushwoosh.Call<string>("getPushToken"); }
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
#endif
}
