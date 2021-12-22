#import <Foundation/Foundation.h>
#import <UnityAds/UnityAdsDelegate.h>
#import <UnityAds/UnityAdsPlacementState.h>

@interface UnityAdsDelegateUtil : NSObject

+ (void)unityAdsReady: (NSString *)placementId;
+ (void)unityAdsDidError: (UnityAdsError)error withMessage: (NSString *)message;
+ (void)unityAdsDidStart: (NSString *)placementId;
+ (void)unityAdsDidFinish: (NSString *)placementId
          withFinishState: (UnityAdsFinishState)state;
+ (void)unityAdsDoClick: (NSString *)placementId;
+ (void)unityAdsPlacementStateChange: (NSString *)placementId oldState: (UnityAdsPlacementState)oldState newState: (UnityAdsPlacementState)newState;

@end
