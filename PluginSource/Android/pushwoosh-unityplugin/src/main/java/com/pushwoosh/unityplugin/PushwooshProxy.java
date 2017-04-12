package com.pushwoosh.unityplugin;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Bundle;

import com.pushwoosh.PushManager;
import com.pushwoosh.inapp.InAppFacade;
import com.pushwoosh.internal.utils.JsonUtils;
import com.pushwoosh.notification.SoundType;
import com.pushwoosh.notification.VibrateType;
import com.pushwoosh.BasePushMessageReceiver;
import com.pushwoosh.BaseRegistrationReceiver;
import com.pushwoosh.internal.utils.PWLog;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.math.BigDecimal;
import java.util.Date;
import java.util.HashMap;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

public class PushwooshProxy
{
	private static final String TAG = "UnityPushwooshProxy";

	private static String listenerName;
	private Context context;
	static PushwooshProxy INSTANCE = null;

	private static String pushEventString = null;
	private static String registerEventString = null;
	private static String registerErrorEventString = null;
	
	private PushManager pushManager = null;

	private boolean broadcastPush = true;

	public PushwooshProxy()
	{
		INSTANCE = this;

		Activity currentActivity = com.unity3d.player.UnityPlayer.currentActivity;
		setContext(currentActivity);

		readConfig(currentActivity);

		registerReceivers();

		pushManager = PushManager.getInstance(currentActivity);

		try
		{	
			pushManager.onStartup(currentActivity);
		}
		catch (Exception e)
		{
			PWLog.exception(e);
		}
	}

	private void readConfig(Context context)
	{
		ApplicationInfo ai = null;
		try
		{
			ai = context.getPackageManager().getApplicationInfo(context.getApplicationContext().getPackageName(), PackageManager.GET_META_DATA);
		}
		catch (NameNotFoundException e)
		{
			PWLog.exception(e);
			return;
		}
		
		if (ai == null || ai.metaData == null)
		{
			PWLog.warn(TAG, "No metadata found");
			return;
		}

		broadcastPush = ai.metaData.getBoolean("PW_BROADCAST_PUSH", true);
		PWLog.debug(TAG, "Broadcast push = " + broadcastPush);
	}

	public static void dumpIntent(Intent i)
	{
		if (i == null)
		{
			PWLog.error(TAG, "Intent is null");
			return;
		}
		
		Bundle bundle = i.getExtras();
		if (bundle != null)
		{
			Set<String> keys = bundle.keySet();
			Iterator<String> it = keys.iterator();
			PWLog.debug(TAG, "Dumping Intent start");
			while (it.hasNext())
			{
				String key = it.next();
				PWLog.debug(TAG, "[" + key + "=" + bundle.get(key) + "]");
			}
			PWLog.debug(TAG, "Dumping Intent end");
		}
	}

	public static void initialize(String appId, String projectId)
	{
		Activity currentActivity = com.unity3d.player.UnityPlayer.currentActivity;
		Context context = currentActivity.getApplicationContext();

		PushManager.initializePushManager(context, appId, projectId);
	}

	public static PushwooshProxy instance()
	{
		if (INSTANCE == null)
			INSTANCE = new PushwooshProxy();

		return INSTANCE;
	}
	
	public static PushwooshProxy getInstanceOrNull()
	{
		return INSTANCE;
	}

	public PushManager getPushManager()
	{
		return pushManager;
	}

	public void setContext(Context ctx)
	{
		context = ctx;
	}

	public void registerForPushNotifications()
	{
		PushManager.getInstance(context).registerForPushNotifications();
	}

	public void unregisterFromPushNotifications()
	{
		PushManager.getInstance(context).unregisterForPushNotifications();
	}

	public void setListenerName(String name)
	{
		PWLog.debug(TAG, "Listener name: " + name);
		listenerName = name;

		if (pushEventString != null)
		{
			UnityPlayer.UnitySendMessage(listenerName, "onPushNotificationsReceived", pushEventString);
			pushEventString = null;
		}

		if (registerEventString != null)
		{
			UnityPlayer.UnitySendMessage(listenerName, "onRegisteredForPushNotifications", registerEventString);
			registerEventString = null;
		}


		if (registerErrorEventString != null)
		{
			UnityPlayer.UnitySendMessage(listenerName, "onFailedToRegisteredForPushNotifications", registerErrorEventString);
			registerErrorEventString = null;
		}
	}

	public String getLaunchNotification() 
	{
		return pushManager.getLaunchNotification();
	}

	public void clearLaunchNotification() 
	{
		pushManager.clearLaunchNotification();
	}

