/*
	LDB.cpp
	Created on: Apr 10, 2013
	Author: xinyang

    LDB - Local Difference Binary 
    Reference implementation of
    [1] Xin Yang and Kwang-Ting(Tim) Cheng. LDB: An Ultra-Fast Feature for 
	Scalable Augmened Reality on Mobile Device. In Proceedings of
    IEEE International Symposium on Mixed and Augmented Reality(ISMAR2012).

    Copyright (C) 2012  The Learning-Based Multimedia, University of California, 
	Santa Barbara, Xin Yang, Kwang-Ting(Tim) Cheng.

    This file is part of LDB.

    LDB is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LDB is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LDB.  If not, see <http://www.gnu.org/licenses/>.
*/

//Add by ygx 20160817

#include <fstream>
#include <iostream>
#include <ctime>
#include <set>
#include <vector>

#include "ldb.h"

using namespace std;
using namespace cv;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
vector<vector<vector<int> > > rotated_X_;
vector<vector<vector<int> > > rotated_Y_;

bool generateCoordFlag = false;
vector<vector<int> > coordinates2by2_;
vector<vector<int> > coordinates3by3_;
vector<vector<int> > coordinates4by4_;
vector<vector<int> > coordinates5by5_;
vector<int> randomSequence_;

/** the end of a row in a circular patch */
std::vector<int> u_max_;

//#define TRAINING

int n_levels = 0;

#define total_bits 1386
#ifdef TRAINING
#define selected_bits 1384
#else
#define selected_bits 256
#endif

#define LEVEL5

static int bit_pattern_31_[selected_bits * 4]; //number of tests * 4 (x1,y1,x2,y2)
static int bit_pattern_256_[selected_bits] = 
{ //trained based on Liberty dataset
	3, 4, 11, 12, 13, 34, 47, 48, 
	50, 56, 58, 59, 62, 71, 78, 79, 
	101, 102, 109, 119, 122, 125, 126, 135, 
	140, 147, 160, 175, 181, 182, 187, 193, 
	199, 215, 220, 224, 227, 232, 234, 235, 
	244, 246, 260, 261, 263, 273, 274, 280, 
	285, 286, 288, 299, 300, 310, 327, 330, 
	331, 343, 346, 353, 360, 361, 362, 365, 
	386, 389, 398, 401, 411, 412, 415, 423, 
	424, 427, 428, 430, 432, 433, 436, 448, 
	451, 454, 463, 464, 469, 477, 478, 480, 
	495, 512, 517, 520, 541, 550, 553, 556, 
	566, 571, 581, 585, 587, 592, 596, 609, 
	610, 611, 614, 615, 640, 655, 657, 664, 
	670, 672, 673, 684, 685, 699, 703, 705, 
	706, 718, 720, 721, 733, 734, 737, 742, 
	744, 745, 748, 749, 752, 764, 766, 774, 
	775, 778, 779, 781, 789, 794, 801, 804, 
	808, 815, 816, 817, 827, 828, 835, 838, 
	841, 844, 846, 850, 853, 855, 868, 879, 
	885, 896, 899, 907, 914, 922, 924, 925, 
	934, 937, 938, 940, 943, 953, 954, 958, 
	968, 969, 970, 973, 990, 991, 994, 1000, 
	1003, 1005, 1007, 1020, 1027, 1029, 1030, 1033, 
	1036, 1037, 1038, 1042, 1044, 1048, 1052, 1068, 
	1072, 1078, 1098, 1100, 1117, 1125, 1126, 1132, 
	1134, 1135, 1140, 1144, 1149, 1150, 1153, 1159, 
	1163, 1168, 1171, 1172, 1175, 1183, 1187, 1192, 
	1195, 1198, 1200, 1202, 1207, 1212, 1215, 1228, 
	1247, 1255, 1262, 1265, 1278, 1283, 1286, 1289, 
	1290, 1305, 1311, 1312, 1314, 1321, 1328, 1331, 
	1334, 1336, 1343, 1350, 1351, 1354, 1356, 1373
};

