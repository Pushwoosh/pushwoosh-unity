using UnityEngine;
using System.Collections;

#if UNITY_IPHONE && !UNITY_EDITOR
public class Pushwoosh : SingletonBase<PushNotificationsIOS> 
#elif UNITY_ANDROID && !UNITY_EDITOR
public class Pushwoosh : SingletonBase<PushNotificationsAndroid> 
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
public class Pushwoosh : SingletonBase<PushNotificationsWP8> 
#else 
public class Pushwoosh : SingletonBase<Pushwoosh> 
#endif
{
	public const string APP_CODE = "ENTER_PUSHWOOSH_APP_ID_HERE";
 
	public const string GCM_PROJECT_NUMBER = "ENTER_GOOGLE_PROJECT_NUMBER_HERE";


	public delegate void RegistrationSuccessHandler(string token);
	
	public delegate void RegistrationErrorHandler(string error);
	
	public delegate void NotificationHandler(string payload);


	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event NotificationHandler OnPushNotificationsReceived = delegate {};

	public virtual string HWID
	{
		get 
		{
			Debug.Log("Error: Pushwoosh.HWID is not supported on this platform");
			return "Unsupported platform"; 
		}
	}

	public virtual string PushToken
	{
		get 
		{ 
			Debug.Log("Error: Pushwoosh.PushToken is not supported on this platform");
			return "Unsupported platform"; 
		}
	}

	public virtual void startTrackingGeoPushes()
	{
		Debug.Log("Error: Pushwoosh.startTrackingGeoPushes() is not supported on this platform");
	}

	public virtual void stopTrackingGeoPushes()
	{
		Debug.Log("Error: Pushwoosh.stopTrackingGeoPushes() is not supported on this platform");
	}

	// singleton
	protected Pushwoosh() {}

	protected void RegisteredForPushNotifications(string token)
	{
		OnRegisteredForPushNotifications(token);
	}

	protected void FailedToRegisteredForPushNotifications(string error)
	{
		OnFailedToRegisteredForPushNotifications(error);
	}

	protected void PushNotificationsReceived(string payload)
	{
		OnPushNotificationsReceived(payload);
	}
}
