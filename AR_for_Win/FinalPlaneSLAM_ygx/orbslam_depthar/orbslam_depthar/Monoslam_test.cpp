// Monoslam_test.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include<iostream>
#include<fstream>

//#include<ros/ros.h>
//#include<ros/package.h>
#include<boost/thread.hpp>

#include<opencv2/core/core.hpp>

#include "Tracking.h"
#include "FramePublisher.h"
#include "Map.h"
#include "MapPublisher.h"
#include "LocalMapping.h"
#include "LoopClosing.h"
#include "KeyFrameDatabase.h"
#include "ORBVocabulary.h"
//#include "ComputeScale.h"

#include "Converter.h"

#include <windows.h>

//可视化
//#include "ShowTrajectory.h"

using namespace std;

//ORB_SLAM的windows版本
//基于Raul Mur-Artal的源码修改
int _tmain(int argc, _TCHAR* argv[])
{
    cout << endl << "ORB-SLAM Copyright (C) 2014 Raul Mur-Artal" << endl;
	cout << endl << "***************  ORB-SLAM (Windows Version)  ***************" << endl;

	//ORB_SLAM::AbsScale* ScaleCompute = new ORB_SLAM::AbsScale();
	//boost::thread ScaleComputeThread(&ORB_SLAM::AbsScale::Run, ScaleCompute);

	//int h = 480, w = 640;
//	//左摄像头的参数(320 * 240)
//#pragma region Para_l
//
//	double *mi_l;
//	double *md_l;
//	mi_l = new double[3*3];
//	md_l = new double[5];
//
//	CvMat intrinsic_matrix_l,distortion_coeffs_l;
//	//摄像机内参数
//	cvInitMatHeader(&intrinsic_matrix_l,3,3,CV_64FC1,mi_l);
//	//镜头畸变参数
//	cvInitMatHeader(&distortion_coeffs_l,1,5,CV_64FC1,md_l);
//
//	//320*240 120度广角,参数由matlab获得
//	double fc1_l,fc2_l,cc1_l,cc2_l,kc1_l,kc2_l,kc3_l,kc4_l,kc5_l;
//	fc1_l = 911.209558394816;
//	fc2_l = 914.479682240085;
//	cc1_l = 373.892499278010;
//	cc2_l = 289.533979829650;
//	kc1_l = -0.0449198968106401;
//	kc2_l = 1.00795051665001;
//	kc3_l = 0.00492335728168905;
//	kc4_l = 0.00709819148693618;
//	kc5_l = 0;
//
//	cvmSet(&intrinsic_matrix_l, 0, 0, fc1_l);
//	cvmSet(&intrinsic_matrix_l, 0, 1, 0);
//	cvmSet(&intrinsic_matrix_l, 0, 2, cc1_l);
//	cvmSet(&intrinsic_matrix_l, 1, 0, 0);
//	cvmSet(&intrinsic_matrix_l, 1, 1, fc2_l);
//	cvmSet(&intrinsic_matrix_l, 1, 2, cc2_l);
//	cvmSet(&intrinsic_matrix_l, 2, 0, 0);
//	cvmSet(&intrinsic_matrix_l, 2, 1, 0);
//	cvmSet(&intrinsic_matrix_l, 2, 2, 1);
//
//	cvmSet(&distortion_coeffs_l, 0, 0, kc1_l);
//	cvmSet(&distortion_coeffs_l, 0, 1, kc2_l);
//	cvmSet(&distortion_coeffs_l, 0, 2, kc3_l);
//	cvmSet(&distortion_coeffs_l, 0, 3, kc4_l);
//	cvmSet(&distortion_coeffs_l, 0, 4, kc5_l);
//
//#pragma endregion 
//
//	//右摄像头的参数(320 * 240)
//#pragma region Para_r
//
//	double *mi_r;
//	double *md_r;
//	mi_r = new double[3*3];
//	md_r = new double[5];
//
//	CvMat intrinsic_matrix_r,distortion_coeffs_r;
//	//摄像机内参数
//	cvInitMatHeader(&intrinsic_matrix_r,3,3,CV_64FC1,mi_r);
//	//镜头畸变参数
//	cvInitMatHeader(&distortion_coeffs_r,1,5,CV_64FC1,md_r);
//
//	//320*240 120度广角,参数由matlab获得
//	double fc1_r,fc2_r,cc1_r,cc2_r,kc1_r,kc2_r,kc3_r,kc4_r,kc5_r;
//	fc1_r = 915.754897786877;
//	fc2_r = 915.478804690998;
//	cc1_r = 346.397451714853;
//	cc2_r = 239.610477260551;
//	kc1_r = -0.0346309017572761;
//	kc2_r = 0.286728081699172;
//	kc3_r = -0.000412430514445309;
//	kc4_r = 0.00649992054572432;
//	kc5_r = 0;
//
//	cvmSet(&intrinsic_matrix_r, 0, 0, fc1_r);
//	cvmSet(&intrinsic_matrix_r, 0, 1, 0);
//	cvmSet(&intrinsic_matrix_r, 0, 2, cc1_r);
//	cvmSet(&intrinsic_matrix_r, 1, 0, 0);
//	cvmSet(&intrinsic_matrix_r, 1, 1, fc2_r);
//	cvmSet(&intrinsic_matrix_r, 1, 2, cc2_r);
//	cvmSet(&intrinsic_matrix_r, 2, 0, 0);
//	cvmSet(&intrinsic_matrix_r, 2, 1, 0);
//	cvmSet(&intrinsic_matrix_r, 2, 2, 1);
//
//	cvmSet(&distortion_coeffs_r, 0, 0, kc1_r);
//	cvmSet(&distortion_coeffs_r, 0, 1, kc2_r);
//	cvmSet(&distortion_coeffs_r, 0, 2, kc3_r);
//	cvmSet(&distortion_coeffs_r, 0, 3, kc4_r);
//	cvmSet(&distortion_coeffs_r, 0, 4, kc5_r);
//
//#pragma endregion 
//
//	//双目系统参数
//#pragma region Para_r2l(320*240)
//	double *R_r2l;
//	double *T_r2l;
//	R_r2l = new double[3*3];
//	T_r2l = new double[3];
//
//	CvMat rotation_matrix, translation_matrix;
//	//摄像机内参数
//	cvInitMatHeader(&rotation_matrix,3,3,CV_64FC1,R_r2l);
//	//镜头畸变参数
//	cvInitMatHeader(&translation_matrix,3,1,CV_64FC1,T_r2l);
//
//	cvmSet(&rotation_matrix, 0, 0, 0.999627850781327);
//	cvmSet(&rotation_matrix, 1, 0, 0.0128547422481060);
//	cvmSet(&rotation_matrix, 2, 0, 0.0240606638320564);
//	cvmSet(&rotation_matrix, 0, 1, -0.0127762255256435);
//	cvmSet(&rotation_matrix, 1, 1, 0.999912551919367);
//	cvmSet(&rotation_matrix, 2, 1, -0.00341417272799277);
//	cvmSet(&rotation_matrix, 0, 2, -0.0241024480835945);
//	cvmSet(&rotation_matrix, 1, 2, 0.00310549767886456);
//	cvmSet(&rotation_matrix, 2, 2, 0.999704670330465);
//
//	cvmSet(&translation_matrix, 0, 0, -101.371528966913);
//	cvmSet(&translation_matrix, 1, 0, -2.81510529390187);
//	cvmSet(&translation_matrix, 2, 0, -10.3699782319836);
//
//#pragma endregion
//
//	//双目校正
//#pragma region Undistort
//	CvMat* mx1 = cvCreateMat( h, w, CV_32F );
//	CvMat* my1 = cvCreateMat( h, w, CV_32F );
//	CvMat* mx2 = cvCreateMat( h, w, CV_32F );
//	CvMat* my2 = cvCreateMat( h, w, CV_32F );
//	double R1[3][3], R2[3][3], P1[3][4], P2[3][4], Q[4][4];
//	CvMat t_R1 = cvMat(3, 3, CV_64F, R1);
//	CvMat t_R2 = cvMat(3, 3, CV_64F, R2);
//	CvMat t_Q = cvMat(4, 4, CV_64F, Q );
//	CvRect roi1, roi2;
//
//	CvMat t_P1 = cvMat(3, 4, CV_64F, P1);
//	CvMat t_P2 = cvMat(3, 4, CV_64F, P2);
//	cvStereoRectify( &intrinsic_matrix_l, &intrinsic_matrix_r,
//		&distortion_coeffs_l, &distortion_coeffs_r, cvSize(w,h),
//		&rotation_matrix, &translation_matrix, 
//		&t_R1, &t_R2, &t_P1, &t_P2, &t_Q,
//		CV_CALIB_ZERO_DISPARITY,
//		0,cvSize(w,h), &roi1, &roi2); 
//	// 计算mx1,my1,mx2,my2用于cvRemap()
//	cvInitUndistortRectifyMap(&intrinsic_matrix_l,
//		&distortion_coeffs_l,&t_R1,&t_P1, mx1, my1);
//	cvInitUndistortRectifyMap(&intrinsic_matrix_r,
//		&distortion_coeffs_r,&t_R2,&t_P2, mx2, my2);
//#pragma endregion
//
//	//视差图计算元
//#pragma region StereoBM
//	cv::StereoBM* bm = new cv::StereoBM;
//	int numberOfDisparities = 0;
//	numberOfDisparities = numberOfDisparities > 0 ? numberOfDisparities : ((w/8) + 15) & -16;  
//
//	bm->state->roi1 = roi1;  
//	bm->state->roi2 = roi2;  
//	bm->state->preFilterCap = 31;
//
//	int SADWindowSize = 0;
//	bm->state->SADWindowSize = SADWindowSize > 0 ? SADWindowSize : 9;  
//	bm->state->minDisparity = 0;  
//	bm->state->numberOfDisparities = numberOfDisparities;  
//	bm->state->textureThreshold = 10;  
//	bm->state->uniquenessRatio = 15;  
//	bm->state->speckleWindowSize = 100;  
//	bm->state->speckleRange = 32;  
//	bm->state->disp12MaxDiff = 1;
//#pragma endregion




	//定义相机
	cv::VideoCapture* Camera_L = new cv::VideoCapture(0);
	Camera_L->set(CV_CAP_PROP_FRAME_WIDTH,640);
	Camera_L->set(CV_CAP_PROP_FRAME_HEIGHT,480);

	//cv::VideoCapture* Camera_R = new cv::VideoCapture(1);
	//Camera_R->set(CV_CAP_PROP_FRAME_WIDTH,640);
	//Camera_R->set(CV_CAP_PROP_FRAME_HEIGHT,480);

	if (!Camera_L->isOpened())
	{
		cout << "打开摄像头失败！" << endl;
		cvWaitKey();
	}

	//保存为avi视频
	//cv::VideoWriter* writer = new cv::VideoWriter("VideoTest.avi", -1, 20.0, cv::Size(640, 500),true);
	//if (!writer->isOpened())
	//{
	//	cout << "VideoWriter ERROR!" << endl;
	//}

	// 读取设置文件，并校验
	string strSettingsFile = "Settings.yaml";

	cv::FileStorage* fsSettings = new cv::FileStorage(strSettingsFile.c_str(), cv::FileStorage::READ);
	if(!fsSettings->isOpened())
	{
		cout << "Wrong path to settings. Path must be absolut or relative to ORB_SLAM package directory." << endl;
		return 1;
	}

	//Create Frame Publisher for image_view
	ORB_SLAM::FramePublisher* FramePub = new ORB_SLAM::FramePublisher;

	//载入Vocabulary
	string strVocFile = "ORBvoc.yml";
	cout << endl << "Loading ORB Vocabulary. This could take a while." << endl;
	cv::FileStorage* fsVoc = new cv::FileStorage(strVocFile.c_str(), cv::FileStorage::READ);
	if(!fsVoc->isOpened())
	{
		cout << endl << "Wrong path to vocabulary. Path must be absolut or relative to ORB_SLAM package directory." << endl;
		return 1;
	}
	ORB_SLAM::ORBVocabulary* Vocabulary = new ORB_SLAM::ORBVocabulary;
	Vocabulary->load(*fsVoc);
	cout << "Vocabulary loaded!" << endl << endl;
	delete fsVoc;

	//Create KeyFrame Database
	ORB_SLAM::KeyFrameDatabase* Database = new ORB_SLAM::KeyFrameDatabase(*Vocabulary);

	////Create the map
	ORB_SLAM::Map* World = new ORB_SLAM::Map;
	FramePub->SetMap(World);
	//FramePub->writer = *writer; //定义视频输出

	////Create Map Publisher for Rviz
	ORB_SLAM::MapPublisher* MapPub = new ORB_SLAM::MapPublisher(World);

	//Initialize the Tracking Thread and launch
	//ORB_SLAM::Tracking Tracker(&Vocabulary, 
	//	//&FramePub, &MapPub, 
	//	&World, strSettingsFile);
	ORB_SLAM::Tracking* Tracker = new ORB_SLAM::Tracking(Vocabulary, 
		FramePub, 
		MapPub, 
		World, strSettingsFile);
	Tracker->Camera = *Camera_L;
	//Tracker->Camera_R = *Camera_R;
	boost::thread trackingThread(&ORB_SLAM::Tracking::Run,Tracker);

	Tracker->SetKeyFrameDatabase(Database);

	//Initialize the Local Mapping Thread and launch
	ORB_SLAM::LocalMapping* LocalMapper = new ORB_SLAM::LocalMapping(World);
	boost::thread localMappingThread(&ORB_SLAM::LocalMapping::Run,LocalMapper);

	//Initialize the Loop Closing Thread and launch
	ORB_SLAM::LoopClosing* LoopCloser = new ORB_SLAM::LoopClosing(World, Database, Vocabulary);
	boost::thread loopClosingThread(&ORB_SLAM::LoopClosing::Run, LoopCloser);

	//创建一个线程计算尺度
	//ORB_SLAM::AbsScale* ScaleCompute = new ORB_SLAM::AbsScale(FramePub, World);
	//boost::thread ScaleComputeThread(&ORB_SLAM::AbsScale::Run, ScaleCompute, FramePub, World);


    //Set pointers between threads
    Tracker->SetLocalMapper(LocalMapper);
    Tracker->SetLoopClosing(LoopCloser);

    LocalMapper->SetTracker(Tracker);
    LocalMapper->SetLoopCloser(LoopCloser);

    LoopCloser->SetTracker(Tracker);
    LoopCloser->SetLocalMapper(LocalMapper);


	//This "main" thread will show the current processed frame and publish the map
	float fps = fsSettings->state["Camera.fps"];
	if(fps==0)
		fps=30;

	LARGE_INTEGER d_Freq;
	QueryPerformanceFrequency(&d_Freq);
	LARGE_INTEGER d_Start;
	LARGE_INTEGER d_Now;


	boost::mutex mMutexSave;

	int k = 0;   //按键标识
	int SaveImageNum = 0;
	float MapScale = 1;  //模型尺度

	bool isScale = false;


	//保存采集点的旋转矩阵
	//cv::FileStorage SaveRotation("ParasCalculation\\Img\\RotationMat.xml", cv::FileStorage::WRITE);
	//time_t rawtime; time(&rawtime);
	//SaveRotation << "calibrationDate" << asctime(localtime(&rawtime));
	//SaveRotation << "RotMat" << "[";

	while(1)
	{
		QueryPerformanceCounter(&d_Start);
		{
			FramePub->Refresh();
			MapPub->Refresh();
			Tracker->CheckResetByPublishers();
			
			k = cv::waitKey(100);
			if (k == 13) //回车退出
			{
				break;
			}
			//else if(k == 32 ) //空格采集
			//{
			//	boost::mutex::scoped_lock lock0(mMutexSave);

			//	char str[10];
			//	
			//	pcl::PointCloud<pcl::PointXYZRGB>::Ptr SaveCurrentDatas(new pcl::PointCloud<pcl::PointXYZRGB>);
			//	pcl::PointXYZRGB SaveCurrentData;
			//	SaveCurrentDatas->clear();

			//	vector<ORB_SLAM::MapPoint*> MatchedMapPoints = FramePub->GetMatchedMapPoints();
			//	vector<bool> Outliers = FramePub->GetOutliers();
			//	vector<cv::KeyPoint> CurrentKeys = FramePub->GetCurrentKeys();

			//	cv::Mat CurrentImg = FramePub->GetCurrentImage();

			//	cv::Mat CurrentPose = Tracker->mCurrentFrame.mTcw;
			//	cv::Mat CurrentImg2 = Tracker->mCurrentFrame.im;

			//	cv::Mat CurrentRwc = CurrentPose.rowRange(0,3).colRange(0,3).t();
			//	cv::Mat Currenttwc = -CurrentRwc*CurrentPose.rowRange(0,3).col(3);

			//	for(unsigned int i = 0; i < MatchedMapPoints.size();i++)
			//	{
			//		if(MatchedMapPoints[i] || Outliers[i])
			//		{
			//			if(!Outliers[i])
			//			{
			//				SaveCurrentData.x = MatchedMapPoints[i]->GetWorldPos().at<float>(0);
			//				SaveCurrentData.y = MatchedMapPoints[i]->GetWorldPos().at<float>(1);
			//				SaveCurrentData.z = MatchedMapPoints[i]->GetWorldPos().at<float>(2);

			//				SaveCurrentData.rgb = ((int)CurrentKeys[i].pt.y * 640) + (int)CurrentKeys[i].pt.x;

			//				SaveCurrentDatas->push_back(SaveCurrentData);
			//			}
			//		}
			//	}

				
				//_itoa_s(SaveImageNum, str, 10);
				//cv::imwrite("ParasCalculation\\Img\\left_" + (string)str + ".bmp", FramePub->CurrentLeftImage);
				//pcl::io::savePCDFileBinary("ParasCalculation\\Img\\" + (string)str + ".pcd", *SaveCurrentDatas);
				//cv::imwrite("ParasCalculation\\Img\\camera_" + (string)str + ".bmp", CurrentImg);
				//cv::imwrite("ParasCalculation\\Img\\camera2_" + (string)str + ".bmp", CurrentImg2);

				//SaveRotation << "{:";
				//SaveRotation << "TranVec" << Currenttwc;
				//SaveRotation << "RotMatrix" << CurrentRwc;
				//if (k == 32)
				//{
					//SaveRotation << "Type" << 0;
				//}
				//SaveRotation << "}";

				//cout << SaveImageNum << " saved!" << endl;
				//imshow("Saved", FramePub->CurrentLeftImage);

				//SaveImageNum++;
			//}
		}
		for(;;)
		{
			QueryPerformanceCounter(&d_Now);
			double time=( ((d_Now.QuadPart - 
				d_Start.QuadPart)*1000000)/(double)d_Freq.QuadPart);
			if (time >= 2000) //r(500)
				break;
		}
	}

	//SaveRotation << "]";
	//SaveRotation.release();  //旋转矩阵保存结束

	//PC:摄像机关键点； PW：Map中特征点
	pcl::PointCloud<pcl::PointXYZ>::Ptr PC(new pcl::PointCloud<pcl::PointXYZ>);
	pcl::PointCloud<pcl::PointXYZ>::Ptr PW(new pcl::PointCloud<pcl::PointXYZ>);
	
	//摄像机关键点
	vector<ORB_SLAM::KeyFrame*> vpKFs = World->GetAllKeyFrames();
	sort(vpKFs.begin(),vpKFs.end(),ORB_SLAM::KeyFrame::lId);

	pcl::PointXYZ p;
	for (int i = 0; i < vpKFs.size(); i++)
	{
		if(vpKFs[i]->isBad())
			continue;
		p.x = vpKFs[i]->GetCameraCenter().at<float>(0);
		p.y = vpKFs[i]->GetCameraCenter().at<float>(1);
		p.z = vpKFs[i]->GetCameraCenter().at<float>(2);
		PC->points.push_back(p);
	}


	//Map中特征点
	vector<ORB_SLAM::MapPoint*> vMapPoints = World->GetAllMapPoints();

	for (int i = 0; i < vMapPoints.size(); i++)
	{
		if(vMapPoints[i]->isBad())
			continue;
		p.x = vMapPoints[i]->GetWorldPos().at<float>(0);
		p.y = vMapPoints[i]->GetWorldPos().at<float>(1);
		p.z = vMapPoints[i]->GetWorldPos().at<float>(2);
		PW->points.push_back(p);
	}

	pcl::visualization::PCLVisualizer viewer("3D Trajectory");
	viewer.setBackgroundColor(0,0,0);
	viewer.initCameraParameters();

	pcl::PointXYZ PP1, PP2;
	char DrawSign[5];
	for (int i = 0; i < PC->size() - 1; i++)
	{
		PP1 = PC->points[i];
		PP2 = PC->points[i+1];
		
		itoa(i, DrawSign, 10);
		viewer.addLine(PP1, PP2, DrawSign);
	}

	viewer.addPointCloud(PW);

	pcl::io::savePCDFileBinary("PC.pcd", *PC);
	pcl::io::savePCDFileBinary("PW.pcd", *PW);

	while (!viewer.wasStopped ())
	{
		viewer.spinOnce (100);
		boost::this_thread::sleep (boost::posix_time::microseconds (100000));
		int keyy = 0;
		keyy = cvWaitKey(100);
		if (keyy == 13)
		{
			//writer->~VideoWriter();
			break;
		}
	}

	delete FramePub;
	delete MapPub;
	delete Tracker;
	delete LocalMapper;
	delete LoopCloser;


	return 0;
}

