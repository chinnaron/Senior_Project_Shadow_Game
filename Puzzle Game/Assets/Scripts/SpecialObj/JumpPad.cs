using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
	public PlayerController player;
	public GridOverlay grid;



	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();
	}

//	void Update () {
//		if (Physics.Raycast (transform.position, Vector3.up, out hit, rayDistanceDefault)) {
//			list = Physics.RaycastAll (transform.position, wayP [i], rayDistanceDefault);
//
//			if (list.Length < 2)
//				rayDistance [i] = rayDistanceDefault;
//			else {
//				if (i % 2 == 0)
//					rayDistance [i] = Mathf.Abs (grid.ToPoint0Y (list [1].collider.transform.position).z - grid.ToPoint0Y (transform.position).z) - 1f;
//				else
//					rayDistance [i] = Mathf.Abs (grid.ToPoint0Y (list [1].collider.transform.position).x - grid.ToPoint0Y (transform.position).x) - 1f;
//			}
//
//			if (i % 2 == 0)
//				longL [i] = Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z) - 1f;
//			else
//				longL [i] = Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x) - 1f;
//
//			if (longL [i] > rayDistanceDefault - 1)
//				longL [i] = rayDistanceDefault - 1;
//			else if (longL [i] < 0)
//				longL [i] = 0;
//
//			line [i].SetPosition (line [i].numPositions - 1, wayP [i] * longL [i]);
//
//			if (hit [i].collider.GetComponent<ObjectController> ().isPushable
//				&& ((i % 2 == 0 && hit [i].collider.transform.position.x < transform.position.x + 0.1f
//					&& hit [i].collider.transform.position.x > transform.position.x - 0.1f
//					&& Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).z - grid.ToPoint0Y (transform.position).z) < rayDistance [i])
//					|| (i % 2 == 1 && hit [i].collider.transform.position.z < transform.position.z + 0.1f
//						&& hit [i].collider.transform.position.z > transform.position.z - 0.1f
//						&& Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).x - grid.ToPoint0Y (transform.position).x) < rayDistance [i]))) {
//				obj [i] = hit [i].collider.GetComponent<PushController> ();
//
//				if (!obj [i].moving) {
//					if (obj [i].gameObject == player.gameObject)
//						player.Stop ();
//					else if (obj [i] == player.GetGrabPush ()) {
//						if ((i % 2 == 0 && grid.ToPoint0Y (player.transform.position).x == grid.ToPoint0Y (transform.position).x)
//							|| (i % 2 == 1 && grid.ToPoint0Y (player.transform.position).z == grid.ToPoint0Y (transform.position).z)) {
//							player.Stop ();
//							player.SetPushController (transform.position + wayP [i] * (rayDistance [i] + 1), wayP [i]);
//						} else {
//							player.GrabRelease ();
//						}
//					}
//
//					obj [i].SetMoveTo (transform.position + wayP [i] * (rayDistance [i]), wayP [i]);
//				}
//			}
//		} else {
//			rayDistance [i] = rayDistanceDefault;
//			line [i].SetPosition (line [i].numPositions - 1, wayP [i] * (rayDistance [i] - 1));
//		}
//	} else {
//		if (onPic [i] != null)
//			Destroy (onPic [i]);
//		rayDistance [i] = rayDistanceDefault;
//		line [i].SetPosition (line [i].numPositions - 1, Vector3.zero);
//	}
}