static float IC_Angle(const Mat& image, const int half_k, Point2f pt,
	const vector<int> & u_max)
{
	int m_01 = 0, m_10 = 0;

	const uchar* center = &image.at<uchar> (cvRound(pt.y), cvRound(pt.x));

	// Treat the center line differently, v=0
	for (int u = -half_k; u <= half_k; ++u)
		m_10 += u * center[u];

	// Go line by line in the circular patch
	int step = (int)image.step1();
	for (int v = 1; v <= half_k; ++v)
	{
		// Proceed over the two lines
		int v_sum = 0;
		int d = u_max[v];
		for (int u = -d; u <= d; ++u)
		{
			int val_plus = center[u + v*step], val_minus = center[u - v*step];
			v_sum += (val_plus - val_minus);
			m_10 += u * (val_plus + val_minus);
		}
		m_01 += v * v_sum;
	}

	return fastAtan2((float)m_01, (float)m_10);
}

static void generateRotatedPatterns(const int& patch_size,
	const int& kNumAngles,
	vector<vector<vector<int> > >& rotatedX, 
	vector<vector<vector<int> > >& rotatedY)
{
	int win_offset = (patch_size-1)/2;
	for (int i = 0; i < kNumAngles; i++)
	{
		vector<vector<int> > mappedX, mappedY;
		for(int m = 0; m < patch_size; m++){
			vector<int> one_row(patch_size);
			mappedX.push_back(one_row);
			mappedY.push_back(one_row);
		}
		float descriptor_dir = (float)(i * 2* CV_PI/ 30);
		float sin_dir = sin(descriptor_dir);
		float cos_dir = cos(descriptor_dir);
		int a, b; a = 0;
		for(int m = win_offset; m >= -win_offset; m--, a++){
			b = 0;
			for(int n = -win_offset; n <= win_offset; n++, b++){
				float pixel_x = n*cos_dir + m*sin_dir;
				float pixel_y = -n*sin_dir + m*cos_dir;

				int x = cvRound(pixel_x);
				int y = cvRound(pixel_y);

				mappedX[a][b] = x;
				mappedY[a][b] = y;
			}
		}
		rotatedX.push_back(mappedX);
		rotatedY.push_back(mappedY);
	}
}

/** computer the grid coordinates for computing LDB*/
void computeCoordinates(vector<vector<int> >& coordinates, int step, int patch_size)
{
	int win_size = step - 1;
	int m = 0;
	for(int i = -patch_size/2 + 1; i < patch_size/2 - 2; i+= step){
		for(int j = -patch_size/2 + 1; j < patch_size/2 - 2; j+=step, m++){
			vector<int> coord;
			int x1 = j;				int y1 = i;
			int x2 = j+win_size;	int y2 = i;
			int x3 = j;				int y3 = i+win_size;
			int x4 = j+win_size;	int y4 = i+win_size;

			int x5 = j+win_size/2;	int y5 = i;
			int x6 = j+win_size;	int y6 = i+win_size/2;
			int x7 = j+win_size/2;	int y7 = i+win_size;
			int x8 = j;				int y8 = i+win_size/2;

			int x9 = j+win_size/2;	int y9 = i+win_size/2;

			coord.push_back(x1); coord.push_back(y1);
			coord.push_back(x2); coord.push_back(y2);
			coord.push_back(x3); coord.push_back(y3);
			coord.push_back(x4); coord.push_back(y4);

			coord.push_back(x5); coord.push_back(y5);
			coord.push_back(x6); coord.push_back(y6);
			coord.push_back(x7); coord.push_back(y7);
			coord.push_back(x8); coord.push_back(y8);

			coord.push_back(x9); coord.push_back(y9);

			coordinates.push_back(coord);
		}
	}
}

