using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
	public bool[] dir = new bool[4];
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private Vector3 direction;
	private Vector3 reflectDirection;

	void Awake () {
		for (int i = 0; i < 4; i++) {
			if (dir [i]) {
				direction = wayP [i];
				reflectDirection = wayP [i + 1 % 4];
				transform.Rotate (0, 225 + i * 90, 0);
			}
		}
	}

	public Vector3 Reflect (Vector3 dIn) {
		if (-dIn == direction)
			return reflectDirection;
		if (-dIn == reflectDirection)
			return direction;
		return Vector3.zero;
	}
}
