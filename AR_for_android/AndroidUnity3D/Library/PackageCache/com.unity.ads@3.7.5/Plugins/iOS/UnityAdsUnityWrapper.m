#import "UnityAppController.h"
#import "Unity/UnityInterface.h"

#import "UnityAds/UnityAds.h"
#import <UnityAds/UADSBanner.h>
#import "UnityAds/UADSMetaData.h"

#import "UnityAdsUtilities.h"
#import "UnityAdsPurchasingWrapper.h"
#import "UnityAdsInitializationListener.h"
#import "UnityAdsLoadListener.h"
#import "UnityAdsShowListener.h"
#import <UnityAds/UnityAdsFinishState.h>

typedef void (*UnityAdsReadyCallback)(const char * placementId);
typedef void (*UnityAdsDidErrorCallback)(long rawError, const char * message);
typedef void (*UnityAdsDidStartCallback)(const char * placementId);
typedef void (*UnityAdsDidFinishCallback)(const char * placementId, long rawFinishState);

static UnityAdsReadyCallback readyCallback = NULL;
static UnityAdsDidErrorCallback errorCallback = NULL;
static UnityAdsDidStartCallback startCallback = NULL;
static UnityAdsDidFinishCallback finishCallback = NULL;

@interface UnityAdsUnityWrapperDelegate : NSObject <UnityAdsDelegate>
@end

@implementation UnityAdsUnityWrapperDelegate

- (void)unityAdsReady:(NSString *)placementId {
    if(readyCallback != NULL) {
        const char * rawPlacementId = UnityAdsCopyString([placementId UTF8String]);
        readyCallback(rawPlacementId);
        free((void *)rawPlacementId);
    }
}

- (void)unityAdsDidError:(UnityAdsError)error withMessage:(NSString *)message {
    if(errorCallback != NULL) {
        const char * rawMessage = UnityAdsCopyString([message UTF8String]);
        errorCallback(error, rawMessage);
        free((void *)rawMessage);
    }
}

- (void)unityAdsDidStart:(NSString *)placementId {
    UnityPause(1);
    if(startCallback != NULL) {
        const char * rawPlacementId = UnityAdsCopyString([placementId UTF8String]);
        startCallback(rawPlacementId);
        free((void *)rawPlacementId);
    }
}

- (void)unityAdsDidFinish:(NSString *)placementId withFinishState:(UnityAdsFinishState)state {
    UnityPause(0);
    if(finishCallback != NULL) {
        const char * rawPlacementId = UnityAdsCopyString([placementId UTF8String]);
        finishCallback(rawPlacementId, state);
        free((void *)rawPlacementId);
    }
}

@end

void UnityAdsInitialize(const char * gameId, bool testMode, bool enablePerPlacementLoad, void *listenerPtr) {
    UnityAdsInitializationListener *listener = listenerPtr ? (__bridge UnityAdsInitializationListener *)listenerPtr : nil;
    [UnityAds initialize:[NSString stringWithUTF8String:gameId] testMode:testMode enablePerPlacementLoad:enablePerPlacementLoad initializationDelegate:listener];
    InitializeUnityAdsPurchasingWrapper();
}

void UnityAdsLoad(const char * placementId, void *listenerPtr) {
    UnityAdsLoadListener *listener = listenerPtr ? (__bridge UnityAdsLoadListener *)listenerPtr : nil;
    [UnityAds load:[NSString stringWithUTF8String:placementId] loadDelegate:listener];
}

void UnityAdsShow(const char * placementId, void *listenerPtr) {
    UnityAdsShowListener *listener = listenerPtr ? (__bridge UnityAdsShowListener *)listenerPtr : nil;
    [UnityAds show:UnityGetGLViewController() placementId:NSSTRING_OR_EMPTY(placementId) showDelegate:listener];
}

const char *UnityAdsGetDefaultPlacementID() {
    NSString *returnedPlacementID = @"";
    id placement = NSClassFromString(@"UADSPlacement");
    if (placement) {
        SEL getPlacementSelector = NSSelectorFromString(@"getDefaultPlacement");
        if ([placement respondsToSelector:getPlacementSelector]) {
            IMP getPlacementIMP = [placement methodForSelector:getPlacementSelector];
            id (*getPlacementFunc)(void) = (void *) getPlacementIMP;
            NSString *placementString = getPlacementFunc();
            if (placementString != NULL) {
                returnedPlacementID = placementString;
            }
        }
    }
    return CStringFromNSString(returnedPlacementID);
}

bool UnityAdsGetDebugMode() {
    return [UnityAds getDebugMode];
}

void UnityAdsSetDebugMode(bool debugMode) {
    [UnityAds setDebugMode:debugMode];
}

bool UnityAdsIsSupported() {
    return [UnityAds isSupported];
}

bool UnityAdsIsReady(const char * placementId) {
    if(placementId == NULL) {
        return [UnityAds isReady];
    } else {
        return [UnityAds isReady:[NSString stringWithUTF8String:placementId]];
    }
}

long UnityAdsGetPlacementState(const char * placementId) {
    if(placementId == NULL) {
        return [UnityAds getPlacementState];
    } else {
        return [UnityAds getPlacementState:[NSString stringWithUTF8String:placementId]];
    }
}

const char * UnityAdsGetVersion() {
    return UnityAdsCopyString([[UnityAds getVersion] UTF8String]);
}

bool UnityAdsIsInitialized() {
    return [UnityAds isInitialized];
}

void UnityAdsSetMetaData(const char * category, const char * data) {
    if(category != NULL && data != NULL) {
        UADSMetaData* metaData = [[UADSMetaData alloc] initWithCategory:[NSString stringWithUTF8String:category]];
        NSDictionary* json = [NSJSONSerialization JSONObjectWithData:[[NSString stringWithUTF8String:data] dataUsingEncoding:NSUTF8StringEncoding] options:0 error:nil];
        for(id key in json) {
            [metaData set:key value:[json objectForKey:key]];
        }
        [metaData commit];
    }
}

void UnityAdsSetReadyCallback(UnityAdsReadyCallback callback) {
    readyCallback = callback;
}

void UnityAdsSetDidErrorCallback(UnityAdsDidErrorCallback callback) {
    errorCallback = callback;
}

void UnityAdsSetDidStartCallback(UnityAdsDidStartCallback callback) {
    startCallback = callback;
}

void UnityAdsSetDidFinishCallback(UnityAdsDidFinishCallback callback) {
    finishCallback = callback;
}