/** generate random sequence size of 256 for computing LDP*/
void generateRandSequence(vector<int>& randomSequence)
{
	srand(time(NULL));
	set<int> visited;
	int count = 0;
	do{
		int randNum = rand() % total_bits;
		if(visited.find(randNum) == visited.end()){
			visited.insert(randNum);
			count++;
		}
	}while(count < selected_bits);
	for(std::set<int>::const_iterator it = visited.begin(); it != visited.end(); it++)
		randomSequence.push_back(*it);
}

static inline int angle2Wedge(const int& kNumAngles, float angle)
{
	static float scale = float(kNumAngles) / 360.0f;
	return std::min(int(std::floor(angle * scale)), kNumAngles - 1);
}

inline void rotatedIntegralImage(double descriptor_dir,
	const cv::KeyPoint& kpt,
	const cv::Mat& img,
	const int& patch_size,
	Mat& win_integral_image)
{

	//* Nearest neighbour version (faster) */
	descriptor_dir *= (float)(CV_PI/180);
	float sin_dir = sin(descriptor_dir);
	float cos_dir = cos(descriptor_dir);
	float win_offset = (int)(patch_size/2);
	Mat win(patch_size, patch_size, CV_8U);
	//******************************************************//
	// faster version: xin yang @ 2012-07-05 11:22am
	//******************************************************//
	float start_x = kpt.pt.x - win_offset*cos_dir + win_offset*sin_dir;
	float start_y = kpt.pt.y - win_offset*sin_dir - win_offset*cos_dir;

	for(int i = 0; i < patch_size; i++, start_x -= sin_dir, start_y += cos_dir){
		float pixel_x = start_x;
		float pixel_y = start_y;
		for(int j = 0; j < patch_size; j++, pixel_x += cos_dir, pixel_y += sin_dir){
			int x = (int)(pixel_x + 0.5);
			int y = (int)(pixel_y + 0.5);
			//int x = std::min(std::max(cvRound(pixel_x), 0), img.cols-1);
			//int y = std::min(std::max(cvRound(pixel_y), 0), img.rows-1);
			win.at<unsigned char>(i, j) = img.at<unsigned char>(y, x);
		}
	}
	integral(win, win_integral_image, CV_32S);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

static void computeLdbDescriptor(const cv::KeyPoint& kpt, const cv::Mat& img, const cv::Mat& sum, unsigned char * desc, const int& patch_size,
	const vector<vector<int> >& coordinates2by2_, const vector<vector<int> >& coordinates3by3_,
	const vector<vector<int> >& coordinates4by4_, const vector<vector<int> >& coordinates5by5_,
	const vector<int>& randSequence)
{
#ifdef TRAINING
	for(int i = 0; i < randSequence.size(); i++)
		bit_pattern_256_[i] = randSequence[i];
#endif

	// Compute the pointer to the center of the feature
	int img_y = (int)(kpt.pt.y + 0.5);
	int img_x = (int)(kpt.pt.x + 0.5);
	const int * center = reinterpret_cast<const int *> (sum.ptr(img_y)) + img_x;

	int sum2by2[4], sum3by3[9], sum4by4[16], sum5by5[25];
	int dx2by2[4], dx3by3[9], dx4by4[16], dx5by5[25];
	int dy2by2[4], dy3by3[9], dy4by4[16], dy5by5[25];
	int dxy2by2[4], dxy3by3[9], dxy4by4[16], dxy5by5[25];

	int sum2by2_size = 4, sum3by3_size = 9, sum4by4_size = 16, sum5by5_size = 25;

	int /*patch_size = PATCH_SIZE; int */offset = patch_size/2;
	Mat win_integral_image(patch_size+1, patch_size+1, CV_32S);

	if(kpt.angle != -1)
		rotatedIntegralImage(kpt.angle, kpt, img, patch_size, win_integral_image);

	for(int i = 0; i < sum2by2_size; i++){
		if(kpt.angle == -1){
			int a = coordinates2by2_[i][1] * sum.cols + coordinates2by2_[i][0];
			int b = coordinates2by2_[i][3] * sum.cols + coordinates2by2_[i][2];
			int c = coordinates2by2_[i][5] * sum.cols + coordinates2by2_[i][4];
			int d = coordinates2by2_[i][7] * sum.cols + coordinates2by2_[i][6];

			sum2by2[i] = *(center + a) + *(center + d) - *(center + c) - *(center + b);

			int e = coordinates2by2_[i][9] * sum.cols + coordinates2by2_[i][8];
			int f = coordinates2by2_[i][11] * sum.cols + coordinates2by2_[i][10];
			int g = coordinates2by2_[i][13] * sum.cols + coordinates2by2_[i][12];
			int h = coordinates2by2_[i][15] * sum.cols + coordinates2by2_[i][14];

			dx2by2[i] = sum2by2[i] - 2*(*(center + a) + *(center + g) - *(center + e) - *(center + c));
			dy2by2[i] = sum2by2[i] - 2*(*(center + a) + *(center + f) - *(center + b) - *(center + h));
		}
		else{
			sum2by2[i] = win_integral_image.at<int>(coordinates2by2_[i][1]+offset, coordinates2by2_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates2by2_[i][7]+offset, coordinates2by2_[i][6]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][3]+offset, coordinates2by2_[i][2]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][5]+offset, coordinates2by2_[i][4]+offset);

			dx2by2[i] = sum2by2[i] 
			- 2*(win_integral_image.at<int>(coordinates2by2_[i][1]+offset, coordinates2by2_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates2by2_[i][13]+offset, coordinates2by2_[i][12]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][9]+offset, coordinates2by2_[i][8]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][5]+offset, coordinates2by2_[i][4]+offset));

			dy2by2[i] = sum2by2[i] 
			- 2*(win_integral_image.at<int>(coordinates2by2_[i][1]+offset, coordinates2by2_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates2by2_[i][11]+offset, coordinates2by2_[i][10]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][3]+offset, coordinates2by2_[i][2]+offset)
				- win_integral_image.at<int>(coordinates2by2_[i][15]+offset, coordinates2by2_[i][14]+offset));
		}
	}

	for(int i = 0; i < sum3by3_size; i++){
		if(kpt.angle == -1){
			int a = coordinates3by3_[i][1] * sum.cols + coordinates3by3_[i][0];
			int b = coordinates3by3_[i][3] * sum.cols + coordinates3by3_[i][2];
			int c = coordinates3by3_[i][5] * sum.cols + coordinates3by3_[i][4];
			int d = coordinates3by3_[i][7] * sum.cols + coordinates3by3_[i][6];
			sum3by3[i] = *(center + a) + *(center + d) - *(center + c) - *(center + b);

			int e = coordinates3by3_[i][9] * sum.cols + coordinates3by3_[i][8];
			int f = coordinates3by3_[i][11] * sum.cols + coordinates3by3_[i][10];
			int g = coordinates3by3_[i][13] * sum.cols + coordinates3by3_[i][12];
			int h = coordinates3by3_[i][15] * sum.cols + coordinates3by3_[i][14];

			dx3by3[i] = sum3by3[i] - 2*(*(center + a) + *(center + g) - *(center + e) - *(center + c));
			dy3by3[i] = sum3by3[i] - 2*(*(center + a) + *(center + f) - *(center + b) - *(center + h));}
		else{
			sum3by3[i] = win_integral_image.at<int>(coordinates3by3_[i][1]+offset, coordinates3by3_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates3by3_[i][7]+offset, coordinates3by3_[i][6]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][3]+offset, coordinates3by3_[i][2]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][5]+offset, coordinates3by3_[i][4]+offset);

			dx3by3[i] = sum3by3[i] 
			- 2*(win_integral_image.at<int>(coordinates3by3_[i][1]+offset, coordinates3by3_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates3by3_[i][13]+offset, coordinates3by3_[i][12]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][9]+offset, coordinates3by3_[i][8]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][5]+offset, coordinates3by3_[i][4]+offset));

			dy3by3[i] = sum3by3[i] 
			- 2*(win_integral_image.at<int>(coordinates3by3_[i][1]+offset, coordinates3by3_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates3by3_[i][11]+offset, coordinates3by3_[i][10]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][3]+offset, coordinates3by3_[i][2]+offset)
				- win_integral_image.at<int>(coordinates3by3_[i][15]+offset, coordinates3by3_[i][14]+offset));
		}
	}

	for(int i = 0; i < sum4by4_size; i++){
		if(kpt.angle == -1){
			int a = coordinates4by4_[i][1] * sum.cols + coordinates4by4_[i][0];
			int b = coordinates4by4_[i][3] * sum.cols + coordinates4by4_[i][2];
			int c = coordinates4by4_[i][5] * sum.cols + coordinates4by4_[i][4];
			int d = coordinates4by4_[i][7] * sum.cols + coordinates4by4_[i][6];
			sum4by4[i] = *(center + a) + *(center + d) - *(center + c) - *(center + b);

			int e = coordinates4by4_[i][9] * sum.cols + coordinates4by4_[i][8];
			int f = coordinates4by4_[i][11] * sum.cols + coordinates4by4_[i][10];
			int g = coordinates4by4_[i][13] * sum.cols + coordinates4by4_[i][12];
			int h = coordinates4by4_[i][15] * sum.cols + coordinates4by4_[i][14];

			dx4by4[i] = sum4by4[i] - 2*(*(center + a) + *(center + g) - *(center + e) - *(center + c));
			dy4by4[i] = sum4by4[i] - 2*(*(center + a) + *(center + f) - *(center + b) - *(center + h));
		}
		else{
			sum4by4[i] = win_integral_image.at<int>(coordinates4by4_[i][1]+offset, coordinates4by4_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates4by4_[i][7]+offset, coordinates4by4_[i][6]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][3]+offset, coordinates4by4_[i][2]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][5]+offset, coordinates4by4_[i][4]+offset);

			dx4by4[i] = sum4by4[i] 
			- 2*(win_integral_image.at<int>(coordinates4by4_[i][1]+offset, coordinates4by4_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates4by4_[i][13]+offset, coordinates4by4_[i][12]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][9]+offset, coordinates4by4_[i][8]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][5]+offset, coordinates4by4_[i][4]+offset));

			dy4by4[i] = sum4by4[i] 
			- 2*(win_integral_image.at<int>(coordinates4by4_[i][1]+offset, coordinates4by4_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates4by4_[i][11]+offset, coordinates4by4_[i][10]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][3]+offset, coordinates4by4_[i][2]+offset)
				- win_integral_image.at<int>(coordinates4by4_[i][15]+offset, coordinates4by4_[i][14]+offset));
		}
	}
