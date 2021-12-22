#import "UnityAdsLoadListener.h"

@implementation UnityAdsLoadListener

- (id)initWithSuccessCallback:(LoadSuccessCallback)loadSuccessCallback failCallback:(LoadFailureCallback)loadFailureCallback {
    self = [super init];

    if (self) {
        self.loadSuccessCallback = loadSuccessCallback;
        self.loadFailureCallback = loadFailureCallback;
    }

    return self;
}

- (void)unityAdsAdFailedToLoad:(NSString *)placementId withError:(UnityAdsLoadError)error withMessage:(NSString *)message {
    if (self.loadFailureCallback) {
        self.loadFailureCallback((__bridge void *)self, [placementId UTF8String], (int)error, [message UTF8String]);
    }
}

- (void)unityAdsAdLoaded:(NSString *)placementId {
    if (self.loadSuccessCallback) {
        self.loadSuccessCallback((__bridge void *)self, [placementId UTF8String]);
    }
}

@end

#ifdef __cplusplus
extern "C" {
#endif

void * UnityAdsLoadListenerCreate(LoadSuccessCallback loadSuccessCallback, LoadFailureCallback loadFailureCallback) {
    UnityAdsLoadListener *listener = [[UnityAdsLoadListener alloc] initWithSuccessCallback:loadSuccessCallback failCallback:loadFailureCallback];
    return (__bridge_retained void *)listener;
}

void UnityAdsLoadListenerDestroy(void *ptr) {
    if (!ptr) return;

    UnityAdsLoadListener *listener = (__bridge_transfer UnityAdsLoadListener *)ptr;

    listener.loadSuccessCallback = nil;
    listener.loadFailureCallback = nil;
}

#ifdef __cplusplus
}
#endif
