package com.pushwoosh.unityplugin;

public interface NotificationChannelDelegate {
    /**
     * @param channelName name of the channel specified in Android payload as "pw_channel" attribute.
     * If no attribute was specified, parameter gives default channel name
     * @return name that you want to assign to the channel on its creation. Note that empty name
     * will be ignored and default channel name will be assigned to the channel instead
     */
    String ChannelName(String channelName);
    /**
     * @param channelName name of the channel specified in Android payload as "pw_channel" attribute.
     * If no attribute was specified, parameter gives default channel name
     * @return description that you want to assign to the channel on its creation
     */
    String ChannelDescription(String channelName);
}
