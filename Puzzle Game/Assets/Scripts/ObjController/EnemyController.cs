using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	public MenuScript menu;
	public AudioClip[] sound;
	public AudioSource[] sounds;

	public Vector3[] inputPath;
	private List<Vector3> pathDestination = new List<Vector3> ();
	private Vector3 movement;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	public Animator anim;
	private GameObject grabObj;
	private PushController grabPush;
	private PushController playerPush;
	private GridOverlay grid;

	private bool dying;
	private bool walking;
	private bool click;
	private bool cannotWalk;
	private bool reverse;
	private bool falling;

	private int dieSpeed;
	private int grabType;
	private int now;

	private float sinkSpeed = 2.5f;
	private float flashSpeed = 5f; 
	private float nearest = 0f;
	private readonly float speed = 4f;
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
		destination = transform.position;
		grabPoint = Vector3.zero;
		dieSpeed = 10;
		reverse = false;
		now = 0;
//		print (inputPath.Length);

		if (inputPath.Length > 0) {
			int i = 0;
			int b = 0;

			Vector3 v = inputPath [0];
			Vector3 dirV = Vector3.zero;

			while(i < inputPath.Length - 1 && b < 100){
				dirV = (inputPath [i + 1] - inputPath [i]).normalized;

				while(v != inputPath[i + 1] && b < 100){
//					print (v);
					pathDestination.Add (v);
					v += dirV;
					b++;
				}

				i++;
				b++;
			}
			pathDestination.Add (v);

			walking = true;
			movement = pathDestination [1] - pathDestination [0];
		} else
			walking = false;


		anim = GetComponent<Animator> ();
		playerPush = GetComponent<PushController> ();
		grid = FindObjectOfType<GridOverlay> ();
	}

	void FixedUpdate () {
//		print (walking);

		if (!walking && !playerPush.moving && !playerPush.falling && !playerPush.jumping) {
//			print (grid.GetGrid (transform.position));
			if (grid.GetGrid (transform.position) == grid.unwalkable) {
				Fall ();
//				print ("A");
			} else if (pathDestination.Count > 1) {
				walking = true;
//				print ("B");
			}
		}

		if (playerPush.falling) {
			walking = false;
		}

		if (!grid.IsWalkable (pathDestination [now], playerPush.GetOnFloor ()))
			walking = false;

		if (walking) {

			if (transform.position == pathDestination[now] + Vector3.up) {
				if (!playerPush.falling && playerPush.CheckFall ()) {
					movement = Vector3.zero;
					walking = false;
					playerPush.SetFall ();
				}
			}

			if (transform.position == pathDestination[now]) {
				
				if (now == pathDestination.Count - 1) {
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

				lookAt = Quaternion.LookRotation (movement);
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (transform.position + movement - pathDestination[now]).normalized
				, grid.Set0Y (transform.position - pathDestination[now]).normalized) == -1f)
				movement = grid.Set0Y (pathDestination[now] - transform.position);

			transform.position = transform.position + movement;

		}

		transform.rotation = Quaternion.Lerp (transform.rotation, lookAt, Time.deltaTime * turnSpeed);

		if (falling) {
			transform.position = transform.position + Vector3.down * dieSpeed * Time.deltaTime;
			dieSpeed++;

			if (transform.position.y < -8)
				Destroy (gameObject);
		}
			
		anim.SetBool ("IsWalking", walking);
	}

	void Fall(){
		falling = true;
	}

	public void YouDied(){
		dying = true;
		anim.SetBool ("IsDead",dying);
		Stop ();
		Destroy (gameObject);
	}

	public void ContinueWalking () {
		walking = true;
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