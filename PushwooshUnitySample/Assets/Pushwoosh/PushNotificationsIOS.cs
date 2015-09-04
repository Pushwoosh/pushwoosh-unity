using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class PushNotificationsIOS : Pushwoosh 
{
#if UNITY_IPHONE && !UNITY_EDITOR
	//
	//private methods
	//
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void internalSendStringTags (string tagName, string[] tags);

	//
	//public
	//

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void registerForRemoteNotifications();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void unregisterForRemoteNotifications();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void setListenerName(string listenerName);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public System.IntPtr _getPushToken();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public System.IntPtr _getPushwooshHWID();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void setIntTag(string tagName, int tagValue);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void setStringTag(string tagName, string tagValue);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void startLocationTracking();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void stopLocationTracking();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void clearNotificationCenter();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void setBadgeNumber(int badge);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void addBadgeNumber(int deltaBadge);

	static public void setListTag(string tagName, List<object> tagValues)
	{
		List <string> stringTags = new List<string>();

		foreach (object tagValue in tagValues) {
			string stringTag = tagValue.ToString();

			if (stringTag != null)
				stringTags.Add(stringTag);
		}

		string[] array = stringTags.ToArray();

		internalSendStringTags (tagName, array);
	}

	// Use this for initialization
	void Start () {
		registerForRemoteNotifications();
		setListenerName(this.gameObject.name);
		Debug.Log(PushToken);
	}

	public override string HWID
	{
		get { return Marshal.PtrToStringAnsi(_getPushwooshHWID()); }
	}

	public override string PushToken
	{
		get { return Marshal.PtrToStringAnsi(_getPushToken()); }
	}

	public override void startTrackingGeoPushes()
	{
		startLocationTracking();
	}

	public override void stopTrackingGeoPushes()
	{
		stopLocationTracking();
	}

	static public void setBadge(int number)
	{
		setBadgeNumber (number);
	}

	static public void addBadge(int deltaBadge)
	{
		addBadgeNumber (deltaBadge);
	}

	void onRegisteredForPushNotifications(string token)
	{
		RegisteredForPushNotifications (token);
	}

	void onFailedToRegisteredForPushNotifications(string error)
	{
		FailedToRegisteredForPushNotifications (error);
	}

	void onPushNotificationsReceived(string payload)
	{
		PushNotificationsReceived (payload);
	}
#endif
}
