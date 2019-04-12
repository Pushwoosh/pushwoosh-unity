package com.pushwoosh.unityplugin;

import android.app.Notification;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;

import com.pushwoosh.notification.PushMessage;
import com.pushwoosh.notification.PushwooshNotificationFactory;

public class UnityNotificationFactory extends PushwooshNotificationFactory {
    private OnAddNotificationChannelListener onAddNotificationChannelListener;
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
        if (onAddNotificationChannelListener != null) {
            return onAddNotificationChannelListener.channelName(channelName);
        }
        return super.channelName(channelName);
    }

    @Nullable
    @Override
    public String channelDescription(String channelName) {
        if (onAddNotificationChannelListener != null) {
            return onAddNotificationChannelListener.channelDescription(channelName);
        }
        return super.channelDescription(channelName);
    }

    public void setOnAddNotificationChannelListener(OnAddNotificationChannelListener listener) {
        this.onAddNotificationChannelListener = listener;
    }
}
