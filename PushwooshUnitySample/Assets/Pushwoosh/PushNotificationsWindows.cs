using UnityEngine;
using System.Collections.Generic;

public class PushNotificationsWindows: Pushwoosh 
{
#if (UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0) && !UNITY_EDITOR
	private PushwooshForWindows.Pushwoosh pushwoosh = null;

	void TokenReceived(object sender, PushwooshForWindows.TokenEventArgs events)
	{
		RegisteredForPushNotifications (events.Token);
	}

	void TokenError(object sender, PushwooshForWindows.TokenErrorEventArgs events)
	{
		FailedToRegisteredForPushNotifications (events.ErrorMessage);
	}

	void PushReceived(object sender, PushwooshForWindows.PushEventArgs events)
	{
		PushNotificationsReceived (events.PushPayload);
	}

	// Use this for initialization
	void Start () {
		pushwoosh = new PushwooshForWindows.Pushwoosh(Pushwoosh.APP_CODE);
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
