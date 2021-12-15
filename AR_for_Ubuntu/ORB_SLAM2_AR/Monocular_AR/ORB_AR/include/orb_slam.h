#ifndef ORB_SLAM_H
#define ORB_SLAM_H
#include "ORB_SLAM/System.h"

glm::mat4 getViewMatrix(bool slamMode);
Mat getCameraMatrix();
void stereoRemap(Mat frame_left, Mat frame_right, Mat& frame_left_rectified, Mat& frame_right_rectified);
bool initTracking(const char * Extrincis_path);
bool trackStereo(Mat CameraPose);

#endif //BOB_AR_ORB_SLAM_H
