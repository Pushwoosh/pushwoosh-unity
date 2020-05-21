package com.pushwoosh.unityplugin;

import android.app.Notification;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.pushwoosh.notification.PushMessage;
import com.pushwoosh.notification.PushwooshNotificationFactory;

public class UnityNotificationFactory extends PushwooshNotificationFactory {
    private NotificationChannelDelegate notificationChannelDelegate;
    static UnityNotificationFactory INSTANCE;

    public UnityNotificationFactory() {
        if (INSTANCE == null) {
            INSTANCE = this;
        }
    }

    public static UnityNotificationFactory instance() {
        if (INSTANCE == null) {
            INSTANCE = new UnityNotificationFactory();
        }
        return INSTANCE;
    }

    @Nullable
    @Override
    public Notification onGenerateNotification(@NonNull PushMessage pushMessage) {
        return super.onGenerateNotification(pushMessage);
    }

    @Override
    public String channelName(String channelName) {
        if (notificationChannelDelegate != null) {
            return notificationChannelDelegate.ChannelName(channelName);
        }
        return super.channelName(channelName);
    }

    @Nullable
    @Override
    public String channelDescription(String channelName) {
        if (notificationChannelDelegate != null) {
            return notificationChannelDelegate.ChannelDescription(channelName);
        }
        return super.channelDescription(channelName);
    }

    public void setNotificationChannelDelegate(NotificationChannelDelegate delegate) {
        this.notificationChannelDelegate = delegate;
    }
}
