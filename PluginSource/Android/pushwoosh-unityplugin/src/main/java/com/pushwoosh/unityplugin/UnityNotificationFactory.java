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
    public String channelName(String name) {
        if (onAddNotificationChannelListener != null) {
            return onAddNotificationChannelListener.channelName(name);
        }
        return super.channelName(name);
    }

    @Nullable
    @Override
    public String channelDescription(String description) {
        if (onAddNotificationChannelListener != null) {
            return onAddNotificationChannelListener.channelDescription(description);
        }
        return super.channelDescription(description);
    }

    public void setOnAddNotificationChannelListener(OnAddNotificationChannelListener listener) {
        this.onAddNotificationChannelListener = listener;
    }
}
