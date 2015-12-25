using UnityEngine;
using System.Collections.Generic;

#if UNITY_IPHONE && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsIOS;
#elif UNITY_ANDROID && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsAndroid;
#elif (UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
using PushwooshInstanceType = PushNotificationsWP8; 
#else 
using PushwooshInstanceType = Pushwoosh;
#endif

public class Pushwoosh : MonoBehaviour
{
	public const string APP_CODE = "4FC89B6D14A655.46488481";
	
	public const string GCM_PROJECT_NUMBER = "60756016005";
	

	public delegate void RegistrationSuccessHandler(string token);
	
	public delegate void RegistrationErrorHandler(string error);
	
	public delegate void NotificationHandler(string payload);

	public delegate void InitializationHandler();


	public event RegistrationSuccessHandler OnRegisteredForPushNotifications = delegate {};
	
	public event RegistrationErrorHandler OnFailedToRegisteredForPushNotifications = delegate {};
	
	public event NotificationHandler OnPushNotificationsReceived = delegate {};

	public event InitializationHandler OnInitialized = delegate {};


	public virtual string HWID
	{
		get 
		{
			UnsupportedPlatform ();
			return "Unsupported platform"; 
		}
	}

	public virtual string PushToken
	{
		get 
		{ 
			UnsupportedPlatform ();
			return "Unsupported platform"; 
		}
	}

	public virtual void StartTrackingGeoPushes()
	{
		UnsupportedPlatform ();
	}

	public virtual void StopTrackingGeoPushes()
	{
		UnsupportedPlatform ();
	}

	public virtual void SetIntTag(string tagName, int tagValue)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void SetStringTag(string tagName, string tagValue)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void SetListTag(string tagName, List<object> tagValues)
	{
		UnsupportedPlatform ();
	}

	public virtual void ClearNotificationCenter()
	{
		UnsupportedPlatform ();
	}

	public virtual void SetBadgeNumber(int number)
	{
		UnsupportedPlatform ();
	}
	
	public virtual void AddBadgeNumber(int deltaBadge)
	{
		UnsupportedPlatform ();
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

	protected void Initialized()
	{
		OnInitialized ();
	}

	private void UnsupportedPlatform()
	{
		var frame = new System.Diagnostics.StackFrame(1);
		var method = frame.GetMethod();
		string methodName = method.Name;
		Debug.Log ("[Pushwoosh] Error: " + methodName + " is not supported on this platform");
	}

	// Singleton
	private static PushwooshInstanceType _instance;
	
	private static object _lock = new object();

	protected Pushwoosh() {}
	
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
						_instance = singleton.AddComponent<PushwooshInstanceType>();
						singleton.name = "(singleton) "+ typeof(PushwooshInstanceType).ToString();
						
						DontDestroyOnLoad(singleton);
						
						Debug.Log("[Singleton] An instance of " + typeof(PushwooshInstanceType) + 
						          " is needed in the scene, so '" + singleton +
						          "' was created with DontDestroyOnLoad.");
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
