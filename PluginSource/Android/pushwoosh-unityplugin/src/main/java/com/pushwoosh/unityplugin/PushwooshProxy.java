package com.pushwoosh.unityplugin;

import java.math.BigDecimal;
import java.util.Collections;
import java.util.List;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.NotificationManagerCompat;

import com.pushwoosh.Pushwoosh;
import com.pushwoosh.badge.PushwooshBadge;
import com.pushwoosh.beacon.PushwooshBeacon;
import com.pushwoosh.exception.GetTagsException;
import com.pushwoosh.exception.RegisterForPushNotificationsException;
import com.pushwoosh.exception.UnregisterForPushNotificationException;
import com.pushwoosh.function.Callback;
import com.pushwoosh.function.Result;
import com.pushwoosh.inapp.PushwooshInApp;
import com.pushwoosh.internal.utils.PWLog;
import com.pushwoosh.location.PushwooshLocation;
import com.pushwoosh.notification.LocalNotification;
import com.pushwoosh.notification.LocalNotificationReceiver;
import com.pushwoosh.notification.PushMessage;
import com.pushwoosh.notification.PushwooshNotificationSettings;
import com.pushwoosh.notification.SoundType;
import com.pushwoosh.notification.VibrateType;
import com.pushwoosh.tags.Tags;
import com.pushwoosh.tags.TagsBundle;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;


import static com.unity3d.player.UnityPlayer.currentActivity;

public class PushwooshProxy {
	static final String TAG = "UnityPushwooshProxy";

	private static final Object sPushwooshProxyMutex = new Object();

	private static final Object sStartPushLock = new Object();
	private static String receivePushData = null;
	private static String openPushData = null;

	private static String registerEventString = null;
	private static String registerErrorEventString = null;

	private static String listenerName;

	static PushwooshProxy INSTANCE = null;

	public static void initialize(String appId, String projectId) {
		Pushwoosh.getInstance().setAppId(appId);
		Pushwoosh.getInstance().setSenderId(projectId);
	}

	public static PushwooshProxy instance() {
		if (INSTANCE == null) {
			synchronized (sPushwooshProxyMutex) {
				if (INSTANCE == null) {
					new PushwooshProxy();
				}
			}
		}

		return INSTANCE;
	}

	public PushwooshProxy() {
		INSTANCE = this;
	}

	public void onResume() {

	}

	public void onPause() {

	}

	public void registerForPushNotifications() {
		Pushwoosh.getInstance().registerForPushNotifications(new Callback<String, RegisterForPushNotificationsException>() {
			@Override
			public void process(@NonNull Result<String, RegisterForPushNotificationsException> result) {
				if (result.isSuccess()) {
					onRegisterEvent(result.getData());
				} else if (result.getException() != null) {
					onRegisterErrorEvent(result.getException().getLocalizedMessage());
				}
			}
		});
	}

	public void unregisterFromPushNotifications() {
		Pushwoosh.getInstance().unregisterForPushNotifications(new Callback<String, UnregisterForPushNotificationException>() {
			@Override
			public void process(@NonNull Result<String, UnregisterForPushNotificationException> result) {
				if (result.isSuccess()) {
					onUnRegisterEvent(result.getData());
				} else if (result.getException() != null) {
					onUnRegisterErrorEvent(result.getException().getLocalizedMessage());
				}
			}
		});
	}

	public void setListenerName(String name) {
		PWLog.debug(TAG, "Listener name: " + name);
		listenerName = name;

		if (receivePushData != null) {
			onPushReceiveEvent(receivePushData);
		}

		if (registerEventString != null) {
			onRegisterEvent(registerEventString);
		}


		if (registerErrorEventString != null) {
			onRegisterErrorEvent(registerErrorEventString);
		}
	}

	public String getLaunchNotification() {
		PushMessage data = Pushwoosh.getInstance().getLaunchNotification();
		if (data != null) {
			return data.toJson().toString();
		}

		return null;
	}

