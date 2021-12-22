#include <jni.h>
#include <opencv2/opencv.hpp>
#include <opencv2/legacy/legacy.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/features2d/features2d.hpp>
using namespace cv;
#include <iostream>
#include <fstream>
using namespace std;
#include <math.h>

Mat r, t;
Mat template_img;
bool istracking;

ORB orb;
vector<KeyPoint> keyPoints_1;
Mat descriptors_1;

Mat prev_gray;
Mat curr_gray;
vector<Point2f> prev_keyPoints;
vector<Point2f> curr_keyPoints;
vector<Point2f> prev_corners;
vector<Point2f> curr_corners;

vector<Mat> prevPyr, nextPyr;
vector<unsigned char> track_status;

float f_x = 640.0f;
float f_y = 640.0f;
float c_x = 240.0f;
float c_y = 320.0f;

float camera_matrix[] =
{
    f_x, 0.0f, c_x,
    0.0f, f_y, c_y,
    0.0f, 0.0f, 1.0f
};

float dist_coeff[] = {0.0f, 0.0f, 0.0f, 0.0f};
vector<Point3f> m_corners_3d;
Mat m_camera_matrix = Mat(3, 3, CV_32FC1, camera_matrix).clone();
Mat m_dist_coeff = Mat(1, 4, CV_32FC1, dist_coeff).clone();

void estimateTransformToCamera(vector<Point3f> corners_3d, vector<Point2f> corners_2d, cv::Mat& camera_matrix, cv::Mat& dist_coeff, cv::Mat& rmat, cv::Mat& tvec)
{
    Mat rot_vec;
    solvePnP(corners_3d, corners_2d, camera_matrix, dist_coeff, rot_vec, tvec);
    Rodrigues(rot_vec, rmat);
}

Mat createMask(cv::Size img_size, vector<Point2f>& pts)
{
    Mat mask(img_size,CV_8UC1);
    float zero = 0;
    mask = zero;
    
    // ax+by+c=0
    float a[4];
    float b[4];
    float c[4];
    
    a[0] = pts[3].y - pts[0].y;
    a[1] = pts[2].y - pts[1].y;
    a[2] = pts[1].y - pts[0].y;
    a[3] = pts[2].y - pts[3].y;
    
    b[0] = pts[0].x - pts[3].x;
    b[1] = pts[1].x - pts[2].x;
    b[2] = pts[0].x - pts[1].x;
    b[3] = pts[3].x - pts[2].x;
    
    c[0] = pts[0].y * pts[3].x - pts[3].y * pts[0].x;
    c[1] = pts[1].y * pts[2].x - pts[2].y * pts[1].x;
    c[2] = pts[0].y * pts[1].x - pts[1].y * pts[0].x;
    c[3] = pts[3].y * pts[2].x - pts[2].y * pts[3].x;
    
    float max_x, min_x, max_y, min_y;
    max_x = 0;
    min_x = img_size.width;
    max_y = 0;
    min_y = img_size.height;
    
    int i;
    for(i=0;i<4;i++){
        if(pts[i].x > max_x)
            max_x = pts[i].x;
        if(pts[i].x < min_x)
            min_x = pts[i].x;
        if(pts[i].y > max_y)
            max_y = pts[i].y;
        if(pts[i].y < min_y)
            min_y = pts[i].y;
    }
    if(max_x >= img_size.width)
        max_x = img_size.width - 1;
    if(max_y >= img_size.height)
        max_y = img_size.height - 1;
    if(min_x < 0)
        min_x = 0;
    if(min_y < 0)
        min_y = 0;
    
    unsigned char *ptr = mask.data;
    int x,y;
    int offset;
    float val[4];
    for(y=min_y; y<=max_y; y++){
        offset = y * img_size.width;
        for(x=min_x; x<=max_x; x++){
            for(i=0; i<4; i++){
                val[i] = a[i]*x + b[i]*y + c[i];
            }
            if(val[0]*val[1] <= 0 && val[2]*val[3] <= 0)
                *(ptr + offset + x)=255;
        }
    }
    
    return mask;
}

extern "C"
{
    void Initialize()
    {
        template_img = imread("/storage/emulated/0/ygx/template.bmp");
        
        orb(template_img, Mat(), keyPoints_1, descriptors_1);
        
        int w = template_img.cols;
        int h = template_img.rows;
        float bz = (float)w / (float)h;
        Point3f corners_3d[] =
        {
            Point3f(-bz, -1.0f, 0),
            Point3f( bz, -1.0f, 0),
            Point3f( bz,  1.0f, 0),
            Point3f(-bz,  1.0f, 0)
        };
        m_corners_3d = vector<Point3f>(corners_3d, corners_3d + 4);
        
        istracking = false;
    }
}

