using System;
using UnityEngine;

class OnAddNotificationChannelListener : AndroidJavaProxy
{
    public OnAddNotificationChannelListener() : base("com.pushwoosh.unityplugin.OnAddNotificationChannelListener") { }

    public string channelName(string channelName)
    {
        // Implement your channel name localization logic here
        return channelName;
    }

    public string channelDescription(string channelDescription)
    {
        // Implement your channel description localization logic here
        return channelDescription;
    }
}