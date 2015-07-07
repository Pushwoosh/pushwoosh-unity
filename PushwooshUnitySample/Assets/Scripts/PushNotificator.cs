using UnityEngine;
using System.Collections;

/// <summary>
///  Pushwoosh sample class
/// </summary>
public class PushNotificator : MonoBehaviour 
{

	public UnityEngine.UI.Text uiText;

	PushNotificationsAndroid androidPushManager = null;

	string notificationText = "Pushwoosh is not initialized";

	int[] localNotifications = new int[20];

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
			Debug.Log("Android");
			androidPushManager = Pushwoosh.Instance.AndroidPushNotificationsManager;
			localNotifications[0] = androidPushManager.scheduleLocalNotification ("notification1", 1);
			localNotifications[1] = androidPushManager.scheduleLocalNotification ("notification2", 2);
//			localNotifications[2] = androidPushManager.scheduleLocalNotification ("notification3", 3);
//			localNotifications[3] = androidPushManager.scheduleLocalNotification ("notification4", 4);
//			localNotifications[4] = androidPushManager.scheduleLocalNotification ("notification5", 5);
			
//			localNotifications[5] = androidPushManager.scheduleLocalNotification ("notification6", 6);
//			localNotifications[6] = androidPushManager.scheduleLocalNotification ("notification7", 7);
//			localNotifications[7] = androidPushManager.scheduleLocalNotification ("notification8", 8);
//			localNotifications[8] = androidPushManager.scheduleLocalNotification ("notification9", 9);
//			localNotifications[9] = androidPushManager.scheduleLocalNotification ("notification10", 10);
			
//			androidPushManager.clearLocalNotification (localNotifications[0]);
			androidPushManager.clearLocalNotification (localNotifications[1]);
//			androidPushManager.clearLocalNotification (localNotifications[2]);
//			androidPushManager.clearLocalNotification (localNotifications[3]);
//			androidPushManager.clearLocalNotification (localNotifications[4]);
			#endif
		}
#if UNITY_ANDROID && !UNITY_EDITOR
//		if (counter > 500)
//			androidPushManager.clearLocalNotification (localNotifications[5]);
#endif
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
