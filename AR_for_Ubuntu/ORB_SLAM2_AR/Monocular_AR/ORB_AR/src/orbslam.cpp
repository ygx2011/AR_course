#include <iostream>

#include <opencv2/opencv.hpp>

#include <glm/glm.hpp>
#include <glm/gtx/transform.hpp>

#include "ORB_SLAM/System.h"

using namespace std;
using namespace cv;

Mat RCalib, TCalib, P1, P2, K;
Mat R, tvec, initRvec, initTvec;
cv::Mat cvToGl = cv::Mat::zeros(4, 4, CV_64F);

Mat getCameraMatrix(){
    return K;
}

glm::mat4 getViewMatrix(bool slamMode){
    glm::mat4 V;
    Mat viewMatrix = cv::Mat::zeros(4, 4, CV_64FC1);

    if (slamMode){
        R.convertTo(R, CV_64F);
        tvec.convertTo(tvec, CV_64F);

        for(unsigned int row=0; row<3; ++row)
        {
            for(unsigned int col=0; col<3; ++col)
            {
                viewMatrix.at<double>(row, col) = R.at<double>(row, col);
            }
            viewMatrix.at<double>(row, 3) = tvec.at<double>(row, 0);
        }
        viewMatrix.at<double>(3, 3) = 1.0f;

        viewMatrix = cvToGl * viewMatrix;
    } else{
        viewMatrix = cvToGl;
    }

    viewMatrix.convertTo(viewMatrix, CV_32F);


    for (int i = 0; i < 4; i++){
        for (int j = 0; j < 4; j++) {
            V[i][j] = viewMatrix.at<float>(j,i);
        }
    }

    return V;

}

bool initTracking(const char * Extrinsics_path){

    cvToGl.at<double>(0, 0) = 1.0f;
    cvToGl.at<double>(1, 1) = -1.0f;
    // Invert the y axis
    cvToGl.at<double>(2, 2) = -1.0f;
    // invert the z axis
    cvToGl.at<double>(3, 3) = 1.0f;


    cv::FileStorage f1;
    f1.open(Extrinsics_path, cv::FileStorage::READ);

    if (f1.isOpened())
    {
        f1["R"] >> RCalib;
        f1["T"] >> TCalib;
        f1["P1"] >> P1;
        f1["P2"] >> P2;
        f1.release();
    }
    else
    {
        cout << "Couldn't open Remap.xml or Extrinsics.xml" << endl;
        return 0;
    }

    P1.rowRange(0,3).colRange(0,3).copyTo(K);

    return 1;
}

bool trackStereo(Mat CameraPose){

    if (CameraPose.empty()) {
        return 0;
    }

    CameraPose.rowRange(0,3).colRange(0,3).copyTo(R);
    CameraPose.rowRange(0,3).col(3).copyTo(tvec);
    
    float t0 = tvec.at<float>(0, 0);
    float t1 = tvec.at<float>(1, 0);
    float t2 = tvec.at<float>(2, 0);
    
    tvec.at<float>(0, 0) = 8.9 * t0;
    tvec.at<float>(1, 0) = 8.9 * t1;
    tvec.at<float>(2, 0) = 8.9 * t2;

    return 1;
}