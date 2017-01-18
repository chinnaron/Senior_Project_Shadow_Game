using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}


	void TaskOnClick(){
		//Debug.Log ("You have clicked the button!");


		Camera.main.transform.Rotate (new Vector3 (90, 0, 0));
	}
}
