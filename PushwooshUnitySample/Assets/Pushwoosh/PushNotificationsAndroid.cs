using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PushNotificationsAndroid : MonoBehaviour 
{
#if UNITY_ANDROID && !UNITY_EDITOR
	public event Pushwoosh.RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.NotificationHandler OnPushNotificationsReceived = delegate {};

	// Use this for initialization
	void Start () {
		InitPushwoosh();
		registerForPushNotifications();
		
		Debug.Log(this.gameObject.name);
		Debug.Log(getPushToken());
	}

	private static AndroidJavaObject pushwoosh = null;
	
	void InitPushwoosh() {
		if(pushwoosh != null)
			return;
		
		using(var pluginClass = new AndroidJavaClass("com.arellomobile.android.push.PushwooshProxy")) {
			pluginClass.CallStatic("initialize", Pushwoosh.APP_CODE, Pushwoosh.GCM_PROJECT_NUMBER);
			pushwoosh = pluginClass.CallStatic<AndroidJavaObject>("instance");
		}
		
		pushwoosh.Call("setListenerName", this.gameObject.name);
	}
 
	public void setIntTag(string tagName, int tagValue)
	{
		pushwoosh.Call("setIntTag", tagName, tagValue);
	}

	public void registerForPushNotifications()
	{
		pushwoosh.Call("registerForPushNotifications");
	}

	public void unregisterForPushNotifications()
	{
		pushwoosh.Call("unregisterFromPushNotifications");
	}

	public void setStringTag(string tagName, string tagValue)
	{
		pushwoosh.Call("setStringTag", tagName, tagValue);
	}

	public void setListTag(string tagName, List<object> tagValues)
	{
		AndroidJavaObject tags = new AndroidJavaObject ("com.arellomobile.android.push.TagValues");

		foreach( var tagValue in tagValues )
		{
			tags.Call ("addValue", tagValue);
		}

		pushwoosh.Call ("setListTag", tagName, tags);
	}

	public String[] getPushHistory()
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
	
	public void clearPushHistory()
	{
		pushwoosh.Call("clearPushHistory");
	}

	public void sendLocation(double lat, double lon)
	{
		pushwoosh.Call("sendLocation", lat, lon);
	}

	public void startTrackingGeoPushes()
	{
		pushwoosh.Call("startTrackingGeoPushes");
	}

	public void stopTrackingGeoPushes()
	{
		pushwoosh.Call("stopTrackingGeoPushes");
	}
	
	public void startTrackingBeaconPushes()
	{
		pushwoosh.Call("startTrackingBeaconPushes");
	}

	public void stopTrackingBeaconPushes()
	{
		pushwoosh.Call("stopTrackingBeaconPushes");
	}

	public void setBeaconBackgroundMode(bool backgroundMode)
	{
		pushwoosh.Call("setBeaconBackgroundMode", backgroundMode);
	}
	
	public void clearLocalNotifications()
	{
		pushwoosh.Call("clearLocalNotifications");
	}

	public void clearNotificationCenter()
	{
		pushwoosh.Call("clearNotificationCenter");
	}

	public int scheduleLocalNotification(string message, int seconds)
	{
		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds);
	}

	public int scheduleLocalNotification(string message, int seconds, string userdata)
	{
		return pushwoosh.Call<int>("scheduleLocalNotification", message, seconds, userdata);
	}

	public void clearLocalNotification(int id)
	{
		pushwoosh.Call("clearLocalNotification", id);
	}
	
	public void setMultiNotificationMode()
	{
		pushwoosh.Call("setMultiNotificationMode");
	}

	public void setSimpleNotificationMode()
	{
		pushwoosh.Call("setSimpleNotificationMode");
	}

	/* 
	 * Sound notification types:
	 * 0 - default mode
	 * 1 - no sound
	 * 2 - always
	 */
	public void setSoundNotificationType(int soundNotificationType)
	{
		pushwoosh.Call("setSoundNotificationType", soundNotificationType);
	}

	/* 
	 * Vibrate notification types:
	 * 0 - default mode
	 * 1 - no vibrate
	 * 2 - always
	 */
	public void setVibrateNotificationType(int vibrateNotificationType)
	{
		pushwoosh.Call("setVibrateNotificationType", vibrateNotificationType);
	}

	public void setLightScreenOnNotification(bool lightsOn)
	{
		pushwoosh.Call("setLightScreenOnNotification", lightsOn);
	}

	public void setEnableLED(bool ledOn)
	{
		pushwoosh.Call("setEnableLED", ledOn);
	}
	
	public string getPushToken()
	{
		return pushwoosh.Call<string>("getPushToken");
	}

	public string getPushwooshHWID()
	{
		return pushwoosh.Call<string>("getPushwooshHWID");
	}

	void onRegisteredForPushNotifications(string token)
	{
		OnRegisteredForPushNotifications (token);
	}

	void onFailedToRegisteredForPushNotifications(string error)
	{
		OnFailedToRegisteredForPushNotifications (error);
	}

	void onPushNotificationsReceived(string payload)
	{
		OnPushNotificationsReceived (payload);
	}

	void OnApplicationPause(bool paused)
	{
		//make sure everything runs smoothly even if pushwoosh is not initialized yet
		if (pushwoosh == null)
			InitPushwoosh();

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
