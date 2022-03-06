#include <random>
#include <Eigen/Core>
#include <vector>
#include <array>
#include <set>
#include "PlaneModel.h"

#include <opencv2\core\core.hpp>
#include <opencv2\highgui\highgui.hpp>
#include <iostream>

#include<iostream>
#include<fstream>
#include<boost/thread.hpp>

#include "Tracking.h"
#include "Map.h"
#include "LocalMapping.h"
#include "LoopClosing.h"
#include "KeyFrameDatabase.h"
#include "ORBVocabulary.h"

#include "Converter.h"

#include "SaveLoadWorld.h"
#include <opencv2\opencv.hpp>

#include "error.h"
using namespace std;

extern "C" __declspec(dllexport) ERROR_CODE Initialize(int);
extern "C" __declspec(dllexport) void choose_Plane(float [],float []);
extern "C" __declspec(dllexport) ERROR_CODE process_Image(uchar [],float [],float [],bool &,bool &, bool &);
extern "C" __declspec(dllexport) void Release();

static bool isStop = FALSE;

cv::VideoCapture capCam;

ORB_SLAM::Tracking *Tracker;
ORB_SLAM::LocalMapping *LocalMapper;
ORB_SLAM::LoopClosing *LoopCloser;

ORB_SLAM::ORBVocabulary* Vocabulary;

ORB_SLAM::KeyFrameDatabase* Database;
 //Create the map
ORB_SLAM::Map* World;

std::vector<cv::Point3f> ygxPoints;
/*
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
*/
///////////////////////////////////////////////////////////////////////////////////////////
PlaneModel ransac(const std::vector<Eigen::Vector3d>& data, double threshold, int numIterations)
{
    if (data.size()<PlaneModel::ModelSize)
        throw std::runtime_error("Not enough data");
    
    int bestInliers=-1;
    PlaneModel bestModel;
    
    for (int it=0;it<numIterations;it++)
    {
        // select points
        int found=0;
        std::array<size_t,PlaneModel::ModelSize> indices;
        std::set<size_t> usedIndices;
        while (found < PlaneModel::ModelSize)
        {
			unsigned int randomSeed = std::rand();
            size_t sample = (size_t)(randomSeed % (data.size()-1));
            if (usedIndices.find(sample)!=usedIndices.end())
                continue;
            usedIndices.insert(sample);
            indices[found++]=sample;
        }
        
        // compute model
        PlaneModel m;
        m.compute(data,indices);
        
        int inliers=m.computeInliers(data,threshold);
        if (inliers>bestInliers)
        {
            bestModel=m;
            bestInliers=inliers;
        }
    }
    bestModel.refine(data,threshold);
    return bestModel;
}
///////////////////////////////////////////////////////////////////////////////////////////