#ifdef LEVEL5
	for(int i = 0; i < sum5by5_size; i++){
		if(kpt.angle == -1){
			int a = coordinates5by5_[i][1] * sum.cols + coordinates5by5_[i][0];
			int b = coordinates5by5_[i][3] * sum.cols + coordinates5by5_[i][2];
			int c = coordinates5by5_[i][5] * sum.cols + coordinates5by5_[i][4];
			int d = coordinates5by5_[i][7] * sum.cols + coordinates5by5_[i][6];
			sum5by5[i] = *(center + a) + *(center + d) - *(center + c) - *(center + b);

			int e = coordinates5by5_[i][9] * sum.cols + coordinates5by5_[i][8];
			int f = coordinates5by5_[i][11] * sum.cols + coordinates5by5_[i][10];
			int g = coordinates5by5_[i][13] * sum.cols + coordinates5by5_[i][12];
			int h = coordinates5by5_[i][15] * sum.cols + coordinates5by5_[i][14];

			dx5by5[i] = sum5by5[i] - 2*(*(center + a) + *(center + g) - *(center + e) - *(center + c));
			dy5by5[i] = sum5by5[i] - 2*(*(center + a) + *(center + f) - *(center + b) - *(center + h));
		}
		else{
			sum5by5[i] = win_integral_image.at<int>(coordinates5by5_[i][1]+offset, coordinates5by5_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates5by5_[i][7]+offset, coordinates5by5_[i][6]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][3]+offset, coordinates5by5_[i][2]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][5]+offset, coordinates5by5_[i][4]+offset);

			dx5by5[i] = sum5by5[i] 
			- 2*(win_integral_image.at<int>(coordinates5by5_[i][1]+offset, coordinates5by5_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates5by5_[i][13]+offset, coordinates5by5_[i][12]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][9]+offset, coordinates5by5_[i][8]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][5]+offset, coordinates5by5_[i][4]+offset));

			dy5by5[i] = sum5by5[i] 
			- 2*(win_integral_image.at<int>(coordinates5by5_[i][1]+offset, coordinates5by5_[i][0]+offset)
				+ win_integral_image.at<int>(coordinates5by5_[i][11]+offset, coordinates5by5_[i][10]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][3]+offset, coordinates5by5_[i][2]+offset)
				- win_integral_image.at<int>(coordinates5by5_[i][15]+offset, coordinates5by5_[i][14]+offset));
		}
	}
