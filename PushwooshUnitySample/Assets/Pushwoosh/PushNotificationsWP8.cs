using UnityEngine;
using System.Collections.Generic;

public class PushNotificationsWP8: Pushwoosh 
{
#if (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
	private PushwooshForWindowsPhone.Pushwoosh pushwoosh = null;

	void TokenReceived(object sender, PushwooshForWindowsPhone.TokenEventArgs events)
	{
		RegisteredForPushNotifications (events.Token);
	}

	void TokenError(object sender, PushwooshForWindowsPhone.TokenErrorEventArgs events)
	{
		FailedToRegisteredForPushNotifications (events.ErrorMessage);
	}

	void PushReceived(object sender, PushwooshForWindowsPhone.PushEventArgs events)
	{
		PushNotificationsReceived (events.PushPayload);
	}

	// Use this for initialization
	void Start () {
		pushwoosh = new PushwooshForWindowsPhone.Pushwoosh(Pushwoosh.APP_CODE);
		pushwoosh.OnPushTokenReceived += TokenReceived;
		pushwoosh.OnPushTokenFailed += TokenError;
		pushwoosh.OnPushAccepted += PushReceived;

		pushwoosh.SubscribeForPushNotifications ();

		Initialized ();
	}

	public override string HWID
	{
		get { return pushwoosh.HWID; }
	}

	public override string PushToken
	{
		get { return pushwoosh.PushToken; }
	}

	public override void StartTrackingGeoPushes()
	{
		pushwoosh.StartGeoLocation();
	}

	public override void StopTrackingGeoPushes()
	{
		pushwoosh.StopGeoLocation();
	}

	public override void SetIntTag(string tagName, int tagValue)
	{
		var tags = new List<KeyValuePair<string, object>>();
		tags.Add (new KeyValuePair<string, object>(tagName, tagValue));
		pushwoosh.SetTags (tags);
	}
	
	public override void SetStringTag(string tagName, string tagValue)
	{
		var tags = new List<KeyValuePair<string, object>>();
		tags.Add (new KeyValuePair<string, object>(tagName, tagValue));
		pushwoosh.SetTags (tags);
	}
	
	public override void SetListTag(string tagName, List<object> tagValues)
	{
		var tags = new List<KeyValuePair<string, object>>();
		tags.Add (new KeyValuePair<string, object>(tagName, tagValues));
		pushwoosh.SetTags (tags);
	}
#endif
}
