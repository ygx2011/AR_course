#import "UnityAdsInitializationListener.h"

@implementation UnityAdsInitializationListener

- (id)initWithSuccessCallback:(InitSuccessCallback)initSuccessCallback failCallback:(InitFailCallback)initFailCallback {
    self = [super init];

    if (self) {
        self.initSuccessCallback = initSuccessCallback;
        self.initFailCallback = initFailCallback;
    }

    return self;
}

- (void)initializationFailed:(UnityAdsInitializationError)error withMessage:(NSString *)message {
    if (self.initFailCallback) {
        self.initFailCallback((__bridge void *)self, (int)error, [message UTF8String]);
    }
}

- (void)initializationComplete {
    if (self.initSuccessCallback) {
        self.initSuccessCallback((__bridge void *)self);
    }
}

@end

#ifdef __cplusplus
extern "C" {
#endif

void * UnityAdsInitializationListenerCreate(InitSuccessCallback initSuccessCallback, InitFailCallback initFailCallback) {
    UnityAdsInitializationListener *listener = [[UnityAdsInitializationListener alloc] initWithSuccessCallback:initSuccessCallback failCallback:initFailCallback];
    return (__bridge_retained void *)listener;
}

void UnityAdsInitializationListenerDestroy(void *ptr) {
    if (!ptr) return;

    UnityAdsInitializationListener *listener = (__bridge_transfer UnityAdsInitializationListener *)ptr;

    listener.initSuccessCallback = nil;
    listener.initFailCallback = nil;
}

#ifdef __cplusplus
}
#endif
