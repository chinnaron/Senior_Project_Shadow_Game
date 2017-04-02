using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
	public MenuScript menu;
	private GameObject grabPlane;
//	private GameObject desNotPlane;
	private GameObject desPlane;
//	private GameObject desNotPic;
	private GameObject desPic;
	private GameObject grabPic;
	public Image desNotPic;
	public AudioClip[] sound;
	public AudioSource[] sounds;

	private Vector3 movement;
	private Vector3 pathDestination;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Animator anim;
	private GameObject grabObj;
	private PushController grabPush;
	private PushController playerPush;
	private GridOverlay grid;

	private bool goToLever;
	private bool goToGrab;
	private bool dying;
	private bool walking;
	private bool grabbing;
	private bool pulling;
	private bool pushing;
	private bool click;
	private bool cannotWalk;

	private int dieSpeed;
	private int playerMask;
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
		destination = pathDestination = transform.position;
		movement = grabPoint = Vector3.zero;
		walking = grabbing = dying = goToGrab = goToLever = click = false;
		dieSpeed = 10;

		anim = GetComponent<Animator> ();
		playerPush = GetComponent<PushController> ();
		playerMask = LayerMask.GetMask ("Player");
		grid = FindObjectOfType<GridOverlay> ();
		desPic = Resources.Load ("DesPic", typeof(GameObject)) as GameObject;
//		desNotPic = Resources.Load ("DesNotPic", typeof(GameObject)) as GameObject;
		grabPic = Resources.Load ("GrabPic", typeof(GameObject)) as GameObject;
	}

	void FixedUpdate () {

		if (walking) {
			if (nearest >= Vector3.Distance (transform.position, destination)) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination)) {
				nearest = 0;
				transform.position = pathDestination;
			}

			if (transform.position == pathDestination + Vector3.up) {
				if (grabbing && !grabPush.moving) {
					if (!playerPush.falling && !grabPush.falling && (playerPush.CheckFall () || grabPush.CheckFall ())) {
						playerPush.SetFall ();
						grabPush.SetFall ();
						grid.SetGridHere (transform.position + grabPoint);
						GrabRelease ();
					} else
						grid.SetGridHere (transform.position + grabPoint);
				} else {
					if (!playerPush.falling && playerPush.CheckFall ()) {
						movement = Vector3.zero;
						walking = false;
						playerPush.SetFall ();
					}
				}
			}

			if (transform.position == pathDestination) {
				if (pathDestination == destination) {
					if (goToGrab) {
						goToGrab = false;
						grabbing = true;
						grabPoint = grid.ToPoint0Y (grabPush.transform.position) - grid.ToPoint0Y (destination);
						lookAt = Quaternion.LookRotation (grabPoint);

						if (grabPush.GetComponent<ObjectController> ().isBlock)
							grabType = grid.block;
						else
							grabType = grid.block2;

						grabPlane = Instantiate (grabPic, grabPoint + transform.position, Quaternion.LookRotation (Vector3.forward), grabPush.transform);
					}

					if (goToLever) {
						goToLever = false;
						grabPointLever = grid.ToPoint0Y (grabLever.transform.position) - grid.ToPoint0Y (transform.position);
						lookAt = Quaternion.LookRotation (grabPointLever);
						leverController.changeState ();
					}

					Destroy (desPlane);
					movement = Vector3.zero;
					walking = false;
				}
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (transform.position + movement - pathDestination).normalized
				, grid.Set0Y (transform.position - pathDestination).normalized) == -1f)
				movement = grid.Set0Y (pathDestination - transform.position);

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
		//		anim.SetBool ("IsGrabbing", grabbing);
		anim.SetBool ("IsPushing" , pushing);
		anim.SetBool ("IsPulling", pulling);
	}

	void YouDied(){
		dying = true;
	}

	void StartToWalk (Vector3 des, Vector3 grabPoi) {
		Destroy (desPlane);
//		Destroy (desNotPlane);
		destination = des - grabPoi;

		if (grid.GetGrid (des - grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable || (grabbing && grid.GetGrid (des - grabPoi) == grid.block))
			destination.y = 0f;
		else
			destination.y = 1f;

//		pathDestination = path.Peek ();

		if (grid.GetGrid (destination + grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable)
			desPlane = Instantiate (desPic, grid.Set0Y (destination + grabPoi), Quaternion.LookRotation (Vector3.forward));
		else
			desPlane = Instantiate (desPic, grid.Set1Y (destination + grabPoi), Quaternion.LookRotation (Vector3.forward));
		
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

	public void GrabRelease () {
		grabbing = false;
		pulling = false;
		pushing = false;
		Destroy (grabPlane);
		grabPoint = Vector3.zero;
		Destroy (desPlane);
		movement = Vector3.zero;
		walking = false;
		grabPush = null;
		grabObj = null;
		goToGrab = false;
		nearest = 0;
	}

	public bool IsGrabbing(){
		return grabbing;
	}

	public Vector3 GetGrabPoint(){
		return grabPoint;
	}

	public void Stop () {
		walking = false;
		Destroy (desPlane);
		goToGrab = false;
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

	public void PushingSound () {
		if (pushing||pulling) {
			sounds = GetComponents<AudioSource>();
			sounds[1].clip = sound [1];
			sounds[1].Play ();
		}
	}
}