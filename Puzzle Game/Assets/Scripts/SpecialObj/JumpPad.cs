using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
	private PlayerController player;
	private GridOverlay grid;

	public bool[] way = { false, false, false, false };
	private readonly float rayDistance = 3f;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };  
	private Vector3 direction;

	private RaycastHit hit;
	private PushController obj;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();

		direction = Vector3.zero;

		for (int i = 0; i < 4; i++) {
			if (way [i])
				direction = wayP [i];
		}
	}

	void Update () {
		if (Physics.Raycast (transform.position, Vector3.up, out hit, rayDistance)) {
			if (hit.collider.GetComponent<ObjectController> ().isPushable 
				&& hit.collider.transform.position.x < transform.position.x + 0.1f
				&& hit.collider.transform.position.x > transform.position.x - 0.1f
				&& hit.collider.transform.position.z < transform.position.z + 0.1f
				&& hit.collider.transform.position.z > transform.position.z - 0.1f) {
				obj = hit.collider.GetComponent<PushController> ();

				if (!obj.moving) {
					if (obj.gameObject == player.gameObject)
						player.Stop ();
					else if (obj == player.GetGrabPush ()) {
						player.GrabRelease ();
					}

					obj.SetJumpTo (transform.position, direction);
				}
			}
		}
	}
}