extern "C"
{
    int process_Image(uchar * ImageData, float T[], float wxyz[], bool &isShow)
    {
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
        
        /////////////////////////////////////////////////////////////////////////////////
        {
            
            if(istracking == false)
            {
                vector<KeyPoint> keyPoints_2;
                Mat descriptors_2;
                
                orb(img, Mat(), keyPoints_2, descriptors_2);
                
                if(keyPoints_2.size()>=10)
                {
                    
                    BruteForceMatcher<HammingLUT> matcher;
                    vector<DMatch> matches;
                    matcher.match(descriptors_1, descriptors_2, matches);
                    
                    double max_dist = 0; double min_dist = 100;
                    
                    for( int i = 0; i < descriptors_1.rows; i++ )
                    {
                        double dist = matches[i].distance;
                        if( dist < min_dist ) min_dist = dist;
                        if( dist > max_dist ) max_dist = dist;
                    }
                    
                    std::vector< DMatch > good_matches;
                    
                    vector<Point2f> train_p, query_p;
                    
                    for( int i = 0; i < descriptors_1.rows; i++ )
                    {
                        if( matches[i].distance < 0.5*max_dist )
                        {
                            good_matches.push_back( matches[i]);
                            
                            int query_i = matches[i].queryIdx;
                            int train_i = matches[i].trainIdx;
                            KeyPoint query_kp = keyPoints_1[query_i];
                            KeyPoint train_kp = keyPoints_2[train_i];
                            train_p.push_back(train_kp.pt);
                            query_p.push_back(query_kp.pt);
                        }
                    }
                    
                    if(train_p.size()>=4 && train_p.size()>=(int)(0.1*descriptors_1.rows))
                    {
                        
                        Mat H = findHomography( query_p, train_p, RANSAC, 10 );
                        
                        std::vector<Point2f> obj_corners(4);
                        obj_corners[0] = cv::Point(0,0);
                        obj_corners[1] = cv::Point( template_img.cols, 0 );
                        obj_corners[2] = cv::Point( template_img.cols, template_img.rows );
                        obj_corners[3] = cv::Point( 0, template_img.rows );
                        std::vector<Point2f> scene_corners(4);
                        
                        perspectiveTransform( obj_corners, scene_corners, H);
                        
                        estimateTransformToCamera(m_corners_3d, scene_corners, m_camera_matrix, m_dist_coeff, r, t);
                        
                        prev_keyPoints.clear();
                        prev_corners.clear();
                        cvtColor(img, prev_gray, CV_BGRA2GRAY);
                        for(int i=0; i<4; i++)
                        {
                            prev_corners.push_back(scene_corners[i]);
                        }
                        istracking = true;
                        
                        Mat mask = createMask(img.size(), prev_corners);
                        goodFeaturesToTrack(prev_gray, prev_keyPoints, 80, 0.15, 5, mask);
                        prevPyr.clear();
                        track_status.clear();
                        
                    }
                }
            }
            else
            {
                int num = 0;
                for(int i=0; i<4; i++)
                {
                    if(prev_corners[i].x<=0 || prev_corners[i].x>=img.cols || prev_corners[i].y<=0 || prev_corners[i].y>=img.rows)
                    {
                        num++;
                    }
                }
                if(num != 0)
                {
                    Mat mask = createMask(img.size(), prev_corners);
                    goodFeaturesToTrack(prev_gray, prev_keyPoints, 80, 0.15, 5, mask);
                }
                
                if(prev_keyPoints.size()>=15)
                {
                    cvtColor(img, curr_gray, CV_BGRA2GRAY);
                    
                    vector<float> err;
                    if(prevPyr.empty()){
                        cv::buildOpticalFlowPyramid(prev_gray, prevPyr, cv::Size(21,21), 3, true);
                    }
                    cv::buildOpticalFlowPyramid(curr_gray, nextPyr, cv::Size(21,21), 3, true);
                    calcOpticalFlowPyrLK(prevPyr, nextPyr, prev_keyPoints, curr_keyPoints, track_status, err, cv::Size(21,21), 3);
                    
                    int tr_num = 0;
                    std::vector<cv::Point2f> trackedPrePts;
                    std::vector<cv::Point2f> trackedPts;
                    for (size_t i=0; i<track_status.size(); i++)
                    {
                        if (track_status[i] && prev_keyPoints.size()>i && norm(curr_keyPoints[i] - prev_keyPoints[i]) <= 15)
                        {
                            tr_num++;
                            trackedPrePts.push_back(prev_keyPoints[i]);
                            trackedPts.push_back(curr_keyPoints[i]);
                        }
                    }
                    if(tr_num>=15)
                    {
                        Mat homographyMat = findHomography(cv::Mat(trackedPrePts), cv::Mat(trackedPts),cv::RANSAC,10);
                        
                        if(countNonZero(homographyMat) != 0)
                        {
                            curr_corners.clear();
                            perspectiveTransform( prev_corners, curr_corners, homographyMat);
                            
                            swap(prev_gray,curr_gray);
                            prevPyr.swap(nextPyr);
                            prev_keyPoints = trackedPts;
                            prev_corners = curr_corners;
                            
                            if(prev_corners.size() >= 4)
                            {
                                estimateTransformToCamera(m_corners_3d, curr_corners, m_camera_matrix, m_dist_coeff, r, t);
                            }
                            
                        }
                    }
                    if(tr_num < 15)
                    {
                        istracking = false;
                    }
                }
                else
                {
                    istracking = false;
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////
        
        isShow = istracking;
        
        Mat r_ygx, t_ygx;
        r.convertTo(r_ygx, CV_32FC1);
        t.convertTo(t_ygx, CV_32FC1);
        
        float R[9];
        
        memcpy(R, r_ygx.data, r_ygx.cols*r_ygx.rows*sizeof(float));
        memcpy(T, t_ygx.data, t_ygx.cols*t_ygx.rows*sizeof(float));
        
        wxyz[0] = sqrtf(1.0f + R[0] + R[4] + R[8]) / 2.0f;
        wxyz[1] = -(R[7] - R[5]) / (4*wxyz[0]) ;
        wxyz[3] = -(R[2] - R[6]) / (4*wxyz[0]) ;
        wxyz[2] = (R[3] - R[1]) / (4*wxyz[0]) ;
        
        return 0;
    }
    
}
