using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {
	private PlayerController player;
	public GameObject camera;
	private readonly float turnSpeed = 10f;
	private Quaternion lookAt;
	private Quaternion[] axises = new Quaternion[4];
	private int current;
//	private int last;
	public Button bLeft;
	public Button bRight;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		gameObject.SetActive (true);
		#elif UNITY_ANDROID
		gameObject.SetActive(false);
		#endif

		bLeft.onClick.AddListener (TaskOnClickL);
		bRight.onClick.AddListener (TaskOnClickR);

		player = FindObjectOfType<PlayerController> ();
		current = 0;

		for (int i = 0; i < 3; i++)
			axises [i + 1] = Quaternion.Euler (axises [i].eulerAngles + Vector3.up * 90);
	}
	
	void TaskOnClickL(){
		//Debug.Log ("You have clicked the button!");
//		last = current;
		current = (current + 1) % 4;
		lookAt = axises [current];
	}

	void TaskOnClickR(){
		//Debug.Log ("You have clicked the button!");
//		last = current;
		current = (current + 3) % 4;
		lookAt = axises [current];
	}
//
//	Vector3 SetAxis(Quaternion q){
//		if (Mathf.Abs (q.eulerAngles.y - Mathf.Floor (q.eulerAngles.y)) > Mathf.Abs (q.eulerAngles.y - Mathf.Ceil (q.eulerAngles.y)))
//			return new Vector3 (q.eulerAngles.x, Mathf.Ceil (q.eulerAngles.y), q.eulerAngles.z);
//		else
//			return new Vector3 (q.eulerAngles.x, Mathf.Floor (q.eulerAngles.y), q.eulerAngles.z);
//	}

	void FixedUpdate(){
		//print ("" + camera.transform.rotation.eulerAngles + lookAt.eulerAngles + current);
		//print ("" + camera.transform.rotation + lookAt + Time.deltaTime * turnSpeed);
		camera.transform.rotation = Quaternion.Lerp (camera.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
	}
}
