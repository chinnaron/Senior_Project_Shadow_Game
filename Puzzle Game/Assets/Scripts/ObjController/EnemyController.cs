using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	public MenuScript menu;
	public AudioClip[] sound;
	public AudioSource[] sounds;

	private Vector3 movement;
	private Vector3[] pathDestination;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Animator anim;
	private GameObject grabObj;
	private PushController grabPush;
	private PushController playerPush;
	private GridOverlay grid;

	private bool dying;
	private bool walking;
	private bool click;
	private bool cannotWalk;

	private int dieSpeed;
	private int grabType;

	private float flashSpeed = 5f; 
	private float nearest = 0f;
	private readonly float speed = 5f;
	private readonly float turnSpeed = 10f;
	private readonly float camRayLength = 100f;

	private Quaternion lookAt;

	private GameObject grabLever;
	private Vector3 grabPointLever;
	private LeverController leverController;

	public void Click(){
		click = true;
	}

	void Awake () {
		InvokeRepeating("WalkingSound", 0f, 0.2f);
		InvokeRepeating("PushingSound", 0f, 0.2f);
		destination = transform.position;
		movement = grabPoint = Vector3.zero;
		dieSpeed = 10;

		anim = GetComponent<Animator> ();
		playerPush = GetComponent<PushController> ();
		grid = FindObjectOfType<GridOverlay> ();
	}

	void FixedUpdate () {
		if (walking) {
			if (nearest >= Vector3.Distance (transform.position, destination)) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination)) {
				nearest = 0;
//				transform.position = pathDestination;
			}

//			if (transform.position == pathDestination + Vector3.up) {
//				if (!playerPush.falling && playerPush.CheckFall ()) {
//					movement = Vector3.zero;
//					walking = false;
//					playerPush.SetFall ();
//				}
//			}

//			if (transform.position == pathDestination) {
//				if (pathDestination == destination) {
//					movement = Vector3.zero;
//					walking = false;
//				}
//			}

			movement = movement.normalized * speed * Time.deltaTime;

//			if (Vector3.Dot (grid.Set0Y (transform.position + movement - pathDestination).normalized
//				, grid.Set0Y (transform.position - pathDestination).normalized) == -1f)
//				movement = grid.Set0Y (pathDestination - transform.position);

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

	public PushController GetGrabPush () {
		return grabPush;
	}

	public void SetPushController (Vector3 des, Vector3 dir) {
		playerPush.SetMoveTo (des, dir);
	}


	public void Stop () {
		walking = false;
		nearest = 0;
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