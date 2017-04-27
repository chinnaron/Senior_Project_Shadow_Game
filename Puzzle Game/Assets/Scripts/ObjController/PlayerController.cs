using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
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
	private bool falling;
	private bool walking;
	private bool grabbing;
	private bool pulling;
	private bool pushing;
	private bool click;
	private bool cannotWalk;

	private int count = 0;
	private int dieSpeed;
	private int playerMask;
	private int ignMask;
	private int grabType;

	private float sinkSpeed = 0.5f;
	private float flashSpeed = 5f; 
	private float nearest = 0f;
	private readonly float speed = 5f;
	private readonly float turnSpeed = 10f;
	private readonly float camRayLength = 100f;

	private Quaternion lookAt;

	private Ray ray;
	private RaycastHit floorHit;
	private RaycastHit hit;
	private RaycastHit grabHit;

	private GameObject grabLever;
	private Vector3 grabPointLever;
	private LeverController leverController;

	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public Stack<Vector3> path = new Stack<Vector3> ();

	public void Click(){
		click = true;
	}

	void Awake () {
		InvokeRepeating("WalkingSound", 0f, 0.2f);
		InvokeRepeating("PushingSound", 0f, 0.2f);
		destination = pathDestination = transform.position;
		movement = grabPoint = Vector3.zero;
		walking = grabbing = falling = dying = goToGrab = goToLever = click = false;
		dieSpeed = 10;

		anim = GetComponent<Animator> ();
		playerPush = GetComponent<PushController> ();
		playerMask = LayerMask.GetMask ("Player");
		ignMask = LayerMask.GetMask ("Ignore Raycast");
		grid = FindObjectOfType<GridOverlay> ();
		desPic = Resources.Load ("DesPic", typeof(GameObject)) as GameObject;
//		desNotPic = Resources.Load ("DesNotPic", typeof(GameObject)) as GameObject;
		grabPic = Resources.Load ("GrabPic", typeof(GameObject)) as GameObject;
	}

	void Update (){
		if (!menu._isPaused && !walking && !playerPush.moving && !playerPush.falling && !playerPush.jumping) {
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (Input.GetButtonDown ("Fire1")) {
				
			#else
			if (click) {
				click = false;

			#endif
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//				print(hit.collider.GetComponent<ObjectController> ().isTempWalkable + "" + hit.collider.gameObject.name);
				if (Physics.Raycast (ray, out hit, camRayLength, ~playerMask & ~ignMask)) {
					if (hit.collider.GetComponent<ObjectController> ().isWalkable || hit.collider.GetComponent<ObjectController> ().isWalkable2
						|| hit.collider.GetComponent<ObjectController> ().isTempWalkable || hit.collider.GetComponent<ObjectController> ().isTempWalkable2) {
						point = grid.ToPoint (hit.point);
						if (grabbing) {
							if (point != grid.ToPoint (transform.position + grabPoint) && point != grid.ToPoint (transform.position)) {
								//push
								if (grid.Set0Y (point - transform.position + grabPoint).normalized == grabPoint
								    && grid.IsWalkable (point, transform.position + (grabPoint * 2), playerPush.GetOnFloor ())) {
									path.Clear ();
									path = grid.FindGrabPath (point - grabPoint, transform.position + grabPoint * 2, grabPoint);
									path.Push (grid.ToPointY (transform.position + grabPoint, playerPush.GetOnFloor ()));
									path.Push (grid.ToPointY (transform.position, playerPush.GetOnFloor ()));
//									print(path.Peek());
									StartToWalk (point, grabPoint);
									lookAt = Quaternion.LookRotation (grabPoint);

									if (grabType == grid.block)
										grid.SetGrid (transform.position + grabPoint, grid.walkable);
									else
										grid.SetGrid (transform.position + grabPoint, grid.walkable2);
									
									nearest = Vector3.Distance (transform.position, destination);
									movement = grid.Set0Y (pathDestination - transform.position);
									//pull
								} else if (grid.Set0Y (point - transform.position).normalized == -grabPoint
								           && grid.IsWalkable (point, transform.position - grabPoint, playerPush.GetOnFloor ())) {
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

									nearest = Vector3.Distance (transform.position, destination);
									movement = grid.Set0Y (pathDestination - transform.position);
								} else {
									grid.SetGridHere (transform.position + grabPoint);
									GrabRelease ();

									path.Clear ();
									path = grid.FindPath (transform.position, point, playerPush.GetOnFloor ());

									if (path.Count > 0) {
										StartToWalk (point, Vector3.zero);
										nearest = Vector3.Distance (transform.position, destination);
										movement = grid.Set0Y (pathDestination - transform.position);
										lookAt = Quaternion.LookRotation (movement);
									} else
										cannotWalk = true;
								}
							}
						} else {
							path.Clear ();
							path = grid.FindPath (transform.position, point, playerPush.GetOnFloor ());
//							print(path.Count);
							if (path.Count > 0) {
								StartToWalk (point, Vector3.zero);
								nearest = Vector3.Distance (transform.position, destination);
								movement = grid.Set0Y (pathDestination - transform.position);
								lookAt = Quaternion.LookRotation (movement);
							} else
								cannotWalk = true;
						}
					}else if (Physics.Raycast (ray, out grabHit, camRayLength, ~playerMask) && grabHit.collider.GetComponent<ObjectController> ().isMoveable) {
						if (!grabbing) {
							grabObj = grabHit.collider.gameObject;
							grabPush = grabObj.GetComponent<PushController> ();
							grabPoint = grid.ToPoint0Y (grabPush.transform.position) - grid.ToPoint0Y (transform.position);
							path = grid.FindNearestPath (transform.position, grabPush.transform.position, playerPush.GetOnFloor (), grabPush.GetOnFloor ());

//							print (path.Count);
							if (path.Count > 1) {
								point = path.Peek ();
								path.Pop ();
								StartToWalk (point, Vector3.zero);
								nearest = Vector3.Distance (transform.position, destination);
								movement = grid.Set0Y (pathDestination - transform.position);
								lookAt = Quaternion.LookRotation (movement);
								goToGrab = true;
							} else if (grabPoint.magnitude == 1f && grabPush.GetOnFloor () == playerPush.GetOnFloor ()) {
								grabbing = true;
								lookAt = Quaternion.LookRotation (grabPoint);

								if (grabPush.GetComponent<ObjectController> ().isBlock)
									grabType = grid.block;
								else
									grabType = grid.block2;

								grabPlane = Instantiate (grabPic, grabPoint + transform.position, Quaternion.LookRotation (Vector3.forward), grabPush.transform);
							}
						} else {
							grid.SetGridHere (transform.position + grabPoint);
							GrabRelease ();
						}
					}else if (Physics.Raycast (ray, out grabHit, camRayLength, ~playerMask) && grabHit.collider.GetComponent<ObjectController> ().isLever) {
						if (grabbing) {
							grid.SetGridHere (transform.position + grabPoint);
							GrabRelease ();
						}

						grabLever = grabHit.collider.gameObject;
						//grabPush = grabObj.GetComponent<PushController> ();
						grabPointLever = grid.ToPoint0Y (grabLever.transform.position) - grid.ToPoint0Y (transform.position);
						leverController = grabLever.GetComponent<LeverController> ();
						path = grid.FindNearestPath (transform.position, grabLever.transform.position, playerPush.GetOnFloor (), grabLever.transform.position.y < 0 ? true : false);

						if (path.Count > 1) {
							point = path.Peek ();
							path.Pop ();
							StartToWalk (point, Vector3.zero);
							nearest = Vector3.Distance (transform.position, destination);
							movement = grid.Set0Y (pathDestination - transform.position);
							lookAt = Quaternion.LookRotation (movement);
							goToLever = true;
						} else if (grabPointLever.magnitude == 1f) {
							lookAt = Quaternion.LookRotation (grabPointLever);
							//grabPlane = Instantiate (grabPic, grabPointLever + transform.position, Quaternion.LookRotation (Vector3.forward));
//							Debug.Log ("Lever Grabbed");
							leverController.changeState ();
							//grabLever.changeState ();
						}
					}
					//======================
				}
			}
		}
	}

	void FixedUpdate () {
//		print(grid.GetGrid(transform.position + Vector3.forward * 2));

		if (!walking && !playerPush.moving && !playerPush.falling && !playerPush.jumping && grid.GetGrid (transform.position) == grid.unwalkable) {
			Fall ();
		}

		if (!walking) {
			pushing = false;
			pulling = false;
		}

		if (playerPush.moving) {
			walking = false;
			Destroy (desPlane);
		}

		if (playerPush.falling) {
			walking = false;
		}

		if(cannotWalk)
		{
			// ... set the colour of the damageImage to the flash colour.
			desNotPic.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			desNotPic.color = Color.Lerp (desNotPic.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		cannotWalk = false;

		if (walking) {
			if (nearest >= Vector3.Distance (transform.position, destination) && path.Count > 0) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination) && path.Count == 0) {
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
					if (grabbing && !grabPush.moving) {
						if (!playerPush.falling && !grabPush.falling && (playerPush.CheckFall () || grabPush.CheckFall ())) {
							playerPush.SetFall ();
							grabPush.SetFall ();
							grid.SetGridHere (transform.position + grabPoint);
							GrabRelease ();
						} else
							grid.SetGridHere (transform.position + grabPoint);
					}

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
					path.Clear ();
					movement = Vector3.zero;
					walking = false;
				} else if (path.Count > 0) {
					pathDestination = path.Peek ();
					path.Pop ();
					movement = grid.Set0Y (pathDestination - transform.position);
					walking = true;

					if (grabbing && !grabPush.moving) {
						if (!playerPush.falling && !grabPush.falling && (playerPush.CheckFall () || grabPush.CheckFall ())) {
							playerPush.SetFall ();
							grabPush.SetFall ();
							grid.SetGridHere (transform.position + grabPoint);
							GrabRelease ();
						}
					} else if (!grabbing)
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

		if (falling) {
			transform.position = transform.position + Vector3.down * dieSpeed * Time.deltaTime;
			dieSpeed++;

			if (transform.position.y < -8)
				//reset game
				Application.LoadLevel (Application.loadedLevel);
		}

		if (dying) {
			count++;
			if (count > 50)
				anim.SetBool ("IsDead", true);
			if (count > 100)
				Application.LoadLevel (Application.loadedLevel);
		}

		anim.SetBool ("IsWalking", walking);
		//anim.SetBool ("IsGrabbing", grabbing);
		anim.SetBool ("IsPushing" , pushing);
		anim.SetBool ("IsPulling", pulling);
	}

	public void YouDied(){
		Stop ();
		dying = true;
//		if (i == 2)
			count = 50;
		GetComponent<BoxCollider> ().enabled = false;
	}

	void Fall(){
		falling = true;
	}

	void StartToWalk (Vector3 des, Vector3 grabPoi) {
		Destroy (desPlane);
//		Destroy (desNotPlane);
		destination = des - grabPoi;
//		print(grabbing && grid.GetGrid (des - grabPoi) == grid.block);
		if (grid.GetGrid (des - grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable || (grabbing && grid.GetGrid (des - grabPoi) == grid.block))
			destination.y = 0f;
		else
			destination.y = 1f;

		pathDestination = path.Peek ();
		path.Pop ();

		if (grid.GetGrid (des - grabPoi) == grid.walkable || grid.GetGrid (des - grabPoi) == grid.tempWalkable || (grabbing && grid.GetGrid (des - grabPoi) == grid.block))
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
		grabPush = null;
		grabObj = null;
		goToGrab = false;
		nearest = 0;
		transform.position = grid.ToPoint (transform.position);
	}

	public bool IsGrabbing(){
		return grabbing;
	}

	public Vector3 GetGrabPoint(){
		return grabPoint;
	}

	public void Stop () {
		walking = false;
		path.Clear ();
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


//	void CannotWalk(Vector3 v){
//		
////		Destroy (desNotPlane);
////
////		if (grid.GetGrid (v) == grid.walkable || grid.GetGrid (v) == grid.tempWalkable)
////			desNotPlane = Instantiate (desNotPic, grid.Set0Y (v), Quaternion.LookRotation (Vector3.forward));
////		else
////			desNotPlane = Instantiate (desNotPic, grid.Set1Y (v), Quaternion.LookRotation (Vector3.forward));
////		
////		Destroy (desNotPlane, 1f);
//	}
}