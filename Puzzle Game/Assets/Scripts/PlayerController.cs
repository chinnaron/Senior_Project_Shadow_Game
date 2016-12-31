using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
	
	public float speed = 2f;

	private Animator anim;
	private bool walking;
//	private bool grabbing;
	private int floorMask;
	private float camRayLength = 400f;
	private Vector3 movement;
	private Rigidbody playerRigidbody;
	private Vector3 destination;
	private Ray ray;
	private RaycastHit hit;
	private List<Vector3> path;
	private NavMeshAgent navMeshAgent;


	void Awake (){
		floorMask = LayerMask.GetMask ("Floor");
		destination = transform.position;


		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		navMeshAgent = GetComponent <NavMeshAgent> ();
	}

	void FixedUpdate (){
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		Move (ray, hit);
//		Turning (ray, hit);
		Animating ();
	}

	void Move (Ray ray, RaycastHit hit){

		if (Input.GetButtonDown ("Fire1") && Physics.Raycast(ray, out hit, camRayLength, floorMask)) {
//			path = findPath (hit.point, transform.position);
			walking = true;
			navMeshAgent.destination = hit.point;
			navMeshAgent.Resume ();
		}

		if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) < float.Epsilon)) {
			walking = false;
		} else {
			walking = true;
		}
	}

//	void Turning (Ray ray, RaycastHit hit){
//		
//	}

	void Animating (){
		anim.SetBool ("IsWalking", walking);
//		anim.SetBool ("IsGrabbing", grabbing);
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
