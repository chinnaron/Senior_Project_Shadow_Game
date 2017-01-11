using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLightShadow : MonoBehaviour {
	public float scale;

	private GridOverlay grid;
	private float halfWide;
	private Vector3 direction;
	private Vector3 start;

	void Awake () {
		scale = 9f;

		grid = FindObjectOfType(typeof(GridOverlay)) as GridOverlay;

		halfWide = transform.localScale.y / 2f;

		if (Quaternion.LookRotation (Vector3.forward) == transform.rotation) {
			direction = Vector3.forward;
			start = transform.position + Vector3.forward;
			transform.position = transform.position + new Vector3 (0f, 0f, halfWide + 0.5f);
		} else if (Quaternion.LookRotation (Vector3.right) == transform.rotation) {
			direction = Vector3.right;
			start = transform.position + Vector3.right;
			transform.position = transform.position + new Vector3 (halfWide + 0.5f, 0f, 0f);
		} else if (Quaternion.LookRotation (Vector3.back) == transform.rotation) {
			direction = Vector3.back;
			start = transform.position + Vector3.back;
			transform.position = transform.position - new Vector3 (0f, 0f, halfWide + 0.5f);
		} else {
			direction = Vector3.left;
			start = transform.position + Vector3.left;
			transform.position = transform.position - new Vector3 (halfWide + 0.5f, 0f, 0f);
		}
			
		for (float i = 0; i < transform.localScale.y; i++) {
			grid.SetGrid (start, grid.walkable);
			start = start + direction;
		}

	}
}
