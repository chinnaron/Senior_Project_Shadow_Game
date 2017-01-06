using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private GridOverlay grid;
	private GameObject grabObj;

	private float speed = 1.5f;
	private float turnSpeed = 5f;

	private Animator anim;
	private Rigidbody playerRigidbody;
	private Rigidbody grabRigidbody;

	private bool walking;
	private bool grabbing;

	private int floorMask;
	private int moveableMask;
	private float camRayLength = 100f;

	private Vector3 pathDestination;
	private Vector3 movement;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit floorHit;
	private RaycastHit grabHit;

	private Stack<Vector3> path = new Stack<Vector3> ();

	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		moveableMask = LayerMask.GetMask ("Moveable");
		destination = transform.position;
		movement = grabPoint = Vector3.zero;

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		grid = FindObjectOfType(typeof(GridOverlay)) as GridOverlay;
	}

	void Update (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);//Change Input to mobile ver.
		if (Input.GetButtonDown ("Fire1") && Physics.Raycast (ray, out floorHit, camRayLength, floorMask) && grid.toPoint (floorHit.point) != transform.position) {//Change Input to mobile ver.
			point = grid.toPoint (floorHit.point);
			path = grid.findPath (grid.toPoint (transform.position), point);
			if (grabbing) {
				if (((path.Count > 0) || point == transform.position + grabPoint) && ((point - transform.position).normalized == grabPoint || (point - transform.position).normalized == -grabPoint) && grid.isOutOfGrid (point + grabPoint)) {
					print ("in");
					grid.SetGridFalse (point + grabPoint);
					grid.SetGridTrue (destination + grabPoint);
					destination = pathDestination = point;
					movement = pathDestination - transform.position;
					walking = true;
				}
			} else {
				if (path.Count > 0) {
					destination = point;
					path.Pop ();
					pathDestination = path.Peek ();
					movement = pathDestination - transform.position;
					lookAt = Quaternion.LookRotation (pathDestination - transform.position);
					walking = true;
				}
			}
		}

		if (!walking && Input.GetButtonDown ("Fire2") && Physics.Raycast (ray, out grabHit, camRayLength, moveableMask)) {
			grabPoint = grid.toPoint (grabHit.collider.transform.position) - transform.position;
			if (!grabbing && grabPoint.magnitude == 1f) {
				grabbing = true;
				lookAt = Quaternion.LookRotation (grabPoint + transform.position);
				grabRigidbody = grabHit.collider.GetComponent <Rigidbody> ();
			} else {
				grabbing = false;
				grabPoint = Vector3.zero;
			}
			print ("grab:" + grabbing);
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
		if (grabbing) {
			grabRigidbody.MovePosition (grabRigidbody.transform.position + movement);
		}
	}

	void Animating (){
		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}
}