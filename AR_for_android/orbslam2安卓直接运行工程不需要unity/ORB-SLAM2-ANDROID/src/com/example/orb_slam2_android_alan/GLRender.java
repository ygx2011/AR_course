package com.example.orb_slam2_android_alan;

import javax.microedition.khronos.egl.EGLConfig;
import javax.microedition.khronos.opengles.GL10;
import android.opengl.GLU;
import android.opengl.GLSurfaceView.Renderer;
import android.opengl.Matrix;

public class GLRender implements Renderer{
	
	public static float[] currentRt = new float[16];
	private Cube mCube = new Cube();
    private float mCubeRotation;
    public static float[] projectionMatrix = new float[16];
    public static float[] cameraMatrix = new float[9];
    public static float[] modelMatrix = new float[16];

   public void onDrawFrame( GL10 gl ) {
     
       gl.glClear(GL10.GL_COLOR_BUFFER_BIT | GL10.GL_DEPTH_BUFFER_BIT);     
   
       getCameraMatrix(320, 240);
       gl.glMatrixMode(GL10.GL_PROJECTION);
       gl.glLoadMatrixf(projectionMatrix, 0);

       gl.glMatrixMode(GL10.GL_MODELVIEW);
       gl.glLoadIdentity();

       gl.glPushMatrix();
       
       float qw = (float) (Math.sqrt(1.0 + currentRt[0] + currentRt[5] + currentRt[10]) / 2.0);
       float qx = -(currentRt[9] - currentRt[6]) / (4*qw) ;
       float qy = -(currentRt[2] - currentRt[8]) / (4*qw) ;
       float qz = -(currentRt[4] - currentRt[1]) / (4*qw) ;
       float t0 = -currentRt[12];
       float t1 = -currentRt[13];
       float t2 = -currentRt[14];
       
       modelMatrix[0] = 1 - 2*qy*qy - 2*qz*qz;
       modelMatrix[1] = 2*qx*qy + 2*qz*qw;
       modelMatrix[2] = 2*qx*qz - 2*qy*qw;
       modelMatrix[3] = 0;
       modelMatrix[4] = 2*qx*qy - 2*qz*qw;
       modelMatrix[5] = 1 - 2*qx*qx - 2*qz*qz;
       modelMatrix[6] = 2*qy*qz + 2*qx*qw;
       modelMatrix[7] = 0;
       modelMatrix[8] = 2*qx*qz + 2*qy*qw;
       modelMatrix[9] = 2*qy*qz - 2*qx*qw;
       modelMatrix[10] = 1 - 2*qx*qx - 2*qy*qy;
       modelMatrix[11] = 0;
       modelMatrix[12] = t0;
       modelMatrix[13] = t1;
       modelMatrix[14] = t2;
       modelMatrix[15] = 1;
       
       gl.glMultMatrixf(modelMatrix,0);
       
       gl.glScalef(0.05f, 0.05f, 0.05f);
       mCube.draw(gl);
       gl.glPopMatrix();
         
   }

   public void onSurfaceChanged( GL10 gl, int width, int height ) {
       // This is called whenever the dimensions of the surface have changed.
       // We need to adapt this change for the GL viewport.

       gl.glMatrixMode(GL10.GL_PROJECTION);
       gl.glLoadIdentity();

       GLU.gluPerspective(gl, 45.0f, (float)4 / (float)3, 0.1f, 100.0f);  //0.1
       gl.glViewport(240, 0, width-480, height);

       gl.glMatrixMode(GL10.GL_MODELVIEW);
       gl.glLoadIdentity();
       
   }

   public void onSurfaceCreated( GL10 gl, EGLConfig config ) {
       // No need to do anything here.
   		gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f); 
       
   		gl.glClearDepthf(1.0f);
   		gl.glEnable(GL10.GL_DEPTH_TEST);
   		gl.glDepthFunc(GL10.GL_LEQUAL);

   		gl.glHint(GL10.GL_PERSPECTIVE_CORRECTION_HINT,
                 GL10.GL_NICEST);
   		
   		for (int i=0; i<9; i++)
   			cameraMatrix[i] = 0;
   		
   		cameraMatrix[0] = (float) 251.101; // Focal length in x axis
	    cameraMatrix[4] = (float) 251.342; // Focal length in y axis (usually the same?)
	    cameraMatrix[2] = (float) 161.259; // Camera primary point x
	    cameraMatrix[5] = (float) 117.985; // Camera primary point y
   }
   
   void getCameraMatrix(int width, int height)
   {
	   	float near = (float) 0.01;  // Near clipping distance
	    float far  = 100;  // Far clipping distance
	    
	    // Camera parameters
	    float f_x = cameraMatrix[0]; // Focal length in x axis
	    float f_y = cameraMatrix[4]; // Focal length in y axis (usually the same?)
	    float c_x = cameraMatrix[2]; // Camera primary point x
	    float c_y = cameraMatrix[5]; // Camera primary point y
	    
	    projectionMatrix[0] = (float) (- 2.0 * f_x / width);
	    projectionMatrix[1] = (float) 0.0;
	    projectionMatrix[2] = (float) 0.0;
	    projectionMatrix[3] = (float) 0.0;
	    
	    projectionMatrix[4] = (float) 0.0;
	    projectionMatrix[5] = (float) (2.0 * f_y / height);
	    projectionMatrix[6] = (float) 0.0;
	    projectionMatrix[7] = (float) 0.0;
	    
	    projectionMatrix[8] = (float) (2.0 * c_x / width - 1.0);
	    projectionMatrix[9] = (float) (2.0 * c_y / height - 1.0);
	    projectionMatrix[10] = -( far+near ) / ( far - near );
	    projectionMatrix[11] = (float) -1.0;
	    
	    projectionMatrix[12] = (float) 0.0;
	    projectionMatrix[13] = (float) 0.0;
	    projectionMatrix[14] = (float) (-2.0 * far * near / ( far - near ));
	    projectionMatrix[15] = (float) 0.0;
	   
   }
}