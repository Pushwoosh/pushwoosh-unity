//
//  PushRuntime.m
//  Pushwoosh SDK
//  (c) Pushwoosh 2012
//

#import "PushNotificationManager.h"
#import <UserNotifications/UserNotifications.h>
#import <objc/runtime.h>

static char * g_pw_tokenStr = 0;
static char * g_pw_registerErrStr = 0;
static char * g_pw_pushMessageStr = 0;
static char * g_pw_listenerName = 0;
static NSString * g_pw_launchNotification = nil;

void pw_registerForRemoteNotifications() {
	[[PushNotificationManager pushManager] registerForPushNotifications];
}

void pw_initializePushManager(char *appId, char *appName) {
	NSString *appCodeStr = [[NSString alloc] initWithUTF8String:appId];
	NSString *appNameStr = appName ? [[NSString alloc] initWithUTF8String:appName] :
        [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleDisplayName"];
	[PushNotificationManager initializeWithAppCode:appCodeStr appName:appNameStr];

	[[PushNotificationManager pushManager] sendAppOpen];
	[PushNotificationManager pushManager].delegate = (NSObject<PushNotificationDelegate> *)[UIApplication sharedApplication];
	[UNUserNotificationCenter currentNotificationCenter].delegate = [PushNotificationManager pushManager].notificationCenterDelegate;
}

void pw_unregisterForRemoteNotifications() {
	[[PushNotificationManager pushManager] unregisterForPushNotifications];
}

void *pw_getPushToken() {
	return (void *)[[[PushNotificationManager pushManager] getPushToken] UTF8String];
}

void *pw_getPushwooshHWID() {
	return (void *)[[[PushNotificationManager pushManager] getHWID] UTF8String];
}

void *pw_getLaunchNotification() {
	if (g_pw_launchNotification) {
		return (void *)[g_pw_launchNotification UTF8String];
	}
	
	return NULL;
}

void pw_clearLaunchNotification() {
	g_pw_launchNotification = nil;
}

void pw_setUserId(char *userId) {
	NSString *userIdStr = [[NSString alloc] initWithUTF8String:userId];
	[[PushNotificationManager pushManager] setUserId:userIdStr];
}

void pw_postEvent(char *event, char *attributes) {
	NSString *eventStr = [[NSString alloc] initWithUTF8String:event];
	NSString *attributesStr = [[NSString alloc] initWithUTF8String:attributes];

	NSDictionary *json = [NSJSONSerialization JSONObjectWithData:[attributesStr dataUsingEncoding:NSUTF8StringEncoding] options:0 error:nil];
	if ([json isKindOfClass:[NSDictionary class]]) {
		[[PushNotificationManager pushManager] postEvent:eventStr withAttributes:json];
	}
	else {
		NSLog(@"Invalid postEvent attribute argument: %@", json);
	}
}

void pw_sendPurchase(char *productId, double price, char *currency) {
	NSString *productIdStr = [[NSString alloc] initWithUTF8String:productId];
	NSDecimalNumber *priceDecimal = [[NSDecimalNumber alloc] initWithDouble:price];
	NSString *currencyStr = [[NSString alloc] initWithUTF8String:currency];

	[[PushNotificationManager pushManager] sendPurchase:productIdStr withPrice:priceDecimal currencyCode:currencyStr andDate:[NSDate date]];
}

void pw_setListenerName(char *listenerName) {
	free(g_pw_listenerName); g_pw_listenerName = 0;
	int len = strlen(listenerName);
	g_pw_listenerName = malloc(len + 1);
	strcpy(g_pw_listenerName, listenerName);
	
	if(g_pw_tokenStr) {
		UnitySendMessage(g_pw_listenerName, "onRegisteredForPushNotifications", g_pw_tokenStr);
		free(g_pw_tokenStr); g_pw_tokenStr = 0;
	}
	
	if(g_pw_registerErrStr) {
		UnitySendMessage(g_pw_listenerName, "onFailedToRegisteredForPushNotifications", g_pw_registerErrStr);
		free(g_pw_registerErrStr); g_pw_registerErrStr = 0;
	}
	
	if(g_pw_pushMessageStr) {
		UnitySendMessage(g_pw_listenerName, "onPushNotificationsReceived", g_pw_pushMessageStr);
	}
    
    if(g_pw_pushMessageStr) {
        UnitySendMessage(g_pw_listenerName, "onPushNotificationsOpened", g_pw_pushMessageStr);
        free(g_pw_pushMessageStr); g_pw_pushMessageStr = 0;
    }
}

void pw_setIntTag(char *tagName, int tagValue) {
	NSString *tagNameStr = [[NSString alloc] initWithUTF8String:tagName];
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:[NSNumber numberWithInt:tagValue], tagNameStr, nil];
	[[PushNotificationManager pushManager] setTags:dict];
	
#if !__has_feature(objc_arc)
	[tagNameStr release];
#endif
}

