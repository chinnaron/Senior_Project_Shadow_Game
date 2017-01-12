using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed = 5f;

	public GridOverlay grid;
	public GameObject floor;
	public GameObject desPic;
	public GameObject grabPic;

	private Vector3 grabPicV = new Vector3 (0f, 1.2f, 0f);
	private Vector3 desPicV = new Vector3 (0f, 0.3f, 0f);
	private GameObject grabPlane;
	private GameObject desPlane;
	private GameObject grabObj;

	private float turnSpeed = 5f;

	private Animator anim;
	private Rigidbody playerRigidbody;
	private Rigidbody grabRigidbody;

	private bool walking;
	private bool grabbing;

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
		destination = transform.position;
		movement = grabPoint = Vector3.zero;

		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerMask = LayerMask.GetMask ("Player");
	}

	void Update (){
		//check is not walking and is click
		if (Input.GetButtonDown ("Fire1") && !walking) {//Change Input to mobile ver.
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);//Change Input to mobile ver.
			//check is hit sth and not player
			if (Physics.Raycast (ray, out hit, camRayLength, ~playerMask)) {
				//check is hit the floor or not
				if (hit.collider.GetComponent<ObjectController> ().isWalkable) {
					point = grid.ToPoint (hit.point);
					path = grid.FindPath (transform.position, point);
					if (grabbing) {
						//check is has path or is grabbing point and same angle as grabbing and walkable
						if (((path.Count > 0) || point == transform.position + grabPoint)
						    && ((point - transform.position).normalized == grabPoint || (point - transform.position).normalized == -grabPoint)
						    && !grid.IsOutOfGrid (point + grabPoint)) {
							Destroy (desPlane);
							destination = pathDestination = point;
							if (transform.position + grabPoint == destination) {
								grid.SwapGrid (transform.position + grabPoint, destination + grabPoint);
								grid.SwapGrid (transform.position, destination);
							} else {
								grid.SwapGrid (transform.position, destination);
								grid.SwapGrid (transform.position + grabPoint, destination + grabPoint);
							}
							movement = pathDestination - transform.position;
							walking = true;
							desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
						}
					} else {
						//check is has path
						if (path.Count > 0) {
							Destroy (desPlane);
							destination = point;
							grid.SwapGrid (transform.position, destination);
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
				if (Physics.Raycast (ray, out grabHit, camRayLength) && grabHit.collider.GetComponent<ObjectController> ().isMoveable) {
					grabPoint = grid.ToPoint0Y (grabHit.collider.GetComponentInParent<Transform> ().position) - transform.position;
					//check is not grabbing and is next to player
					if (!grabbing && grabPoint.magnitude == 1f) {
						grabbing = true;
						lookAt = Quaternion.LookRotation (grabPoint);
						grabRigidbody = grabHit.collider.GetComponentInParent<Rigidbody> ();
						grabPlane = Instantiate (grabPic, grabPoint + transform.position + grabPicV, Quaternion.LookRotation (Vector3.forward), grabRigidbody.transform);
					} else {
						grabbing = false;
						Destroy (grabPlane);
						grabPoint = Vector3.zero;
					}
				}
			}
		}
		//check is arrive destination
		if (Vector3.Dot((destination - transform.position).normalized, movement.normalized) == -1f) {
			Destroy (desPlane);
			if (grabbing) {
				grabRigidbody.transform.position = destination + grabPoint + Vector3.up * grabRigidbody.transform.position.y;
			}
			transform.position = destination;
			movement = Vector3.zero;
			path.Clear ();
			walking = false;
		}
		//check is arrive pathDestination
		if (Vector3.Dot ((pathDestination - transform.position).normalized, movement.normalized) == -1f) {
			print (path.Count);
			if (path.Count > 0) {
				if (grabbing) {
					grabRigidbody.transform.position = pathDestination + grabPoint + Vector3.up * grabRigidbody.transform.position.y;
				}
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