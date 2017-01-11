using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject floor;
	public GameObject desPic;
	public GameObject grabPic;

	private Vector3 grabPicV = new Vector3 (0f, 1.2f, 0f);
	private Vector3 desPicV = new Vector3 (0f, 0.3f, 0f);
	private GameObject grabPlane;
	private GameObject desPlane;
	private GridOverlay grid;
	private GameObject grabObj;

	private float speed = 5f;
	private float turnSpeed = 5f;

	private Animator anim;
	private Rigidbody playerRigidbody;
	private Rigidbody grabRigidbody;

	private bool walking;
	private bool grabbing;

	private int floorMask;
	private int moveableMask;
	private int playerMask;
	private float camRayLength = 100f;

	private Vector3 pathDestination;
	private Vector3 movement;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit floorHit;
	private RaycastHit hit;
	private RaycastHit grabHit;

	private Stack<Vector3> path = new Stack<Vector3> ();

	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		moveableMask = LayerMask.GetMask ("Moveable");
		playerMask = LayerMask.GetMask ("Player");
		destination = transform.position;
		movement = grabPoint = Vector3.zero;

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		grid = floor.GetComponent <GridOverlay> ();
	}

	void Update (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);//Change Input to mobile ver.

		if (Input.GetButtonDown ("Fire1") && !walking) {//Change Input to mobile ver.
			if (Physics.Raycast (ray, out hit, camRayLength, ~playerMask)) {
				//check is hit the floor or not
				if (hit.collider.gameObject == floor) {
					point = grid.toPoint (hit.point);
					path = grid.findPath (grid.toPoint (transform.position), point);
					if (grabbing) {
						if (((path.Count > 0) || point == transform.position + grabPoint)
						    && ((point - transform.position).normalized == grabPoint || (point - transform.position).normalized == -grabPoint)
							&& grid.isOutOfGrid (point + grabPoint)) {
							Destroy (desPlane);
							grid.swapGrid (point, destination);
							grid.swapGrid (point + grabPoint, destination + grabPoint);
							destination = pathDestination = point;
							movement = pathDestination - transform.position;
							walking = true;
							desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
						}
					} else {
						if (path.Count > 0) {
							Destroy (desPlane);
							grid.swapGrid (point, destination);
							destination = point;
							path.Pop ();
							pathDestination = path.Peek ();
							movement = pathDestination - transform.position;
							lookAt = Quaternion.LookRotation (pathDestination - transform.position);
							walking = true;
							desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
						}
					}
				}

				//check is hit moveable obj
				if (Physics.Raycast (ray, out grabHit, camRayLength, moveableMask) && grid.GetGrid (hit.collider.GetComponentInParent <Transform> ().position) % grid.moveable == 0) {
					grabPoint = grid.toPoint (grabHit.collider.GetComponentInParent <Transform> ().position) - transform.position;
					if (!grabbing && grabPoint.magnitude == 1f) {
						grabbing = true;
						lookAt = Quaternion.LookRotation (grabPoint);
						grabRigidbody = grabHit.collider.GetComponentInParent <Rigidbody> ();
						grabPlane = Instantiate (grabPic, grabPoint + transform.position + grabPicV, Quaternion.LookRotation (Vector3.forward), grabRigidbody.transform);
					} else {
						grabbing = false;
						Destroy (grabPlane);
						grabPoint = Vector3.zero;
					}
				}
			}
		}
		
		if (Vector3.Dot((destination - transform.position).normalized, movement.normalized) == -1f) {
			Destroy (desPlane);
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
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
		playerRigidbody.MoveRotation (Quaternion.Lerp(transform.rotation, lookAt, Time.fixedDeltaTime * turnSpeed));
		if (grabbing) {
			grabRigidbody.MovePosition (grabRigidbody.transform.position + movement);
		}

		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}
}