//
//  PWInAppManager.h
//  Pushwoosh SDK
//  (c) Pushwoosh 2018
//

#import "PWMUserNotificationCenterDelegateProxy.h"

@interface PWMUserNotificationCenterDelegateProxy()<UNUserNotificationCenterDelegate>

@property (nonatomic, weak) id<UNUserNotificationCenterDelegate> pushDelegate;
@property (nonatomic, weak) id<UNUserNotificationCenterDelegate> prevDelegate;

@end

@implementation PWMUserNotificationCenterDelegateProxy


+ (BOOL)setupWithPushDelegate:(id<UNUserNotificationCenterDelegate>)pushDelegate {
    static PWMUserNotificationCenterDelegateProxy *instance = nil;
    if (instance)
        return YES;
    NSNumber *use = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"PWUserNotificationCenterDelegateProxy"];
    if (use && [use boolValue] == NO) {
        [UNUserNotificationCenter currentNotificationCenter].delegate = pushDelegate;
        return NO;
    }
    instance = [PWMUserNotificationCenterDelegateProxy new];
    instance.pushDelegate = pushDelegate;
    instance.prevDelegate = [UNUserNotificationCenter currentNotificationCenter].delegate;
    [UNUserNotificationCenter currentNotificationCenter].delegate = instance;
    
    return YES;
}

- (BOOL)isPush:(UNNotification *)notification {
    return [notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]] || [notification.request.content.userInfo objectForKey:@"aps"] || [notification.request.content.userInfo objectForKey:@"pw_push"];
}

- (BOOL)isPushwooshNotification:(UNNotification *)notification {
    return (!_prevDelegate || [self isPush:notification]);
}

- (void)userNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler {
    if ([self isPushwooshNotification:notification]) {
        if ([_pushDelegate respondsToSelector:@selector(userNotificationCenter:willPresentNotification:withCompletionHandler:)]) {
            [_pushDelegate userNotificationCenter:center willPresentNotification:notification withCompletionHandler:completionHandler];
        }
    } else {
        if ([_prevDelegate respondsToSelector:@selector(userNotificationCenter:willPresentNotification:withCompletionHandler:)]) {
            [_prevDelegate userNotificationCenter:center willPresentNotification:notification withCompletionHandler:completionHandler];
        }
    }
}

- (void)userNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void(^)(void))completionHandler {
    if ([self isPushwooshNotification:response.notification]) {
        if ([_pushDelegate respondsToSelector:@selector(userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:)]) {
            [_pushDelegate userNotificationCenter:center didReceiveNotificationResponse:response withCompletionHandler:completionHandler];
        }
    } else {
        if ([_prevDelegate respondsToSelector:@selector(userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:)]) {
            [_prevDelegate userNotificationCenter:center didReceiveNotificationResponse:response withCompletionHandler:completionHandler];
        }
    }
}

@end
