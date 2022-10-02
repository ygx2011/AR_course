package com.example.orb_slam2_android_alan;


import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;

import org.opencv.android.BaseLoaderCallback;
import org.opencv.android.CameraBridgeViewBase;
import org.opencv.android.LoaderCallbackInterface;
import org.opencv.android.OpenCVLoader;
import org.opencv.android.CameraBridgeViewBase.CvCameraViewFrame;
import org.opencv.android.CameraBridgeViewBase.CvCameraViewListener2;
import org.opencv.core.Core;
import org.opencv.core.CvType;
import org.opencv.core.Mat;
import org.opencv.core.Point;
import org.opencv.core.Scalar;
import org.opencv.imgproc.Imgproc;

import android.app.Activity;
import android.content.Context;
import android.content.pm.ActivityInfo;
import android.graphics.PixelFormat;
import android.opengl.GLSurfaceView;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.SurfaceView;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.WindowManager.LayoutParams;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.Toast;



public class MainActivity extends Activity implements OnTouchListener, CvCameraViewListener2 {

	private Mat mRgba;
	private CameraBridgeViewBase mOpenCvCameraView;
	private long lastTime = 0;
	private long costTime = 0;

	private BaseLoaderCallback  mLoaderCallback = new BaseLoaderCallback(this) {
	        @Override
	    public void onManagerConnected(int status) {
	        switch (status) {
	            case LoaderCallbackInterface.SUCCESS:
	            {
	                mOpenCvCameraView.enableView();                 
	            } break;
	            default:
	            {
	                super.onManagerConnected(status);
	            } break;
	        }
	    }
	};
	    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		CopyAssets("data","/storage/emulated/0/Alan");
		if (OpenCVLoader.initDebug()) {
			System.loadLibrary("opencv_java");
			System.loadLibrary("SLAM-ALAN");// load libraries
		}	
		 
		super.onCreate(savedInstanceState);
		
	    getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		setContentView(R.layout.activity_main);
		
		mOpenCvCameraView = (CameraBridgeViewBase) findViewById(R.id.activity_surface_view);
		mOpenCvCameraView.setMaxFrameSize(320, 240);
//		mOpenCvCameraView.setMaxFrameSize(640, 480);
		mOpenCvCameraView.enableFpsMeter();
		mOpenCvCameraView.setVisibility(SurfaceView.VISIBLE);
		mOpenCvCameraView.setCvCameraViewListener(this);
     
        // Now let's create an OpenGL surface.
        GLSurfaceView glView = new GLSurfaceView( this );
        // To see the camera preview, the OpenGL surface has to be created translucently.
        // See link above.
        glView.setEGLConfigChooser( 8, 8, 8, 8, 16, 0 );
        glView.setRenderer( new GLRender() );
        glView.getHolder().setFormat( PixelFormat.TRANSLUCENT );
        glView.setZOrderOnTop(true);
        // Now set this as the main view.
        addContentView( glView, new LayoutParams( LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT ) );
	}
	
    @Override
    public void onPause()
    {
        super.onPause();
        if (mOpenCvCameraView != null)
            mOpenCvCameraView.disableView();
    }

    @Override
    public void onResume()
    {
        super.onResume();
        mLoaderCallback.onManagerConnected(LoaderCallbackInterface.SUCCESS);
    }

    public void onDestroy() {
        super.onDestroy();
        if (mOpenCvCameraView != null)
            mOpenCvCameraView.disableView();
    }

	@Override
	public void onCameraViewStarted(int width, int height) {
		// TODO Auto-generated method stub
		 mRgba = new Mat(height, width, CvType.CV_8UC4);
	}

	@Override
	public void onCameraViewStopped() {
		// TODO Auto-generated method stub
		mRgba.release();
	}

	@Override
	public Mat onCameraFrame(CvCameraViewFrame inputFrame) {
		// TODO Auto-generated method stub
		mRgba = inputFrame.rgba();
		
		this.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				NativeLoaderORB.runSLAM_Alan(mRgba.getNativeObjAddr());
				GLRender.currentRt = NativeLoaderORB.getRTfromSLAM();
			}
		});
		
		return mRgba;
	}

	@Override
	public boolean onTouch(View arg0, MotionEvent arg1) {
		// TODO Auto-generated method stub
		return false;
	}
	
    private void CopyAssets(String assetDir,String dir) {  
        String[] files;      
         try      
         {      
             files = this.getResources().getAssets().list(assetDir);      
         }      
         catch (IOException e1)      
         {      
             return;      
         }      
         File mWorkingPath = new File(dir);  
         //if this directory does not exists, make one.   
         if(!mWorkingPath.exists())      
         {      
             if(!mWorkingPath.mkdirs())      
             {      
                      
             }      
         }      
           
         for(int i = 0; i < files.length; i++)      
         {      
             try      
             {      
                 String fileName = files[i];   
                 //we make sure file name not contains '.' to be a folder.   
                 if(!fileName.contains("."))  
                 {  
                     if(0==assetDir.length())  
                     {  
                         CopyAssets(fileName,dir+fileName+"/");  
                     }  
                     else  
                     {  
                         CopyAssets(assetDir+"/"+fileName,dir+fileName+"/");  
                     }  
                     continue;  
                 }  
                 File outFile = new File(mWorkingPath, fileName);      
                 if(outFile.exists())   
                     outFile.delete();  
                 InputStream in =null;  
                 if(0!=assetDir.length())  
                     in = getAssets().open(assetDir+"/"+fileName);      
                 else  
                     in = getAssets().open(fileName);  
                 OutputStream out = new FileOutputStream(outFile);      
           
                 // Transfer bytes from in to out     
                 byte[] buf = new byte[1024];      
                 int len;      
                 while ((len = in.read(buf)) > 0)      
                 {      
                     out.write(buf, 0, len);      
                 }      
           
                 in.close();      
                 out.close();      
             }      
             catch (FileNotFoundException e)      
             {      
                 e.printStackTrace();      
             }      
             catch (IOException e)      
             {      
                 e.printStackTrace();      
             }           
        }  
    }

}

