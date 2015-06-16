using UnityEngine;
using System.Collections;

public class PushwooshWP8 : MonoBehaviour {

	private PushwooshForWindowsPhone.Pushwoosh pushwoosh = null;

	string token = "";

	void TokenReceived(object sender, PushwooshForWindowsPhone.TokenEventArgs events)
	{
		token += events.Token + "\n";
	}

	void TokenError(object sender, PushwooshForWindowsPhone.TokenErrorEventArgs events)
	{
		token += events.ErrorMessage + "\n";
	}

	void PushReceived(object sender, PushwooshForWindowsPhone.PushEventArgs events)
	{
		token += events.PushPayload + "\n";
	}

	// Use this for initialization
	void Start () {
		pushwoosh = new PushwooshForWindowsPhone.Pushwoosh("3A43A-A3EAB");
		pushwoosh.OnPushTokenReceived += TokenReceived;
		pushwoosh.OnPushTokenFailed += TokenError;
		pushwoosh.OnPushAccepted += PushReceived;

		pushwoosh.SubscribeForPushNotifications ();
	}

	void OnGUI () {
		Rect screen = new Rect (0, 0, Screen.width, Screen.height);
		GUIStyle style = new GUIStyle { fontSize = 32, alignment = TextAnchor.MiddleCenter };

		//string token = pushwoosh.PushToken;

		GUI.Label (screen, token, style);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