ERROR_CODE Initialize(int deviceID)
{
	capCam.open(deviceID);
	if (!capCam.isOpened())
	{
		return CAMERA_OPEN_FAILED;
	}
	capCam.set(CV_CAP_PROP_FRAME_WIDTH, 640);
	capCam.set(CV_CAP_PROP_FRAME_HEIGHT, 480);
	capCam.set(CV_CAP_PROP_FRAME_COUNT, 30);
	
	string strSettingsFile = "./Settings.yaml";
    cv::FileStorage fsSettings(strSettingsFile.c_str(), cv::FileStorage::READ);
    if(!fsSettings.isOpened())
    {
        return WRONG_SETTING_FILE_PATH;
    }

    //Load ORB Vocabulary
	string strVocFile = "./ORBvoc.bin";
	// cout << "Loading Vocabulary!" << endl;
	
    Vocabulary = new ORB_SLAM::ORBVocabulary();
	Vocabulary->loadFromBinaryFile(strVocFile);

   // cout << "Vocabulary loaded!" << endl << endl;

    //Create KeyFrame Database
    Database = new ORB_SLAM::KeyFrameDatabase(*Vocabulary);

    //Create the map
    World = new ORB_SLAM::Map();

	// ------------------------------------
    KeyFrame *tpLastKF;
    bool bReadOK = LoadWroldFromFile(Database, World, Vocabulary, tpLastKF);
    if(bReadOK)
    {
        //cout<<"load world file successfully."<<endl;

        vector<KeyFrame*> vpAllKFs=World->GetAllKeyFrames();
        for(vector<KeyFrame*>::iterator vit=vpAllKFs.begin(),vend=vpAllKFs.end();vit!=vend;vit++)
        {
            KeyFrame* pKF = *vit;
            if(!pKF)
			{
                //cout<<"pKF==NULL"<<endl;
			}
            KeyFrame* pKFpar = pKF->GetParent();
            set<KeyFrame*> spKFch = pKF->GetChilds();
            //cout<<"pKF-"<<pKF->mnId;
            if(pKF->mnId!=0)
			{
                //cout<<", parent-"<<pKFpar->mnId;
			}
            if(spKFch.empty())
            {
                cout<<endl;
                continue;
            }
            //cout<<", children-";
            for(set<KeyFrame*>::iterator sit=spKFch.begin(),send=spKFch.end();sit!=send;sit++)
            {
                KeyFrame* pKFc = *sit;
                if(!pKFc)
				{
                    //cout<<"pKFc==NULL"<<endl;
				}
                //cout<<pKFc->mnId<<", ";
            }
            //cout<<endl;
        }
    }
    else
    {
        //cout<<"load world file failed."<<endl;
        // operations
        World->clear();
        Database->clear();
    }
    // ------------------------------------

    //Initialize the Tracking Thread and launch
    Tracker = new ORB_SLAM::Tracking(Vocabulary,/* &FramePub, &MapPub,*/ World, strSettingsFile/*, capCam*/);
    boost::thread trackingThread(&ORB_SLAM::Tracking::Run,Tracker);

    Tracker->SetKeyFrameDatabase(Database);

	// ------------------------------------
    if(bReadOK)
    {
        Tracker->mState = Tracking::LOST;
        Tracker->mLastProcessedState = Tracking::LOST;
        Tracker->SetLastKeyframe(tpLastKF);
        Tracker->SetLastFrameId(Frame::nNextId-1);
    }
    // ------------------------------------

    //Initialize the Local Mapping Thread and launch
    LocalMapper = new ORB_SLAM::LocalMapping(World);
    boost::thread localMappingThread(&ORB_SLAM::LocalMapping::Run,LocalMapper);

    //Initialize the Loop Closing Thread and launch
    LoopCloser = new ORB_SLAM::LoopClosing(World, Database, Vocabulary);
    boost::thread loopClosingThread(&ORB_SLAM::LoopClosing::Run, LoopCloser);

    //Set pointers between threads
    Tracker->SetLocalMapper(LocalMapper);
    Tracker->SetLoopClosing(LoopCloser);

    LocalMapper->SetTracker(Tracker);
    LocalMapper->SetLoopCloser(LoopCloser);

    LoopCloser->SetTracker(Tracker);
    LoopCloser->SetLocalMapper(LocalMapper);    

	return WORKING;
}
/*
void choose_Plane(float P[],float E[])
{
	float sumX = 0.0;
	float sumY = 0.0;
	float sumZ = 0.0;
	CvMat *points_mat = cvCreateMat(ygxPoints.size(), 3, CV_32FC1);//定义用来存储需要拟合点的矩阵
	for (int i=0;i < ygxPoints.size(); ++i)
	{
		points_mat->data.fl[i * 3 + 0] = ygxPoints[i].x;//X的坐标值
		points_mat->data.fl[i * 3 + 1] = ygxPoints[i].y;//Y的坐标值
		points_mat->data.fl[i * 3 + 2] = ygxPoints[i].z;//Z的坐标值

		sumX += ygxPoints[i].x;
		sumY += ygxPoints[i].y;
		sumZ += ygxPoints[i].z;
	}
	float plane12[4] = { 0 };//定义用来储存平面参数的数组
	cvFitPlane(points_mat, plane12);//调用方程
	float meanX = sumX / ygxPoints.size();
	float meanY = sumY / ygxPoints.size();
	float meanZ = sumZ / ygxPoints.size();

	P[0] = meanX;
	P[1] = meanY;
	P[2] = meanZ;

	E[0] = plane12[0];
	E[1] = plane12[1];
	E[2] = plane12[2];
}
*/
void choose_Plane(float P[],float E[])
{
	float sumX = 0.0;
	float sumY = 0.0;

	std::vector<Eigen::Vector3d> points;
	for (int i=0;i < ygxPoints.size(); ++i)
	{
		Eigen::Vector3d point = Eigen::Vector3d(ygxPoints[i].x,ygxPoints[i].y,ygxPoints[i].z);
		points.push_back(point);

		sumX += ygxPoints[i].x;
		sumY += ygxPoints[i].y;
	}

	const PlaneModel m = ransac(points,0.05,300);

	E[0] = (float)m.n[0];
	E[1] = (float)m.n[1];
	E[2] = (float)m.n[2];

	float meanX = sumX / ygxPoints.size();
	float meanY = sumY / ygxPoints.size();

	P[0] = meanX;
	P[1] = meanY;
	P[2] = ((float)m.d-(E[0]*meanX)-(E[1]*meanY))/E[2];
}

