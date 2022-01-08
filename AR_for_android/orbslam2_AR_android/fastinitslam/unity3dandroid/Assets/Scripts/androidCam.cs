using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class androidCam : MonoBehaviour
{
	Gyroscope gyro;
	float w,x,y,z;
	Quaternion quatMult;
	Quaternion quatMap;

	float[] transform1 = new float[3];
	float[] rotation1 = new float[9];

	float[] P = new float[3];
	float[] E = new float[3];

	[DllImport("SLAMAR")]
	private static extern void choose_Plane (float[] Position,float[] EulerAngles);

	[DllImport ("SLAMAR")]
	private static extern int process_Image (byte[] ImageData, float[] T, float[] wxyz, ref bool isShow);

	[DllImport("SLAMAR")]
	private static extern void reset ();

	Color32[] data;
	public WebCamTexture webcamTexture;
	Texture2D tex;

	byte[] imgData;

	bool isIMU;

	bool isProcess;
	bool isshow;
	bool isFirst;
	public GameObject fightman;

	public Shader curShader;
	private Material curMaterial;

	void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (curShader != null)
		{
			material.SetTexture("tex", tex);
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}
	public Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}

	// Use this for initialization
	void Start ()
	{
		w = 0.0f;
		x = 0.0f;
		y = 0.0f;
		z = 0.0f;
		quatMult = new Quaternion(0, 0, 1, 0.4f);

		fightman.SetActive (false);

		isProcess = false;
		isFirst = true;

		isIMU = true;

		imgData = new byte[640 * 480 * 4];

		Camera.main.depthTextureMode = DepthTextureMode.Depth;

		data = new Color32[640 * 480];
		tex = new Texture2D (640, 480, TextureFormat.RGBA32, false);

		webcamTexture = new WebCamTexture(640, 480, 30);

		webcamTexture.Play();
	}

	void Color32ArrayToByteArray(Color32[] colors)
	{
		GCHandle handle = default(GCHandle);
		handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
		IntPtr ptr = handle.AddrOfPinnedObject();
		Marshal.Copy(ptr, imgData, 0, 640*480*4);

		if (handle != default(GCHandle))
			handle.Free();
	}
	
	// Update is called once per frame
	void Update ()
	{
		webcamTexture.GetPixels32 (data);
		if (isProcess == true) {

			if (isIMU == true) {
				Input.gyro.enabled = false;
				Input.gyro.enabled = true;
				gyro = Input.gyro;
				isIMU = false;
			}

			Color32ArrayToByteArray (data);
			process_Image (imgData, /*rotation1,*/transform1, rotation1, ref isshow);

			if (isshow == true) {
				fightman.SetActive (true);
				if (isFirst == true) {

					quatMap = new Quaternion(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z, gyro.attitude.w);  
					Quaternion qt = quatMap * quatMult;
					w = qt.w;
					x = -qt.y;
					y = 0.0f;// qt.x;
					z = 0.0f;// qt.z;

					choose_Plane (P, E);
					fightman.transform.position = new Vector3 (P [0], P [1], P [2]);

					// imu not ok...  as 课后作业
					//Quaternion ygx_newQ = new Quaternion(x, z, y, -w);
					//fightman.transform.rotation = ygx_newQ;

					Vector3 velocity = new Vector3 (E [0], E [1], E [2]);
					fightman.transform.eulerAngles = Quaternion.FromToRotation (Vector3.down, velocity).eulerAngles;

					isFirst = false;
				}
			} else {
				fightman.SetActive (false);
			}

			Matrix4x4 ygx = Matrix4x4.identity;
			Vector4 y1 = new Vector4 (rotation1 [0], rotation1 [1], rotation1 [2], 0);
			ygx.SetColumn (0, y1);
			Vector4 y2 = new Vector4 (rotation1 [3], rotation1 [4], rotation1 [5], 0);
			ygx.SetColumn (1, y2);
			Vector4 y3 = new Vector4 (rotation1 [6], rotation1 [7], rotation1 [8], 0);
			ygx.SetColumn (2, y3);
			Vector4 y4 = new Vector4 (0, 0, 0, 1);
			ygx.SetColumn (3, y4);

			Vector4 vz = ygx.GetColumn (1);
			Vector4 vy = ygx.GetColumn (2);
			Quaternion newQ = Quaternion.LookRotation (new Vector3 (vz.x, vz.y, vz.z), new Vector3 (vy.x, vy.y, vy.z));
			transform.rotation = newQ;

			Vector4 t = new Vector4 (transform1 [0], transform1 [1], transform1 [2], 1);
			transform.position = ygx * t;
		}
		tex.SetPixels32 (data);
		tex.Apply ();
	}

	internal void OnGUI()
	{
		if (GUI.Button (new Rect (500, 1800, 120, 40), "start"))
		//if (GUI.Button (new Rect (10, 20, 120, 40), "start"))
		{
			fightman.SetActive (false);
			reset ();
			isProcess = true;
			isFirst = true;
			isIMU = true;
		}
	}

}
