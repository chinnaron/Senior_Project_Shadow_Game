using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;

	private Vector3 playerTransform;

	void Awake () {
		playerTransform = transform.position - player.transform.position;
	}

	void FixedUpdate () {
		transform.position = player.transform.position + playerTransform;
	}
}