ERROR_CODE process_Image(uchar imgData[],float R[],float T[],bool &isTracking,bool &mapPoints,bool &drawBox)
{
	    isTracking = false;

		cv::Mat im;
		capCam >> im;

		cv::Mat im_ygx = im.clone();
		cv::cvtColor(im_ygx, im_ygx, CV_BGR2RGB);
		memcpy(imgData,im_ygx.data,640*480*3); 
		
		Tracker->GrabImage(im);
		//cv::imshow("orbslam",im);
		//cv::waitKey(5);

		switch (Tracker->mState)
		{
		case -1:
			{
				return SYSTEM_NOT_READY;	
			}
		case 0:
			{
				return SYSTEM_NOT_READY;	
			}
		case 1:
			{
				return NOT_INITIALIZED;
			}
		case 2:
			{
				return INITIALIZING;
			}
		case 3:
			{
				isTracking = true;

				cv::Mat R_Temp = Tracker->GetPose_R();
				memcpy(R,R_Temp.data,R_Temp.cols*R_Temp.rows*sizeof(float));
				
				cv::Mat T_Temp = Tracker->GetPose_T();				
				memcpy(T,T_Temp.data,T_Temp.cols*T_Temp.rows*sizeof(float));
				//cout<<"working"<<endl;

				if(mapPoints == true)
				{
					ygxPoints.clear();
					std::vector<cv::Point3f> ygx_allMapPoints = Tracker->Get_allMapPoints();
					cv::Mat rVec;
                    cv::Rodrigues(R_Temp, rVec);
                    cv::Mat tVec = T_Temp.clone();
                    std::vector<cv::Point2f> ygx_projectedPoints;
				    cv::projectPoints(ygx_allMapPoints, rVec, tVec, Tracker->mK, Tracker->mDistCoef, ygx_projectedPoints);
				    for (size_t j = 0; j < ygx_projectedPoints.size(); ++j)
                    {
                        cv::Point2f r1 = ygx_projectedPoints[j];
                        cv::circle(im, cv::Point(r1.x, r1.y), 2, cv::Scalar(0, 255, 0), 1, 8);

						if(r1.x>=260 && r1.y>=180 && r1.x<=380 && r1.y<=300)
						{
							ygxPoints.push_back(ygx_allMapPoints[j]);
						}
                    }
					cv::Mat im_ygx = im.clone();
					cv::cvtColor(im_ygx, im_ygx, CV_BGR2RGB);
					memcpy(imgData,im_ygx.data,640*480*3);
				}

				if(drawBox == true)
				{
					cv::rectangle(im, cvPoint(260,180),cvPoint(380,300), cv::Scalar(0,0,255), 2);
					cv::Mat im_ygx = im.clone();
					cv::cvtColor(im_ygx, im_ygx, CV_BGR2RGB);
					memcpy(imgData,im_ygx.data,640*480*3);
				}

				//cv::imshow("orbslam",im);
		        //cv::waitKey(5);
				
				return WORKING;
			}
		case 4:
			{
				//cout<<"lost"<<endl;
				return TRACKING_LOST;
			}
		default:
			{
				return UNSPECIFIC_ERROR;
			}
		}
}

void Release()
{
	SaveWorldToFile(*World,*Database);

	delete LocalMapper;
	delete LoopCloser;

	delete Tracker;

	delete Vocabulary;
	delete Database;
	delete World;

	capCam.release();

	exit(0);
}

/*
int main()
{
	uchar imgData[640*480*3] = {0};
	float R[9] = {0};
	float T[3] = {0};

	float P[3] = {0};
	float E[3] = {0};

	bool isTracking;
	bool mapPoints = true;
	
	Initialize(1);

	choose_Plane(P,E);

	while(true)
	{
		process_Image(imgData,R,T,isTracking,mapPoints);
	}
	Release();
	return 0;
}
*/