	public static void onPushReceiveEvent(String string)
	{
		if (listenerName == null)
		{
			pushEventString = string;
			return;
		}
		
		if(string == null)
			return;

		UnityPlayer.UnitySendMessage(listenerName, "onPushNotificationsReceived", string);
	}

	public static void onRegisterEvent(String string)
	{
		if (listenerName == null)
		{
			registerEventString = string;
			return;
		}

		if(string == null)
			return;

		UnityPlayer.UnitySendMessage(listenerName, "onRegisteredForPushNotifications", string);
	}

	public static void onUnRegisterEvent(String string)
	{
		PWLog.debug(TAG, "Unregistered from push notifications");
	}

	public static void onRegisterErrorEvent(String string)
	{
		if (listenerName == null)
		{
			registerErrorEventString = string;
			return;
		}

		if(string == null)
			return;

		UnityPlayer.UnitySendMessage(listenerName, "onFailedToRegisteredForPushNotifications", string);
	}

	public void onUnRegisterErrorEvent(String string)
	{
		PWLog.debug(TAG, "Failed to unregister");
	}

	public void setIntTag(String name, int value)
	{
		Map<String, Object> tags = new HashMap<String, Object>();
		tags.put(name, new Integer(value));

		PushManager.sendTags(context, tags, null);
	}

	public void setStringTag(String name, String value)
	{
		Map<String, Object> tags = new HashMap<String, Object>();
		tags.put(name, value);

		PushManager.sendTags(context, tags, null);
	}

	public void setListTag(String name, TagValues value)
	{
		Map<String, Object> tags = new HashMap<String, Object>();
		tags.put(name, value);

		PushManager.sendTags(context, tags, null);
	}

	public void getTags()
	{
		PushManager.getTagsAsync(context, new PushManager.GetTagsListener() {
			@Override
			public void onTagsReceived(Map<String, Object> map) {
				JSONObject json = JsonUtils.mapToJson(map);
				UnityPlayer.UnitySendMessage(listenerName, "onTagsReceived", json.toString());
			}

			@Override
			public void onError(Exception e) {
				UnityPlayer.UnitySendMessage(listenerName, "onFailedToReceiveTags", e.getMessage());
			}
		});
	}

	public String getPushToken()
	{
		return PushManager.getPushToken(context);
	}

	public String getPushwooshHWID()
	{
		return PushManager.getPushwooshHWID(context);
	}

	public void clearLocalNotifications()
	{
		PushManager.clearLocalNotifications(context);
	}

	public int scheduleLocalNotification(String message, int seconds)
	{
		return scheduleLocalNotification(message, seconds, new Bundle());
	}

	public int scheduleLocalNotification(String message, int seconds, Bundle extras)
	{
		return PushManager.scheduleLocalNotification(context, message, extras, seconds);
	}

	public void clearLocalNotification(int id)
	{
		PushManager.clearLocalNotification(context, id);
	}

	public void setMultiNotificationMode()
	{
		PushManager.setMultiNotificationMode(context);
	}

	public void setSimpleNotificationMode()
	{
		PushManager.setSimpleNotificationMode(context);
	}

	public void setSoundNotificationType(int soundNotificationType)
	{
		PushManager.setSoundNotificationType(context, SoundType.fromInt(soundNotificationType));
	}

	public void setVibrateNotificationType(int vibrateNotificationType)
	{
		PushManager.setVibrateNotificationType(context, VibrateType.fromInt(vibrateNotificationType));
	}

	public void setLightScreenOnNotification(boolean lightsOn)
	{
		PushManager.setLightScreenOnNotification(context, lightsOn);
	}

	public void startTrackingGeoPushes()
	{
		PushManager.startTrackingGeoPushes(context);
	}

	public void stopTrackingGeoPushes()
	{
		PushManager.stopTrackingGeoPushes(context);
	}

	public void startTrackingBeaconPushes() {
		PushManager.getInstance(context).startTrackingBeaconPushes();
	}

	public void stopTrackingBeaconPushes() {
		PushManager.getInstance(context).stopTrackingBeaconPushes();
	}
	
	public void setBeaconBackgroundMode(boolean backgroundMode)
	{
		PushManager.setBeaconBackgroundMode(context, backgroundMode);
	}

	public void setEnableLED(boolean ledOn)
	{
		PushManager.setEnableLED(context, ledOn);
	}

	public void setBadgeNumber(int newBadge)
	{
		pushManager.setBadgeNumber(newBadge);
	}
	
	public void addBadgeNumber(int deltaBadge)
	{
		pushManager.addBadgeNumber(deltaBadge);
	}

