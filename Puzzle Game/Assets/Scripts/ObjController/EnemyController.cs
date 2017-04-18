using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	public MenuScript menu;
	public AudioClip[] sound;
	public AudioSource[] sounds;

	private Vector3 movement;
	public Vector3[] pathDestination;
	private Vector3 destination;
	private Vector3 point;

	private Animator anim;
	private PushController playerPush;
	private GridOverlay grid;

	private bool dying;
	private bool walking;
	private bool click;
	private bool cannotWalk;
	private bool reverse;

	private int now;
	private int dieSpeed;

	private float flashSpeed = 5f; 
	private float nearest = 0f;
	private readonly float speed = 5f;
	private readonly float turnSpeed = 10f;
	private readonly float camRayLength = 100f;

	private Quaternion lookAt;

	void Awake () {
		InvokeRepeating("WalkingSound", 0f, 0.2f);
		destination = transform.position;
		movement = Vector3.zero;
		dieSpeed = 10;
		now = 0;
		reverse = dying = false;

		if (pathDestination.Length > 1) {
			walking = true;
			movement = pathDestination [1] - pathDestination [0];
		}
		
		anim = GetComponent<Animator> ();
		playerPush = GetComponent<PushController> ();
		grid = FindObjectOfType<GridOverlay> ();
	}

	void FixedUpdate () {
		if (walking) {
//			if (nearest >= Vector3.Distance (transform.position, pathDestination[now])) {
//				nearest = Vector3.Distance (transform.position, pathDestination[now]);
//			} else if (nearest < Vector3.Distance (transform.position, pathDestination[now])) {
//				nearest = 0;
//				transform.position = pathDestination[now];
//			}

			if (transform.position == pathDestination[now] + Vector3.up) {
				if (!playerPush.falling && playerPush.CheckFall ()) {
					movement = Vector3.zero;
					walking = false;
					playerPush.SetFall ();
				}
			}

			if (transform.position == pathDestination[now]) {
//				print (now +""+ (pathDestination.Length - 1));
				if (now == pathDestination.Length - 1) {
					reverse = true;
					now--;
					movement = pathDestination [now] - pathDestination [now + 1];
				} else if (now == 0) {
					reverse = false;
					now++;
					movement = pathDestination [now] - pathDestination [now - 1];
				} else {
					if (reverse) {
						now--;
						movement = pathDestination [now] - pathDestination [now + 1];
					} else {
						now++;
						movement = pathDestination [now] - pathDestination [now - 1];
					}
				}
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (transform.position + movement - pathDestination[now]).normalized
				, grid.Set0Y (transform.position - pathDestination[now]).normalized) == -1f)
				movement = grid.Set0Y (pathDestination[now] - transform.position);

			transform.position = transform.position + movement;
				
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, lookAt, Time.deltaTime * turnSpeed);

		if (dying) {
			transform.position = transform.position + Vector3.down * dieSpeed * Time.deltaTime;
			dieSpeed++;

			if (transform.position.y < -8)
				Application.LoadLevel (Application.loadedLevel);
		}

		anim.SetBool ("IsWalking", walking);
	}

	void YouDied(){
		dying = true;
	}

	public Vector3 GetMovement(){
		return movement;
	}

	public void SetPushController (Vector3 des, Vector3 dir) {
		playerPush.SetMoveTo (des, dir);
	}

	public void PlaySound (int s) {
		
	}

	public void WalkingSound () {
		if (walking) {
			sounds = GetComponents<AudioSource>();
			sounds[0].clip = sound [0];
			sounds[0].Play ();
		}
	}

}