#endif
	int pt = 0;
	int entire_pt = 0;
	uchar desc_bitstring[selected_bits];
	static const uchar score[] = {1 << 0, 1 << 1, 1 << 2, 1 << 3, 1 << 4, 1 << 5, 1 << 6, 1 << 7};

	for(int i = 0; i < sum2by2_size; i++){
		int sum1 = sum2by2[i];
		int dx1 = dx2by2[i];
		int dy1 = dy2by2[i];
		for(int j = i+1; j < sum2by2_size; j++){
			if(bit_pattern_256_[pt] == entire_pt){
				int sum2 = sum2by2[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (sum1 > sum2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dx2 = dx2by2[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dx1 > dx2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dy2 = dy2by2[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dy1 > dy2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;	
		}
	}
	for(int i = 0; i < sum3by3_size; i++){
		int sum1 = sum3by3[i];
		int dx1 = dx3by3[i];
		int dy1 = dy3by3[i];
		for(int j = i+1; j < sum3by3_size; j++){
			if(bit_pattern_256_[pt] == entire_pt){
				int sum2 = sum3by3[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (sum1 > sum2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dx2 = dx3by3[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dx1 > dx2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dy2 = dy3by3[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dy1 > dy2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;	
		}
	}
	for(int i = 0; i < sum4by4_size; i++){
		int sum1 = sum4by4[i];
		int dx1 = dx4by4[i];
		int dy1 = dy4by4[i];
		for(int j = i+1; j < sum4by4_size; j++){
			if(bit_pattern_256_[pt] == entire_pt){
				int sum2 = sum4by4[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (sum1 > sum2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dx2 = dx4by4[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dx1 > dx2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dy2 = dy4by4[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dy1 > dy2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
		}
	}
#ifdef LEVEL5
	for(int i = 0; i < sum5by5_size; i++){
		int sum1 = sum5by5[i];
		int dx1 = dx5by5[i];
		int dy1 = dy5by5[i];
		for(int j = i+1; j < sum5by5_size; j++){
			if(bit_pattern_256_[pt] == entire_pt){
				int sum2 = sum5by5[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (sum1 > sum2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dx2 = dx5by5[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dx1 > dx2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
			if(bit_pattern_256_[pt] == entire_pt){
				int dy2 = dy5by5[j];
				int idx = pt % 8;
				desc_bitstring[pt] = (dy1 > dy2) ? score[idx] : 0;
				pt++;
			}
			entire_pt++;
		}
	}
#endif
	for (int i = 0, j = 0; i < selected_bits/8; i++, j += 8)
	{
		desc[i] = (desc_bitstring[j]) | (desc_bitstring[j+1])
			| (desc_bitstring[j+2]) | (desc_bitstring[j+3])
			| (desc_bitstring[j+4]) | (desc_bitstring[j+5])
			| (desc_bitstring[j+6]) | (desc_bitstring[j+7]);
	}
}

static void computeOrientation(const Mat& image, vector<KeyPoint>& keypoints,
	int halfPatchSize, const vector<int>& umax)
{
	// Process each keypoint
	for (vector<KeyPoint>::iterator keypoint = keypoints.begin(),
		keypointEnd = keypoints.end(); keypoint != keypointEnd; ++keypoint)
	{
		keypoint->angle = IC_Angle(image, halfPatchSize, keypoint->pt, umax);
	}
}

/** Compute the LDB decriptors
* @param image the image to compute the features and descriptors on
* @param integral_image the integral image of the image (can be empty, but the computation will be slower)
* @param level the scale at which we compute the orientation
* @param keypoints the keypoints to use
* @param descriptors the resulting descriptors
*/
static void computeDescriptors(const Mat& image, const cv::Mat& integral_image,
	const int& patchSize, vector<KeyPoint>& keypoints, Mat& descriptors, int dsize)
{
	//convert to grayscale if more than one color
	CV_Assert(image.type() == CV_8UC1);
	//create the descriptor mat, keypoints.size() rows, BYTES cols
	descriptors = Mat::zeros((int)keypoints.size(), dsize, CV_8UC1);

	for (size_t i = 0; i < keypoints.size(); i++)
		computeLdbDescriptor(keypoints[i], image, integral_image, descriptors.ptr((int)i),
		patchSize, coordinates2by2_, coordinates3by3_,	coordinates4by4_, coordinates5by5_,randomSequence_);
}

static void initializeLdbPattern(const int& nlevels, const int& patchSize)
{
	if(!generateCoordFlag){
		int patch_size = patchSize;
		rotated_X_.clear();
		rotated_Y_.clear();
		generateRotatedPatterns(patch_size, 30, rotated_X_, rotated_Y_);

		computeCoordinates(coordinates2by2_, (int)(patch_size/2), patch_size);
		computeCoordinates(coordinates3by3_, (int)(patch_size/3), patch_size);
		computeCoordinates(coordinates4by4_, (int)(patch_size/4), patch_size);
		computeCoordinates(coordinates5by5_, (int)(patch_size/5), patch_size);

		generateCoordFlag = true;
	}

	if(randomSequence_.empty())
		generateRandSequence(randomSequence_);

	n_levels = nlevels;
}

static inline float getScale(int level, int firstLevel, double scaleFactor)
{
	return (float)std::pow(scaleFactor, (double)(level - firstLevel));
}

/** Constructor
* @param detector_params parameters to use
*/
LDB::LDB(int _bytes, int _nlevels, int _patchSize)
{
	scaleFactor = 1.2;
	patchSize = 48;
	firstLevel = 0;
	kBytes = (int)(selected_bits/8);
	_nlevels = 3;
	initializeLdbPattern(_nlevels, patchSize);
}

LDB::~LDB()
{
}

int LDB::descriptorSize() const
{
	return kBytes;
}

int LDB::descriptorType() const
{
	return CV_8U;
}

void LDB::compute( const Mat& _image, vector<KeyPoint>& _keypoints, Mat& _descriptors) const
{
	if(_image.empty() )
		return;

	//ROI handling
	int halfPatchSize = patchSize / 2;
	int border = halfPatchSize*1.415 + 1;

	if( _image.type() != CV_8UC1 )
    {
		cvtColor(_image, _image, CV_BGR2GRAY);
    }

	int levelsNum = 0;
	for( size_t i = 0; i < _keypoints.size(); i++ )
		levelsNum = std::max(levelsNum, std::max(_keypoints[i].octave, 0));
	levelsNum++;

	// Pre-compute the scale pyramids
	vector<Mat> imagePyramid(levelsNum);
	for (int level = 0; level < levelsNum; ++level)
	{
		float scale = 1/getScale(level, firstLevel, scaleFactor);
		Size sz(cvRound(_image.cols*scale), cvRound(_image.rows*scale));
		Size wholeSize(sz.width + border*2, sz.height + border*2);
		Mat temp(wholeSize, _image.type()), masktemp;
		imagePyramid[level] = temp(Rect(border, border, sz.width, sz.height));

		// Compute the resized image
		if( level != firstLevel )
		{
			if( level < firstLevel )
				resize(_image, imagePyramid[level], sz, 0, 0, INTER_LINEAR);
			else
				resize(imagePyramid[level-1], imagePyramid[level], sz, 0, 0, INTER_LINEAR);

			copyMakeBorder(imagePyramid[level], temp, border, border, border, border,
				BORDER_REFLECT_101+BORDER_ISOLATED);
		}
		else
			copyMakeBorder(_image, temp, border, border, border, border,
				BORDER_REFLECT_101);
	}

	// Pre-compute the keypoints (we keep the best over all scales, so this has to be done beforehand
	vector < vector<KeyPoint> > allKeypoints;

	// Remove keypoints very close to the border
	//KeyPointsFilter::runByImageBorder(_keypoints, image.size(), edgeThreshold);
	
	// Cluster the input keypoints depending on the level they were computed at
	allKeypoints.resize(levelsNum);
	for (vector<KeyPoint>::iterator keypoint = _keypoints.begin(),
		keypointEnd = _keypoints.end(); keypoint != keypointEnd; ++keypoint)
		allKeypoints[keypoint->octave].push_back(*keypoint);

	// Make sure we rescale the coordinates
	for (int level = 0; level < levelsNum; ++level)
	{
		if (level == firstLevel)
			continue;

		vector<KeyPoint> & keypoints = allKeypoints[level];
		float scale = 1/getScale(level, firstLevel, scaleFactor);
		for (vector<KeyPoint>::iterator keypoint = keypoints.begin(),
			keypointEnd = keypoints.end(); keypoint != keypointEnd; ++keypoint)
			keypoint->pt *= scale;
	}

	Mat descriptors;
	vector<Point> pattern;

	int nkeypoints = 0;
	for (int level = 0; level < levelsNum; ++level){
		vector<KeyPoint>& keypoints = allKeypoints[level];
		Mat& workingMat = imagePyramid[level];
		if(keypoints.size() > 1)
			KeyPointsFilter::runByImageBorder(keypoints, workingMat.size(), border); 

		nkeypoints += keypoints.size();
	}
	if( nkeypoints == 0 )
		_descriptors.release();
	else
	{
		_descriptors.create(nkeypoints, descriptorSize(), CV_8U);
		descriptors = _descriptors;
	}

	_keypoints.clear();
	int offset = 0;
	for (int level = 0; level < levelsNum; ++level)
	{
		// preprocess the resized image
		Mat& workingMat = imagePyramid[level];
		// Get the features and compute their orientation
		vector<KeyPoint>& keypoints = allKeypoints[level];
		if(keypoints.size() > 1)
			KeyPointsFilter::runByImageBorder(keypoints, workingMat.size(), border); 
		int nkeypoints = (int)keypoints.size();

		// Compute the descriptors
		Mat desc;
		if (!descriptors.empty())
		{
			desc = descriptors.rowRange(offset, offset + nkeypoints);
		}

		offset += nkeypoints;
		//boxFilter(working_mat, working_mat, working_mat.depth(), Size(5,5), Point(-1,-1), true, BORDER_REFLECT_101);
		GaussianBlur(workingMat, workingMat, Size(7, 7), 2, 2, BORDER_REFLECT_101);
		cv::Mat integral_image;
		integral(workingMat, integral_image, CV_32S);
		computeDescriptors(workingMat, integral_image, patchSize, keypoints, desc, descriptorSize());

		// Copy to the output data
		if (level != firstLevel)
		{
			float scale = getScale(level, firstLevel, scaleFactor);
			for (vector<KeyPoint>::iterator keypoint = keypoints.begin(),
				keypointEnd = keypoints.end(); keypoint != keypointEnd; ++keypoint)
				keypoint->pt *= scale;
		}
		// And add the keypoints to the output
		_keypoints.insert(_keypoints.end(), keypoints.begin(), keypoints.end());
	}
	
}

