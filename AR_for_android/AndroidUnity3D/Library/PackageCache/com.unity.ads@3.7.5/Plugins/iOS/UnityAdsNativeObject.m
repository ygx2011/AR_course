#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C" {
#endif

void UnityAdsBridgeTransfer(void *x) {
    if (!x) return;

    (__bridge_transfer id)x;
}

#ifdef __cplusplus
}
#endif
