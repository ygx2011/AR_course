LOCAL_PATH:= $(call my-dir)  

#############DLib模块##################

include $(CLEAR_VARS)
MAINDIR:= $(LOCAL_PATH)
include $(MAINDIR)/Thirdparty/clapack/Android.mk
LOCAL_PATH := $(MAINDIR)

include $(CLEAR_VARS)
MAINDIR:= $(LOCAL_PATH)
LOCAL_MODULE:= lapack
LOCAL_SHORT_COMMANDS := true
LOCAL_STATIC_LIBRARIES := tmglib clapack blas f2c

LOCAL_EXPORT_C_INCLUDES := $(LOCAL_C_INCLUDES)
LOCAL_EXPORT_LDLIBS := $(LOCAL_LDLIBS)
LOCAL_PATH := $(MAINDIR)
include $(BUILD_SHARED_LIBRARY)
######################################

#############DLib模块##################
include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)
OPENCV_LIB_TYPE:=STATIC
ifeq ("$(wildcard $(OPENCV_MK_PATH))","")  
#try to load OpenCV.mk from default install location  
include /Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk
else  
include $(OPENCV_MK_PATH)  
endif 
LOCAL_PATH:=$(MAIN_DIR)
LOCAL_MODULE:=DLib

####################源文件部分######################################
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include/DUtils
FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/DBoW2/DLib/src/DUtils/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

#LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include/DUtilsCV
#FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/DBoW2/DLib/src/DUtilsCV/*.cpp)
#LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)
#
#LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include/DVision
#FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/DBoW2/DLib/src/DVision/*.cpp)
#LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

################BOOST#####################部分
BOOST_VERSION      := 1_49
PROJECT_ROOT       := $(LOCAL_PATH)
BOOST_INCLUDE_PATH := $(PROJECT_ROOT)/Thirdparty/Boost/include/boost-1_49
BOOST_LIB_PATH     := $(PROJECT_ROOT)/Thirdparty/Boost/lib
LOCAL_C_INCLUDES+= $(BOOST_INCLUDE_PATH) 
LOCAL_LDLIBS    := -llog
# The order of these libraries is often important.
LOCAL_LDLIBS += -L$(BOOST_LIB_PATH)     
LOCAL_LDLIBS    +=-lz -llog -landroid -lEGL -lGLESv1_CM
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS += -D__cplusplus=201103L
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm
#LOCAL_EXPORT_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include/DUtils
#LOCAL_EXPORT_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include/DUtilsCV
LOCAL_EXPORT_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/DLib/include
LOCAL_EXPORT_C_INCLUDES+=$(BOOST_INCLUDE_PATH)
LOCAL_EXPORT_C_INCLUDES+=/Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/include
LOCAL_PATH:=$(MAIN_DIR)
include $(BUILD_SHARED_LIBRARY)
###############################################################

##############DBoW2模块#########################################
include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)

OPENCV_LIB_TYPE:=STATIC
ifeq ("$(wildcard $(OPENCV_MK_PATH))","")  
#try to load OpenCV.mk from default install location  
include /Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk
else  
include $(OPENCV_MK_PATH)  
endif 
LOCAL_PATH:=$(MAIN_DIR)

LOCAL_MODULE:=DBoW2
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/include/DBoW2
FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/DBoW2/src/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)
LOCAL_SHARED_LIBRARIES+=DLib
LOCAL_EXPORT_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/DBoW2/include
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS += -D__cplusplus=201103L
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm
LOCAL_PATH:=$(MAIN_DIR)
include $(BUILD_SHARED_LIBRARY)
###############################################################


###################G2O模块##################################
include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/g2o/g2o/core
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/g2o/g2o/stuff
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/g2o/g2o/solvers
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/g2o/g2o/types
FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/g2o/g2o/core/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/g2o/g2o/solvers/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/g2o/g2o/stuff/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

FILE_LIST:=$(wildcard $(LOCAL_PATH)/Thirdparty/g2o/g2o/types/*.cpp)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)

LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/eigen3
LOCAL_SHARED_LIBRARIES+=lapack
LOCAL_MODULE:=g2o
LOCAL_EXPORT_LDLIBS := $(LOCAL_LDLIBS)
LOCAL_EXPORT_C_INCLUDES+=LOCAL_C_INCLUDES
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS += -D__cplusplus=201103L
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm
LOCAL_PATH:=$(MAIN_DIR)
include $(BUILD_SHARED_LIBRARY)
############################################################

##############ORB_SLAM2模块##################################
include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)
OPENCV_LIB_TYPE:=STATIC
ifeq ("$(wildcard $(OPENCV_MK_PATH))","")  
#try to load OpenCV.mk from default install location  
include /Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk
else  
include $(OPENCV_MK_PATH)  
endif 
LOCAL_MODULE := ORB_SLAM2
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/eigen3
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/ORB_SLAM2/include
FILE_LIST:=$(wildcard $(LOCAL_PATH)/ORB_SLAM2/src/*.cc)
LOCAL_SRC_FILES+=$(FILE_LIST:$(LOCAL_PATH)/%=%)
LOCAL_CPP_EXTENSION := .cc
LOCAL_SHARED_LIBRARIES+=DBoW2
LOCAL_SHARED_LIBRARIES+=DLib
LOCAL_SHARED_LIBRARIES+=g2o
LOCAL_EXPORT_C_INCLUDES+=$(LOCAL_PATH)/ORB_SLAM2/include
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -fopenmp -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS += -D__cplusplus=201103L -fopenmp 
LOCAL_LDFLAGS += -fopenmp 
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm
#LOCAL_CFLAGS += -fopenmp 
LOCAL_PATH:=$(MAIN_DIR)
include $(BUILD_SHARED_LIBRARY)
############################################################

##############ORB_SLAM2 執行模块###############################
include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)
OPENCV_LIB_TYPE:=STATIC
ifeq ("$(wildcard $(OPENCV_MK_PATH))","")  
#try to load OpenCV.mk from default install location  
include /Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk
else  
include $(OPENCV_MK_PATH)  
endif 
LOCAL_PATH:=$(MAIN_DIR)
LOCAL_MODULE:=OpenCV
include $(BUILD_SHARED_LIBRARY)


include $(CLEAR_VARS)
OpenCV_CAMERA_MODULES:=on
OpenCV_INSTALL_MODULES:=off
OpenCV_LIB_TYPE:=STATIC
include /Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/orbslam2_AR_android/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk

LOCAL_MODULE := SLAMAR
LOCAL_C_INCLUDES+=$(LOCAL_PATH)/Thirdparty/eigen3
LOCAL_SRC_FILES+=ORB-SLAM2-ANDROID.cpp
LOCAL_SHARED_LIBRARIES+=OpenCV
LOCAL_SHARED_LIBRARIES+=ORB_SLAM2
LOCAL_SHARED_LIBRARIES+=g2o
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -fopenmp #-ftemplate-backtrace-limit=0
#LOCAL_CPPFLAGS += -D__cplusplus=201103L
LOCAL_LDLIBS += -llog -ldl -DNDEBUG
LOCAL_LDFLAGS += -pthread -fopenmp 
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm
LOCAL_PATH:=$(MAIN_DIR)
include $(BUILD_SHARED_LIBRARY)
############################################################
