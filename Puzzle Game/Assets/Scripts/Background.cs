using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	private float speed = 1f;

	void Update () {
		transform.Rotate (new Vector3 (0f, 0f, speed) * Time.deltaTime);
	}
}
