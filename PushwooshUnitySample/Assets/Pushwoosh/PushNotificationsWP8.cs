using UnityEngine;
using System.Collections;

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
	}

	public override string HWID
	{
		get { return pushwoosh.HWID; }
	}

	public override string PushToken
	{
		get { return pushwoosh.PushToken; }
	}

	public override void startTrackingGeoPushes()
	{
		pushwoosh.StartGeoLocation();
	}

	public override void stopTrackingGeoPushes()
	{
		pushwoosh.StopGeoLocation();
	}


#endif
}
