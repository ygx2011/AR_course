package com.example.orb_slam2_android_alan;

public class NativeLoaderORB {
	
	public static native void runSLAM_Alan(long inputImage);

	public static native float[] getRTfromSLAM();

}

