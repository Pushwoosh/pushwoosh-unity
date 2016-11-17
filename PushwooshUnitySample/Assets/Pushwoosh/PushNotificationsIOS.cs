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
	extern static public void initializePushManager(string appCode, string appName);

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
	extern static public System.IntPtr _getLaunchNotification();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _clearLaunchNotification();
	
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

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _setUserId(string userId);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void _postEvent(string eventId, string attributes);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static public void pw_sendPurchase(string productId, double price, string currency);

	protected override void Initialize () 
	{
		initializePushManager(Pushwoosh.ApplicationCode, Application.productName);
		setListenerName(this.gameObject.name);
	}

	public override void RegisterForPushNotifications()
	{
		registerForRemoteNotifications();
	}

	public override void UnregisterForPushNotifications()
	{
		unregisterForRemoteNotifications();
	}

	public override string HWID
	{
		get { return Marshal.PtrToStringAnsi(_getPushwooshHWID()); }
	}

	public override string PushToken
	{
		get { return Marshal.PtrToStringAnsi(_getPushToken()); }
	}

	public string GetLaunchNotification()
	{
		return Marshal.PtrToStringAnsi(_getLaunchNotification()); 
	}

	public void ClearLaunchNotification()
	{
		_clearLaunchNotification();
	}

	public override void SetUserId(string userId)
	{
		_setUserId(userId);
	}

	protected override void PostEventInternal(string eventId, string attributes)
	{
		_postEvent(eventId, attributes);
	}

	public override void StartTrackingGeoPushes()
	{
		startLocationTracking();
	}

	public override void StopTrackingGeoPushes()
	{
		stopLocationTracking();
	}

	public override void SetListTag(string tagName, List<object> tagValues)
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

	public override void SetIntTag(string tagName, int tagValue)
	{
		setIntTag(tagName, tagValue);
	}
	
	public override void SetStringTag(string tagName, string tagValue)
	{
		setStringTag(tagName, tagValue);
	}

	public override void ClearNotificationCenter()
	{
		clearNotificationCenter ();
	}

	public override void SetBadgeNumber(int number)
	{
		setBadgeNumber (number);
	}

	public override void AddBadgeNumber(int deltaBadge)
	{
		addBadgeNumber (deltaBadge);
	}

	public override void SendPurchase(string productId, double price, string currency)
	{
		pw_sendPurchase(productId, price, currency);
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
