using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeRight;
	}
	
	// Update is called once per frame
	void Update () {
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
}
