using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
	private float speed = 100f;

	void Update () {
		transform.Rotate (new Vector3 (0f, speed, 0f) * Time.deltaTime);
	}
}
