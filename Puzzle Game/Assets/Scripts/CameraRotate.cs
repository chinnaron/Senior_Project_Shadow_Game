using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {
	public GameObject bg;
	public GameObject camera;
	private readonly float turnSpeed = 10f;
	private Quaternion lookAt;

	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

		lookAt = Quaternion.LookRotation (Vector3.forward);
	}
	
	void TaskOnClick(){
		//Debug.Log ("You have clicked the button!");
		lookAt = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * 90);
	}

	void FixedUpdate(){
		print(""+lookAt.eulerAngles+transform.rotation.eulerAngles+Quaternion.Slerp (camera.transform.rotation, lookAt, Time.deltaTime * turnSpeed));
		camera.transform.rotation = Quaternion.Slerp (camera.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
		bg.transform.rotation = Quaternion.Slerp (bg.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
	}
}
