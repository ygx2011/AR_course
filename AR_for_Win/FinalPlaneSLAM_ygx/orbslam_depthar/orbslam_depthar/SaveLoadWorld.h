/*
* Added by wangjing
*/

#ifndef SAVELOADWORLD_H
#define SAVELOADWORLD_H

#include "Frame.h"
#include "KeyFrame.h"
#include "MapPoint.h"
#include "Map.h"
#include "KeyFrameDatabase.h"
#include "LocalMapping.h"

#include <opencv2\core\core.hpp>
#include <fstream>
#include <string>

using namespace std;
using namespace cv;
using namespace ORB_SLAM;

typedef map<long unsigned int, MapPoint*> MapMPIndexPointer;
typedef map<long unsigned int, KeyFrame*> MapKFIndexPointer;
typedef vector<long unsigned int> VecUL;

// load MapPoint data
bool loadMPVariables(KeyFrameDatabase *db, Map *wd, MapMPIndexPointer *mpIdxPtMap,
                     VecUL &_VecMPmnId, VecUL &_vRefKFIdInMP);

// load KeyFrame data
bool loadKFVariables(KeyFrameDatabase *db, Map *wd, ORBVocabulary* mpvoc,
                     MapKFIndexPointer *kfIdxPtMap, VecUL &_VecKFmnId);

//	// load voc-keyframe invert index, after keyframe data is loaded
//bool loadKFDatabase(KeyFrameDatabase *db, MapKFIndexPointer &kfIdxPtMap);

// associate pointers in MPs and KFs
bool loadMPKFPointers(MapMPIndexPointer &mpIdxPtMap, MapKFIndexPointer &kfIdxPtMap,
                      const VecUL& VecKFmnId, const VecUL& VecMPmnId, const VecUL &vRefKFIdInMP);


// load all data. main function
bool LoadWroldFromFile(KeyFrameDatabase *db, Map *wd, ORBVocabulary* mpvoc, KeyFrame *pLastKF);


static bool myOpenFile(ifstream &ifs, string strFile)
{
    ifs.open(strFile.c_str(), ios::in | ios::binary);
    if(!ifs.is_open())
    {
        cout<<strFile<<" open failed."<<endl;
        return false;
    }
    if(ifs.eof())
    {
        cout<<strFile<<" empty."<<endl;
        ifs.close();
        return false;
    }
    return true;
}


