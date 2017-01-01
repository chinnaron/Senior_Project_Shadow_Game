using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public float speed = 1f;
	public GameObject floor;
//	public GameObject light;

	public int[,] grid;

	private static int north = 0;
	private static int east = 1;
	private static int south = 2;
	private static int west = 3;

	private Animator anim;
	private Rigidbody playerRigidbody;

	private bool walking;
	private bool grabbing;

	private int floorMask;
	private int unWalkableMask;
	private int direction = north;
	private float camRayLength = 100f;
	private int[] destination;
	private int[] position;

	private Vector3 movement;
	private Vector3 rayOrigin = new Vector3 (0f, 10f, 0f);
	private Vector3 rayDirection = Vector3.down;

	private Ray ray;
	private RaycastHit hit;

//	private List<Vector3> path;

	void Awake (){
		floorMask = LayerMask.GetMask ("Floor");
		unWalkableMask = LayerMask.GetMask ("UnWalkable");
		position = destination = toAInt (transform.position);
		direction = (int)(transform.rotation.y % 360 / 90);

		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();

		grid = new int[(int)floor.transform.localScale.x, (int)floor.transform.localScale.y];
		ray = new Ray (rayOrigin, rayDirection);
		for (int x = 0; x < grid.GetLength (0); x++) {
			for (int z = 0; z < grid.GetLength (1); z++) {
				rayOrigin.x = rayDirection.x = x;
				rayOrigin.z = rayDirection.z = z;
				ray.direction = rayDirection;
				ray.origin = rayOrigin;
				if (Physics.Raycast (ray, out hit, camRayLength, unWalkableMask))
					grid [x, z] = 0;
				else
					grid [x, z] = 1;
			}
		}
	}

	void FixedUpdate (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetButtonDown ("Fire2")) {
			grabbing = true;
		}

		Move (ray, hit);

		Animating ();
	}

	void Move (Ray ray, RaycastHit hit){
		if (Input.GetButtonDown ("Fire1") && Physics.Raycast(ray, out hit, camRayLength, floorMask)) {
//			path = findPath (hit.point, transform.position);
			walking = true;
			destination = toAInt (hit.point);
		}

		if (destination == position) {
			walking = false;
		} else {
			walking = true;
		}

		movement = transform.position.normalized;
		movement *= speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Animating (){
		anim.SetBool ("IsWalking", walking);
		anim.SetBool ("IsGrabbing", grabbing);
	}

	int[] toAInt (Vector3 v){
		return new int[] { (int)v.x, (int)v.y };
	}
		
	bool isSamePoint (Vector3 a, Vector3 b){
		if (a.x != b.x)
			return false;
		if (a.z != b.z)
			return false;
		return true;
	}

//	List<Vector3> findPath (Vector3 sta, Vector3 des){
//		sta.x = (int)sta.x;
//		des.x = (int)des.x;
//		sta.z = (int)sta.z;
//		des.z = (int)des.z;
//		int len = Mathf.Abs ((int)des.x - (int)sta.x);
//		for (int i = 0; i < len; i++) {
//			path.Add (new Vector3 (sta.x, 0f, sta.z));
//			if (des.x < sta.x)
//				sta.x--;
//			else
//				sta.x++;
//		}
//		len = Mathf.Abs ((int)des.z - (int)sta.z);
//		for (int i = 0; i < len; i++) {
//			path.Add (new Vector3 (sta.x, 0f, sta.z));
//			if (des.z < sta.z)
//				sta.z--;
//			else
//				sta.z++;
//		}
//
//		return path;
//	}
}
