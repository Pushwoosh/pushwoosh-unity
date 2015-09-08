using UnityEngine;
using System.Collections.Generic;

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
		Pushwoosh.Instance.OnRegisteredForPushNotifications += OnRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += OnFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += OnPushNotificationsReceived;
		Pushwoosh.Instance.OnInitialized += OnPushwooshInitialized;
	}

	void Update()
	{
		// workaround for Windows Phone: set_text can only be called from the main thread.
		uiText.text = notificationText;
	}

	void OnPushwooshInitialized()
	{
		Pushwoosh.Instance.SetStringTag ("UserName", "Alex");
		Pushwoosh.Instance.SetIntTag ("Age", 42);
		Pushwoosh.Instance.SetListTag ("Hobbies", new List<object> (new[] { "Football", "Tennis", "Fishing" }));

		Pushwoosh.Instance.AddBadgeNumber (1);
	}

	void OnRegisteredForPushNotifications(string token)
	{
		notificationText = "Received token: \n" + token;
		
		//do handling here
		Debug.Log(notificationText);
		Debug.Log ("HWID: " + Pushwoosh.Instance.HWID);
		Debug.Log ("PushToken: " + Pushwoosh.Instance.PushToken);
	}
	
	void OnFailedToRegisteredForPushNotifications(string error)
	{
		notificationText = "Error ocurred while registering to push notifications: \n" + error;
		
		//do handling here
		Debug.Log(notificationText);
	}
	
	void OnPushNotificationsReceived(string payload)
	{
		notificationText = "Received push notificaiton: \n" + payload;
		
		//do handling here
		Debug.Log(notificationText);
	}
}
