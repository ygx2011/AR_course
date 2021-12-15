#ifndef PLANAR_TRACKING_H
#define PLANAR_TRACKING_H

#include <opencv2/opencv.hpp>
#include <opencv2/legacy/legacy.hpp>
#include <opencv2/features2d/features2d.hpp>

using namespace std;
using namespace cv;

class Tracker
{
public:
    Tracker(Mat _K) : K(_K) {}
    //Tracker(){}

    void setFirstFrame(const char * first_frame_path);

    bool process(const Mat frame_left, bool slamMode);

    glm::mat4 getInitModelMatrix();

protected:
    Mat K, rvec, tvec;
    Mat first_frame, first_desc;
    vector<KeyPoint> first_kp;
    vector<Point2f> object_bb;
};

#endif //BOB_AR_PLANAR_TRACKING_H