void SaveWorldToFile( Map& World, KeyFrameDatabase& Database)
{
    unsigned char saveHeader[2]={0xeb,0x90};

    string strFile;
    long unsigned int mpSaveCnt,kfSaveCnt;
    mpSaveCnt=0;
    kfSaveCnt=0;


    //1 2. save mappoint files
    //1 -----------------------------------------------
    {
        fstream fmpVar,fmpObs;
        cout << endl << "Saving MapPoint" << endl;

        strFile = "./myMap/mpVariables.bin";
        fmpVar.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/mpObservations.bin";
        fmpObs.open(strFile.c_str(), ios::out | ios::binary);

        long unsigned int tmpIdx=0;
        vector<MapPoint*> vMapPoints = World.GetAllMapPoints();
        for(std::vector<MapPoint*>::iterator vit=vMapPoints.begin(), vend=vMapPoints.end(); vit!=vend; vit++, tmpIdx++)
        {
            MapPoint* pMPi = *vit;
            if(!pMPi)
                cerr<<"MapPoint pointer = NULL. shouldn't"<<endl;
            if(!pMPi->isBad())	//only save those not bad
            {
                //2 2.1 save plain variable
                //2 ---------------------------
                // 0xeb,0x90,nNextId,mnId,mnFirstKFid,mTrackPorjX,mTrackProjY,mbTrackInView,mnTrackScaleLevel,mTrackViewCos,
                //  WorldPos,mDescriptor,mpRefKF
                {
                    fmpVar.write(reinterpret_cast<char*>(saveHeader),2);
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->nNextId),sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnId),sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnFirstKFid),sizeof(long int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mTrackProjX),sizeof(float));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mTrackProjY),sizeof(float));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mbTrackInView),sizeof(bool));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnTrackScaleLevel),sizeof(int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mTrackViewCos),sizeof(float));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnTrackReferenceForFrame),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnLastFrameSeen),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnBALocalForKF),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnFuseCandidateForKF),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnLoopPointForKF),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnCorrectedByKF),	sizeof(long unsigned int));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->mnCorrectedReference),	sizeof(long unsigned int));
                    // protected
                    cv::Mat twp = pMPi->GetWorldPos();
                    for(int ti=0;ti<3;ti++)
                        fmpVar.write(reinterpret_cast<char*>(&twp.at<float>(ti)),sizeof(float));
                    cv::Mat tdes = pMPi->GetDescriptor();	//256b, 8*uint32_t. cv::Mat::zeros(1,32,CV_8U);
                    for(int ti=0;ti<32;ti++)
                        fmpVar.write(reinterpret_cast<char*>(&tdes.at<unsigned char>(ti)),sizeof(unsigned char));
                    fmpVar.write(reinterpret_cast<char*>(&pMPi->GetReferenceKeyFrame()->mnId),sizeof(long unsigned int));
                }
                //2 ---------------------------

                //2 2.2 save observations
                //2 ---------------------------
                // 0xeb,0x90,Nobs,Nobs*(ob.kfmnId,ob.mpIdx)
                {
                    fmpObs.write(reinterpret_cast<char*>(saveHeader),2);
                    map<KeyFrame*,size_t> observations = pMPi->GetObservations();
                    size_t nObs = observations.size();
                    if(nObs==0)	cerr<<"save nObs=0, shouldn't"<<endl;
                    fmpObs.write(reinterpret_cast<char*>(&nObs),sizeof(size_t));
                    size_t saveObsCnt=0;
                    for(std::map<KeyFrame*, size_t>::iterator mit=observations.begin(), mend=observations.end(); mit!=mend; mit++, saveObsCnt++)
                    {
                        KeyFrame* pKFm = mit->first;
                        size_t idMPinKF = mit->second;
                        fmpObs.write(reinterpret_cast<char*>(&pKFm->mnId), sizeof(long unsigned int));
                        fmpObs.write(reinterpret_cast<char*>(&idMPinKF), sizeof(size_t));	//KF id and MP index in KF
                    }
                    if(saveObsCnt!=nObs)
                        cerr<<"saveObsCnt!=nObs, shouldn't"<<endl;
                }
                //2 ---------------------------

                mpSaveCnt++;
            }

        }

        fmpVar.close();
        fmpObs.close();

        cout<<"total "<<mpSaveCnt<<" MapPoints saved."<<endl;
    }
    //1 -----------------------------------------------



    //1 3. save keyframe files
    //1 ------------------------------------------------
    {
        fstream fkfVar,fkfKeys,fkfKeysUn,fkfDes,fkfMPids,fkfLPEGs,fkfmGrid;
        cout << endl << "Saving KeyFrames" << endl;
        strFile = "./myMap/kfVariables.bin";
        fkfVar.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfKeyPoints.bin";
        fkfKeys.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfKeyPointsUn.bin";
        fkfKeysUn.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfDescriptors.bin";
        fkfDes.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfMapPointsID.bin";
        fkfMPids.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfLoopEdges.bin";
        fkfLPEGs.open(strFile.c_str(), ios::out | ios::binary);

        strFile = "./myMap/kfmGrid.bin";
        fkfmGrid.open(strFile.c_str(), ios::out | ios::binary);

        long unsigned int tmpIdx=0;
        vector<KeyFrame*> vKeyFrames = World.GetAllKeyFrames();
        for(std::vector<KeyFrame*>::iterator vitKFs=vKeyFrames.begin(), vendKFs=vKeyFrames.end(); vitKFs!=vendKFs; vitKFs++, tmpIdx++)
        {
            KeyFrame* pKFi = *vitKFs;
            if(!pKFi)
                cerr<<"KeyFrame pointer = NULL, shouldn't."<<endl;
            if(!pKFi->isBad())
            {
                //2 3.1 save plain variables
                //2 -------------------------------
                // 0xeb,0x90, nNextId, mnId, mnFrameId, mTimeStamp, Rcw, tcw
                fkfVar.write(reinterpret_cast<char*>(saveHeader),2);
                fkfVar.write(reinterpret_cast<char*>(&pKFi->nNextId), sizeof(long unsigned int));
                fkfVar.write(reinterpret_cast<char*>(&pKFi->mnId), sizeof(long unsigned int));
                fkfVar.write(reinterpret_cast<char*>(&pKFi->mnFrameId), sizeof(long unsigned int));
                fkfVar.write(reinterpret_cast<char*>(&pKFi->mTimeStamp), sizeof(double));
                // protected
                cv::Mat Rcwi=pKFi->GetRotation();
                for(int ti=0;ti<3;ti++)
                    for(int tj=0;tj<3;tj++)
                        fkfVar.write(reinterpret_cast<char*>(&Rcwi.at<float>(ti,tj)), sizeof(float));
                cv::Mat tcwi = pKFi->GetTranslation();
                for(int ti=0;ti<3;ti++)
                    fkfVar.write(reinterpret_cast<char*>(&tcwi.at<float>(ti)), sizeof(float));
                //2 -------------------------------

                //2 3.2 save KeyPoints and KeyPointsUn
                //2 -------------------------------
                // 0xeb,0x90, nKeys, nKeys*(ptx,pty,size,angle,responsse,  octave,classid)
                {
                    vector<cv::KeyPoint> mvKeysi = pKFi->GetKeyPoints();
                    size_t nKeys=mvKeysi.size();
                    fkfKeys.write(reinterpret_cast<char*>(saveHeader),2);
                    fkfKeys.write(reinterpret_cast<char*>(&nKeys), sizeof(size_t));
                    for(vector<cv::KeyPoint>::iterator vitkeys=mvKeysi.begin(), vendkeys=mvKeysi.end(); vitkeys!=vendkeys; vitkeys++)
                    {
                        cv::KeyPoint kpi = *vitkeys;	float tmpf; int tmpi;
                        tmpf=kpi.pt.x;		fkfKeys.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.pt.y;		fkfKeys.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.size;		fkfKeys.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.angle;		fkfKeys.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.response;	fkfKeys.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpi=kpi.octave;	fkfKeys.write(reinterpret_cast<char*>(&tmpi), sizeof(int));
                        tmpi=kpi.class_id;	fkfKeys.write(reinterpret_cast<char*>(&tmpi), sizeof(int));
                    }
                }

                {
                    vector<cv::KeyPoint> mvKeysiUn = pKFi->GetKeyPointsUn();
                    size_t nKeysUn=mvKeysiUn.size();
                    fkfKeysUn.write(reinterpret_cast<char*>(saveHeader),2);
                    fkfKeysUn.write(reinterpret_cast<char*>(&nKeysUn), sizeof(size_t));
                    for(vector<cv::KeyPoint>::iterator vitkeysun=mvKeysiUn.begin(), vendkeysun=mvKeysiUn.end(); vitkeysun!=vendkeysun; vitkeysun++)
                    {
                        cv::KeyPoint kpi = *vitkeysun;	float tmpf; int tmpi;
                        tmpf=kpi.pt.x;		fkfKeysUn.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.pt.y;		fkfKeysUn.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.size;		fkfKeysUn.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.angle;		fkfKeysUn.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpf=kpi.response;	fkfKeysUn.write(reinterpret_cast<char*>(&tmpf), sizeof(float));
                        tmpi=kpi.octave;	fkfKeysUn.write(reinterpret_cast<char*>(&tmpi), sizeof(int));
                        tmpi=kpi.class_id;	fkfKeysUn.write(reinterpret_cast<char*>(&tmpi), sizeof(int));
                    }
                }
                //2 -------------------------------

                //2 3.3 save descriptors
                //2 -------------------------------
                // 0xeb,0x90, nDes, nDes*(32*uint8)
                {
                    cv::Mat descriptorsi = pKFi->GetDescriptors();
                    int desnumi = descriptorsi.rows;
                    fkfDes.write(reinterpret_cast<char*>(saveHeader),2);
                    fkfDes.write(reinterpret_cast<char*>(&desnumi), sizeof(int));
                    for(int ti=0;ti<desnumi;ti++)
                    {
                        cv::Mat tdes = descriptorsi.row(ti);
                        for(int tj=0;tj<32;tj++)
                            fkfDes.write(reinterpret_cast<char*>(&(tdes.at<unsigned char>(tj))),sizeof(unsigned char));
                    }
                }
                //2 -------------------------------


                //2 3.4 save mappoint id
                //2 -------------------------------
                // 0xeb,0x90, KF.mnId, nMPs, nMPs*(MPmnId, MPidx)
                {
                    vector<MapPoint*> vpsi = pKFi->GetMapPointMatches();
                    size_t nMPcnt=0;
                    //compute valid mappoint number
                    for(size_t mvpMPidx=0, iend=vpsi.size();mvpMPidx<iend;mvpMPidx++)
                    {
                        MapPoint* vit = vpsi[mvpMPidx];
                        if(vit)
                            if(!vit->isBad())
                                nMPcnt++;
                    }
                    if(nMPcnt==0)	cerr<<"nMPcnt=0, shouldn't"<<endl;
                    fkfMPids.write(reinterpret_cast<char*>(saveHeader),2);
                    fkfMPids.write(reinterpret_cast<char*>(&pKFi->mnId), sizeof(long unsigned int));
                    fkfMPids.write(reinterpret_cast<char*>(&nMPcnt), sizeof(size_t));	//mnId & number of mappoints

                    size_t scnt=0;
                    for(size_t mvpMPidx=0, iend=vpsi.size();mvpMPidx<iend;mvpMPidx++)
                    {
                        MapPoint* vit = vpsi[mvpMPidx];
                        if(vit)
                            if(!vit->isBad())
                            {
                                fkfMPids.write(reinterpret_cast<char*>(&vit->mnId), sizeof(long unsigned int));
                                fkfMPids.write(reinterpret_cast<char*>(&mvpMPidx), sizeof(size_t));	//mnId & number of mappoints
                                scnt++;
                            }
                    }
                    if(scnt==0)	cerr<<"scnt==0, no good mappoint? shouldn't"<<endl;
                    if(scnt!=pKFi->GetMapPoints().size() || scnt!=nMPcnt)
                        cerr<<"scnt,pKFi->GetMapPoints.size,nMPcnt = "<<scnt<<","<<pKFi->GetMapPoints().size()<<","<<nMPcnt<<"scnt!=pKFi->GetMapPoints().size(), shouldn't"<<endl;
                }
                //2 -------------------------------


                //2 3.5 save loopedges
                //2 -------------------------------
                // 0xeb,0x90, KF.mnId, nLPEGs, nLPEGs*(lpKFmnId)
                //          parentKF.mnId ,  nChild, nChild*(childKFmnId)
                // (add:)   (0 for firstKF)
                {
                    set<KeyFrame*> lpedges = pKFi->GetLoopEdges();
                    size_t nlpegs = lpedges.size();
                    fkfLPEGs.write(reinterpret_cast<char*>(saveHeader),2);
                    fkfLPEGs.write(reinterpret_cast<char*>(&pKFi->mnId), sizeof(long unsigned int));
                    fkfLPEGs.write(reinterpret_cast<char*>(&nlpegs), sizeof(size_t));
                    if(nlpegs>0)
                    {
                        for(set<KeyFrame*>::iterator sit=lpedges.begin(), send=lpedges.end(); sit!=send; sit++)
                        {
                            KeyFrame * tpKFs=*sit;
                            fkfLPEGs.write(reinterpret_cast<char*>(&tpKFs->mnId), sizeof(long unsigned int));
                        }
                    }
                    // parent KeyFrame
                    size_t parentId=0;
                    if(pKFi->mnId!=0)
                        parentId = pKFi->GetParent()->mnId;
                    // Added by wangjing 160603
                    else
                    {
                        KeyFrame* pKFtmpp = pKFi->GetParent();
                        if(pKFtmpp)
                            parentId = pKFtmpp->mnId;
                    }
                    fkfLPEGs.write(reinterpret_cast<char*>(&parentId), sizeof(size_t));

                    // for test
                    if(pKFi->mnId==0)
                        if(pKFi->GetParent())
                        {
                            cout<<"parent of KF mnId=0 is not NULL!!!"<<endl;
                            cout<<"parent Id: "<<pKFi->GetParent()->mnId<<endl;
                        }

                    // children KeyFrames
                    set<KeyFrame*> spChildKFs = pKFi->GetChilds();
                    size_t nChild = spChildKFs.size();
                    fkfLPEGs.write(reinterpret_cast<char*>(&nChild), sizeof(size_t));
                    if(nChild>0)
                    {
                        size_t tmpn=0;
                        for(set<KeyFrame*>::iterator sit=spChildKFs.begin(),send=spChildKFs.end();sit!=send;sit++,tmpn++)
                        {
                            KeyFrame* pKFc = *sit;
                            fkfLPEGs.write(reinterpret_cast<char*>(&pKFc->mnId), sizeof(long unsigned int));

                            if(pKFc->isBad())
                                cout<<"child is bad??? shouldn't"<<endl;
                        }
                        if(tmpn!=nChild)
                            cout<<"tmpn!=nChild,shouldn't"<<endl;
                    }

                }
                //2 -------------------------------

                //2 3.6 save mGrid
                //2 -------------------------------
                // 0xeb,0x90, FRAME_GRID_COLS*FRAME_GRID_ROWS*(gridN, gridN*mGrid_ijk)
                {
                    vector< vector <vector<size_t> > > mGrid = pKFi->GetmGrid();
                    fkfmGrid.write(reinterpret_cast<char*>(saveHeader),2);
                    for(int i=0; i<FRAME_GRID_COLS;i++)
                    {
                        for(int j=0; j<FRAME_GRID_ROWS; j++)
                        {
                            vector<size_t> gridij = mGrid[i][j];
                            size_t gridN=gridij.size();
                            fkfmGrid.write(reinterpret_cast<char*>(&gridN),sizeof(size_t));
                            for(size_t k=0; k<gridN; k++)
                            {
                                fkfmGrid.write(reinterpret_cast<char*>(&gridij[k]),sizeof(size_t));
                            }
                        }
                    }
                }
                //2 -------------------------------


                kfSaveCnt++;

            }

        }

        fkfVar.close();
        fkfKeys.close();
        fkfKeysUn.close();
        fkfDes.close();
        fkfMPids.close();
        fkfLPEGs.close();
        fkfmGrid.close();

        cout<<"total "<<kfSaveCnt<<" KeyFrames saved."<<endl;
    }
    //1 ------------------------------------------------


    //1 4. save global parameters
    //1 ------------------------------------------------
    {
        fstream f;
        cout<<endl<<"Saving global params"<<endl;
        strFile = "./myMap/GlobalParams.bin";
        f.open(strFile.c_str(), ios::out|ios::binary);

        //3 Line1. MP.nNextID, mpSaveCnt, kfSaveCnt, Frame::nNextId, KeyFrame::nNextId
        //3 ------------------------------------------------
        long unsigned int tMPnNextId,tFRnNextId,tKFnNextId;
        tMPnNextId=MapPoint::nNextId;
        tFRnNextId=Frame::nNextId;
        tKFnNextId=KeyFrame::nNextId;
        f.write(reinterpret_cast<char*>(&tMPnNextId),	sizeof(long unsigned int));
        f.write(reinterpret_cast<char*>(&mpSaveCnt),	sizeof(long unsigned int));
        f.write(reinterpret_cast<char*>(&kfSaveCnt),	sizeof(long unsigned int));
        f.write(reinterpret_cast<char*>(&tFRnNextId),	sizeof(long unsigned int));
        f.write(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        //KeyFrame data
        KeyFrame* pKF0=static_cast<KeyFrame*>(NULL);
        vector<KeyFrame*> vpKFt = World.GetAllKeyFrames();
        for(vector<KeyFrame*>::iterator tvit=vpKFt.begin(), tvend=vpKFt.end(); tvit!=tvend; tvit++)
        {
            pKF0 = *tvit;
            if(pKF0->mnId==0)
                break;
        }
        if(pKF0->mnId != 0)
            cerr<<"pKF0->mnId != 0, shouldn't. Note!!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"<<endl;
        if(pKF0->isBad())
            cerr<<"pKF0->isBad(), shouldn't"<<endl;
        //3 ------------------------------------------------

        //3 Line2.  KF.nNextID, mfGridElementWidthInv, mfGridElementHeightInv,fx/fy/cx/cy,
        //3 (cont.) mnMinX,mnMinY,mnMaxX,mnMaxY
        //3 ------------------------------------------------
        f.write(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        f.write(reinterpret_cast<char*>(&pKF0->mfGridElementWidthInv),	sizeof(float));
        f.write(reinterpret_cast<char*>(&pKF0->mfGridElementHeightInv),	sizeof(float));
        f.write(reinterpret_cast<char*>(&pKF0->fx),	sizeof(float));
        f.write(reinterpret_cast<char*>(&pKF0->fy),	sizeof(float));
        f.write(reinterpret_cast<char*>(&pKF0->cx),	sizeof(float));
        f.write(reinterpret_cast<char*>(&pKF0->cy),	sizeof(float));
        vector<int> tv=pKF0->GetMinMaxXY();
        for(int ti=0;ti<4;ti++)
            f.write(reinterpret_cast<char*>(&tv[ti]),	sizeof(int));
        //3 ------------------------------------------------

        //3 Line3. mnScaleLevels0(N), N*mvScaleFactors0, N*mvLevelSigma20 for the first 2 KFs.
        //3 ------------------------------------------------
        int mscalelevels=pKF0->GetScaleLevels();
        f.write(reinterpret_cast<char*>(&mscalelevels),sizeof(int));
        vector<float> sfactors =  pKF0->GetScaleFactors();
        for(int i=0;i<mscalelevels;i++)
            f.write(reinterpret_cast<char*>(&sfactors[i]),sizeof(float));
        vector<float> lsigma2 = pKF0->GetVectorScaleSigma2();	//mvInvLevelSigma2 is 1/mvLevelSigma2
        for(int i=0;i<mscalelevels;i++)
            f.write(reinterpret_cast<char*>(&lsigma2[i]),sizeof(float));
        if((size_t)mscalelevels!=sfactors.size() || (size_t)mscalelevels!=lsigma2.size())
            cerr<<"mscalelevels!=sfactors.size() || mscalelevels!=lsigma2.size(), shouldn't"<<endl;
        //3 ------------------------------------------------

        //3 Line4. mnScaleLevels(N), N*mvScaleFactors, N*mvLevelSigma2 for other KFs.
        //3 ------------------------------------------------
        KeyFrame* pKFg=static_cast<KeyFrame*>(NULL);
        for(vector<KeyFrame*>::iterator kfit=vpKFt.begin(), kfend=vpKFt.end(); kfit!=kfend; kfit++)
        {
            pKFg=*kfit;
            if(pKFg)
                if(pKFg->mnId>2 && !pKFg->isBad())
                    break;
        }
        if(!pKFg) cerr<<"pKFg=NULL, shouldn't"<<endl;

        int mslevelsg=pKFg->GetScaleLevels();
        f.write(reinterpret_cast<char*>(&mslevelsg),sizeof(int));
        vector<float> sfactorsg =  pKFg->GetScaleFactors();
        for(int i=0;i<mslevelsg;i++)
            f.write(reinterpret_cast<char*>(&sfactorsg[i]),sizeof(float));
        vector<float> lsigma2g = pKFg->GetVectorScaleSigma2();	//mvInvLevelSigma2 is 1/mvLevelSigma2
        for(int i=0;i<mslevelsg;i++)
            f.write(reinterpret_cast<char*>(&lsigma2g[i]),sizeof(float));
        if((size_t)mslevelsg!=sfactorsg.size() || (size_t)mslevelsg!=lsigma2g.size())
            cerr<<"mslevelsg!=sfactorsg.size() || mslevelsg!=lsigma2g.size()"<<endl;
        //3 ------------------------------------------------


        f.close();
    }
    //1 ------------------------------------------------


}

static bool myOpenFile(fstream &ifs, string strFile)
{
    ifs.open(strFile.c_str(), ios::in | ios::binary);
    if(!ifs.is_open())
    {
        cout<<strFile<<" open failed."<<endl;
        return false;
    }
    if(ifs.eof())
    {
        cout<<strFile<<" empty."<<endl;
        ifs.close();
        return false;
    }
    return true;
}


// load plain variables of mappoints from file
bool loadMPVariables(KeyFrameDatabase *db, Map *wd, MapMPIndexPointer *mpIdxPtMap, 
                     VecUL &_VecMPmnId, VecUL &_vRefKFIdInMP)
{
    fstream ifs,ifGlobal;
    if(	!myOpenFile(ifs, "./myMap/mpVariables.bin")	||
            !myOpenFile(ifGlobal,	"./myMap/GlobalParams.bin")   )
        return false;

    //read global params
    long unsigned int tMPnNextId,mpSaveCnt,kfSaveCnt,tFRnNextId,tKFnNextId;
    cout<<endl<<"Reading MapPoint params"<<endl;
    {
        //3 Line1. MP.nNextID, mpSaveCnt, kfSaveCnt, Frame::nNextId, KeyFrame::nNextId
        //3 ------------------------------------------------
        ifGlobal.read(reinterpret_cast<char*>(&tMPnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&mpSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&kfSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tFRnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        //3 ------------------------------------------------

        MapPoint::nNextId = tMPnNextId;
        Frame::nNextId = tFRnNextId;
        KeyFrame::nNextId = tKFnNextId;
    }

    //record the reference KF's mnId, in the order of lines saved in file
    VecUL vRefKFIdInMP(mpSaveCnt);
    VecUL VecMPmnId(mpSaveCnt);

    Frame tmpFrame;
    tmpFrame.mTcw = Mat::eye(4,4,CV_32F);
    KeyFrame * tmpKF = new KeyFrame(tmpFrame, wd, db);


    unsigned long int linecnt=0;
    for(unsigned long int smpcnt=0;smpcnt<mpSaveCnt;smpcnt++)
    {
        unsigned char hd[2];	//header1=0xeb,header2=0x90.

        //2 2.1 read plain variable
        //2 ---------------------------
        // 0xeb,0x90,nNextId,mnId,mnFirstKFid,mTrackPorjX,mTrackProjY,mbTrackInView,mnTrackScaleLevel,mTrackViewCos,
        // mnTrackReferenceForFrame,mnLastFrameSeen,mnBALocalForKF,mnFuseCandidateForKF,mnLoopPointForKF,mnCorrectedByKF,mnCorrectedReference
        //  WorldPos,mDescriptor,mpRefKF
        // plain variables in MapPoint
        unsigned long int nNextId;
        long unsigned int mnId, mpRefKFId;
        long int mnFirstKFid;
        float mTrackProjX,mTrackProjY,mTrackViewCos;
        bool mbTrackInView;
        int mnTrackScaleLevel;
        long unsigned int mnTrackReferenceForFrame,mnLastFrameSeen,mnBALocalForKF,mnFuseCandidateForKF,mnLoopPointForKF,mnCorrectedByKF,mnCorrectedReference;
        Mat mWorldPos = Mat::zeros(3, 1, CV_32F);
        Mat mDescriptor = Mat::zeros(1, 32, CV_8UC1);
        {
            ifs.read(reinterpret_cast<char*>(hd),2);
            if(hd[0]!=0xeb || hd[1]!=0x90)
                cerr<<"header error mpVariables, shouldn't"<<endl;
            ifs.read(reinterpret_cast<char*>(&nNextId),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnId),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnFirstKFid),	sizeof(long int));
            ifs.read(reinterpret_cast<char*>(&mTrackProjX),sizeof(float));
            ifs.read(reinterpret_cast<char*>(&mTrackProjY),sizeof(float));
            ifs.read(reinterpret_cast<char*>(&mbTrackInView),sizeof(bool));
            ifs.read(reinterpret_cast<char*>(&mnTrackScaleLevel),sizeof(int));
            ifs.read(reinterpret_cast<char*>(&mTrackViewCos),sizeof(float));
            ifs.read(reinterpret_cast<char*>(&mnTrackReferenceForFrame),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnLastFrameSeen),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnBALocalForKF),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnFuseCandidateForKF),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnLoopPointForKF),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnCorrectedByKF),	sizeof(long unsigned int));
            ifs.read(reinterpret_cast<char*>(&mnCorrectedReference),	sizeof(long unsigned int));
            // protected
            for(int ti=0;ti<3;ti++)
            {
                float tmpf;
                ifs.read(reinterpret_cast<char*>(&tmpf),sizeof(float));
                mWorldPos.at<float>(ti)=tmpf;
            }
            for(int ti=0;ti<32;ti++)
            {
                unsigned char tmpuc;
                ifs.read(reinterpret_cast<char*>(&tmpuc),sizeof(unsigned char));
                mDescriptor.at<unsigned char>(ti)=tmpuc;
            }
            ifs.read(reinterpret_cast<char*>(&mpRefKFId),sizeof(long unsigned int));
            if(ifs.fail()) cerr<<"ifsfail in mpVariables, shouldn't."<<endl;
            if(mnFirstKFid<0) cerr<<"mnFirstKFid<0, shouldn't."<<endl;
        }
        //2 ---------------------------


        // new MapPoint memory space and pointer
        MapPoint* tmpMP = new MapPoint(mWorldPos, tmpKF, wd);
        MapPoint::nNextId = nNextId;

        //public
        tmpMP->mnId = mnId;
        tmpMP->mnFirstKFid = mnFirstKFid;

        tmpMP->mTrackProjX = mTrackProjX;
        tmpMP->mTrackProjY = mTrackProjY;
        tmpMP->mbTrackInView = mbTrackInView;
        tmpMP->mnTrackScaleLevel = mnTrackScaleLevel;
        tmpMP->mTrackViewCos = mTrackViewCos;
        tmpMP->mnTrackReferenceForFrame = mnTrackReferenceForFrame;
        tmpMP->mnLastFrameSeen = mnLastFrameSeen;
        tmpMP->mnBALocalForKF = mnBALocalForKF;
        tmpMP->mnFuseCandidateForKF = mnFuseCandidateForKF;
        tmpMP->mnLoopPointForKF = mnLoopPointForKF;
        tmpMP->mnCorrectedByKF = mnCorrectedByKF;
        tmpMP->mnCorrectedReference = mnCorrectedReference;

        //protected
        tmpMP->SetWorldPos(mWorldPos);
        tmpMP->SetDescriptor(mDescriptor);
        //		tmpMP->SetNormalVec(mNormalVector);		//set by UpdateNormalAndDepth(). need mObservations/mpRefKF/mWorldPos and KF.Tcw
        //		tmpMP->SetmnVisible(1);//(mnVisible);
        //      tmpMP->SetmnFound(1);//(mnFound);
        //		tmpMP->SetMinDistance(mfMaxDistance);
        //		tmpMP->SetMaxDistance(mfMaxDistance);

        //record reference KF id of this mappoint
        vRefKFIdInMP[linecnt] = mpRefKFId;

        //record mnId
        VecMPmnId[linecnt] = mnId;

        // add to the mapping from index to pointer
        if(mpIdxPtMap->count(mnId)>0)
            cerr<<mnId<<" mappoint count "<< mpIdxPtMap->count(mnId)<<"exist? shouldn't!!"<<endl;
        (*mpIdxPtMap)[mnId] = tmpMP;

        //		//to be added
        //		tmpMP->mObservations;
        //		tmpMP->mpRefKF;

        // increment count
        linecnt++;
    }

    //evaluate refId and mnId vector
    _vRefKFIdInMP = vRefKFIdInMP;
    _VecMPmnId = VecMPmnId;

    //
    cout<<"total "<<linecnt<<" MapPoints loaded."<<endl;
    if(linecnt!=mpSaveCnt)
        cerr<<"linecnt != mpSaveCnt, shouldn't"<<endl;

    //close file
    ifs.close();
    ifGlobal.close();

    //delete tmpKF;

    return true;
}


