using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
	public PlayerController player;
	public GridOverlay grid;

	private readonly float rayDistance = 3f;

	private RaycastHit hit;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();
	}

	void Update () {
		if (Physics.Raycast (transform.position, Vector3.up, out hit, rayDistance)) {
			if (hit.collider.GetComponent<ObjectController> ().isPushable) {
				
			}
		} else {
			
		}
	}
}
