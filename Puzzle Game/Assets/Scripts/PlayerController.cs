using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GridOverlay grid;

	public GameObject desPic;
	public GameObject grabPic;
	private GameObject grabPlane;
	private GameObject desPlane;

	private readonly Vector3 grabPicV = new Vector3 (0f, 1.2f, 0f);
	private readonly Vector3 desPicV = new Vector3 (0f, 0.3f, 0f);
	private Vector3 movement;
	private Vector3 pathDestination;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Animator anim;
	private Rigidbody playerRigidbody;
	public Rigidbody grabRigidbody;

	public bool walking;
	public bool grabbing;

	private int playerMask;

	private readonly float speed = 3f;
	private readonly float turnSpeed = 5f;
	private readonly float camRayLength = 100f;
	private float distance;
	private float pathDistance;
	private float lowestDistance;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit floorHit;
	private RaycastHit hit;
	private RaycastHit grabHit;

	private Stack<Vector3> path = new Stack<Vector3> ();

	void Awake () {
		destination = pathDestination = transform.position;
		movement = grabPoint = Vector3.zero;
		walking = grabbing = false;

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
						if ((path.Count > 0) && point != transform.position + grabPoint) {
							if ((point - transform.position + grabPoint).normalized == grabPoint) {
								Destroy (desPlane);
								destination = pathDestination = point - grabPoint;
								lowestDistance = distance = pathDistance = Vector3.Distance (point - grabPoint, transform.position);
								grid.SwapGrid (transform.position + grabPoint, destination + grabPoint);
								grid.SwapGrid (transform.position, destination);
								desPlane = Instantiate (desPic, destination + grabPoint + desPicV, Quaternion.LookRotation (Vector3.forward));
								movement = pathDestination - transform.position;
								walking = true;
								grid.moving = true;
								path.Clear ();
							} else if ((point - transform.position).normalized == -grabPoint) {
								Destroy (desPlane);
								destination = pathDestination = point;
								lowestDistance = distance = pathDistance = Vector3.Distance (point, transform.position);
								grid.SwapGrid (transform.position, destination);
								grid.SwapGrid (transform.position + grabPoint, destination + grabPoint);
								desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
								movement = pathDestination - transform.position;
								walking = true;
								grid.moving = true;
								path.Clear ();
							}
						}
					} else {
						//check is has path
						if (path.Count > 0) {
							Destroy (desPlane);
							destination = point;
							distance = Vector3.Distance (point, transform.position);
							pathDestination = path.Peek ();
							lowestDistance = pathDistance = Vector3.Distance (pathDestination, transform.position);
							grid.SwapGrid (transform.position, destination);
							movement = pathDestination - transform.position;
							lookAt = Quaternion.LookRotation (pathDestination - transform.position);
							walking = true;
							grid.moving = true;
							path.Pop ();
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

		//check is arrive pathDestination
		if (transform.position == pathDestination) {
			if (pathDestination == destination) {
				if (grabbing) {
					grabRigidbody.transform.position = pathDestination + grabPoint + Vector3.up * grabRigidbody.transform.position.y;
				}
				Destroy (desPlane);
				transform.position = destination;
				path.Clear ();
				lowestDistance = pathDistance = distance = 0f;
				movement = Vector3.zero;
				walking = false;
				grid.moving = false;
			}

			if (path.Count > 0) {
				if (grabbing) {
					grabRigidbody.transform.position = pathDestination + grabPoint + Vector3.up * grabRigidbody.transform.position.y;
				}
				transform.position = pathDestination;
				pathDestination = path.Peek ();
				path.Pop ();
				lowestDistance = pathDistance = Vector3.Distance (transform.position, pathDestination);
				movement = pathDestination - transform.position;
				lookAt = Quaternion.LookRotation (movement);
				walking = true;
				grid.moving = true;
			}
		} else
			lowestDistance = pathDistance;
		if (walking) {
			pathDistance = distance = Vector3.Distance (transform.position, pathDestination);
		}

	}

	void FixedUpdate () {
		movement = movement.normalized * speed * Time.deltaTime;
		if (Vector3.Dot ((transform.position + movement - pathDestination).normalized, (transform.position - pathDestination).normalized) == -1f)
			movement = pathDestination - transform.position;
		playerRigidbody.MovePosition (transform.position + movement);
		playerRigidbody.MoveRotation (Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * turnSpeed));
		if (grabbing) {
			grabRigidbody.MovePosition (grabRigidbody.transform.position + movement);
		}

		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}
}