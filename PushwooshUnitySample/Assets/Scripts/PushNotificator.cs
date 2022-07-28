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

	public UnityEngine.UI.Text postEventKeyUIText;
	public UnityEngine.UI.Text postEventAttributeUIText;
	public UnityEngine.UI.Button sendEventButton;

	public GameObject AndroidSpecific;

	// Use this for initialization
	void Start () 
	{
		Pushwoosh.ApplicationCode = "7559E-DE020";
		Pushwoosh.FcmProjectNumber = "747084596472";
		Pushwoosh.Instance.OnRegisteredForPushNotifications += OnRegisteredForPushNotifications;
		Pushwoosh.Instance.OnFailedToRegisteredForPushNotifications += OnFailedToRegisteredForPushNotifications;
		Pushwoosh.Instance.OnPushNotificationsReceived += OnPushNotificationsReceived;
		Pushwoosh.Instance.OnPushNotificationsOpened += OnPushNotificationsOpened;

		Pushwoosh.Instance.SetStringTag ("UserName", "Alex");
		Pushwoosh.Instance.SetIntTag ("Age", 42);
		Pushwoosh.Instance.SetListTag ("Hobbies", new List<object> (new[] { "Football", "Tennis", "Fishing" }));

		Pushwoosh.Instance.SetBadgeNumber (0);

		Pushwoosh.Instance.SendPurchase("com.pushwoosh.Developer", 49.95, "USD");
        NotificationSettings notificationSettings = Pushwoosh.Instance.GetRemoteNotificationStatus();
        if (notificationSettings != null) { 
            Debug.Log("Notification status enabled: " + notificationSettings.enabled
#if UNITY_IPHONE && !UNITY_EDITOR
                      + " alert: " + notificationSettings.pushAlert
                      + " badge: " + notificationSettings.pushBadge
                      + " sound: " + notificationSettings.pushSound
#endif
                     );
        }
		Pushwoosh.Instance.SetUserId("%userId%");

		Pushwoosh.Instance.SetUser("666777", new List<string> (new[] { "test1@test.com", "test2@test.com" }));

		Pushwoosh.Instance.SetEmails(new List<string> (new[] { "test3@test.com", "test4@test.com" }));

		Pushwoosh.Instance.SetEmail("test5@test.com");

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

#if UNITY_ANDROID
		Dictionary<string, string> parameters = new Dictionary<string, string>() {
			{ "l", "https://www.pushwoosh.com/" },
			{ "u", "custom data" }
		};

		Pushwoosh.Instance.ScheduleLocalNotification ("Hello, Android!", 5, parameters);
        Pushwoosh.Instance.SetNotificationChannelDelegate(new MyNotificationChannelDelegate());
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

    public void OnStartLocationTracking()
    {
        Pushwoosh.Instance.StartTrackingGeoPushes();
    }

    public void OnStopLocationTracking()
    {
        Pushwoosh.Instance.StopTrackingGeoPushes();
    }


    public void OnSendPostEvent()
	{
		Debug.Log ("On Send post event key: " + postEventKeyUIText.text + "; attribute: " +postEventAttributeUIText.text );
		Dictionary<string, object> parameters = new Dictionary<string, object> ();
		parameters.Add (postEventKeyUIText.text, postEventAttributeUIText.text);
		Pushwoosh.Instance.PostEvent (postEventKeyUIText.text, parameters);
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
        Debug.Log(Pushwoosh.Instance.GetRemoteNotificationStatus());
	}

	void OnFailedToRegisteredForPushNotifications(string error)
	{
		tokenString = "Error ocurred while registering to push notifications: \n" + error;

		Debug.Log(tokenString);
	}
	
	void OnPushNotificationsReceived(string payload)
	{
		notificationString = "NotificationReceived: " + payload;

		Debug.Log ("NotificationReceived: " + payload);
	}

	void OnPushNotificationsOpened(string payload)
	{
		notificationString = "NotificationOpened: " + payload;
		Debug.Log ("NotificationOpened: " + payload);
	}

    private class MyNotificationChannelDelegate : NotificationChannelDelegate
    {
        public override string ChannelDescription(string channelName)
        {
            // Implement your channel description customization logic here
            return "";
        }

        public override string ChannelName(string channelName)
        {
            // Implement your channel name customization logic here
            return channelName;
        }
    }
}
