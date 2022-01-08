
#include <jni.h>
#include <opencv2/opencv.hpp>
using namespace cv;
#include <iostream>
#include <fstream>
using namespace std;
#include "System.h"
#include <iomanip>
#include<string>
#include<thread>
#include<opencv2/core/core.hpp>

#include "Tracking.h"
#include "FrameDrawer.h"
#include "MapDrawer.h"
#include "Map.h"
#include "LocalMapping.h"
//#include "LoopClosing.h"
#include "KeyFrameDatabase.h"
#include "ORBVocabulary.h"
#include "Viewer.h"

string settings_file = "/storage/emulated/0/4DAR/Settings.yaml";
string voc_file = "/storage/emulated/0/4DAR/ORBvoc.bin";

Mat pose;

vector<Point3f> ygx_allMapPoints;

static float transform_ygx[] =
{
    1, 0, 0, 0,
    0, -1, 0, 0,
    0, 0, -1, 0,
    0, 0, 0, 1
};
Mat_<float> transformM(4, 4, transform_ygx);

///////////////////////////////////////////////////////////////////////////////////////////
void cvFitPlane(const CvMat* points, float* plane)
{
    // Estimate geometric centroid.
    int nrows = points->rows;
    int ncols = points->cols;
    int type = points->type;
    CvMat* centroid = cvCreateMat(1, ncols, type);
    cvSet(centroid, cvScalar(0));
    for (int c = 0; c<ncols; c++){
        for (int r = 0; r < nrows; r++)
        {
            centroid->data.fl[c] += points->data.fl[ncols*r + c];
        }
        centroid->data.fl[c] /= nrows;
    }
    // Subtract geometric centroid from each point.
    CvMat* points2 = cvCreateMat(nrows, ncols, type);
    for (int r = 0; r<nrows; r++)
        for (int c = 0; c<ncols; c++)
            points2->data.fl[ncols*r + c] = points->data.fl[ncols*r + c] - centroid->data.fl[c];
    // Evaluate SVD of covariance matrix.
    CvMat* A = cvCreateMat(ncols, ncols, type);
    CvMat* W = cvCreateMat(ncols, ncols, type);
    CvMat* V = cvCreateMat(ncols, ncols, type);
    cvGEMM(points2, points, 1, NULL, 0, A, CV_GEMM_A_T);
    cvSVD(A, W, NULL, V, CV_SVD_V_T);
    // Assign plane coefficients by singular vector corresponding to smallest singular value.
    plane[ncols] = 0;
    for (int c = 0; c<ncols; c++){
        plane[c] = V->data.fl[ncols*(ncols - 1) + c];
        plane[ncols] += plane[c] * centroid->data.fl[c];
    }
    // Release allocated resources.
    cvReleaseMat(&centroid);
    cvReleaseMat(&points2);
    cvReleaseMat(&A);
    cvReleaseMat(&W);
    cvReleaseMat(&V);
}
///////////////////////////////////////////////////////////////////////////////////////////

ORB_SLAM2::System SLAM(voc_file, settings_file, ORB_SLAM2::System::MONOCULAR, false);

