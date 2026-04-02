//
//  PWInAppManager.h
//  Pushwoosh SDK
//  (c) Pushwoosh 2018
//

#import <Foundation/Foundation.h>
#import <UserNotifications/UserNotifications.h>

@interface PWMUserNotificationCenterDelegateProxy : NSObject

+ (BOOL)setupWithPushDelegate:(id<UNUserNotificationCenterDelegate>)pushDelegate;

@end