void pw_setStringTag(char *tagName, char *tagValue) {
	NSString *tagNameStr = [[NSString alloc] initWithUTF8String:tagName];
	NSString *tagValueStr = [[NSString alloc] initWithUTF8String:tagValue];
	
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:tagValueStr, tagNameStr, nil];
	[[PushNotificationManager pushManager] setTags:dict];
    
#if !__has_feature(objc_arc)
	[tagNameStr release];
	[tagValueStr release];
#endif
}

void pw_internalSendStringTags (char *tagName, char **tags) {
    size_t length = 0;
    while (tags[length] != NULL) length++;
    
    NSMutableArray *tagsArray = [NSMutableArray array];
    NSString *tagNameStr = [[NSString alloc] initWithUTF8String:tagName];
    
    for (int i = 0; i < length; i++) {
        char *tagValue = tags[i];
        NSString *tagValueStr = [[NSString alloc] initWithUTF8String:tagValue];
        
        if (tagValueStr) {
            [tagsArray addObject:tagValueStr];
        }
#if !__has_feature(objc_arc)
        [tagValueStr release];
#endif
    }
    
    if (tagsArray.count) {
        [[PushNotificationManager pushManager] setTags:@{tagNameStr : tagsArray}];
    }
#if !__has_feature(objc_arc)
    [tagNameStr release];
#endif
}

void pw_getTags() {
	[[PushNotificationManager pushManager] loadTags:^(NSDictionary *tags) {
		NSError *error;
		NSData *jsonData = [NSJSONSerialization dataWithJSONObject:tags options:0 error:&error];
		if (error == nil) {
			NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
			UnitySendMessage(g_pw_listenerName, "onTagsReceived", [json UTF8String]);
		}
		else {
			UnitySendMessage(g_pw_listenerName, "onFailedToReceiveTags", [[error description] UTF8String]);
		}
	} error:^(NSError *error) {
		UnitySendMessage(g_pw_listenerName, "onFailedToReceiveTags", [[error description] UTF8String]);
	}];
}

void pw_startLocationTracking() {
	[[PushNotificationManager pushManager] startLocationTracking];
}

void pw_clearNotificationCenter() {
	[PushNotificationManager clearNotificationCenter];
}

void pw_stopLocationTracking() {
	[[PushNotificationManager pushManager] stopLocationTracking];
}

void pw_setBadgeNumber(int badge) {
	[[UIApplication sharedApplication] setApplicationIconBadgeNumber:badge];
}

void pw_addBadgeNumber(int deltaBadge) {
	int badge = [UIApplication sharedApplication].applicationIconBadgeNumber + deltaBadge;
	pw_setBadgeNumber(badge);
}

@implementation UIApplication(InternalPushRuntime)

- (NSObject<PushNotificationDelegate> *)getPushwooshDelegate {
	return (NSObject<PushNotificationDelegate> *)[UIApplication sharedApplication];
}

- (BOOL)pushwooshUseRuntimeMagic {
	return YES;
}

//succesfully registered for push notifications
- (void)onDidRegisterForRemoteNotificationsWithDeviceToken:(NSString *)token {
	const char * str = [token UTF8String];
	if(!g_pw_listenerName) {
		g_pw_tokenStr = malloc(strlen(str)+1);
		strcpy(g_pw_tokenStr, str);
		return;
	}
	
	UnitySendMessage(g_pw_listenerName, "onRegisteredForPushNotifications", str);
}

//failed to register for push notifications
- (void)onDidFailToRegisterForRemoteNotificationsWithError:(NSError *)error {
	const char * str = [[error description] UTF8String];
	if(!g_pw_listenerName) {
		if (str) {
			g_pw_registerErrStr = malloc(strlen(str)+1);
			strcpy(g_pw_registerErrStr, str);
		}
		return;
	}
	
	UnitySendMessage(g_pw_listenerName, "onFailedToRegisteredForPushNotifications", str);
}

- (const char *)jsonRequestDataWithNotification:(NSDictionary *)pushNotification onStart:(BOOL)onStart {
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:pushNotification options:0 error:nil];
    NSString *jsonRequestData = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    if (onStart) {
        g_pw_launchNotification = jsonRequestData;
    }
    
    const char * str = [jsonRequestData UTF8String];
    
    if(!g_pw_listenerName) {
        g_pw_pushMessageStr = malloc(strlen(str)+1);
        strcpy(g_pw_pushMessageStr, str);
        return NULL;
    } else {
        return str;
    }
}

- (void)onPushReceived:(PushNotificationManager *)pushManager withNotification:(NSDictionary *)pushNotification onStart:(BOOL)onStart {
    const char * str = [self jsonRequestDataWithNotification:pushNotification onStart:onStart];
    if (str != NULL) {
        UnitySendMessage(g_pw_listenerName, "onPushNotificationsReceived", str);
    }
}

- (void)onPushAccepted:(PushNotificationManager *)pushManager withNotification:(NSDictionary *)pushNotification onStart:(BOOL)onStart {
    const char * str = [self jsonRequestDataWithNotification:pushNotification onStart:onStart];
    if (str != NULL) {
        UnitySendMessage(g_pw_listenerName, "onPushNotificationsOpened", str);
    }
}

@end
