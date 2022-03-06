#ifndef PLANEMODEL_H
#define PLANEMODEL_H

#include <Eigen/Core>
#include <Eigen/Dense>
#include <vector>
#include <array>

struct PlaneModel
{
	static const int ModelSize=3;

	Eigen::Vector3d n;
	double d;

	void compute(const std::vector<Eigen::Vector3d>& data, const std::array<size_t,3>& indices)
	{
		Eigen::Vector3d a=data[indices[1]]-data[indices[0]];
		Eigen::Vector3d b=data[indices[2]]-data[indices[0]];
		n=a.cross(b).normalized();
		d=n.dot(data[indices[0]]);
	}
	int computeInliers(const std::vector<Eigen::Vector3d>& data, double threshold)
	{
		int inliers=0;
		for (size_t i=0;i<data.size();i++)
		{
			if (fabs(n.dot(data[i])-d)<threshold)
				inliers++;
		}
		return inliers;
	}
	void refine(const std::vector<Eigen::Vector3d>& data, double threshold)
	{
		Eigen::Vector3d Ex=Eigen::Vector3d::Zero();
		Eigen::Matrix3d Exsqr=Eigen::Matrix3d::Zero();
		int inliers=0;
		for (size_t i=0;i<data.size();i++)
		{
			if (fabs(n.dot(data[i])-d)<threshold)
			{
				inliers++;
				Ex+=data[i];
				Exsqr+=data[i]*data[i].transpose();
			}
		}
		Ex/=inliers;
		Exsqr/=inliers;

		Eigen::Matrix3d cov=Exsqr-Ex*Ex.transpose();

		Eigen::SelfAdjointEigenSolver<Eigen::Matrix3d> eig(cov);
		n=eig.eigenvectors().col(0);
		d=n.dot(Ex);
	}
};

#endif
