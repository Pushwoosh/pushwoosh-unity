using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PushNotificationsAndroid : Pushwoosh 
{
#if UNITY_ANDROID && !UNITY_EDITOR
	private static AndroidJavaObject pushwoosh = null;
	
	protected override void Initialize() 
	{
		if(pushwoosh != null)
			return;
		
		using(var pluginClass = new AndroidJavaClass("com.pushwoosh.PushwooshProxy")) {
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
		AndroidJavaObject tags = new AndroidJavaObject ("com.pushwoosh.TagValues");

		foreach( var tagValue in tagValues )
		{
			tags.Call ("addValue", tagValue);
		}

		pushwoosh.Call ("setListTag", tagName, tags);
	}

	public string GetLaunchNotification()
	{
		return pushwoosh.Call<string>("getLaunchNotification");
	}

	public String[] GetPushHistory()
	{
		AndroidJavaObject history = pushwoosh.Call<AndroidJavaObject>("getPushHistory");
		if (history.GetRawObject().ToInt32() == 0)
		{
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
		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds, userdata);
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

	void OnApplicationPause(bool paused)
	{
		//make sure everything runs smoothly even if pushwoosh is not initialized yet
		if (pushwoosh == null)
			Initialize();

		if(paused)
		{
			pushwoosh.Call("onPause");
		}
		else
		{
			pushwoosh.Call("onResume");
		}
	}
#endif
}
