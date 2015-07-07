using UnityEngine;
using System.Collections;

public class LocalAndroidNotificator : MonoBehaviour 
{
	public UnityEngine.UI.Text uiText;
	
	PushNotificationsAndroid androidPushManager = null;
	
	string notificationText = "Pushwoosh is not initialized";
	
	int[] localNotifications = new int[8];
	
	int counter = 0;
	
	// Use this for initialization
	void Start () 
	{
		Pushwoosh.Instance.OnRegisteredForPushNotifications += onRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += onFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += onPushNotificationsReceived;
	}
	
	void Update()
	{
		// workaround for Windows Phone: set_text can only be called from the main thread.
		uiText.text = notificationText;
		
		counter++;
		
		if (counter == 100) {
#if UNITY_ANDROID && !UNITY_EDITOR
			androidPushManager = Pushwoosh.Instance.AndroidPushNotificationsManager;
			localNotifications[0] = androidPushManager.scheduleLocalNotification ("test1 failed", 1);
			localNotifications[1] = androidPushManager.scheduleLocalNotification ("test2 failed", 2);
			localNotifications[2] = androidPushManager.scheduleLocalNotification ("test3 failed", 3);
			localNotifications[3] = androidPushManager.scheduleLocalNotification ("test4 failed", 4);
			localNotifications[4] = androidPushManager.scheduleLocalNotification ("test5 failed", 5);

			androidPushManager.clearLocalNotifications();

			localNotifications[5] = androidPushManager.scheduleLocalNotification ("test6 passed", 4);
			localNotifications[6] = androidPushManager.scheduleLocalNotification ("test7 failed", 5);
			localNotifications[7] = androidPushManager.scheduleLocalNotification ("test8 failed", 6);

			androidPushManager.clearLocalNotification (localNotifications[6]);
			androidPushManager.clearLocalNotification (localNotifications[7]);
#endif
		}
	}
	
	void onRegisteredForPushNotifications(string token)
	{
		notificationText = "Received token: \n" + token;
		
		//do handling here
		Debug.Log(notificationText);
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
