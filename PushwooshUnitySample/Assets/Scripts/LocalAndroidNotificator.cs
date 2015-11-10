using UnityEngine;
using System.Collections;

public class LocalAndroidNotificator : MonoBehaviour 
{
	public UnityEngine.UI.Text uiText;
	
	PushNotificationsAndroid androidPushManager = null;
	
	string notificationText = "Pushwoosh is not initialized";
	
	int[] localNotifications = new int[8];
	
	// Use this for initialization
	void Start () 
	{
		Pushwoosh.Instance.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += onPushNotificationsReceived;
		Pushwoosh.Instance.OnInitialized += OnPushwooshInitialized;

#if UNITY_ANDROID && !UNITY_EDITOR
		androidPushManager = Pushwoosh.Instance as PushNotificationsAndroid;
#endif
	}

	void OnPushwooshInitialized()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		string launchNotification = Pushwoosh.Instance.GetLaunchNotification();
		if (launchNotification == null)
			Debug.Log("No launch notification");
		else
			Debug.Log("Launch notification = " + launchNotification);
#endif
	}

	
	void Update()
	{
		// workaround for Windows Phone: set_text can only be called from the main thread.
		uiText.text = notificationText;
	}
	
	void onRegisteredForPushNotifications(string token)
	{
		notificationText = "Received token: \n" + token;
		
		//do handling here
		Debug.Log(notificationText);

#if UNITY_ANDROID && !UNITY_EDITOR
		localNotifications[0] = androidPushManager.ScheduleLocalNotification ("test1 failed", 1);
		localNotifications[1] = androidPushManager.ScheduleLocalNotification ("test2 failed", 2);
		localNotifications[2] = androidPushManager.ScheduleLocalNotification ("test3 failed", 3);
		localNotifications[3] = androidPushManager.ScheduleLocalNotification ("test4 failed", 4);
		localNotifications[4] = androidPushManager.ScheduleLocalNotification ("test5 failed", 5);
		
		androidPushManager.ClearLocalNotifications();
		
		localNotifications[5] = androidPushManager.ScheduleLocalNotification ("test6 passed", 4);
		localNotifications[6] = androidPushManager.ScheduleLocalNotification ("test7 failed", 5);
		localNotifications[7] = androidPushManager.ScheduleLocalNotification ("test8 failed", 6);
		
		androidPushManager.ClearLocalNotification (localNotifications[6]);
		androidPushManager.ClearLocalNotification (localNotifications[7]);
#endif
	}
	
	void onFailedToRegisteredForPushNotifications(string error)
	{
		notificationText = "Error ocurred while registering to push notifications: \n" + error;
		
		//do handling here
		Debug.Log(notificationText);
	}
	
	void onPushNotificationsReceived(string payload)
	{
		notificationText = "Received push notificaiton: \n" + payload;
		
		//do handling here
		Debug.Log(notificationText);
	}
}
