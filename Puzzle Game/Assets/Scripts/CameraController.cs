using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject camera;
	public Transform floor;
	public Vector3 v;

	private Vector3 playerTransform;

	void Awake () {
		transform.position = v;
		camera.transform.LookAt (transform.position);
	}

	void FixedUpdate () {
		
	}
}
