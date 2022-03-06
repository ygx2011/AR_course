#ifndef SHOWTRAJECTORY_H
#define SHOWTRAJECTORY_H


#pragma   push_macro("min")  
#pragma   push_macro("max")  
#undef   min  
#undef   max 
#include <pcl/io/pcd_io.h>
#include <pcl/point_types.h> 
#include <pcl/visualization/pcl_visualizer.h>
#pragma   pop_macro("min")  
#pragma   pop_macro("max")



#include "map.h"
#include "KeyFrame.h"


void GetPointSet(ORB_SLAM::Map* world, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr Points);

void GetPointSetRun(ORB_SLAM::Map* world, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr Points);

void DrawTrajectory(pcl::visualization::PCLVisualizer viewer, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr PointSet);

void DrawTrajectoryRun(pcl::visualization::PCLVisualizer viewer, 
	pcl::PointCloud<pcl::PointXYZ>::Ptr PointSet);

#endif