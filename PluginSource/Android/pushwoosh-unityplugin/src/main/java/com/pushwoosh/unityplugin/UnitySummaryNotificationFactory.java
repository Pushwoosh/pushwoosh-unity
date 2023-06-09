package com.pushwoosh.unityplugin;

import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;

import com.pushwoosh.internal.utils.PWLog;
import com.pushwoosh.notification.PushwooshSummaryNotificationFactory;

public class UnitySummaryNotificationFactory extends PushwooshSummaryNotificationFactory {
    private boolean shouldGenerateSummary;
    @Override
    public boolean shouldGenerateSummaryNotification() {
        try {
            String packageName = getApplicationContext().getPackageName();
            ApplicationInfo ai = getApplicationContext().getPackageManager().getApplicationInfo(packageName, PackageManager.GET_META_DATA);

            if (ai.metaData != null) {
                shouldGenerateSummary = ai.metaData.getBoolean("PW_GENERATE_SUMMARY", true);
            }
        } catch (Exception e) {
            PWLog.exception(e);
        }

        PWLog.debug(PushwooshProxy.TAG, "shouldGenerateSummary = " + shouldGenerateSummary);
        return shouldGenerateSummary;
    }
}
