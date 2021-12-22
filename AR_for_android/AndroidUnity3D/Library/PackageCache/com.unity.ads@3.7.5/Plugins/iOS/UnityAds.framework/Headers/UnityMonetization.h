#import <UnityAds/UMONPlacementContent.h>
#import <UnityAds/UMONRewardablePlacementContent.h>
#import <UnityAds/UMONShowAdPlacementContent.h>
#import <UnityAds/UMONPromoAdPlacementContent.h>
#import <UnityAds/UMONNativePromoAdapter.h>
#import <UnityAds/UnityMonetizationDelegate.h>
#import <UnityAds/UnityMonetizationPlacementContentState.h>

NS_ASSUME_NONNULL_BEGIN

__attribute__((deprecated("Please use the UnityAds interface")))
@interface UnityMonetization : NSObject
+ (void)setDelegate: (id <UnityMonetizationDelegate>)delegate;
+ (nullable id <UnityMonetizationDelegate>)getDelegate;
+ (BOOL)isReady: (NSString *)placementId;
+ (nullable UMONPlacementContent *)getPlacementContent: (NSString *)placementId;

+ (void)initialize: (NSString *)gameId
          delegate: (nullable id<UnityMonetizationDelegate>)delegate;

+ (void)initialize: (NSString *)gameId
          delegate: (nullable id<UnityMonetizationDelegate>)delegate
          testMode: (BOOL)testMode;
@end

NS_ASSUME_NONNULL_END
