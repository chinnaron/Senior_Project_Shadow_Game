using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {
	private PlayerController player;
	public GameObject camera;
	private readonly float turnSpeed = 10f;
	private Quaternion lookAt;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		gameObject.SetActive(true);
		#elif UNITY_ANDROID
		gameObject.SetActive(false);
		#endif

		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

		player = FindObjectOfType<PlayerController> ();
		lookAt = camera.transform.rotation;
	}
	
	void TaskOnClick(){
		//Debug.Log ("You have clicked the button!");

		lookAt = Quaternion.Euler (SetAxis (camera.transform.rotation) + Vector3.up * 90);
	}

	Vector3 SetAxis(Quaternion q){
		if (Mathf.Abs (q.eulerAngles.y - Mathf.Floor (q.eulerAngles.y)) > Mathf.Abs (q.eulerAngles.y - Mathf.Ceil (q.eulerAngles.y)))
			return new Vector3 (q.eulerAngles.x, Mathf.Ceil (q.eulerAngles.y), q.eulerAngles.z);
		else
			return new Vector3 (q.eulerAngles.x, Mathf.Floor (q.eulerAngles.y), q.eulerAngles.z);
	}

	void FixedUpdate(){
		camera.transform.rotation = Quaternion.Lerp (camera.transform.rotation, lookAt, Time.deltaTime * turnSpeed);
	}
}
