using UnityEngine;
using System.Collections.Generic;

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

	public delegate void InitializationHandler();


	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event NotificationHandler OnPushNotificationsReceived = delegate {};

	public event InitializationHandler OnInitialized = delegate {};


	public virtual string HWID
	{
		get 
		{
			UnsupportedPlatform ();
			return "Unsupported platform"; 
		}
	}

	public virtual string PushToken
	{
		get 
		{ 
			UnsupportedPlatform ();
			return "Unsupported platform"; 
		}
	}

	public virtual void StartTrackingGeoPushes()
	{
		UnsupportedPlatform ();
	}

	public virtual void StopTrackingGeoPushes()
	{
		UnsupportedPlatform ();
	}

	public virtual void SetIntTag(string tagName, int tagValue)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void SetStringTag(string tagName, string tagValue)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void SetListTag(string tagName, List<object> tagValues)
	{
		UnsupportedPlatform ();
	}

	public virtual void ClearNotificationCenter()
	{
		UnsupportedPlatform ();
	}

	public virtual void SetBadgeNumber(int number)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void AddBadgeNumber(int deltaBadge)
	{
		UnsupportedPlatform ();
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

	protected void Initialized()
	{
		OnInitialized ();
	}

	private void UnsupportedPlatform()
	{
		var frame = new System.Diagnostics.StackFrame(1);
		var method = frame.GetMethod();
		string methodName = method.Name;
		Debug.Log ("[Pushwoosh] Error: " + methodName + " is not supported on this platform");
	}
}