extern "C"
{
    void choose_Plane(float P[],float E[])
    {
        const vector<ORB_SLAM2::MapPoint*> vpMPs = SLAM.mpTracker->mpMap->GetAllMapPoints();
        for (size_t i = 0; i < vpMPs.size(); i++)
        {
            if (vpMPs[i] && !vpMPs[i]->isBad())
            {
                cv::Point3f pos = cv::Point3f(vpMPs[i]->GetWorldPos());
                ygx_allMapPoints.push_back(pos);
            }
        }
        
        float sumX = 0.0;
        float sumY = 0.0;
        //float sumZ = 0.0;
        CvMat *points_mat = cvCreateMat((int)(ygx_allMapPoints.size()), 3, CV_32FC1);//定义用来存储需要拟合点的矩阵
        for (int i=0;i < ygx_allMapPoints.size(); ++i)
        {
            points_mat->data.fl[i * 3 + 0] = ygx_allMapPoints[i].x;//X的坐标值
            points_mat->data.fl[i * 3 + 1] = ygx_allMapPoints[i].y;//Y的坐标值
            points_mat->data.fl[i * 3 + 2] = ygx_allMapPoints[i].z;//Z的坐标值
            
            sumX += ygx_allMapPoints[i].x;
            sumY += ygx_allMapPoints[i].y;
            //sumZ += ygx_allMapPoints[i].z;
        }
        float plane12[4] = { 0 };//定义用来储存平面参数的数组
        cvFitPlane(points_mat, plane12);//调用方程
        
        E[0] = -plane12[0];
        E[1] = -fabs(plane12[2]);
        E[2] = plane12[1];
        
        float meanX = sumX / ygx_allMapPoints.size();
        float meanY = sumY / ygx_allMapPoints.size();
        //float meanZ = sumZ / ygx_allMapPoints.size();
        float meanZ = (plane12[3]-(plane12[0]*meanX)-(plane12[1]*meanY))/plane12[2];
        
        P[0] = -meanX;
        P[1] = -meanZ;
        P[2] = meanY;
    }
}

extern "C"
{
    int process_Image (uchar * ImageData, float ygxT[], float ygxR[], bool &isShow)
    {
        isShow = false;
        
        Mat img_ygx;
        img_ygx = Mat(480, 640, CV_8UC4, ImageData);
        
        if(img_ygx.empty())
        {
            return -1;
        }
        
        Mat img = img_ygx.clone();
        
        transpose(img, img);
        flip(img, img, 1);
        flip(img, img, 1);
        
        //resize(img, img, Size(240, 320));
        
        Mat imgTemp_gray;
        cvtColor(img, imgTemp_gray, CV_RGBA2GRAY);
        
        pose = SLAM.TrackMonocular(imgTemp_gray, 0);
        
        if(!pose.empty())
        {
            isShow = true;
            
            Mat r,t;
            pose.rowRange(0,3).colRange(0,3).copyTo(r);
            pose.rowRange(0,3).col(3).copyTo(t);
            
            float R[9];
            float T[3];
            
            memcpy(R, r.data, r.cols*r.rows*sizeof(float));
            memcpy(T, t.data, t.cols*t.rows*sizeof(float));
            
            float qw = sqrtf(1.0f + R[0] + R[4] + R[8]) / 2.0f;
            float qx = -(R[7] - R[5]) / (4*qw) ;
            float qy = (R[3] - R[1]) / (4*qw) ;
            float qz = -(R[2] - R[6]) / (4*qw) ;
            
            float rotate_ygx[] =
            {
                1 - 2 * qy * qy - 2 * qz * qz, 2 * qx * qy - 2 * qz * qw, 2 * qx * qz + 2 * qy * qw, 0,
                2 * qx * qy + 2 * qz * qw, 1 - 2 * qx * qx - 2 * qz * qz, 2 * qy * qz - 2 * qx * qw, 0,
                2 * qx * qz - 2 * qy * qw, 2 * qy * qz + 2 * qx * qw, 1 - 2 * qx * qx - 2 * qy * qy, 0,
                0, 0, 0, 1
            };
            
            Mat_<float> rotateM(4, 4, rotate_ygx);
            
            rotateM = transformM * rotateM;
            
            ygxR[0] = rotateM.at<float>(0,0);
            ygxR[1] = rotateM.at<float>(1,0);
            ygxR[2] = rotateM.at<float>(2,0);
            ygxR[3] = rotateM.at<float>(0,1);
            ygxR[4] = rotateM.at<float>(1,1);
            ygxR[5] = rotateM.at<float>(2,1);
            ygxR[6] = rotateM.at<float>(0,2);
            ygxR[7] = rotateM.at<float>(1,2);
            ygxR[8] = rotateM.at<float>(2,2);
            
            ygxT[0] = T[0];
            ygxT[1] = -T[2];
            ygxT[2] = T[1];
        }
        
        return 0;
    }
}

extern "C"
{
    void reset()
    {
        SLAM.Reset();
    }
}

