using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public GridOverlay grid;
	public bool moving;

	private readonly float speed = 10f;

	private Vector3 movement;
	private Vector3 destination;

	private Rigidbody objRigidbody;

	void Awake () {
		moving = false;
		movement = Vector3.zero;
		destination = transform.position;
		objRigidbody = GetComponent<Rigidbody> ();
	}

	public void SetMoveTo (Vector3 des, Vector3 dir) {
		destination = des;
		destination.y = transform.position.y;
		moving = true;
		movement = dir;
	}

	void FixedUpdate ()  {
		if (moving) {
			if (transform.position == destination) {
				movement = Vector3.zero;
				moving = false;
			}
			print ("" + movement + transform.position + destination);
			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
				moving = false;
			}
			print ("" + movement + transform.position + destination);

			objRigidbody.MovePosition (transform.position + movement);
		}
	}
}
