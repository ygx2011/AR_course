LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
MAIN_DIR:=$(LOCAL_PATH)
OPENCV_LIB_TYPE:=STATIC
ifeq ("$(wildcard $(OPENCV_MK_PATH))","")  
#try to load OpenCV.mk from default install location  
include /Users/gaoxuanying/workspace/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk
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

include /Users/gaoxuanying/workspace/OpenCV-2.4.9-android-sdk/sdk/native/jni/OpenCV.mk

LOCAL_MODULE    := MarkerLessARAndroid
LOCAL_C_INCLUDES += $(LOCAL_PATH)
LOCAL_SRC_FILES := $(LOCAL_PATH)/ARMarkerLessAndroid.cpp

LOCAL_SHARED_LIBRARIES+=OpenCV

LOCAL_LDLIBS += -llog -ldl -DNDEBUG
LOCAL_LDFLAGS += -pthread -fopenmp 
LOCAL_CFLAGS += -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon
LOCAL_CPPFLAGS := -std=c++11 -pthread -frtti -fexceptions -DNDEBUG -O1 -O2 -O3 -Os -Ofast -ffunction-sections -fdata-sections -mfloat-abi=softfp -mfpu=neon -fopenmp
LOCAL_ARM_NEON := true
LOCAL_ARM_MODE := arm

include $(BUILD_SHARED_LIBRARY)
