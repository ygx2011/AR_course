//#include "stdafx.h"

#include "ShowTrajectory.h"

//获取相机轨迹
void GetPointSet(ORB_SLAM::Map* world, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr Points)
{
	vector<ORB_SLAM::KeyFrame*> vpKFs = world->GetAllKeyFrames();
	sort(vpKFs.begin(),vpKFs.end(),ORB_SLAM::KeyFrame::lId);

	Points->clear();

	pcl::PointXYZ p;
	for (int i = 0; i < vpKFs.size(); i++)
	{
		if(vpKFs[i]->isBad())
			continue;
		p.x = vpKFs[i]->GetCameraCenter().at<float>(0);
		p.y = vpKFs[i]->GetCameraCenter().at<float>(1);
		p.z = vpKFs[i]->GetCameraCenter().at<float>(2);
		Points->points.push_back(p);
	}
}

void GetPointSetRun(ORB_SLAM::Map* world, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr Points)
{
	Sleep(10000);
	while(1)
	{
		if (world->GetAllKeyFrames().size() != 0)
		{
			GetPointSet(world, Points);
		}
		else
			continue;
		Sleep(5000);
	}
}

//画轨迹
void DrawTrajectory(pcl::visualization::PCLVisualizer viewer, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr PointSet)
{
	pcl::PointXYZ P1, P2;
	char DrawSign[5];
	for (int i = 0; i < PointSet->size() - 1; i++)
	{
		P1 = PointSet->points[i];
		P2 = PointSet->points[i+1];
		
		itoa(i, DrawSign, 10);
		viewer.addLine(P1, P2, DrawSign);
	}
}

void DrawTrajectoryRun(pcl::visualization::PCLVisualizer viewer, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr PointSet)
{
	Sleep(20000);
	while(1)
	{
		if (PointSet->size() <= 0)
		{
			continue;
		}
		DrawTrajectory(viewer, PointSet);

		viewer.spinOnce (100);
		boost::this_thread::sleep (boost::posix_time::milliseconds (100));
	}
}

