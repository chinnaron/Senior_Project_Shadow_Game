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


	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		destination = transform.position;
		movement = Vector3.zero;

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
//		grid = GetComponent <GridOverlay> ();
		grid = FindObjectOfType(typeof(GridOverlay)) as GridOverlay;
	}

	void Update (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);//Change Input to mobile ver.

		if (Input.GetButtonDown ("Fire1") && Physics.Raycast (ray, out hit, camRayLength, floorMask) && grid.GetPoint (hit.point) != transform.position && grid.GetGrid (hit.point) == 0) {//Change Input to mobile ver.
			point = grid.GetPoint (hit.point);
			destination = point;
			destination.y = 0f;
			movement = point - transform.position;
			movement.y = 0f;
			lookAt = Quaternion.LookRotation(point - transform.position);
			walking = true;
		}

		if (Input.GetButtonDown ("Fire1")) {
			grabbing = !grabbing;
		}

		if (Vector3.Dot((destination - transform.position).normalized, movement.normalized) == -1f) {
			transform.position = destination;
			walking = false;
			movement = Vector3.zero;
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



//void Awake ()
//{
//	floorMask = LayerMask.GetMask ("Floor");
//	unWalkableMask = LayerMask.GetMask ("UnWalkable");
//	walkableMask = LayerMask.GetMask ("Walkable");
//	position = destination = toGridIndex (transform.position);
//	direction = (int)(transform.rotation.y % 360 / 90);
//	pathDestination = transform.position;
//
//	anim = GetComponent <Animator> ();
//	playerRigidbody = GetComponent <Rigidbody> ();
//
//	grid = new int[(int)floor.transform.localScale.x, (int)floor.transform.localScale.y];
//	ray = new Ray (new Vector3 (0f, 10f, 0f), Vector3.down);
//
//	for (int x = 0; x < grid.GetLength (0); x++) {
//		for (int z = 0; z < grid.GetLength (1); z++) {
//			ray.origin = new Vector3 (x, 10f, z);
//			if (Physics.Raycast (ray, out hit, camRayLength, floorMask))
//				grid [x, z] = 0;
//			else
//				grid [x, z] = -1;
//		}
//	}
//}
//
//void FixedUpdate ()
//{
//	ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//
//	if (Input.GetButtonDown ("Fire2")) {
//		grabbing = true;
//	}
//
//	Move (ray, hit);
//
//	Animating ();
//}
//
//void Move (Ray ray, RaycastHit hit)
//{
//	if (Input.GetButtonDown ("Fire1") && Physics.Raycast (ray, out hit, camRayLength, floorMask)) {
//		path = findPath (position, toGridIndex (hit.point));
//		if (path.Count > 0) {
//			destination = toGridIndex (hit.point);
//			pathDestination.Set (path.Peek () [0], pathDestination.y, path.Peek () [1]);
//
//			if (path.Peek () [0] > position [0]) {
//				direction = north;
//				movement = Vector3.forward;
//			} else if (path.Peek () [0] < position [0]) {
//				direction = south;
//				movement = Vector3.back;
//			} else if (path.Peek () [1] > position [1]) {
//				direction = east;
//				movement = Vector3.right;
//			} else {
//				direction = west;
//				movement = Vector3.left;
//			}
//
//			path.Pop ();
//			walking = true;
//		} else {
//
//		}
//	}
//
//	if ((direction == north && transform.position.x >= pathDestination.x) || (direction == south && transform.position.x <= pathDestination.x)
//		|| (direction == east && transform.position.z >= pathDestination.z) || (direction == west && transform.position.z <= pathDestination.z)) {
//		transform.position = pathDestination;
//
//		if (path.Count > 0) {
//			position = toGridIndex (transform.position);
//			pathDestination.Set (path.Peek () [0], pathDestination.y, path.Peek () [1]);
//			path.Pop ();
//
//			if (path.Peek () [0] > position [0]) {
//				direction = north;
//				movement = Vector3.forward;
//			} else if (path.Peek () [0] < position [0]) {
//				direction = south;
//				movement = Vector3.back;
//			} else if (path.Peek () [1] > position [1]) {
//				direction = east;
//				movement = Vector3.right;
//			} else {
//				direction = west;
//				movement = Vector3.left;
//			}
//		}
//	}
//
//	if (isEqual(destination, position)) {
//		walking = false;
//		movement = Vector3.zero;
//	} else {
//		walking = true;
//		movement = movement * speed * Time.deltaTime;
//		playerRigidbody.MovePosition (transform.position + movement);
//	}
//}
//
//void Animating ()
//{
//	anim.SetBool ("IsWalking", walking);
//	anim.SetBool ("IsGrabbing", grabbing);
//}
//
//int[] toGridIndex (Vector3 v)
//{
//	return new int[] { (int)(v.x - 0.5f), (int)(v.z - 0.5f) };
//}
//
//bool isSamePoint (Vector3 a, Vector3 b)
//{
//	if (a.x != b.x)
//		return false;
//	if (a.z != b.z)
//		return false;
//	return true;
//}
//
//int[] nearer (int[] a, int[] b, int[] des){
//	return Mathf.Abs (des [0] - a [0]) + Mathf.Abs (des [1] - a [1]) <= Mathf.Abs (des [0] - b [0]) + Mathf.Abs (des [1] - b [1]) ? a : b;
//}
//
//bool isEqual (int[]a, int[]b){
//	if (a [0] != b [0] || a [1] != b [1])
//		return false;
//	return true;
//}
//
//Stack<int[]> findPath (int[] sta, int[] des){
//	Stack<int[]> ans = new Stack<int[]> ();
//	Queue<int[]> q = new Queue<int[]> ();
//	int[,] grid2 = grid;
//	int[] pos = sta;
//	int count = 0;
//
//	q.Enqueue (pos);
//	while (!isEqual (pos, des)) {
//		count++;
//		if (pos [1] != 0 && grid2 [pos [0], pos [1] - 1] == 0) {
//			grid2 [pos [0], pos [1] - 1] = count;
//			q.Enqueue (new int[] { pos [0], pos [1] - 1 });
//		}
//		if (pos [1] != grid2.GetLength (1) - 1 && grid2 [pos [0], pos [1] + 1] == 0) {
//			grid2 [pos [0], pos [1] + 1] = count;
//			q.Enqueue (new int[] { pos [0], pos [1] + 1 });
//		}
//		if (pos [0] != 0 && grid2 [pos [0] - 1, pos [1]] == 0) {
//			grid2 [pos [0] - 1, pos [1]] = count;
//			q.Enqueue (new int[] { pos [0] - 1, pos [1] });
//		}
//		if (pos [0] != grid2.GetLength (0) - 1 && grid2 [pos [0] + 1, pos [1]] == 0) {
//			grid2 [pos [0] + 1, pos [1]] = count;
//			q.Enqueue (new int[] { pos [0] + 1, pos [1] });
//		}
//		q.Dequeue ();
//		if (q.Count < 1)
//			return ans;
//		pos = q.Peek ();
//	}
//
//	int[] near = pos;
//	count = grid2 [pos [0], pos [1]];
//	ans.Push (des);
//	while (!isEqual (pos, sta)) {
//		count--;
//		if (pos [1] != 0 && grid2 [pos [0], pos [1] - 1] == count) {
//			near = nearer (near, new int[] { pos [0], pos [1] - 1 }, sta);
//		}
//		if (pos [1] != grid2.GetLength (1) && grid2 [pos [0], pos [1] + 1] == count) {
//			near = nearer (near, new int[] { pos [0], pos [1] + 1 }, sta);
//		}
//		if (pos [0] != 0 && grid2 [pos [0] - 1, pos [1]] == count) {
//			near = nearer (near, new int[] { pos [0] - 1, pos [1]}, sta);
//		}
//		if (pos [0] != grid2.GetLength (0) && grid2 [pos [0] + 1, pos [1]] == count) {
//			near = nearer (near, new int[] { pos [0] + 1, pos [1]}, sta);
//		}
//		ans.Push (near);
//		pos = near;
//	}
//	ans.Pop ();
//	return ans;
//}