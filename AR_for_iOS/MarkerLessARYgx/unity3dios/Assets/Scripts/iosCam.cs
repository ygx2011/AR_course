using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

public class iosCam : MonoBehaviour
{
	float[] transform1 = new float[3];

	float[] rotation1 = new float[4];

	[DllImport("__Internal")]
	private static extern void Initialize ();

	[DllImport("__Internal")]
	private static extern int process_Image (byte[] ImageData, float[] T, float[] wxyz, ref bool isShow);

	Color32[] data;
	public WebCamTexture webcamTexture;
	Texture2D tex;

	byte[] imgData;

	bool isshow;
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
		fightman.SetActive (false);

		Initialize ();

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
		Color32ArrayToByteArray (data);
		process_Image (imgData, /*rotation1,*/ transform1, rotation1, ref isshow);

		if (isshow == true) {
			fightman.SetActive (true);
		} else {
			fightman.SetActive (false);
		}

		tex.SetPixels32 (data);
		tex.Apply();

		Matrix4x4 x180 = Matrix4x4.identity;
		Vector4 x1 = new Vector4(1, 0, 0, 0);
		x180.SetColumn(0, x1);
		Vector4 x2 = new Vector4(0, -1, 0, 0);
		x180.SetColumn(1, x2);
		Vector4 x3 = new Vector4(0, 0, -1, 0);
		x180.SetColumn(2, x3);
		Vector4 x4 = new Vector4(0, 0, 0, 1);
		x180.SetColumn(3, x4);

		float qx,qy,qz,qw;
		qw = rotation1 [0];
		qx = rotation1 [1];
		qz = rotation1 [3];
		qy = rotation1 [2];

		Matrix4x4 ygx=Matrix4x4.identity;
		Vector4 y1 = new Vector4(1 - 2*qy*qy - 2*qz*qz, 2*qx*qy + 2*qz*qw, 2*qx*qz - 2*qy*qw, 0);
		ygx.SetColumn(0, y1);
		Vector4 y2 = new Vector4(2*qx*qy - 2*qz*qw, 1 - 2*qx*qx - 2*qz*qz, 2*qy*qz + 2*qx*qw, 0);
		ygx.SetColumn(1, y2);
		Vector4 y3 = new Vector4(2*qx*qz + 2*qy*qw, 2*qy*qz - 2*qx*qw, 1 - 2*qx*qx - 2*qy*qy, 0);
		ygx.SetColumn(2, y3);
		Vector4 y4 = new Vector4(0, 0, 0, 1);
		ygx.SetColumn(3, y4);

		ygx = x180 * ygx;

		Vector4 vz = ygx.GetColumn(1);
		Vector4 vy = ygx.GetColumn(2);
		Quaternion newQ = Quaternion.LookRotation(new Vector3(vz.x, vz.y, vz.z), new Vector3(vy.x, vy.y, vy.z));

		transform.rotation = newQ;

		Vector4 t = new Vector4(transform1[0], -transform1[2], transform1[1], 1);
		transform.position = ygx * t;

	}
}
