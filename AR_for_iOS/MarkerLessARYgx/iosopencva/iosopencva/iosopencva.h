//
//  iosopencva.h
//  iosopencva
//
//  Created by Ying Gaoxuan on 16/8/31.
//  Copyright © 2016年 Ying Gaoxuan. All rights reserved.
//

#import <Foundation/Foundation.h>

#include <opencv2/opencv.hpp>

using namespace cv;
using namespace std;

#ifdef __cplusplus
extern "C"
{
#endif
    
    extern void Initialize();
    
    extern int process_Image(uchar * ImageData, /*float R[],*/ float T[], float wxyz[], bool &isShow);
    
#ifdef __cplusplus
}
#endif
