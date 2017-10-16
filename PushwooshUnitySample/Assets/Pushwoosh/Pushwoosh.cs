using UnityEngine;
using System.Collections.Generic;

#if UNITY_IPHONE && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsIOS;
#elif UNITY_ANDROID && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsAndroid;
#elif (UNITY_WP8 || UNITY_WP8_1 || UNITY_WSA || UNITY_WSA_8_0 || UNITY_WSA_8_1 || UNITY_WSA_10_0) && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsWindows; 
#else 
using PushwooshInstanceType = Pushwoosh;
#endif

public class Pushwoosh : MonoBehaviour
{
	public static string ApplicationCode { get; set; }
	
	public static string GcmProjectNumber { get; set; }
	

	public delegate void RegistrationSuccessHandler(string token);
	
	public delegate void RegistrationErrorHandler(string error);
	
	public delegate void NotificationHandler(string payload);

	public delegate void GetTagsHandler(IDictionary<string, object> tags, PushwooshException error);


	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event NotificationHandler OnPushNotificationsReceived = delegate {};

	public event NotificationHandler OnPushNotificationsOpened = delegate {};

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

	// Singleton
	private static PushwooshInstanceType _instance;
	
	private static object _lock = new object();

	protected Pushwoosh() {}

	protected virtual void Initialize() {}
	
	public static PushwooshInstanceType Instance
	{
		get
		{
			if (applicationIsQuitting) {
				Debug.LogWarning("[Singleton] Instance '"+ typeof(PushwooshInstanceType) +
				                 "' already destroyed on application quit." +
				                 " Won't create again - returning null.");
				return null;
			}
			
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = (PushwooshInstanceType) FindObjectOfType(typeof(PushwooshInstanceType));
					
					if ( FindObjectsOfType(typeof(PushwooshInstanceType)).Length > 1 )
					{
						Debug.LogError("[Singleton] Something went really wrong " +
						               " - there should never be more than 1 singleton!" +
						               " Reopening the scene might fix it.");
						return _instance;
					}
					
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						singleton.name = "(singleton) "+ typeof(PushwooshInstanceType).ToString();
						_instance = singleton.AddComponent<PushwooshInstanceType>();
						_instance.Initialize ();
						
						DontDestroyOnLoad(singleton);
						
						Debug.Log("[Singleton] An instance of " + typeof(PushwooshInstanceType) + 
						          " has been created with DontDestroyOnLoad.");
					} else {
						Debug.Log("[Singleton] Using instance already created: " +
						          _instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}
	}
	
	private static bool applicationIsQuitting = false;
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public void OnDestroy () {
		applicationIsQuitting = true;
	}
}
