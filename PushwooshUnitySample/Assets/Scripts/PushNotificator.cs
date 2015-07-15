using UnityEngine;
using System.Collections;

/// <summary>
///  Pushwoosh sample class
/// </summary>
public class PushNotificator : MonoBehaviour 
{
	public UnityEngine.UI.Text uiText;

	string notificationText = "Pushwoosh is not initialized";

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
