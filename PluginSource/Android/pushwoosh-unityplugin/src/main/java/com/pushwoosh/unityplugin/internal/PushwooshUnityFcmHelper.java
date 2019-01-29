package com.pushwoosh.unityplugin.internal;

import android.content.Context;

import com.google.firebase.FirebaseApp;
import com.google.firebase.FirebaseOptions;
import com.pushwoosh.internal.utils.PWLog;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class PushwooshUnityFcmHelper {

    private final String TAG = PushwooshUnityFcmHelper.class.getSimpleName();

    public void initFCM(Context context) {
        if (context == null) {
            PWLog.error("context is null");
            return;
        }
        if (!FirebaseApp.getApps(context).isEmpty()) {
            PWLog.debug( "Firebase already init");
            return;
        }
        String json = readGoogleServiceJsonFromAssets(context);
        PWLog.debug(TAG, "json from google-service.json:" + json);

        try {
            JSONObject jsonObject = new JSONObject(json);
            String gcmSenderId = getSenderId(jsonObject);
            String applicationId = getApplicationId(jsonObject);
            initFirebaseApp(context, applicationId, gcmSenderId);
        } catch (JSONException e) {
            PWLog.error(TAG, "can not parse info for GCM init:" + e);
        }
    }

    private String readGoogleServiceJsonFromAssets(Context context) {
        StringBuilder result = new StringBuilder();
        BufferedReader reader = null;
        try {
            InputStream inputStream = context.getAssets().open("google-services.json");
            reader = new BufferedReader(new InputStreamReader(inputStream));
            String mLine;
            while ((mLine = reader.readLine()) != null) {
                result.append(mLine);
            }
        } catch (IOException e) {
            PWLog.error(TAG, "can not read file google-services.json:" + e);

        } finally {
            if (reader != null) {
                try {
                    reader.close();
                } catch (IOException e) {
                    PWLog.error(TAG, e);
                }
            }
        }
        return result.toString();
    }

    private void initFirebaseApp(Context context, String applicationId, String gcmSenderId) {
        FirebaseOptions options = new FirebaseOptions.Builder()
                .setApplicationId(applicationId)
                .setGcmSenderId(gcmSenderId)
                .build();

        FirebaseApp.initializeApp(context, options);
    }

    private String getSenderId(JSONObject jsonObject) throws JSONException {
        return jsonObject
                .getJSONObject("project_info")
                .getString("project_number");
    }

    private String getApplicationId(JSONObject jsonObject) throws JSONException {
        return jsonObject
                .getJSONArray("client")
                .getJSONObject(0)
                .getJSONArray("oauth_client")
                .getJSONObject(0)
                .getString("client_id");
    }

}
