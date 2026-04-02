using System;
using UnityEngine;

public abstract class NotificationChannelDelegate : AndroidJavaProxy
{
    protected NotificationChannelDelegate() : base("com.pushwoosh.unityplugin.NotificationChannelDelegate") { }

    public abstract string ChannelName(string channelName);

    public abstract string ChannelDescription(string channelName);
}