bool loadKFVariables(KeyFrameDatabase *db, Map *wd, ORBVocabulary* mpvoc,
                     MapKFIndexPointer *kfIdxPtMap, VecUL &_VecKFmnId)
{
    fstream ifkfVar,ifkfKeys,ifkfKeysUn,ifkfDes,ifkfmGrid,ifGlobal;
    if(	!myOpenFile(ifkfVar,	"./myMap/kfVariables.bin") 	||
            !myOpenFile(ifkfKeys,	"./myMap/kfKeyPoints.bin") 	||
            !myOpenFile(ifkfKeysUn,	"./myMap/kfKeyPointsUn.bin") 	||
            !myOpenFile(ifkfDes,	"./myMap/kfDescriptors.bin")	||
            !myOpenFile(ifkfmGrid,	"./myMap/kfmGrid.bin")		||
            !myOpenFile(ifGlobal,	"./myMap/GlobalParams.bin")   )
    {
        return false;
    }

    //read global params
    cout<<endl<<"Reading KeyFrame params"<<endl;

    long unsigned int tMPnNextId,mpSaveCnt,kfSaveCnt,tFRnNextId,tKFnNextId;
    float mfGridElementWidthInv, mfGridElementHeightInv, fx, fy, cx, cy;
    int mnMinX,mnMinY,mnMaxX,mnMaxY;
    int mnScaleLevels0, mnScaleLevelsOther;
    std::vector<float> mvScaleFactors0,mvScaleFactorsOther;
    std::vector<float> mvLevelSigma20,mvLevelSigma2Other;
    std::vector<float> mvInvLevelSigma20,mvInvLevelSigma2Other;
    {
        //3 Line1. MP.nNextID, mpSaveCnt, kfSaveCnt, Frame::nNextId, KeyFrame::nNextId
        //3 ------------------------------------------------
        ifGlobal.read(reinterpret_cast<char*>(&tMPnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&mpSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&kfSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tFRnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        //3 ------------------------------------------------

        MapPoint::nNextId = tMPnNextId;
        Frame::nNextId = tFRnNextId;
        KeyFrame::nNextId = tKFnNextId;
    }

    //record KeyFrame mnId
    VecUL VecKFmnId(kfSaveCnt);

    {
        //3 Line2.  KF.nNextID, mfGridElementWidthInv, mfGridElementHeightInv,fx/fy/cx/cy,
        //3 (cont.) mnMinX,mnMinY,mnMaxX,mnMaxY
        //3 ------------------------------------------------
        ifGlobal.read(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&mfGridElementWidthInv),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&mfGridElementHeightInv),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&fx),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&fy),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&cx),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&cy),	sizeof(float));
        ifGlobal.read(reinterpret_cast<char*>(&mnMinX),	sizeof(int));
        ifGlobal.read(reinterpret_cast<char*>(&mnMinY),	sizeof(int));
        ifGlobal.read(reinterpret_cast<char*>(&mnMaxX),	sizeof(int));
        ifGlobal.read(reinterpret_cast<char*>(&mnMaxY),	sizeof(int));
        //3 ------------------------------------------------
    }

    {
        //3 Line3. mnScaleLevels(N), N*mvScaleFactors, N*mvLevelSigma2 for the first 2 KFs.
        //3 ------------------------------------------------
        ifGlobal.read(reinterpret_cast<char*>(&mnScaleLevels0),	sizeof(int));
        mvScaleFactors0.resize(mnScaleLevels0);
        mvLevelSigma20.resize(mnScaleLevels0);
        mvInvLevelSigma20.resize(mnScaleLevels0);
        for(int i=0;i<mnScaleLevels0;i++)
            ifGlobal.read(reinterpret_cast<char*>(&mvScaleFactors0[i]),	sizeof(float));
        for(int i=0;i<mnScaleLevels0;i++)
        {
            ifGlobal.read(reinterpret_cast<char*>(&mvLevelSigma20[i]),	sizeof(float));
            mvInvLevelSigma20[i] = 1/mvLevelSigma20[i];
        }
        //3 ------------------------------------------------
    }

    {
        //3 Line4. mnScaleLevels(N), N*mvScaleFactors, N*mvLevelSigma2 for other KFs.
        //3 ------------------------------------------------
        ifGlobal.read(reinterpret_cast<char*>(&mnScaleLevelsOther),	sizeof(int));
        mvScaleFactorsOther.resize(mnScaleLevelsOther);
        mvLevelSigma2Other.resize(mnScaleLevelsOther);
        mvInvLevelSigma2Other.resize(mnScaleLevelsOther);
        for(int i=0;i<mnScaleLevelsOther;i++)
        {
            ifGlobal.read(reinterpret_cast<char*>(&mvScaleFactorsOther[i]),	sizeof(float));
            cout<<mvScaleFactorsOther[i]<<" ";
        }
        for(int i=0;i<mnScaleLevelsOther;i++)
        {
            ifGlobal.read(reinterpret_cast<char*>(&mvLevelSigma2Other[i]),	sizeof(float));
            mvInvLevelSigma2Other[i] = 1/mvLevelSigma2Other[i];
            cout<<mvLevelSigma2Other[i]<<" ";
        }
        //3 ------------------------------------------------
    }


    // create a temperary Frame, for global or static params of KeyFrames
    Frame tmpFrame;

    Frame::fx = fx;
    Frame::fy = fy;
    Frame::cx = cx;
    Frame::cy = cy;
    Frame::mfGridElementWidthInv = mfGridElementWidthInv;
    Frame::mfGridElementHeightInv = mfGridElementHeightInv;
    Frame::mnMinX = mnMinX;
    Frame::mnMinY = mnMinY;
    Frame::mnMaxX = mnMaxX;
    Frame::mnMaxY = mnMaxY;
    cv::Mat K = cv::Mat::eye(3,3,CV_32F);
    K.at<float>(0,0) = fx;
    K.at<float>(1,1) = fy;
    K.at<float>(0,2) = cx;
    K.at<float>(1,2) = cy;
    K.copyTo(tmpFrame.mK);
    tmpFrame.mpORBvocabulary = mpvoc;
    tmpFrame.mnScaleLevels = mnScaleLevelsOther;
    tmpFrame.mvScaleFactors = mvScaleFactorsOther;
    tmpFrame.mvLevelSigma2 = mvLevelSigma2Other;
    tmpFrame.mvInvLevelSigma2 = mvInvLevelSigma2Other;
    tmpFrame.mTcw = Mat::eye(4,4,CV_32F);

    // KeyFrame 0&1 is different. ORBextractor settings are fixed as 2000/1.2/8
    Frame tmpFrame0(tmpFrame);
    tmpFrame0.mnScaleLevels = mnScaleLevels0;
    tmpFrame0.mvScaleFactors = mvScaleFactors0;
    tmpFrame0.mvLevelSigma2 = mvLevelSigma20;
    tmpFrame0.mvInvLevelSigma2 = mvInvLevelSigma20;

    // read each row of the files
    unsigned long int linecnt=0;
    for(unsigned long int skfcnt=0;skfcnt<kfSaveCnt;skfcnt++)
    {
        unsigned char hd[2];	//header1=0xeb,header2=0x90.
        //new keyframe from tmp frame
        KeyFrame* tmpKF;
        long unsigned int nNextId,mnId,mnFrameId;

        //1 kfVariables.txt
        //2 3.1 save plain variables - read
        {
            //2 -------------------------------
            // 0xeb,0x90, nNextId, mnId, mnFrameId, mTimeStamp, Rcw, tcw
            //public
            double mTimeStamp;
            //protected
            cv::Mat Rcwi = Mat::eye(3, 3, CV_32F);
            cv::Mat tcwi = Mat::zeros(3, 1, CV_32F);

            ifkfVar.read(reinterpret_cast<char*>(hd),2);
            if(hd[0]!=0xeb || hd[1]!=0x90)
                cerr<<"header error kfVariables, shouldn't"<<endl;

            ifkfVar.read(reinterpret_cast<char*>(&nNextId), sizeof(long unsigned int));
            ifkfVar.read(reinterpret_cast<char*>(&mnId), sizeof(long unsigned int));
            ifkfVar.read(reinterpret_cast<char*>(&mnFrameId), sizeof(long unsigned int));
            ifkfVar.read(reinterpret_cast<char*>(&mTimeStamp), sizeof(double));
            // protected
            for(int ti=0;ti<3;ti++)
                for(int tj=0;tj<3;tj++)
                    ifkfVar.read(reinterpret_cast<char*>(&Rcwi.at<float>(ti,tj)), sizeof(float));
            for(int ti=0;ti<3;ti++)
                ifkfVar.read(reinterpret_cast<char*>(&tcwi.at<float>(ti)), sizeof(float));
            //2 -------------------------------
            if(mnId<=1) //mnId>=0 &&  always >=0
                tmpKF = new KeyFrame(tmpFrame0, wd, db);
            else
                tmpKF = new KeyFrame(tmpFrame, wd, db);

            KeyFrame::nNextId = nNextId;
            //evaluate
            tmpKF->mnId = mnId;
            tmpKF->mnFrameId = mnFrameId;
            tmpKF->mTimeStamp = mTimeStamp;
            tmpKF->SetPose(Rcwi,tcwi);
            tmpKF->mnTrackReferenceForFrame = 0;//mnTrackReferenceForFrame;
            tmpKF->mnFuseTargetForKF = 0;//mnFuseTargetForKF;
            tmpKF->mnBALocalForKF = 0;//mnBALocalForKF;
            tmpKF->mnBAFixedForKF = 0;//mnBAFixedForKF;
            tmpKF->mnLoopQuery = 0;//mnLoopQuery;
            tmpKF->mnLoopWords = 0;//mnLoopWords;
            tmpKF->mLoopScore = 0;//mLoopScore;
            tmpKF->mnRelocQuery = 0;//mnRelocQuery;
            tmpKF->mnRelocWords = 0;//mnRelocWords;
            tmpKF->mRelocScore = 0;//mRelocScore;
            //2 -------------------------------
        }



        //2 kfKeyPoints.txt
        //2 3.2 save KeyPoints and KeyPointsUn - read
        {
            //2 -------------------------------
            // 0xeb,0x90, nKeys, nKeys*(ptx,pty,size,angle,responsse,  octave,classid)
            {
                vector<cv::KeyPoint> tmvKeys;
                size_t kpn;
                ifkfKeys.read(reinterpret_cast<char*>(hd),2);
                if(hd[0]!=0xeb || hd[1]!=0x90)
                    cerr<<"header error kfKeyPoints, shouldn't"<<endl;

                ifkfKeys.read(reinterpret_cast<char*>(&kpn), sizeof(size_t));
                tmvKeys.resize(kpn);
                //for(size_t vit=0;vit<kpn;vit++)
                for(vector<cv::KeyPoint>::iterator vitkeys=tmvKeys.begin(), vendkeys=tmvKeys.end(); vitkeys!=vendkeys; vitkeys++)
                {
                    float ptx,pty,size,angle,response;
                    int octave,classid;
                    ifkfKeys.read(reinterpret_cast<char*>(&ptx), sizeof(float));
                    ifkfKeys.read(reinterpret_cast<char*>(&pty), sizeof(float));
                    ifkfKeys.read(reinterpret_cast<char*>(&size), sizeof(float));
                    ifkfKeys.read(reinterpret_cast<char*>(&angle), sizeof(float));
                    ifkfKeys.read(reinterpret_cast<char*>(&response), sizeof(float));
                    ifkfKeys.read(reinterpret_cast<char*>(&octave), sizeof(int));
                    ifkfKeys.read(reinterpret_cast<char*>(&classid), sizeof(int));
                    //tmvKeys[vit] = cv::KeyPoint(ptx,pty,size,angle,response,octave,classid);
                    *vitkeys = cv::KeyPoint(ptx,pty,size,angle,response,octave,classid);
                    if(ifkfKeys.fail())  cerr<<"loopcnt: "<<skfcnt<<" mnId "<<mnId<<" ifkfKeys fail. shouldn't"<<endl;
                }
                tmpKF->SetKeyPoints(tmvKeys);
            }
            //2 -------------------------------
            //		std::vector<cv::KeyPoint> mvKeys;

            //2 kfKeyPointsUn.txt
            {
                vector<cv::KeyPoint> tmvKeysUn;
                size_t kpun;
                ifkfKeysUn.read(reinterpret_cast<char*>(hd),2);
                if(hd[0]!=0xeb || hd[1]!=0x90)
                    cerr<<"header error kfKeyPointsUn, shouldn't"<<endl;

                ifkfKeysUn.read(reinterpret_cast<char*>(&kpun), sizeof(size_t));
                tmvKeysUn.resize(kpun);
                //for(size_t vit=0;vit<kpun;vit++)
                for(vector<cv::KeyPoint>::iterator vitkeysun=tmvKeysUn.begin(), vendkeysun=tmvKeysUn.end(); vitkeysun!=vendkeysun; vitkeysun++)
                {
                    float ptx,pty,size,angle,response;
                    int octave,classid;
                    ifkfKeysUn.read(reinterpret_cast<char*>(&ptx), sizeof(float));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&pty), sizeof(float));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&size), sizeof(float));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&angle), sizeof(float));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&response), sizeof(float));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&octave), sizeof(int));
                    ifkfKeysUn.read(reinterpret_cast<char*>(&classid), sizeof(int));
                    //tmvKeysUn[vit] = cv::KeyPoint(ptx,pty,size,angle,response,octave,classid);
                    *vitkeysun = cv::KeyPoint(ptx,pty,size,angle,response,octave,classid);
                    if(ifkfKeysUn.fail())  cerr<<"loopcnt: "<<skfcnt<<" mnId "<<mnId<<" ifkfKeysUn fail. shouldn't"<<endl;
                }
                tmpKF->SetKeyPointsUn(tmvKeysUn);
                //		std::vector<cv::KeyPoint> mvKeysUn;
            }
            //2 -------------------------------
        }


        //2 3.3 save descriptors - read
        //3 kfDescriptors.txt
        //2 -------------------------------
        // 0xeb,0x90, nDes, nDes*(32*uint8)
        {
            int desnumi;
            ifkfDes.read(reinterpret_cast<char*>(hd),2);
            if(hd[0]!=0xeb || hd[1]!=0x90)
                cerr<<"header error kfDescriptors, shouldn't"<<endl;

            ifkfDes.read(reinterpret_cast<char*>(&desnumi), sizeof(int));
            cv::Mat matDes = cv::Mat::zeros(desnumi,32,CV_8U);

            for(int ti=0;ti<desnumi;ti++)
            {
                cv::Mat tdes = cv::Mat::zeros(1,32,CV_8U);;
                for(int tj=0;tj<32;tj++)
                    ifkfDes.read(reinterpret_cast<char*>(&(tdes.at<unsigned char>(tj))),sizeof(unsigned char));
                tdes.copyTo(matDes.row(ti));
                if(ifkfDes.fail())  cerr<<"loopcnt: "<<skfcnt<<" mnId "<<mnId<<" ifkfDes fail. shouldn't"<<endl;
            }
            tmpKF->SetDescriptors(matDes);
            tmpKF->ComputeBoW();	//get mBowVec and mFeatVec
            //		cv::Mat mDescriptors;
            //		DBoW2::BowVector mBowVec;
            //		DBoW2::FeatureVector mFeatVec;
        }
        //2 -------------------------------

        //2 3.6 save mGrid
        //2 -------------------------------
        // 0xeb,0x90, FRAME_GRID_COLS*FRAME_GRID_ROWS*(gridN, gridN*mGrid_ijk)
        {
            vector< vector <vector<size_t> > > mGrid;
            ifkfmGrid.read(reinterpret_cast<char*>(hd),2);
            if(hd[0]!=0xeb || hd[1]!=0x90)
                cerr<<"header error kfmGrid, shouldn't"<<endl;
            mGrid.resize(FRAME_GRID_COLS);
            for(int i=0; i<FRAME_GRID_COLS;i++)
            {
                mGrid[i].resize(FRAME_GRID_ROWS);
                for(int j=0; j<FRAME_GRID_ROWS; j++)
                {
                    size_t gridN;
                    ifkfmGrid.read(reinterpret_cast<char*>(&gridN),sizeof(size_t));
                    vector<size_t> gridij(gridN);
                    for(size_t k=0; k<gridN; k++)
                    {
                        ifkfmGrid.read(reinterpret_cast<char*>(&gridij[k]),sizeof(size_t));
                    }
                    mGrid[i][j] = gridij;
                }
            }
            tmpKF->SetmGrid(mGrid);
            if(ifkfmGrid.fail())	cerr<<"ifkfmGrid.fail(), shouldn't"<<endl;
            //std::vector< std::vector <std::vector<size_t> > > mGrid
        }
        //2 -------------------------------


        //record the mnId order
        VecKFmnId[linecnt] = mnId;

        // add to the mapping from index to pointer
        if(kfIdxPtMap->count(mnId)>0)
            cerr<<mnId<<" KF count "<<kfIdxPtMap->count(mnId)<<" exist? shouldn't!!"<<endl;
        (*kfIdxPtMap)[mnId] = tmpKF;

        linecnt++;
    }

    //evaluate mnId vector
    _VecKFmnId = VecKFmnId;

    cout<<"total "<<linecnt<<" KeyFrames loaded."<<endl;
    if(_VecKFmnId.size()!=kfSaveCnt)
        cout<<"_VecKFmnId.size()!=kfSaveCnt"<<endl;
    if(linecnt!=kfSaveCnt)
        cerr<<"linecnt != kfSaveCnt, shouldn't"<<endl;

    //close file
    ifkfVar.close();
    ifkfKeys.close();
    ifkfKeysUn.close();
    ifkfDes.close();
    ifGlobal.close();
    ifkfmGrid.close();

    return true;
}



