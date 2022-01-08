using UnityEngine;
using System.Collections;

public class imuTest : MonoBehaviour {

	Gyroscope gyro;
	float w,x,y,z;
	Quaternion quatMult;
	Quaternion quatMap;

	// Use this for initialization
	void Start () {

		w = 0.0f;
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;

		quatMult = new Quaternion(0, 0, 1, 0.4f);

	}
	
	// Update is called once per frame
	void Update () {

		quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);  
		Quaternion qt = quatMap * quatMult;

		w = qt.w;
		x = -qt.y;
		y = 0.0f;//qt.x;
		z = 0.0f;//qt.z;

	}

	internal void OnGUI()
	{
		if (GUI.Button (new Rect (320, 10, 120, 40), "start"))
		{
			Input.gyro.enabled = false;
			Input.gyro.enabled = true;
			gyro = Input.gyro;
		}
		GUI.Button( new Rect( 1, 1, 200, 100 ), "w: " + w );
		GUI.Button( new Rect( 1, 102, 200, 100 ), "x: " + x );
		GUI.Button( new Rect( 1, 203, 200, 100 ), "y: " + y );
		GUI.Button( new Rect( 1, 304, 200, 100 ), "z: " + z );
	}

}
