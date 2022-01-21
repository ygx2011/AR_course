//
//  ViewController.h
//  iosOpenCVdemo
//
//  Created by 应高选 on 15/10/29.
//  Copyright © 2015年 应高选. All rights reserved.
//

#import <UIKit/UIKit.h>

#import <opencv2/highgui/ios.h>

@interface ViewController : UIViewController<CvVideoCameraDelegate>

- (IBAction)startButtonPressed:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *start;
@property (weak, nonatomic) IBOutlet UIImageView *imageView;
@property (nonatomic, strong) CvVideoCamera* videoCamera;

@end