bool loadMPKFPointers(MapMPIndexPointer &mpIdxPtMap, MapKFIndexPointer &kfIdxPtMap,
                      const VecUL& VecKFmnId, const VecUL& VecMPmnId, const VecUL &vRefKFIdInMP)
{
    fstream ifkfMPids,ifkfLPEGs,ifGlobal,ifmpObs;
    if(	!myOpenFile(ifkfMPids,	"./myMap/kfMapPointsID.bin")	||
            !myOpenFile(ifkfLPEGs,	"./myMap/kfLoopEdges.bin")   	||
            !myOpenFile(ifmpObs,	"./myMap/mpObservations.bin")	||
            !myOpenFile(ifGlobal,	"./myMap/GlobalParams.bin")   	)
    {
        return false;
    }

    //read global params
    long unsigned int tMPnNextId,mpSaveCnt,kfSaveCnt,tFRnNextId,tKFnNextId;
    cout<<endl<<"Setting MapPoint and KeyFrame pointers"<<endl;
    {
        //3 Line1. MP.nNextID, mpSaveCnt, kfSaveCnt, Frame::nNextId, KeyFrame::nNextId
        ifGlobal.read(reinterpret_cast<char*>(&tMPnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&mpSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&kfSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tFRnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        MapPoint::nNextId = tMPnNextId;
        Frame::nNextId = tFRnNextId;
        KeyFrame::nNextId = tKFnNextId;
    }
    //check
    if(VecMPmnId.size()!=mpSaveCnt || vRefKFIdInMP.size()!=mpSaveCnt || mpIdxPtMap.size()!=mpSaveCnt)
        cerr<<"VecMPmnId.size()!=mpSaveCnt || vRefKFIdInMP.size()!=mpSaveCnt || MapMPIndexPointer.size()!=mpSaveCnt, shouldn't"<<endl;

    //------------------------------
    //1 MapPoint pointers
    // MapPoint mnId , line order in file
    for(size_t i=0;i<mpSaveCnt;i++)
    {
        long unsigned int tMPmnid = VecMPmnId[i];
        MapPoint* pMP = mpIdxPtMap[tMPmnid];	//MapPoint pointer of mnId
        if(kfIdxPtMap.count(vRefKFIdInMP[i])==0)
            cerr<<vRefKFIdInMP[i]<<" no KF id of vRefKFIdInMP[i], shouldn't"<<endl;
        KeyFrame* pkFref = kfIdxPtMap[vRefKFIdInMP[i]];    //Keyframe pointer to ReferenceKF
        pMP->SetRefKFPointer(pkFref);	//set mpRefKF
        //	tmpMP.mpRefKF;

        unsigned char hd[2];

        //2 2.2 save observations - read
        //2 ---------------------------
        // observation data
        // 0xeb,0x90,Nobs,Nobs*(ob.kfmnId,ob.mpIdx)
        ifmpObs.read(reinterpret_cast<char*>(hd),2);
        if(hd[0]!=0xeb || hd[1]!=0x90)
            cerr<<"header error mpObservations, shouldn't"<<endl;
        size_t nObs;
        ifmpObs.read(reinterpret_cast<char*>(&nObs),sizeof(size_t));
        if(nObs==0)	cerr<<"read nObs==0, shouldn't"<<endl;
        for(size_t j=0;j<nObs;j++)
        {
            long unsigned int kfIdj; size_t obIdj;	//mnId of KF see this MP, and id of observation
            ifmpObs.read(reinterpret_cast<char*>(&kfIdj), sizeof(long unsigned int));
            ifmpObs.read(reinterpret_cast<char*>(&obIdj), sizeof(size_t));	//KF id and MP index in KF
            if(kfIdxPtMap.count(kfIdj)==0)
            {
                cerr<<kfIdj<<" no KF id of this observation, shouldn't"<<endl;
                cerr<<endl;
            }
            else	//ignore wrong ones
            {
                KeyFrame* pKF = kfIdxPtMap[kfIdj];	//KF
                pMP->AddObservation(pKF,obIdj);		//add observation
            }
            if(ifmpObs.fail())	cerr<<"ifmpObs.fail(), shouldn't"<<endl;
        }
        //2 ---------------------------
    }


    //------------------------------
    //2 KeyFrame pointers
    for(long unsigned int linecnt=0;linecnt<kfSaveCnt;linecnt++)
    {
        unsigned char hd[2];
        //mnId corresponding to this line in file
        long unsigned int kfmnId=VecKFmnId[linecnt];
        //corresponding KeyFrame
        KeyFrame* pKF = kfIdxPtMap[kfmnId];

        //after all mappoints loaded
        //4. kfMapPointsID, for pKF->mvpMapPoints
        //2 3.4 save mappoint id - read
        //2 -------------------------------
        // 0xeb,0x90, KF.mnId, nMPs, nMPs*(MPmnId, MPidx)
        {
            long unsigned int kfmnIdread;
            size_t nMPid;
            ifkfMPids.read(reinterpret_cast<char*>(hd),2);
            if(hd[0]!=0xeb || hd[1]!=0x90)
                cerr<<"header error kfMapPointsID, shouldn't"<<endl;
            ifkfMPids.read(reinterpret_cast<char*>(&kfmnIdread), sizeof(long unsigned int));
            ifkfMPids.read(reinterpret_cast<char*>(&nMPid), sizeof(size_t));	//mnId & number of mappoints
            if(kfmnId!=kfmnIdread)	cerr<<"mpid: kfmnId!=VecKFmnId[linecnt], shouldn't"<<endl;
            if(nMPid==0) 	cerr<<"line " << linecnt<<" nMPid=0. shouldn't"<<endl;

            //KeyPoint number, size of mvpMapPoints
            size_t kpN = pKF->GetKeyPoints().size();
            if(kpN==0)	cerr<<"kpN==0 when read mappoint id, shouldn't"<<endl;
            vector<MapPoint*> mvpMPs = vector<MapPoint*>(kpN,static_cast<MapPoint*>(NULL));
            pKF->SetmvpMapPoints(mvpMPs);	//init mvpMapPoint as NULL (size = KeyPoints.size())

            //for each KeyFrame, set
            for(size_t i=0;i<nMPid;i++)
            {
                long unsigned int tmpid;
                size_t tvpMPidx;
                ifkfMPids.read(reinterpret_cast<char*>(&tmpid), sizeof(long unsigned int));
                ifkfMPids.read(reinterpret_cast<char*>(&tvpMPidx), sizeof(size_t)); //mnId & number of mappoints
                //pointer to the mappoint
                MapPoint* pMP = mpIdxPtMap[tmpid];
                pKF->AddMapPoint(pMP,tvpMPidx);
                //check
                if((int)tvpMPidx!=pMP->GetIndexInKeyFrame(pKF))
                    cerr<<tvpMPidx<<" "<<pMP->GetIndexInKeyFrame(pKF)<<"\ntvpMPidx!=pMP->GetIndexInKeyFrame(pKF), shouldn't"<<endl;
                if(ifkfMPids.fail()) cerr<<"kfmnId:"<<kfmnId<<" linecnt: "<<linecnt<<"ssMPids fail. shouldn't"<<endl;
            }
            //pKF->mvpMapPoints
            //		std::vector<MapPoint*> mvpMapPoints;
            //2 -------------------------------
        }



    }

    ///////////////////////////////////////////
    //after all keyframes and mappoints loaded
    ///////////////////////////////////////////

    //build spanning tree. in increasing order of KeyFrame mnId
    {
        unsigned long int preidx=0;
        for(MapKFIndexPointer::iterator mit=kfIdxPtMap.begin(), mend=kfIdxPtMap.end(); mit!=mend; mit++)
        {
            KeyFrame* pKFm=mit->second;
            pKFm->UpdateConnections();
            if(preidx>pKFm->mnId)
                cerr<<"KeyFrame pre Id > cur Id, shouldn't."<<endl;
            preidx=pKFm->mnId;
        }

        for(MapKFIndexPointer::iterator mit=kfIdxPtMap.begin(), mend=kfIdxPtMap.end(); mit!=mend; mit++)
        {
            KeyFrame* pKFm=mit->second;
            pKFm->clearChild();
        }
        //		std::map<KeyFrame*,int> mConnectedKeyFrameWeights;
        //		std::vector<KeyFrame*> mvpOrderedConnectedKeyFrames;
        //		std::vector<int> mvOrderedWeights;
        //		KeyFrame* mpParent;
        //		std::set<KeyFrame*> mspChildrens;
    }

    //after all keyframes loaded. should after the above 'connection update'
    //5. kfLoopEdges, load kf id
    //2 3.5 save loopedges
    //2 -------------------------------
    // 0xeb,0x90, KF.mnId, nLPEGs, nLPEGs*(lpKFmnId)
    //------------------------------
    //2 KeyFrame pointers
    for(long unsigned int linecnt=0;linecnt<kfSaveCnt;linecnt++)
    {
        unsigned char hd[2];
        //mnId corresponding to this line in file
        long unsigned int kfmnId=VecKFmnId[linecnt];
        //corresponding KeyFrame
        KeyFrame* pKF = kfIdxPtMap[kfmnId];

        long unsigned int kfmnIdread;
        size_t nLPEGid;
        ifkfLPEGs.read(reinterpret_cast<char*>(hd),2);
        if(hd[0]!=0xeb || hd[1]!=0x90)
            cerr<<"header error kfLoopEdges, shouldn't"<<endl;
        ifkfLPEGs.read(reinterpret_cast<char*>(&kfmnIdread), sizeof(long unsigned int));
        ifkfLPEGs.read(reinterpret_cast<char*>(&nLPEGid), sizeof(size_t));
        if(kfmnId!=kfmnIdread) cerr<<"lpeg: kfmnId!=VecKFmnId[linecnt], shouldn't"<<endl;
        //set each loop edge kf pointer
        if(nLPEGid>0)
            for(size_t i=0;i<nLPEGid;i++)
            {
                long unsigned int tkfmnId;
                ifkfLPEGs.read(reinterpret_cast<char*>(&tkfmnId), sizeof(long unsigned int));
                pKF->AddLoopEdge(kfIdxPtMap[tkfmnId]);
            }


        //          parentKF.mnId ,  nChild, nChild*(childKFmnId)
        // (add:)   (0 for firstKF)
        // parent KeyFrame
        size_t parentId=0;
        ifkfLPEGs.read(reinterpret_cast<char*>(&parentId), sizeof(size_t));
        if(pKF->mnId!=0)
            pKF->ChangeParent(kfIdxPtMap[parentId]);
        // Added by wangjing 160603
        else
        {
            if(parentId!=0)
                pKF->ChangeParent(kfIdxPtMap[parentId]);
        }

        // children KeyFrames
        size_t nChild;
        ifkfLPEGs.read(reinterpret_cast<char*>(&nChild), sizeof(size_t));

        if(nChild>0)
        {
            for(size_t i=0;i<nChild;i++)
            {
                long unsigned int tkfmnId;
                ifkfLPEGs.read(reinterpret_cast<char*>(&tkfmnId), sizeof(long unsigned int));
                pKF->AddChild(kfIdxPtMap[tkfmnId]);
            }
        }

        if(ifkfLPEGs.fail())	cerr<<"ifkfLPEGs.fail(), shouldn't"<<endl;
        //pKF->mspLoopEdges
        //		std::set<KeyFrame*> mspLoopEdges;
        //2 -------------------------------
    }


    //UpdateNormalAndDepth, need the mObservations/mpRefKF/mWorldPos
    {
        unsigned long int preidxmp=0;
        for(MapMPIndexPointer::iterator mit=mpIdxPtMap.begin(), mend=mpIdxPtMap.end(); mit!=mend; mit++)
        {
            MapPoint* pMPm=mit->second;
            pMPm->UpdateNormalAndDepth();
            if(preidxmp>pMPm->mnId)
                cerr<<"MapPoint pre Id > cur Id, shouldn't."<<endl;
            preidxmp=pMPm->mnId;
        }
        //		float mfMinDistance;
        //		float mfMaxDistance;
        //		cv::Mat mNormalVector;
    }

    ifkfMPids.close();
    ifkfLPEGs.close();
    ifmpObs.close();
    ifGlobal.close();

    return true;
}

bool LoadWroldFromFile(KeyFrameDatabase *db, Map *wd, ORBVocabulary* mpvoc, KeyFrame *pLastKF)//, MapMPIndexPointer &_mpIdxPtMap)
{
    MapMPIndexPointer mpIdxPtMap;
    MapKFIndexPointer kfIdxPtMap;
    VecUL vRefKFIdInMP;
    VecUL vKFmnId,vMPmnId;
    bool ret1,ret2,ret3,ret4;

    long unsigned int maxKFid=0;

    //1 step 1. load and create all mappoints
    cout<<"loading step 1.."<<endl;
    ret1=loadMPVariables(db,wd,&mpIdxPtMap,vMPmnId,vRefKFIdInMP);

    //1 step 2. load and craete all keyframes
    cout<<"loading step 2.."<<endl;
    ret2=loadKFVariables(db,wd,mpvoc,&kfIdxPtMap,vKFmnId);

    //1 step 3. associate pointers in MPs and KFs
    cout<<"loading step 3.."<<endl;
    ret3=loadMPKFPointers(mpIdxPtMap, kfIdxPtMap, vKFmnId, vMPmnId, vRefKFIdInMP );
    if(mpIdxPtMap.size()!=vMPmnId.size())
        cerr<<"mpIdxPtMap.size()!=vMPmnId.size()"<<endl;
    if(kfIdxPtMap.size()!=vKFmnId.size())
        cerr<<"kfIdxPtMap.size()!=vKFmnId.size()"<<endl;

    //1 step 4. associate pointers in invertfile of vocabulary
    cout<<"loading step 4.."<<endl;
    for(MapKFIndexPointer::iterator mit=kfIdxPtMap.begin(), mend=kfIdxPtMap.end(); mit!=mend; mit++)
    {
        KeyFrame* pKF = mit->second;
        db->add(pKF);
        if(maxKFid<pKF->mnId)
            maxKFid = pKF->mnId;
    }

    //1 step5. evaluate nNextId for Frame/MapPoint/KeyFrame
    ifstream ifGlobal;
    ret4 = myOpenFile(ifGlobal,	"./myMap/GlobalParams.bin");
    if(ret4)
    {
        long unsigned int tMPnNextId,mpSaveCnt,kfSaveCnt,tFRnNextId,tKFnNextId;
        //3 Line1. MP.nNextID, mpSaveCnt, kfSaveCnt, Frame::nNextId, KeyFrame::nNextId
        ifGlobal.read(reinterpret_cast<char*>(&tMPnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&mpSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&kfSaveCnt),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tFRnNextId),	sizeof(long unsigned int));
        ifGlobal.read(reinterpret_cast<char*>(&tKFnNextId),	sizeof(long unsigned int));
        MapPoint::nNextId = tMPnNextId;
        Frame::nNextId = tFRnNextId;
        KeyFrame::nNextId = tKFnNextId;
        ifGlobal.close();
    }

    //1 step 6. world
    if(ret1&&ret2&&ret3&&ret4)
    {
        cout<<"loading step 6.."<<endl;
        for(MapKFIndexPointer::iterator mit=kfIdxPtMap.begin(), mend=kfIdxPtMap.end(); mit!=mend; mit++)
        {
            wd->AddKeyFrame(mit->second);
        }
        for(MapMPIndexPointer::iterator mit=mpIdxPtMap.begin(), mend=mpIdxPtMap.end(); mit!=mend; mit++)
        {
            wd->AddMapPoint(mit->second);
        }
    }

    pLastKF = kfIdxPtMap[maxKFid];
    //_mpIdxPtMap = mpIdxPtMap;

    return (ret1&&ret2&&ret3&&ret4);
}


#endif

