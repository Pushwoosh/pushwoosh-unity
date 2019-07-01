using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class PushNotificationsIOS : Pushwoosh
{
#if UNITY_IPHONE && !UNITY_EDITOR

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_internalSendStringTags (string tagName, string[] tags);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_initializePushManager(string appCode, string appName);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_registerForRemoteNotifications();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_unregisterForRemoteNotifications();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setListenerName(string listenerName);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private System.IntPtr pw_getPushToken();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private System.IntPtr pw_getPushwooshHWID();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private System.IntPtr pw_getLaunchNotification();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_clearLaunchNotification();

    [System.Runtime.InteropServices.DllImport("__Internal")]
    extern static private System.IntPtr pw_getRemoteNotificationStatus();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setIntTag(string tagName, int tagValue);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setStringTag(string tagName, string tagValue);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_getTags();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_startLocationTracking();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_stopLocationTracking();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_clearNotificationCenter();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setBadgeNumber(int badge);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_addBadgeNumber(int deltaBadge);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setUserId(string userId);


	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private bool pw_gdprAvailable();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private bool pw_isCommunicationEnabled();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private bool pw_isDeviceDataRemoved();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_setCommunicationEnabled(bool enabled);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_removeAllDeviceData();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_showGDPRConsentUI();

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_showGDPRDeletionUI();


	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_postEvent(string eventId, string attributes);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	extern static private void pw_sendPurchase(string productId, double price, string currency);

	private Queue<GetTagsHandler> tagsHandlers = new Queue<GetTagsHandler>();

	protected override void Initialize () 
	{
		pw_initializePushManager(Pushwoosh.ApplicationCode, null);
		pw_setListenerName(this.gameObject.name);
	}

	public override void RegisterForPushNotifications()
	{
		pw_registerForRemoteNotifications();
	}

	public override void UnregisterForPushNotifications()
	{
		pw_unregisterForRemoteNotifications();
	}

	public override string HWID
	{
		get { return Marshal.PtrToStringAnsi(pw_getPushwooshHWID()); }
	}

	public override string PushToken
	{
		get { return Marshal.PtrToStringAnsi(pw_getPushToken()); }
	}

	public override string GetLaunchNotification()
	{
		return Marshal.PtrToStringAnsi(pw_getLaunchNotification()); 
	}

	public override void ClearLaunchNotification()
	{
		pw_clearLaunchNotification();
	}

    public override NotificationSettings GetRemoteNotificationStatus()
    {
        string jsonSettings = Marshal.PtrToStringAnsi(pw_getRemoteNotificationStatus());
        return JsonUtility.FromJson<NotificationSettings>(jsonSettings);
    }

	public override void SetUserId(string userId)
	{
		pw_setUserId(userId);
	}

	protected override void PostEventInternal(string eventId, string attributes)
	{
		pw_postEvent(eventId, attributes);
	}

	public override void StartTrackingGeoPushes()
	{
		pw_startLocationTracking();
	}

	public override void StopTrackingGeoPushes()
	{
		pw_stopLocationTracking();
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
		
		pw_internalSendStringTags (tagName, array);
	}

	public override void SetIntTag(string tagName, int tagValue)
	{
		pw_setIntTag(tagName, tagValue);
	}
	
	public override void SetStringTag(string tagName, string tagValue)
	{
		pw_setStringTag(tagName, tagValue);
	}

	public override void GetTags(GetTagsHandler handler)
	{
		tagsHandlers.Enqueue(handler);
		pw_getTags();
	}

	public override void ClearNotificationCenter()
	{
		pw_clearNotificationCenter ();
	}

	public override void SetBadgeNumber(int number)
	{
		pw_setBadgeNumber (number);
	}

	public override void AddBadgeNumber(int deltaBadge)
	{
		pw_addBadgeNumber (deltaBadge);
	}

	public override void SendPurchase(string productId, double price, string currency)
	{
		pw_sendPurchase(productId, price, currency);
	}

	public override bool IsGDPRAvailable ()
	{
		return pw_gdprAvailable();
	}

	public override bool IsCommunicationEnabled ()
	{
		return pw_isCommunicationEnabled();
	}

	public override bool isDeviceDataRemoved ()
	{
		return pw_isDeviceDataRemoved();
	}

	public override void SetCommunicationEnabled(bool enable) {
		pw_setCommunicationEnabled(enabled);
	}

	public override void RemoveAllDeviceData() {
		pw_removeAllDeviceData();
	}

	public override void ShowGDPRConsentUI ()
	{
		pw_showGDPRConsentUI();
	}

	public override void ShowGDPRDeletionUI ()
	{
		pw_showGDPRDeletionUI();
	}

	void onSetCommunicationEnabled(string sucess)
	{
		SetCommunicationEnableCallBack();
	}

	void onFailSetCommunicationEnabled(string error) {
		FailedSetCommunicationEnableCallback(error);
	}

	void onRemoveAllDeviceData(string sucess)
	{
		RemoveAllDataCallBack();
	}

	void onFailRemoveAllDeviceData(string error)
	{
		FailedRemoveAllDataCallback(error);
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

	void onPushNotificationsOpened(string payload)
	{
		PushNotificationsOpened (payload);
	}

	void onTagsReceived(string json)
	{
		GetTagsHandler handler = tagsHandlers.Dequeue();
		if (handler != null) {
			try {
				IDictionary<string, object> tags = PushwooshUtils.JsonToDictionary(json);
				handler(tags, null);
			}
			catch(Exception e) {
				Debug.Log ("Invalid tags: " + e.ToString());
				handler (null, new PushwooshException(e.Message));
			}
		}
	}

	void onFailedToReceiveTags(string error)
	{
		GetTagsHandler handler = tagsHandlers.Dequeue();
		if (handler != null) {
			handler(null, new PushwooshException(error));
		}
	}
#endif
}
