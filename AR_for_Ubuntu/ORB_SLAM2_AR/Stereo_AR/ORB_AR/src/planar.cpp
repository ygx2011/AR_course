#include <glm/glm.hpp>
#include <glm/gtx/transform.hpp>

#include <opencv2/opencv.hpp>
#include "planar_tracking.h"
#include "utils.h"


using namespace cv;
using namespace std;

glm::mat4 Tracker::getInitModelMatrix(){

    glm::mat4 initModelMatrix;
    Mat initR;
    Mat viewMatrix = cv::Mat::zeros(4, 4, CV_64FC1);
    Rodrigues(rvec, initR);

    for(unsigned int row=0; row<3; ++row)
    {
        for(unsigned int col=0; col<3; ++col)
        {
            viewMatrix.at<double>(row, col) = initR.at<double>(row, col);
        }
        viewMatrix.at<double>(row, 3) = tvec.at<double>(row, 0);
    }
    viewMatrix.at<double>(3, 3) = 1.0f;

    //viewMatrix = cvToGl * viewMatrix;

    viewMatrix.convertTo(viewMatrix, CV_32F);


    for (int i = 0; i < 4; i++){
        for (int j = 0; j < 4; j++) {
            initModelMatrix[i][j] = viewMatrix.at<float>(j,i);
        }
    }

    return initModelMatrix;

}

void Tracker::setFirstFrame(const char * first_frame_path)
{
    first_frame = imread(first_frame_path);
    vector<KeyPoint> kp;

    object_bb.push_back(Point2f(0,0));
    object_bb.push_back(Point2f(8,0));
    object_bb.push_back(Point2f(8,6));
    object_bb.push_back(Point2f(0,6));

    vector<Point2f> bb;
    bb.push_back(Point2f(0,0));
    bb.push_back(Point2f(640,0));
    bb.push_back(Point2f(640,480));
    bb.push_back(Point2f(0,480));

    Mat H;
    H = findHomography(bb, object_bb);
    
    ORB orb;
    orb(first_frame, Mat(), kp, first_desc);

    vector<Point2f> tmp_kp_orgn, tmp_kp_homo;

    first_kp = kp;

    for (int i = 0; i <= kp.size(); i++) {
        tmp_kp_orgn.push_back(Point2f(kp[i].pt));
    }

    perspectiveTransform(tmp_kp_orgn, tmp_kp_homo, H);

    for (int i = 0; i <= kp.size(); i++) {
        first_kp[i].pt = tmp_kp_homo[i];
    }
}

bool Tracker::process(const Mat frame_left, bool slamMode)
{

    if (slamMode)
        return 1;

    vector<KeyPoint> kp;
    vector<Point3f> ObjectPoints;
    vector<Point2f> ImagePoints;
    Mat desc;
    
    ORB orb;
    orb(frame_left, Mat(), kp, desc);
    
    if(kp.size()<10)
    {
        return 0;
    }
    
    BruteForceMatcher<HammingLUT> matcher;
    vector<DMatch> matches;
    matcher.match(first_desc, desc, matches);
    
    double max_dist = 0; double min_dist = 100;
    for( int i = 0; i < first_desc.rows; i++ )
    {
        double dist = matches[i].distance;
        if( dist < min_dist ) min_dist = dist;
        if( dist > max_dist ) max_dist = dist;
    }
    std::vector< DMatch > good_matches;
    vector<Point2f> train_p, query_p;
    for( int i = 0; i < first_desc.rows; i++ )
    {
        if( matches[i].distance < 0.5*max_dist )
        {
            good_matches.push_back( matches[i]);
            
            int query_i = matches[i].queryIdx;
            int train_i = matches[i].trainIdx;
            KeyPoint query_kp = first_kp[query_i];
            KeyPoint train_kp = kp[train_i];
            train_p.push_back(train_kp.pt);
            query_p.push_back(query_kp.pt);
        }
    }
    
    Mat inlier_mask, homography;
    
    int thd = (int)(0.08*first_desc.rows);
    
    if(train_p.size() >= thd)
    {
        homography = findHomography( query_p, train_p, RANSAC, 2.5f, inlier_mask);
    }
    
    if(train_p.size() < thd || homography.empty())
    {
        return 0;
    }
    
    for(int i = 0; i < train_p.size(); i++)
    {
        if(inlier_mask.at<uchar>(i))
        {
            ObjectPoints.push_back(Point3f(query_p[i].x, query_p[i].y, 0));
            ImagePoints.push_back(Point2f(train_p[i].x, train_p[i].y));
        }
    }
    
    solvePnP(ObjectPoints, ImagePoints, K, noArray(), rvec, tvec);

    return 1;
}
