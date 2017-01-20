using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject desPic;
	public GameObject grabPic;
	public GameObject grabPlane;
	public GameObject desPlane;
	public MenuScript menu;

	private readonly Vector3 grabPicV = new Vector3 (0f, 1.2f, 0f);
	private readonly Vector3 desPicV = new Vector3 (0f, 0.3f, 0f);
	private Vector3 movement;
	private Vector3 pathDestination;
	private Vector3 destination;
	private Vector3 point;
	private Vector3 grabPoint;

	private Animator anim;
	private Rigidbody playerRigidbody;
	private Rigidbody grabRigidbody;
	private PushController playerPushController;
	private GridOverlay grid;

	private bool walking;
	private bool grabbing;

	private int playerMask;
	private int grabType;

	private readonly float speed = 5f;
	private readonly float turnSpeed = 10f;
	private readonly float camRayLength = 100f;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit floorHit;
	private RaycastHit hit;
	private RaycastHit grabHit;

	public Stack<Vector3> path = new Stack<Vector3> ();

	void Awake () {
		destination = pathDestination = transform.position;
		movement = grabPoint = Vector3.zero;
		walking = grabbing = false;

		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerPushController = GetComponent<PushController> ();
		playerMask = LayerMask.GetMask ("Player");
		grid = FindObjectOfType<GridOverlay> ();
	}

	void Update (){
		if (!menu._isPaused && !walking) {
			//check is not walking and is click
			if (Input.GetButtonDown ("Fire1")) {//Change Input to mobile ver.
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
								//walk front
								if ((point - transform.position + grabPoint).normalized == grabPoint && grid.IsWalkable (point, transform.position + (grabPoint * 2))) {
									Destroy (desPlane);
									destination = pathDestination = point - grabPoint;
									grid.SetGrid (transform.position + grabPoint, grid.walkable);
									desPlane = Instantiate (desPic, destination + grabPoint + desPicV, Quaternion.LookRotation (Vector3.forward));
									movement = pathDestination - transform.position;
									walking = true;
									path.Clear ();
								} else if ((point - transform.position).normalized == -grabPoint && grid.IsWalkable (point, transform.position - grabPoint)) {
									Destroy (desPlane);
									destination = pathDestination = point;
									grid.SetGrid (transform.position + grabPoint, grid.walkable);
									desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
									movement = pathDestination - transform.position;
									walking = true;
									path.Clear ();
								}
							}
						} else {
							//check is has path
							if (path.Count > 0) {
								Destroy (desPlane);
								destination = point;
								pathDestination = path.Peek ();
								movement = pathDestination - transform.position;
								lookAt = Quaternion.LookRotation (pathDestination - transform.position);
								walking = true;
								path.Pop ();
								desPlane = Instantiate (desPic, destination + desPicV, Quaternion.LookRotation (Vector3.forward));
							}
						}
					}
					//check is hit moveable obj
					if (Physics.Raycast (ray, out grabHit, camRayLength) && grabHit.collider.GetComponent<ObjectController> ().isMoveable) {
						grabPoint = grid.ToPoint0Y (grabHit.collider.GetComponentInParent<Transform> ().position) - grid.ToPoint0Y (transform.position);
						//check is not grabbing and is next to player
						if (!grabbing && grabPoint.magnitude == 1f) {
							grabbing = true;
							lookAt = Quaternion.LookRotation (grabPoint);
							grabRigidbody = grabHit.collider.GetComponentInParent<Rigidbody> ();
							grabType = grid.GetGrid (grid.Set0Y (transform.position + grabPoint));
							grabPlane = Instantiate (grabPic, grabPoint + transform.position + grabPicV, Quaternion.LookRotation (Vector3.forward), grabRigidbody.transform);
						} else {
							GrabRelease ();
						}
					}
				}
			}
		}
	}

	void FixedUpdate () {
		if (playerPushController.moving) {
			walking = false;
			Destroy (desPlane);
		}

		if (walking) {
			//check is arrive pathDestination
			if (transform.position == pathDestination) {
				if (pathDestination == destination) {
					if (grabbing)
						grid.SetGrid (destination + grabPoint, grabType);
					Destroy (desPlane);
					path.Clear ();
					movement = Vector3.zero;
					walking = false;
				} else if (path.Count > 0) {
					pathDestination = path.Peek ();
					path.Pop ();
					movement = pathDestination - transform.position;
					lookAt = Quaternion.LookRotation (movement);
					walking = true;
				}
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((transform.position + movement - pathDestination).normalized, (transform.position - pathDestination).normalized) == -1f)
				movement = pathDestination - transform.position;
			
			playerRigidbody.MovePosition (transform.position + movement);

			if (grabbing)
				grabRigidbody.MovePosition (grabRigidbody.transform.position + movement);
		}

		playerRigidbody.MoveRotation (Quaternion.Lerp (transform.rotation, lookAt, Time.deltaTime * turnSpeed));

		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}

	public Rigidbody GetGrabRigidbody () {
		return grabRigidbody;
	}

	public void SetPushController (Vector3 des, Vector3 dir) {
		playerPushController.SetMoveTo (des, dir);
	}

	public void GrabRelease () {
		grabbing = false;
		Destroy (grabPlane);
		grabPoint = Vector3.zero;
		Destroy (desPlane);
		path.Clear ();
		movement = Vector3.zero;
		walking = false;
	}

	public void Stop () {
		walking = false;
		Destroy (desPlane);
	}
}