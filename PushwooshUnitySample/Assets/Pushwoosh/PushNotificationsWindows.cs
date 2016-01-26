using UnityEngine;
using System.Collections.Generic;

#if (UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0) && !UNITY_EDITOR
using PushwooshPlugin = PushwooshForWindows;
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
using PushwooshPlugin = PushwooshForWindowsPhone;
#endif

public class PushNotificationsWindows: Pushwoosh 
{
#if (UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0) && !UNITY_EDITOR
	private PushwooshPlugin.Pushwoosh pushwoosh = null;

	void TokenReceived(object sender, PushwooshPlugin.TokenEventArgs events)
	{
		RegisteredForPushNotifications (events.Token);
	}

	void TokenError(object sender, PushwooshPlugin.TokenErrorEventArgs events)
	{
		FailedToRegisteredForPushNotifications (events.ErrorMessage);
	}

	void PushReceived(object sender, PushwooshPlugin.PushEventArgs events)
	{
		PushNotificationsReceived (events.PushPayload);
	}

	protected override void Initialize ()
	{
		pushwoosh = new PushwooshPlugin.Pushwoosh(Pushwoosh.ApplicationCode);
		pushwoosh.OnPushTokenReceived += TokenReceived;
		pushwoosh.OnPushTokenFailed += TokenError;
		pushwoosh.OnPushAccepted += PushReceived;
	}

	public override void RegisterForPushNotifications()
	{
		pushwoosh.SubscribeForPushNotifications ();
	}

	public override void UnregisterForPushNotifications()
	{
		pushwoosh.UnsubscribeForPushNotifications ();
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
