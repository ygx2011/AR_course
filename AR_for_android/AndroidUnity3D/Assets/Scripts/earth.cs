using UnityEngine;
using System.Collections;

public class earth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 25*Time.deltaTime, 0, Space.Self);
	}
}