	public void clearLaunchNotification() {
		Pushwoosh.getInstance().clearLaunchNotification();
	}

	public static void onPushReceiveEvent(String string) {
		if (listenerName == null) {
			PWLog.debug(TAG, "onPushReceiveEvent: listener is null");
			receivePushData = string;
			return;
		}

		if (string == null) {
			PWLog.debug(TAG, "onPushReceiveEvent: message is null");
			return;
		}

		PWLog.debug(TAG, "onPushReceiveEvent: send payload to onPushNotificationsReceived");
		UnityPlayer.UnitySendMessage(listenerName, "onPushNotificationsReceived", string);
		receivePushData = null;
	}

	public static void onPushOpenEvent(String string) {
		if (listenerName == null) {
			openPushData = string;
			return;
		}

		if (string == null) {
			return;
		}

		UnityPlayer.UnitySendMessage(listenerName, "onPushNotificationsOpened", string);
		openPushData = null;
	}

	public static void onRegisterEvent(String string) {
		if (listenerName == null) {
			registerEventString = string;
			return;
		}

		if (string == null) {
			return;
		}

		UnityPlayer.UnitySendMessage(listenerName, "onRegisteredForPushNotifications", string);
		registerEventString = null;
	}

	public static void onUnRegisterEvent(String string) {
		if (listenerName == null) {
			return;
		}

		UnityPlayer.UnitySendMessage(listenerName, "onUnRegisteredForPushNotifications", string);
	}

	public static void onRegisterErrorEvent(String string) {
		if (listenerName == null) {
			registerErrorEventString = string;
			return;
		}

		if (string == null) {
			return;
		}

		UnityPlayer.UnitySendMessage(listenerName, "onFailedToRegisteredForPushNotifications", string);
		registerErrorEventString = null;
	}

	public void onUnRegisterErrorEvent(String string) {
		if (listenerName == null) {
			return;
		}

		UnityPlayer.UnitySendMessage(listenerName, "onFailedToUnRegisteredForPushNotifications", string);
	}

	public void setIntTag(String name, int value) {
		Pushwoosh.getInstance().sendTags(Tags.intTag(name, value));
	}

	public void setStringTag(String name, String value) {
		Pushwoosh.getInstance().sendTags(Tags.stringTag(name, value));
	}

	public void setListTag(String name, TagValues value) {
		Pushwoosh.getInstance().sendTags(ConversionUtils.convertToTagsBundle(Collections.<String, Object>singletonMap(name, value)));
	}

	public void getTags() {
		Pushwoosh.getInstance().getTags(new Callback<TagsBundle, GetTagsException>() {
			@Override
			public void process(@NonNull Result<TagsBundle, GetTagsException> result) {
				if (result.isSuccess()) {
					UnityPlayer.UnitySendMessage(listenerName, "onTagsReceived", result.getData() == null ? JSONObject.NULL.toString() : result.getData().toJson().toString());
				} else if (result.getException() != null) {
					UnityPlayer.UnitySendMessage(listenerName, "onFailedToReceiveTags", result.getException().getMessage());
				}
			}
		});
	}

	public String getPushToken() {
		return Pushwoosh.getInstance().getPushToken();
	}

	public String getPushwooshHWID() {
		return Pushwoosh.getInstance().getHwid();
	}

	public void clearLocalNotifications() {
		LocalNotificationReceiver.cancelAll();
	}

	public int scheduleLocalNotification(String message, int seconds) {
		return scheduleLocalNotification(message, seconds, new Bundle());
	}

	public int scheduleLocalNotification(String message, int seconds, Bundle extras) {
		LocalNotification notification = new LocalNotification.Builder()
				.setMessage(message)
				.setDelay(seconds)
				.setExtras(extras)
				.build();

		return Pushwoosh.getInstance().scheduleLocalNotification(notification).getRequestId();
	}

