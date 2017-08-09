package com.pushwoosh.unityplugin;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;

import com.pushwoosh.internal.PushManagerImpl;
import com.unity3d.player.UnityPlayerActivity;

import org.json.JSONObject;

public class IntentReceiver extends BroadcastReceiver
{
    @Override
    public void onReceive(final Context context, final Intent intent)
    {
        if (intent == null) {
            return;
        }

        Bundle pushBundle = PushManagerImpl.preHandlePush(context, intent);
        if(pushBundle == null) {
            return;
        }
        
        Intent launchIntent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName());
        launchIntent.addCategory(Intent.CATEGORY_LAUNCHER);
        launchIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
        launchIntent.putExtras(pushBundle);
        context.startActivity(launchIntent);

        JSONObject dataObject = PushManagerImpl.bundleToJSON(pushBundle);
        PushwooshProxy.onPushReceiveEvent(dataObject.toString());

        // Give Unity 1/2sec to initialize and start activity. Otherwise Rich Media may appear under UnityPlayer activity
        (new Handler()).postDelayed(new Runnable() {
            public void run() {
                PushManagerImpl.postHandlePush(context, intent);
            }
        }, 500L);
    }
}
