using System;
using UnityEngine;

class OnAddNotificationChannelListener : AndroidJavaProxy
{
    public OnAddNotificationChannelListener() : base("com.pushwoosh.unityplugin.OnAddNotificationChannelListener") { }

    public string ChannelName(string channelName)
    {
        // Implement your channel name logic here (e.g. localization)
        return channelName;
    }

    public string ChannelDescription(string channelName)
    {
        // Implement your channel description logic here (e.g. localization)
        return "";
    }
}