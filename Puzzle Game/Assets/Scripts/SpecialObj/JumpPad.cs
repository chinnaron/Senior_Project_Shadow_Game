﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
	private PlayerController player;
	private GridOverlay grid;
	public AudioClip[] sound;
	public AudioSource sounds;
	private bool onFloor;
	public bool[] way = { false, false, false, false };
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private Vector3 direction;

	private RaycastHit hit;
	private PushController obj;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();

		direction = Vector3.zero;
		onFloor = transform.position.y < 0.5 ? true : false;

		for (int i = 0; i < 4; i++) {
			if (way [i]) {
				direction = wayP [i];
				transform.Rotate (new Vector3 (0f, 90 * i, 0f));
			}
		}
	}

	void Update () {
		if (Physics.Raycast (transform.position + Vector3.up * 1.5f, Vector3.down, out hit, 1f)) {
			if (hit.collider.GetComponent<ObjectController> ().isPushable
			    && hit.collider.transform.position.x < transform.position.x + 0.1f
			    && hit.collider.transform.position.x > transform.position.x - 0.1f
			    && hit.collider.transform.position.z < transform.position.z + 0.1f
			    && hit.collider.transform.position.z > transform.position.z - 0.1f) {
				obj = hit.collider.GetComponent<PushController> ();

				if (!obj.moving && !obj.jumping && !obj.falling) {
					if ((onFloor && grid.GetGrid (transform.position + direction) != grid.block && grid.GetGrid (transform.position + direction) != grid.block2
					    && grid.GetGrid (transform.position + direction) != grid.walkable2 && grid.GetGrid (transform.position + direction) != grid.tempWalkable2
					    && grid.GetGrid (transform.position + direction * 2) != grid.block && grid.GetGrid (transform.position + direction * 2) != grid.block2
					    && grid.GetGrid (transform.position + direction * 2) != grid.walkable2 && grid.GetGrid (transform.position + direction * 2) != grid.tempWalkable2)
					    || (!onFloor && (grid.GetGrid (transform.position + direction) != grid.block2) && grid.GetGrid (transform.position + direction * 2) != grid.block2)) {
						PlaySound (0);
						obj.SetJumpTo ((transform.position + direction * 2), onFloor, direction, 2);

						CheckObj (obj);
					} else if ((onFloor && grid.GetGrid (transform.position + direction) != grid.block && grid.GetGrid (transform.position + direction) != grid.block2
					           && grid.GetGrid (transform.position + direction) != grid.walkable2 && grid.GetGrid (transform.position + direction) != grid.tempWalkable2
					           && (grid.GetGrid (transform.position + direction * 2) == grid.block || grid.GetGrid (transform.position + direction * 2) == grid.block2
					           || grid.GetGrid (transform.position + direction * 2) == grid.walkable2 || grid.GetGrid (transform.position + direction * 2) == grid.tempWalkable2))
					           || (!onFloor && grid.GetGrid (transform.position + direction) != grid.block2
					           && grid.GetGrid (transform.position + direction * 2) == grid.block2)) {
						PlaySound (0);
						obj.SetJumpTo (transform.position + direction, onFloor, direction, 3);

						CheckObj (obj);
					} else if (onFloor && (grid.GetGrid (transform.position + direction) == grid.walkable2 || grid.GetGrid (transform.position + direction) == grid.tempWalkable2)) {
						PlaySound (0);
						obj.SetJumpTo (transform.position + direction, !onFloor, direction, 1);

						CheckObj (obj);
					}
				}
			}
		}
	}

	void CheckObj(PushController obj){
		if (obj.gameObject == player.gameObject)
			player.Stop ();
		else if (obj.GetComponent<ObjectController> ().isEnemy)
			obj.GetComponent<EnemyController> ().Stop ();
		else if (obj == player.GetGrabPush ()) {
			player.GrabRelease ();
		}
	}

	public void PlaySound(int s){
		sounds = GetComponent<AudioSource> ();
		sounds.clip = sound [s];
		sounds.Play ();
	}
}