	public String[] getPushHistory()
	{
		ArrayList<String> pushHistory = pushManager.getPushHistory();
		String stringArray[] = new String[pushHistory.size()];
		stringArray = pushHistory.toArray(stringArray);

		PWLog.debug(TAG, "Push history:");
		for (String str : stringArray) {
			PWLog.debug(TAG, "    Message = " + str);
		}
		return stringArray;
	}

	public void clearPushHistory()
	{
		pushManager.clearPushHistory();
	}

	public void clearNotificationCenter()
	{
		PushManager.clearNotificationCenter(context);
	}

	public void setUserId(String userId)
	{
		pushManager.setUserId(context, userId);
	}

	public void postEvent(String event, String attributesStr)
	{
		try
		{
			JSONObject json = new JSONObject(attributesStr);
			Map<String, Object> attributes = JsonUtils.jsonToMap(json);
			InAppFacade.postEvent(com.unity3d.player.UnityPlayer.currentActivity, event, attributes);
		}
		catch (JSONException e)
		{
			PWLog.exception(e);
		}
	}

	public void sendPurchase(String productId, double price, String currency)
	{
		Activity currentActivity = com.unity3d.player.UnityPlayer.currentActivity;
		PushManager.trackInAppRequest(currentActivity, productId, new BigDecimal(price), currency, new Date());
	}

	public void onPause()
	{
		PWLog.debug(TAG, "onPause");
		Activity currentActivity = com.unity3d.player.UnityPlayer.currentActivity;
		try
		{
			currentActivity.unregisterReceiver(mReceiver);
		}
		catch (Exception e)
		{
			// pass.
		}

		try
		{
			currentActivity.unregisterReceiver(mBroadcastReceiver);
		}
		catch (Exception e)
		{
			//pass through
		}
	}

	public void onResume()
	{
		PWLog.debug(TAG, "onResume");
		registerReceivers();
	}

	private BroadcastReceiver mReceiver = new BasePushMessageReceiver()
	{
		@Override
		protected void onMessageReceive(Intent intent)
		{
			// foreground push
			PushwooshProxy.instance().onPushReceiveEvent(intent.getStringExtra(JSON_DATA_KEY));
		}
	};

	BroadcastReceiver mBroadcastReceiver = new BaseRegistrationReceiver()
	{
		@Override
		public void onRegisterActionReceive(Context context, Intent intent)
		{
			checkMessage(intent);
		}
	};

	//Registration of the receivers
	public void registerReceivers()
	{
		Activity currentActivity = com.unity3d.player.UnityPlayer.currentActivity;
		IntentFilter intentFilter = new IntentFilter(currentActivity.getPackageName() + ".action.PUSH_MESSAGE_RECEIVE");

		try 
		{
			if (broadcastPush)
				currentActivity.registerReceiver(mReceiver, intentFilter);
		}
		catch(Exception e)
		{
			// ignore
		}

		try
		{
			currentActivity.registerReceiver(mBroadcastReceiver, new IntentFilter(currentActivity.getPackageName() + "." + PushManager.REGISTER_BROAD_CAST_ACTION));
		}
		catch(Exception e)
		{
			// ignore
		}
	}

	public void checkMessage(Intent intent)
	{
		dumpIntent(intent);

		synchronized (this)
		{
			if (null != intent)
			{
				if (intent.hasExtra(PushManager.PUSH_RECEIVE_EVENT))
				{
					PushwooshProxy.instance().onPushReceiveEvent(intent.getExtras().getString(PushManager.PUSH_RECEIVE_EVENT));
				}

				if (intent.hasExtra(PushManager.REGISTER_EVENT))
				{
					PushwooshProxy.instance().onRegisterEvent(intent.getExtras().getString(PushManager.REGISTER_EVENT));
				}

				if (intent.hasExtra(PushManager.UNREGISTER_EVENT))
				{
					PushwooshProxy.instance().onUnRegisterEvent(intent.getExtras().getString(PushManager.UNREGISTER_EVENT));
				}

				if (intent.hasExtra(PushManager.REGISTER_ERROR_EVENT))
				{
					PushwooshProxy.instance().onRegisterErrorEvent(intent.getExtras().getString(PushManager.REGISTER_ERROR_EVENT));
				}

				if (intent.hasExtra(PushManager.UNREGISTER_ERROR_EVENT))
				{
					PushwooshProxy.instance().onUnRegisterErrorEvent(intent.getExtras().getString(PushManager.UNREGISTER_ERROR_EVENT));
				}
			}
		}
	}
}
