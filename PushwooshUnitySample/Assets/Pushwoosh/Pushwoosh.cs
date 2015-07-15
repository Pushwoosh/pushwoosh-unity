using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PushNotificationsAndroid))]
[RequireComponent (typeof (PushNotificationsIOS))]
[RequireComponent (typeof (PushNotificationsWP8))]
public class Pushwoosh : SingletonBase<Pushwoosh> 
{
	public const string APP_CODE = "ENTER_PUSHWOOSH_APP_ID_HERE";

	public const string GCM_PROJECT_NUMBER = "ENTER_GOOGLE_PROJECT_NUMBER_HERE";


	public delegate void RegistrationSuccessHandler(string payload);
	
	public delegate void RegistrationErrorHandler(string error);
	
	public delegate void NotificationHandler(string payload);


	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event NotificationHandler OnPushNotificationsReceived = delegate {};


	// singleton
	protected Pushwoosh() {}

#if UNITY_IPHONE && !UNITY_EDITOR
	public PushNotificationsIOS IOSPushNotificationsManager 
	{
		get { return gameObject.GetComponent<PushNotificationsIOS>(); } 
	}

#elif UNITY_ANDROID && !UNITY_EDITOR
	public PushNotificationsAndroid AndroidPushNotificationsManager {
		get { return gameObject.GetComponent<PushNotificationsAndroid>(); } 
	}

#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
	public PushNotificationsWP8 WP8PushNotificationsManager {
		get { return gameObject.GetComponent<PushNotificationsWP8>(); } 
	}
#endif

	void Start () 
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		PushNotificationsIOS pushNotificationsIOS = IOSPushNotificationsManager;
		pushNotificationsIOS.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsIOS.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsIOS.OnPushNotificationsReceived += onPushNotificationsReceived;
#elif UNITY_ANDROID && !UNITY_EDITOR
		PushNotificationsAndroid pushNotificationsAndroid = AndroidPushNotificationsManager;
		pushNotificationsAndroid.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsAndroid.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsAndroid.OnPushNotificationsReceived += onPushNotificationsReceived;
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
		PushNotificationsWP8 pushNotificationsWP8 = WP8PushNotificationsManager;
		pushNotificationsWP8.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsWP8.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsWP8.OnPushNotificationsReceived += onPushNotificationsReceived;
#endif
	}
	
	// propagate events
	void onRegisteredForPushNotifications(string token)
	{
		OnRegisteredForPushNotifications (token);

		// dispatch only once
#if UNITY_IPHONE && !UNITY_EDITOR
		IOSPushNotificationsManager.OnRegisteredForPushNotifications -= onRegisteredForPushNotifications;
#elif UNITY_ANDROID && !UNITY_EDITOR
		AndroidPushNotificationsManager.OnRegisteredForPushNotifications -= onRegisteredForPushNotifications;
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
		WP8PushNotificationsManager.OnRegisteredForPushNotifications -= onRegisteredForPushNotifications;
#endif
	}
	
	void onFailedToRegisteredForPushNotifications(string error)
	{
		OnFailedToRegisteredForPushNotifications (error);
	}
	
	void onPushNotificationsReceived(string payload)
	{
		OnPushNotificationsReceived (payload);
	}
}
