using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject desPic;
	public GameObject grabPic;
	public GameObject grabPlane;
	public GameObject desPlane;
	public MenuScript menu;

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

	private bool walking;
	private bool grabbing;
	private bool pulling;
	private bool pushing;

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
		playerPush = GetComponent<PushController> ();
		playerMask = LayerMask.GetMask ("Player");
		grid = FindObjectOfType<GridOverlay> ();
	}

	void Update (){
		if (!menu._isPaused && !walking) {
			if (Input.GetButtonDown ("Fire1")) {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, camRayLength, ~playerMask)) {
					if (hit.collider.GetComponent<ObjectController> ().isWalkable || hit.collider.GetComponent<ObjectController> ().isWalkable2) {
						point = grid.ToPoint (hit.point);
						if (grabbing) {
							if (point != grid.ToPoint(transform.position + grabPoint) && point != grid.ToPoint(transform.position)) {
								if (grid.Set0Y (point - transform.position + grabPoint).normalized == grabPoint && grid.IsWalkable (point, transform.position + (grabPoint * 2), playerPush.GetOnFloor ())) {
									path.Clear ();
									path = grid.FindGrabPath (point - grabPoint, transform.position + (grabPoint * 2), grabPoint);
									path.Push (grid.ToPointY (transform.position, playerPush.GetOnFloor ()) + grabPoint);
									StartToWalk (point, grabPoint);
									lookAt = Quaternion.LookRotation (grabPoint);

									if (grabType == grid.block)
										grid.SetGrid (transform.position + grabPoint, grid.walkable);
									else
										grid.SetGrid (transform.position + grabPoint, grid.walkable2);

									movement = grid.Set0Y (pathDestination - transform.position);
								} else if (grid.Set0Y (point - transform.position).normalized == -grabPoint && grid.IsWalkable (point, transform.position - grabPoint, playerPush.GetOnFloor ())) {
									path.Clear ();
									if (grid.ToPoint0Y (point) == grid.ToPoint0Y (transform.position - grabPoint))
										path.Push (point);
									else
										path = grid.FindGrabPath (point, transform.position - grabPoint, -grabPoint);
									
									StartToWalk (point, Vector3.zero);
									lookAt = Quaternion.LookRotation (grabPoint);

									if (grabType == grid.block)
										grid.SetGrid (transform.position + grabPoint, grid.walkable);
									else
										grid.SetGrid (transform.position + grabPoint, grid.walkable2);

									movement = grid.Set0Y (pathDestination - transform.position);
								}
							}
						} else {
							path.Clear ();
							path = grid.FindPath (transform.position, point, playerPush.GetOnFloor ());

							if (path.Count > 0) {
								StartToWalk (point, Vector3.zero);
								movement = grid.Set0Y (pathDestination - transform.position);
								lookAt = Quaternion.LookRotation (movement);
							}
						}
					}

					if (Physics.Raycast (ray, out grabHit, camRayLength,~playerMask) && grabHit.collider.GetComponent<ObjectController> ().isMoveable) {
						grabObj = grabHit.collider.gameObject;
						grabPush = grabObj.GetComponent<PushController> ();
						grabPoint = grid.ToPoint0Y (grabPush.transform.position) - grid.ToPoint0Y (transform.position);

						if (!grabbing && grabPoint.magnitude == 1f && grabPush.GetOnFloor () == playerPush.GetOnFloor ()) {
							grabbing = true;
							lookAt = Quaternion.LookRotation (grabPoint);

							if (grabPush.GetComponent<ObjectController> ().isBlock)
								grabType = grid.block;
							else
								grabType = grid.block2;
							
							grabPlane = Instantiate (grabPic, grabPoint + transform.position, Quaternion.LookRotation (Vector3.forward), grabPush.transform);
						} else {
							GrabRelease ();
							grid.SetGrid (grabPush.transform.position, grabType);
						}
					}
				}
			}
		}
	}

	void FixedUpdate () {
		if (playerPush.moving) {
			walking = false;
			Destroy (desPlane);
		}

		if (playerPush.falling) {
			walking = false;
		}

		if (walking) {
			if (transform.position == pathDestination + Vector3.up) {
				if (grabbing && !grabPush.moving) {
					if (!playerPush.falling && !grabPush.falling && (playerPush.CheckFall () || grabPush.CheckFall ())) {
						playerPush.SetFall ();
						grabPush.SetFall ();
						GrabRelease ();
						grid.SetGrid (grabPush.transform.position, grabType);
					}
				} else {
					print (transform.position);
					if (!playerPush.falling && playerPush.CheckFall ()) {
						movement = Vector3.zero;
						walking = false;
 							playerPush.SetFall ();
					}
				}
			}

			if (transform.position == pathDestination) {
				if (grabbing && !grabPush.moving) {
					if (!playerPush.falling && !grabPush.falling && (playerPush.CheckFall () || grabPush.CheckFall ())) {
						playerPush.SetFall ();
						grabPush.SetFall ();
						GrabRelease ();
						grid.SetGrid (grabPush.transform.position, grabType);
					}
				}

				if (pathDestination == destination) {
					Destroy (desPlane);
					path.Clear ();
					movement = Vector3.zero;
					walking = false;
				} else if (path.Count > 0) {
					pathDestination = path.Peek ();
					path.Pop ();
					movement = grid.Set0Y (pathDestination - transform.position);
					walking = true;

					if (!grabbing)
						lookAt = Quaternion.LookRotation (movement);
				}
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (transform.position + movement - pathDestination).normalized
				, grid.Set0Y (transform.position - pathDestination).normalized) == -1f)
				movement = grid.Set0Y (pathDestination - transform.position);
			
			transform.position = transform.position + movement;

			if (grabbing) {
				grabPush.transform.position = grabPush.transform.position + movement;
				if (grabPoint.x == 1) {
					if (movement.x > 0) {
						pulling = false;
						pushing = true;
					} else if (movement.x < 0) {
						pushing = false;
						pulling = true;
					}
				} else if (grabPoint.x == -1) {
					if (movement.x < 0) {
						pulling = false;
						pushing = true;
					} else if (movement.x > 0) {
						pushing = false;
						pulling = true;
					}
				} else if (grabPoint.z == 1) {
					if (movement.z > 0) {
						pulling = false;
						pushing = true;
					} else if (movement.z < 0) {
						pushing = false;
						pulling = true;
					}
				} else if (grabPoint.z == -1) {
					if (movement.z < 0) {
						pulling = false;
						pushing = true;
					} else if (movement.z > 0) {
						pushing = false;
						pulling = true;
					}
				}

			}

		
		}
			
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAt, Time.deltaTime * turnSpeed);

		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
		anim.SetBool ("IsPushing" , pushing);
		anim.SetBool ("IsPulling", pulling);
	}

	void StartToWalk (Vector3 des, Vector3 grabPoi) {
		Destroy (desPlane);
		destination = des - grabPoi;

		if (grid.GetGrid (des - grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable || (grabbing && grid.GetGrid (des - grabPoi) == grid.block))
			destination.y = 0f;
		else
			destination.y = 1f;
		
		pathDestination = path.Peek ();
		path.Pop ();
		if (grid.GetGrid (destination + grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable)
			desPlane = Instantiate (desPic, grid.Set0Y (destination + grabPoi), Quaternion.LookRotation (Vector3.forward));
		else
			desPlane = Instantiate (desPic, grid.Set1Y (destination + grabPoi), Quaternion.LookRotation (Vector3.forward));
		walking = true;
	}

	public Vector3 GetMovement(){
		return movement;
	}

	public void ContinueWalking () {
		if (path.Count > 0)
			walking = true;
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
		path.Clear ();
		movement = Vector3.zero;
		walking = false;
	}

	public void Stop () {
		walking = false;
		Destroy (desPlane);
	}
}