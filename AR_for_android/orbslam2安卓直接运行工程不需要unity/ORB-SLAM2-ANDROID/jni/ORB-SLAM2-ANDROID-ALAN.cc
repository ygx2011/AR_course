
// Alan Zhang
// 2016.9.1

#include <jni.h>
#include <opencv2/opencv.hpp>
using namespace cv;
#include <iostream>
#include <fstream>
using namespace std;
#include "System.h"
#include "com_example_orb_slam2_android_alan_NativeLoaderORB.h"
#include <iomanip>
#include<string>
#include<thread>
#include<opencv2/core/core.hpp>

#include "Tracking.h"
#include "FrameDrawer.h"
#include "MapDrawer.h"
#include "Map.h"
#include "LocalMapping.h"
#include "LoopClosing.h"
#include "KeyFrameDatabase.h"
#include "ORBVocabulary.h"
#include "Viewer.h"

string settings_file = "/storage/emulated/0/orbslam/redmi320240.yaml";
string voc_file = "/storage/emulated/0/orbslam/ORBvoc.bin";

ORB_SLAM2::System SLAM(voc_file, settings_file, ORB_SLAM2::System::MONOCULAR, true);

JNIEXPORT void JNICALL Java_com_example_orb_1slam2_1android_1alan_NativeLoaderORB_runSLAM_1Alan
	(JNIEnv *env, jobject obj, jlong inputImage)
{
	Mat *imgData = (Mat *)inputImage;
	Mat imggray;
	cvtColor(*imgData, imggray, CV_BGR2GRAY);

	//double t=(double)cvGetTickCount();

	SLAM.TrackMonocular(imggray, 0);

	//t=((double)cvGetTickCount() - t)/(cvGetTickFrequency()*1000);

	//ofstream slamtotalcost("/storage/sdcard0/SLAM_ALAN/time.txt", ios::app);
	//slamtotalcost<<"total: "<<t<<endl;
	//slamtotalcost.close();

}

JNIEXPORT jfloatArray JNICALL Java_com_example_orb_1slam2_1android_1alan_NativeLoaderORB_getRTfromSLAM
	(JNIEnv *env, jobject obj)
{
	const cv::Mat Rt = (SLAM.mpTracker->mCurrentFrame.mTcw).clone();
	jfloat *reusltfloat = new jfloat[4*4];

	if(!Rt.empty()){
		for(int i=0; i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				reusltfloat[(i*4+j)] = Rt.at<float>(j,i);
			}
		}
	}

	int size = 4*4;
	jfloatArray result = env->NewFloatArray(size);
	env->SetFloatArrayRegion(result, 0, size, reusltfloat);
	return result;
}


