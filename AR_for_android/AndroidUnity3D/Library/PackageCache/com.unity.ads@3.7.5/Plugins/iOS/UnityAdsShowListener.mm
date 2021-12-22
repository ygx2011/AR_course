#import "UnityAdsShowListener.h"

@implementation UnityAdsShowListener
- (id)initWithFailureCallback:(ShowFailureCallback)showFailureCallback startCallback:(ShowStartCallback)showStartCallback clickCallback:(ShowClickCallback)showClickCallback completeCallback:(ShowCompleteCallback)showCompleteCallback {
    self = [super init];

    if (self) {
        self.showFailureCallback = showFailureCallback;
        self.showStartCallback = showStartCallback;
        self.showClickCallback = showClickCallback;
        self.showCompleteCallback = showCompleteCallback;
    }

    return self;
}

- (void)unityAdsShowFailed:(NSString *)placementId withError:(UnityAdsShowError)error withMessage:(NSString *)message {
    if (self.showFailureCallback) {
        self.showFailureCallback((__bridge void *)self, [placementId UTF8String], (int)error, [message UTF8String]);
    }
}

- (void)unityAdsShowStart:(NSString *)placementId {
    if (self.showStartCallback) {
        self.showStartCallback((__bridge void *)self, [placementId UTF8String]);
    }
}

- (void)unityAdsShowClick:(NSString *)placementId {
    if (self.showClickCallback) {
        self.showClickCallback((__bridge void *)self, [placementId UTF8String]);
    }
}

- (void)unityAdsShowComplete:(NSString *)placementId withFinishState:(UnityAdsShowCompletionState)state {
    if (self.showCompleteCallback) {
        self.showCompleteCallback((__bridge void *)self, [placementId UTF8String], (int)state);
    }
}

@end

#ifdef __cplusplus
extern "C" {
#endif

void * UnityAdsShowListenerCreate(ShowFailureCallback showFailureCallback, ShowStartCallback showStartCallback, ShowClickCallback showClickCallback, ShowCompleteCallback showCompleteCallback) {
    UnityAdsShowListener *listener = [[UnityAdsShowListener alloc] initWithFailureCallback:showFailureCallback startCallback:showStartCallback clickCallback:showClickCallback completeCallback:showCompleteCallback];
    return (__bridge_retained void *)listener;
}

void UnityAdsShowListenerDestroy(void *ptr) {
    if (!ptr) return;

    UnityAdsShowListener *listener = (__bridge_transfer UnityAdsShowListener *)ptr;

    listener.showFailureCallback = nil;
    listener.showStartCallback = nil;
    listener.showClickCallback = nil;
    listener.showCompleteCallback = nil;
}

#ifdef __cplusplus
}
#endif
