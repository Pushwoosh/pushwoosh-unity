using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Pushwoosh : MonoBehaviour
{
	public static string ApplicationCode { get; set; }

	public static string FcmProjectNumber { get; set; }

    public static string GcmProjectNumber {
        get { return FcmProjectNumber; }
        set { FcmProjectNumber = value; }
    }

	public delegate void RegistrationSuccessHandler(string token);
	public delegate void RegistrationErrorHandler(string error);
    public delegate void CommunicationHandler(string enabled);
	public delegate void NotificationHandler(string payload);
	public delegate void GetTagsHandler(IDictionary<string, object> tags, PushwooshException error);
	public delegate void PurchaseSuccessHandler(string identifier);
	public delegate void PromotedPurchaseSuccessHandler(string identifier);
	public delegate void CompletedTransactionsFailedHandler(string error);
	public delegate void PaymentFailedProductIdentifierHandler(string error);
	public delegate void PurchaseHelperProductsHandler(string identifiers);

	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	public event NotificationHandler OnPushNotificationsReceived = delegate {};
	public event NotificationHandler OnPushNotificationsOpened = delegate {};
    public event CommunicationHandler OnSetCommunicationEnabled = delegate { };
	public event PurchaseSuccessHandler OnPWInAppPurchaseHelperPaymentComplete = delegate { };
	public event PromotedPurchaseSuccessHandler OnPWInAppPurchaseHelperCallPromotedPurchase = delegate { };
	public event CompletedTransactionsFailedHandler OnPWInAppPurchaseHelperRestoreCompletedTransactionsFailed = delegate { };
	public event PaymentFailedProductIdentifierHandler OnPWInAppPurchaseHelperPaymentFailedProductIdentifier = delegate { };
	public event PurchaseHelperProductsHandler OnPWInAppPurchaseHelperProducts = delegate { };

	public virtual string HWID
	{
		get
		{
			Debug.Log ("[Pushwoosh] Error: HWID is not supported on this platform");
			return "Unsupported platform";
		}
	}

	public virtual string PushToken
	{
		get
		{
			Debug.Log ("[Pushwoosh] Error: PushToken is not supported on this platform");
			return "Unsupported platform";
		}
	}

	public virtual void RegisterForPushNotifications()
	{
		Debug.Log ("[Pushwoosh] Error: RegisterForPushNotifications is not supported on this platform");
	}

	public virtual void UnregisterForPushNotifications()
	{
		Debug.Log ("[Pushwoosh] Error: UnregisterForPushNotifications is not supported on this platform");
	}

	public virtual void RegisterExistingToken(string token)
	{
        Debug.Log("[Pushwoosh] Error: RegisterExistingToken is not supported on this platform");
    }

    public virtual void StartTrackingGeoPushes()
	{
		Debug.Log ("[Pushwoosh] Error: StartTrackingGeoPushes is not supported on this platform");
	}

	public virtual void StopTrackingGeoPushes()
	{
		Debug.Log ("[Pushwoosh] Error: StopTrackingGeoPushes is not supported on this platform");
	}

	public virtual void SetIntTag(string tagName, int tagValue)
	{
		Debug.Log ("[Pushwoosh] Error: SetIntTag is not supported on this platform");
	}

	public virtual void SetStringTag(string tagName, string tagValue)
	{
		Debug.Log ("[Pushwoosh] Error: SetStringTag is not supported on this platform");
	}

	public virtual void SetListTag(string tagName, List<object> tagValues)
	{
		Debug.Log ("[Pushwoosh] Error: SetListTag is not supported on this platform");
	}

	public virtual void GetTags(GetTagsHandler handler)
	{
		Debug.Log ("[Pushwoosh] Error: GetTags() is not supported on this platform");
	}

	public virtual void ClearNotificationCenter()
	{
		Debug.Log ("[Pushwoosh] Error: ClearNotificationCenter is not supported on this platform");
	}

    public virtual NotificationSettings GetRemoteNotificationStatus()
    {
        Debug.Log ("[Pushwoosh] Error: GetRemoteNotificationStatus is not supported on this platform");
        return null;
    }

	public virtual void SetBadgeNumber(int number)
	{
		Debug.Log ("[Pushwoosh] Error: SetBadgeNumber is not supported on this platform");
	}

	public virtual void AddBadgeNumber(int deltaBadge)
	{
		Debug.Log ("[Pushwoosh] Error: AddBadgeNumber is not supported on this platform");
	}

	public virtual void SetUserId(string userId)
	{
		Debug.Log ("[Pushwoosh] Error: SetUserId is not supported on this platform");
	}

	public virtual void SetUser(string userId, List<string> emails)
	{
		Debug.Log ("[Pushwoosh] Error: SetUser is not supported on this platform");
	}

	public virtual void SetLanguage(string language)
	{
		Debug.Log ("[Pushwoosh] Error: SetLanguage is not supported on this platform");
	}

	public virtual void SetEmails(List<string> emails)
	{
		Debug.Log ("[Pushwoosh] Error: SetEmails is not supported on this platform");
	}

	public virtual void SetEmail(string email)
	{
		Debug.Log ("[Pushwoosh] Error: SetEmail is not supported on this platform");
	}

	public virtual void PostEvent(string eventId, IDictionary<string, object> attributes)
	{
		string attributesJson = PushwooshUtils.DictionaryToJson(attributes);
		PostEventInternal(eventId, attributesJson);
	}

	public virtual void SendPurchase(string productId, double price, string currency)
	{
		Debug.Log ("[Pushwoosh] Error: SendPurchase is not supported on this platform");
	}

	protected virtual void PostEventInternal(string eventId, string attributes)
	{
		Debug.Log ("[Pushwoosh] Error: PostEvent is not supported on this platform");
	}

	protected void RegisteredForPushNotifications(string token)
	{
		OnRegisteredForPushNotifications(token);
	}

	protected void FailedToRegisteredForPushNotifications(string error)
	{
		OnFailedToRegisteredForPushNotifications(error);
	}

	protected void PushNotificationsReceived(string payload)
	{
		OnPushNotificationsReceived(payload);
	}

	protected void PushNotificationsOpened(string payload)
	{
		OnPushNotificationsOpened(payload);
	}

	protected void PWInAppPurchaseHelperPaymentComplete(string identifier)
	{
		OnPWInAppPurchaseHelperPaymentComplete(identifier);
	}

	protected void PWInAppPurchaseHelperCallPromotedPurchase(string identifier)
	{
		OnPWInAppPurchaseHelperCallPromotedPurchase(identifier);
	}

	protected void PWInAppPurchaseHelperRestoreCompletedTransactionsFailed(string error)
	{
		OnPWInAppPurchaseHelperRestoreCompletedTransactionsFailed(error);
	}

	protected void PWInAppPurchaseHelperPaymentFailedProductIdentifier(string error)
	{
		OnPWInAppPurchaseHelperPaymentFailedProductIdentifier(error);
	}

	protected void PWInAppPurchaseHelperProducts(string identifiers)
	{
		OnPWInAppPurchaseHelperProducts(identifiers);
	}

    public virtual bool IsCommunicationEnabled()
    {
        Debug.Log("[Pushwoosh] Error: IsCommunicationEnabled is not supported on this platform");
        return false;
    }

    public virtual void SetCommunicationEnabled(bool enabled)
    {
        Debug.Log("[Pushwoosh] Error: SetCommunicationEnabled is not supported on this platform");
    }

    public virtual void SetNotificationChannelDelegate(NotificationChannelDelegate notificationChannelDelegate)
    {
        Debug.Log("[Pushwoosh] Error: SetNotificationChannelDelegate is not supported on this platform");
    }

    public virtual string GetLaunchNotification()
    {
        Debug.Log("[Pushwoosh] Error: GetLaunchNotification is not supported on this platform");
        return null;
    }

    public virtual void ClearLaunchNotification()
    {
        Debug.Log("[Pushwoosh] Error: ClearLaunchNotification is not supported on this platform");
    }

    protected void SetCommunicationEnableCallBack(string enabled)
    {
        OnSetCommunicationEnabled(enabled);
    }

	// Singleton
	private static Pushwoosh _instance;
	private static object _lock = new object();

	protected Pushwoosh() {}

	protected virtual void Initialize() {}

	private static Type FindPlatformType()
	{
		var baseType = typeof(Pushwoosh);
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			if (!assembly.FullName.Contains("Pushwoosh"))
				continue;

			foreach (var type in assembly.GetTypes())
			{
				if (type != baseType && baseType.IsAssignableFrom(type))
					return type;
			}
		}
		return baseType;
	}

	public static Pushwoosh Instance
	{
		get
		{
			if (applicationIsQuitting) {
				Debug.LogWarning("[Pushwoosh] Instance already destroyed on application quit. Returning null.");
				return null;
			}

			lock(_lock)
			{
				if (_instance == null)
				{
					Type instanceType = FindPlatformType();

					_instance = (Pushwoosh) FindObjectOfType(instanceType);

					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						singleton.name = "(singleton) " + instanceType.ToString();
						_instance = (Pushwoosh) singleton.AddComponent(instanceType);
						_instance.Initialize();

						DontDestroyOnLoad(singleton);

						Debug.Log("[Pushwoosh] Instance of " + instanceType + " created with DontDestroyOnLoad.");
					}
				}

				return _instance;
			}
		}
	}

	private static bool applicationIsQuitting = false;

	public void OnDestroy () {
		applicationIsQuitting = true;
	}
}
