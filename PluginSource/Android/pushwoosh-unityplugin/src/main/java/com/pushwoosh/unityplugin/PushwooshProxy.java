package com.pushwoosh.unityplugin;

import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import androidx.annotation.NonNull;
import androidx.core.app.NotificationManagerCompat;

import com.pushwoosh.GDPRManager;
import com.pushwoosh.Pushwoosh;
import com.pushwoosh.RegisterForPushNotificationsResultData;
import com.pushwoosh.badge.PushwooshBadge;
import com.pushwoosh.exception.GetTagsException;
import com.pushwoosh.exception.PushwooshException;
import com.pushwoosh.exception.RegisterForPushNotificationsException;
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

import java.math.BigDecimal;
import java.util.Collections;
import java.util.List;

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

	private final Handler handler = new Handler(Looper.getMainLooper());

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
		Pushwoosh.getInstance().registerForPushNotifications(new Callback<RegisterForPushNotificationsResultData, RegisterForPushNotificationsException>() {
			@Override
			public void process(@NonNull Result<RegisterForPushNotificationsResultData, RegisterForPushNotificationsException> result) {
				if (result.isSuccess()) {
					if (result.getData() != null) {
						onRegisterEvent(result.getData().getToken());
					} else {
						onRegisterErrorEvent("Failed to get push token from registration callback");
					}
				} else if (result.getException() != null) {
					onRegisterErrorEvent(result.getException().getLocalizedMessage());
				}
			}
		});
	}

	public void unregisterFromPushNotifications() {
		Pushwoosh.getInstance().unregisterForPushNotifications();
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
		String result = null;
		if (data != null) {
			result = data.toJson().toString();
		}

		return returnStringToUnity(result);
	}

    public void clearLaunchNotification() {
        Pushwoosh.getInstance().clearLaunchNotification();
    }

    public String getRemoteNotificationStatus(){
        JSONObject result = new JSONObject();
        try {
            String enabled = PushwooshNotificationSettings.areNotificationsEnabled() ? "1" : "0";

            result.put("enabled", enabled);
        } catch (Exception e){
            PWLog.exception(e);
        }
        return result.toString();
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
		return returnStringToUnity(Pushwoosh.getInstance().getPushToken());
	}

	public String getPushwooshHWID() {
		return returnStringToUnity(Pushwoosh.getInstance().getHwid());
	}

	public void clearLocalNotifications() {
		LocalNotificationReceiver.cancelAll();
	}

    public int scheduleLocalNotification(String message, int seconds, Bundle extras, String largeIcon) {
        LocalNotification.Builder builder = new LocalNotification.Builder();
        if (largeIcon != null) {
			builder.setLargeIcon(largeIcon);
        }
        if (extras != null) {
            builder.setExtras(extras);
        }
        LocalNotification notification = builder
                .setMessage(message)
                .setDelay(seconds)
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
		handler.post(new Runnable() {
			@Override
			public void run() {
				PushwooshLocation.startLocationTracking();
			}
		});
	}

	public void stopTrackingGeoPushes() {
		handler.post(new Runnable() {
			@Override
			public void run() {
				PushwooshLocation.stopLocationTracking();
			}
		});
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

	public void setLanguage(String language) {
		Pushwoosh.getInstance().setLanguage(language);
	}

	public void setUser(String userId, List<String> emails) {
		Pushwoosh.getInstance().setUser(userId, emails);
	}

	public void setEmails(List<String> emails) {
		Pushwoosh.getInstance().setEmail(emails);
	}

	public void setEmail(String email) {
		Pushwoosh.getInstance().setEmail(email);
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

	public void showGDPRConsentUI(){
		GDPRManager.getInstance().showGDPRConsentUI();
	}

	public void showGDPRDeletionUI(){
		GDPRManager.getInstance().showGDPRDeletionUI();
	}

	public boolean isCommunicationEnabled(){
		return GDPRManager.getInstance().isCommunicationEnabled();
	}

	public boolean isDeviceDataRemoved(){
		return GDPRManager.getInstance().isDeviceDataRemoved();
	}

	public boolean isAvailable(){
		return GDPRManager.getInstance().isAvailable();
	}

	public void setCommunicationEnabled(boolean enable) {
		 GDPRManager.getInstance().setCommunicationEnabled(enable, new Callback<Void, PushwooshException>() {
			@Override
			public void process(@NonNull Result<Void, PushwooshException> result) {
				if(listenerName==null) return;
				if (result.isSuccess()) {
					UnityPlayer.UnitySendMessage(listenerName, "OnSetCommunicationEnabled", "success");
				} else if (result.getException() != null) {
					UnityPlayer.UnitySendMessage(listenerName, "OnFailedSetCommunicationEnabled", result.getException().getMessage());
				}
			}
		});
	}

	public void removeAllDeviceData() {
		GDPRManager.getInstance().removeAllDeviceData(new Callback<Void, PushwooshException>() {
			@Override
			public void process(@NonNull Result<Void, PushwooshException> result) {
				if(listenerName==null) return;
				if (result.isSuccess()) {
					UnityPlayer.UnitySendMessage(listenerName, "OnRemoveAllDeviceData", "success");
				} else if (result.getException() != null) {
					UnityPlayer.UnitySendMessage(listenerName, "OnFailedRemoveAllDeviceData", result.getException().getMessage());
				}
			}
		});
	}

	public void setNotificationChannelDelegate(NotificationChannelDelegate delegate) {
        UnityNotificationFactory.instance().setNotificationChannelDelegate(delegate);
	}

	//HACK: unity 2018 crashes if function returns null instead of string
	private static String returnStringToUnity(String string) {
		if (string == null)
			return "";
		return string;
	}
}
