using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public ObjectController objController;
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
		objController = GetComponent<ObjectController> ();
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
				if (objController.GetType () == objController.player)
					grid.SetGrid (destination, grid.walkable);
				else
					grid.SetGrid (destination, objController.GetType ());
				movement = Vector3.zero;
				moving = false;
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
				moving = false;
			}

			objRigidbody.MovePosition (transform.position + movement);
		}
	}
}
