using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  Pushwoosh sample class
/// </summary>
public class PushNotificator : MonoBehaviour 
{
	public UnityEngine.UI.Text hwidUIText;
	string hwidString = "null";
	public UnityEngine.UI.Text tokenUIText;
	string tokenString = "Unsubscribed";
	public UnityEngine.UI.Text notificationUIText;
	string notificationString = "{}";
	public UnityEngine.UI.Text launchNotificationUIText;
	string launchNotificationString = "{}";

	public GameObject AndroidSpecific;

	// Use this for initialization
	void Start () 
	{
		Pushwoosh.ApplicationCode = "4FC89B6D14A655.46488481";
		Pushwoosh.GcmProjectNumber = "60756016005";
		Pushwoosh.Instance.OnRegisteredForPushNotifications += OnRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += OnFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += OnPushNotificationsReceived;

		Pushwoosh.Instance.SetStringTag ("UserName", "Alex");
		Pushwoosh.Instance.SetIntTag ("Age", 42);
		Pushwoosh.Instance.SetListTag ("Hobbies", new List<object> (new[] { "Football", "Tennis", "Fishing" }));

		Pushwoosh.Instance.SetBadgeNumber (0);

		Pushwoosh.Instance.SendPurchase("com.pushwoosh.Developer", 49.95, "USD");

#if !UNITY_EDITOR
#if UNITY_IOS || UNITY_ANDROID

		Pushwoosh.Instance.SetUserId("%userId%");

		Dictionary<string, object> attributes = new Dictionary<string, object>() {
			{ "attribute", "value" },
		};

		Pushwoosh.Instance.PostEvent("applicationOpened", attributes);

		AndroidSpecific.SetActive (true);
		string launchNotification = Pushwoosh.Instance.GetLaunchNotification();
		if (launchNotification == null)
			launchNotificationString = "No launch notification";
		else
			launchNotificationString = launchNotification;
#endif

#if UNITY_ANDROID
		Dictionary<string, string> parameters = new Dictionary<string, string>() {
			{ "l", "https://www.pushwoosh.com/" },
			{ "u", "custom data" }
		};

		Pushwoosh.Instance.ScheduleLocalNotification ("Hello, Android!", 5, parameters);
#endif
#endif
	}

	void Update()
	{
		tokenUIText.text = tokenString;
		hwidUIText.text = hwidString;
		notificationUIText.text = notificationString;
		launchNotificationUIText.text = launchNotificationString;
	}

	public void OnSubscribe()
	{
		Pushwoosh.Instance.RegisterForPushNotifications ();
	}

	public void OnUnsubscribe()
	{
		tokenString = "Unsubscribed";
		Pushwoosh.Instance.UnregisterForPushNotifications ();
	}

	void OnRegisteredForPushNotifications(string token)
	{
		tokenString = token;
		hwidString = Pushwoosh.Instance.HWID;

		Debug.Log(token);
		Debug.Log ("HWID: " + Pushwoosh.Instance.HWID);
		Debug.Log ("PushToken: " + Pushwoosh.Instance.PushToken);

		Pushwoosh.Instance.GetTags((IDictionary<string, object> tags, PushwooshException error) => {
			string json = PushwooshUtils.DictionaryToJson(tags);
			Debug.Log("Tags: " + json);
		});
	}

	void OnFailedToRegisteredForPushNotifications(string error)
	{
		tokenString = "Error ocurred while registering to push notifications: \n" + error;

		Debug.Log(tokenString);
	}
	
	void OnPushNotificationsReceived(string payload)
	{
		notificationString = payload;

		Debug.Log(notificationString);
	}
}
