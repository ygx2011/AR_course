using UnityEngine;
using System.Collections;
using System.IO;

public class MouseControl : MonoBehaviour {

	float modelScale;

	// Use this for initialization
	void Start () {

		modelScale = 1.0f;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			modelScale -= 0.1f;

			if (modelScale <= 0.0f) {
				modelScale = 0.1f;
			}

			transform.localScale = new Vector3(modelScale, modelScale, modelScale);
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			modelScale += 0.1f;

			transform.localScale = new Vector3(modelScale, modelScale, modelScale);
		}
	
	}
}
