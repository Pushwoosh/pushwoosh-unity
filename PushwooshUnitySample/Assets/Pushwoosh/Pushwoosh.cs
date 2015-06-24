using UnityEngine;
using System.Collections;

public delegate void RegisteredForPushNotificationsHandler(string payload);

public delegate void FailedToRegisteredForPushNotificationsHandler(string payload);

public delegate void PushNotificationsReceivedHandler(string payload);

public class Pushwoosh : SingletonBase<Pushwoosh> {

	public const string APP_CODE = "EA75E-CE4BD";

	public const string GCM_PROJECT_NUMBER = "611761906259";


	public event RegisteredForPushNotificationsHandler OnRegisteredForPushNotifications = delegate {};
	
	public event FailedToRegisteredForPushNotificationsHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event PushNotificationsReceivedHandler OnPushNotificationsReceived = delegate {};

	// singleton
	protected Pushwoosh() {}

#if UNITY_IPHONE && !UNITY_EDITOR
	PushNotificationsIOS pushNotificationsIOS;

	public PushNotificationsIOS IOSPushNotificationsManager {
		get { return pushNotificationsIOS; } 
	}

#elif UNITY_ANDROID && !UNITY_EDITOR

	PushNotificationsAndroid pushNotificationsAndroid;
	
	public PushNotificationsAndroid AndroidPushNotificationsManager {
		get { return pushNotificationsAndroid; } 
	}

#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
	PushNotificationsWP8 pushNotificationsWP8;
	
	public PushNotificationsWP8 WP8PushNotificationsManager {
		get { return pushNotificationsWP8; } 
	}
#endif

	void Start () {
#if UNITY_IPHONE && !UNITY_EDITOR
		gameObject.AddComponent <PushNotificationsIOS>();
		pushNotificationsIOS = gameObject.GetComponent<PushNotificationsIOS>();
		pushNotificationsIOS.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsIOS.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsIOS.OnPushNotificationsReceived += onPushNotificationsReceived;
#elif UNITY_ANDROID && !UNITY_EDITOR
		gameObject.AddComponent <PushNotificationsAndroid>();
		pushNotificationsAndroid = gameObject.GetComponent<PushNotificationsAndroid>();
		pushNotificationsAndroid.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsAndroid.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsAndroid.OnPushNotificationsReceived += onPushNotificationsReceived;
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
		gameObject.AddComponent <PushNotificationsWP8>();
		pushNotificationsWP8 = gameObject.GetComponent<PushNotificationsWP8>();
		pushNotificationsWP8.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		pushNotificationsWP8.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		pushNotificationsWP8.OnPushNotificationsReceived += onPushNotificationsReceived;
#endif
	}
	
	// propagate events
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
}
