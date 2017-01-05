using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject floor;
	private GridOverlay grid;

	private float speed = 1.5f;
	private float turnSpeed = 5f;

	private int north = 0;
	private int east = 1;
	private int south = 2;
	private int west = 3;

	private Animator anim;
	private Rigidbody playerRigidbody;

	private bool walking;
	private bool grabbing;

	private int floorMask;
	private float camRayLength = 100f;

	private Vector3 pathDestination;
	private Vector3 movement;
	private Vector3 destination;
	private Vector3 point;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit hit;

	private Stack<Vector3> path = new Stack<Vector3> ();

	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		destination = transform.position;
		movement = Vector3.zero;

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		grid = FindObjectOfType(typeof(GridOverlay)) as GridOverlay;
	}

	void Update (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);//Change Input to mobile ver.

		if (Input.GetButtonDown ("Fire1") && Physics.Raycast (ray, out hit, camRayLength, floorMask) && grid.toPoint (hit.point) != transform.position && grid.GetGrid (hit.point) == 0) {//Change Input to mobile ver.
			point = grid.toPoint (hit.point);
			point.y = 0f;
			path = grid.findPath (grid.toPoint (transform.position), point);
			if (path.Count > 0) {
				destination = point;
				path.Pop ();
				pathDestination = path.Peek ();
				movement = pathDestination - transform.position;
				lookAt = Quaternion.LookRotation (pathDestination - transform.position);
				walking = true;
			}
		}

		if (Input.GetButtonDown ("Fire2")) {
			grabbing = !grabbing;
		}

		if (Vector3.Dot((destination - transform.position).normalized, movement.normalized) == -1f) {
			transform.position = destination;
			movement = Vector3.zero;
			path.Clear ();
			walking = false;
		}

		if (Vector3.Dot ((pathDestination - transform.position).normalized, movement.normalized) == -1f) {
			if (path.Count > 0) {
				transform.position = pathDestination;
				path.Pop ();
				pathDestination = path.Peek ();
				movement = pathDestination - transform.position;
				lookAt = Quaternion.LookRotation (pathDestination - transform.position);
				walking = true;
			}
		}
	}

	void FixedUpdate () {
		Move ();
		Animating ();
	}

	void Move (){
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
		playerRigidbody.MoveRotation (Quaternion.Lerp(transform.rotation, lookAt, Time.fixedDeltaTime * turnSpeed));
	}

	void Animating (){
		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}
}