	public void clearLocalNotification(int id) {
		LocalNotificationReceiver.cancelNotification(id);
	}

	public void setMultiNotificationMode() {
		PushwooshNotificationSettings.setMultiNotificationMode(true);
	}

	public void setSimpleNotificationMode() {
		PushwooshNotificationSettings.setMultiNotificationMode(false);
	}

	public void setSoundNotificationType(int soundNotificationType) {
		PushwooshNotificationSettings.setSoundNotificationType(SoundType.fromInt(soundNotificationType));
	}

	public void setVibrateNotificationType(int vibrateNotificationType) {
		PushwooshNotificationSettings.setVibrateNotificationType(VibrateType.fromInt(vibrateNotificationType));
	}

	public void setLightScreenOnNotification(boolean lightsOn) {
		PushwooshNotificationSettings.setLightScreenOnNotification(lightsOn);
	}

	public void startTrackingGeoPushes() {
		PushwooshLocation.startLocationTracking();
	}

	public void stopTrackingGeoPushes() {
		PushwooshLocation.stopLocationTracking();
	}

	public void startTrackingBeaconPushes() {
		PushwooshBeacon.startTrackingBeaconPushes();
	}

	public void stopTrackingBeaconPushes() {
		PushwooshBeacon.stopTrackingBeaconPushes();
	}

	public void setBeaconBackgroundMode(boolean backgroundMode) {
		PushwooshBeacon.setBeaconBackgroundMode(backgroundMode);
	}

	public void setEnableLED(boolean ledOn) {
		PushwooshNotificationSettings.setEnableLED(ledOn);
	}

	public void setBadgeNumber(int newBadge) {
		PushwooshBadge.setBadgeNumber(newBadge);
	}

	public void addBadgeNumber(int deltaBadge) {
		PushwooshBadge.addBadgeNumber(deltaBadge);
	}

	public String[] getPushHistory() {
		List<PushMessage> pushHistory = Pushwoosh.getInstance().getPushHistory();
		String stringArray[] = new String[pushHistory.size()];
		int counter = 0;
		PWLog.debug(TAG, "Push history:");

		for (PushMessage pushMessage : pushHistory) {
			stringArray[counter] = pushMessage.toJson().toString();
			PWLog.debug(TAG, "    Message = " + stringArray[counter]);
			counter++;
		}

		return stringArray;
	}

	public void clearPushHistory() {
		Pushwoosh.getInstance().clearPushHistory();
	}

	public void clearNotificationCenter() {
		NotificationManagerCompat.from(currentActivity).cancelAll();
	}

	public void setUserId(String userId) {
		PushwooshInApp.getInstance().setUserId(userId);
	}

	public void postEvent(String event, String attributesStr) {
		try {
			if (attributesStr.isEmpty()) {
				PushwooshInApp.getInstance().postEvent(event);
				return;
			}

			PushwooshInApp.getInstance().postEvent(event, Tags.fromJson(new JSONObject(attributesStr)));
		} catch (JSONException e) {
			PWLog.exception(e);
		}
	}

	public void sendPurchase(String productId, double price, String currency) {
		Pushwoosh.getInstance().sendInappPurchase(productId, new BigDecimal(price), currency);
	}

	static void openPush(String pushData) {
		PWLog.info(TAG, "Push open: " + pushData);

		try {
			synchronized (sStartPushLock) {
				openPushData = pushData;
				onPushOpenEvent(openPushData);
			}
		} catch (Exception e) {
			// React Native is highly unstable
			PWLog.exception(e);
		}
	}

	static void messageReceived(String pushData) {
		PWLog.info(TAG, "Push received: " + pushData);

		try {
			synchronized (sStartPushLock) {
				receivePushData = pushData;
				onPushReceiveEvent(pushData);
			}
		} catch (Exception e) {
			// React Native is highly unstable
			PWLog.exception(e);
		}
	}
}
