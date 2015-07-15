using UnityEngine;
using System.Collections;

public class PushNotificationsWP8: MonoBehaviour {

#if (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
	public event Pushwoosh.RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event Pushwoosh.NotificationHandler OnPushNotificationsReceived = delegate {};

	private PushwooshForWindowsPhone.Pushwoosh pushwoosh = null;

	void TokenReceived(object sender, PushwooshForWindowsPhone.TokenEventArgs events)
	{
		OnRegisteredForPushNotifications (events.Token);
	}

	void TokenError(object sender, PushwooshForWindowsPhone.TokenErrorEventArgs events)
	{
		OnFailedToRegisteredForPushNotifications (events.ErrorMessage);
	}

	void PushReceived(object sender, PushwooshForWindowsPhone.PushEventArgs events)
	{
		OnPushNotificationsReceived (events.PushPayload);
	}

	// Use this for initialization
	void Start () {
		pushwoosh = new PushwooshForWindowsPhone.Pushwoosh(Pushwoosh.APP_CODE);
		pushwoosh.OnPushTokenReceived += TokenReceived;
		pushwoosh.OnPushTokenFailed += TokenError;
		pushwoosh.OnPushAccepted += PushReceived;

		pushwoosh.SubscribeForPushNotifications ();
	}
#endif
}
