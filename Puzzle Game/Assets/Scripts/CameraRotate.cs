using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {
	public bool right;
	public GameObject bg;
	public GameObject camera;

	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}


	void TaskOnClick(){
		//Debug.Log ("You have clicked the button!");

		if (right) {
			camera.transform.Rotate (new Vector3 (0, -90, 0));
			bg.transform.Rotate (new Vector3 (0, -90, 0));
		} else {
			camera.transform.Rotate (new Vector3 (0, 90, 0));
			bg.transform.Rotate (new Vector3 (0, 90, 0));
		}
	}
}
