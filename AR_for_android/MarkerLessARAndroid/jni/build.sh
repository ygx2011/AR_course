#!/bin/bash
# To build a new jni .so
# and copy the file to where it should stay

FILENAME="libMarkerARAndroid.so"

PROJECT_PATH=$(dirname $(cd $(dirname ${BASH_SOURCE[0]} ); pwd ) )
DEST_PATH="/Users/gaoxuanying/Desktop/AR_SDK_ygx/Android-AR-SDK/MarkerLessARAndroid/jni"
NDK_PATH="/Users/gaoxuanying/Library/Android/sdk/ndk-bundle"

if [ $(basename $(pwd) ) != "jni" ]; then
  export NDK_PROJECT_PATH=$PROJECT_PATH
fi

${NDK_PATH}/ndk-build

if [ ! -e ${PROJECT_PATH}/libs/armeabi-v7a/${FILENAME} ]; then
  echo " Compile process failed, no .so file. "
  echo "--------------------------------------"
else
  echo " Has compiled. "
  echo "---------------"

  if [ -e ${DEST_PATH}/armeabi-v7a/${FILENAME} ]; then
    rm --preserve ${DEST_PATH}/armeabi-v7a/${FILENAME}
	echo " The old jni .so has been deleted. "
	echo "-------------------------------"
  fi
  cp ${PROJECT_PATH}/libs/armeabi-v7a/${FILENAME} ${DEST_PATH}/armeabi-v7a/${FILENAME}
fi

#${NDK_PATH}/ndk-build clean
echo " Has done and cleaned."

:<<!EOF!
!EOF!
