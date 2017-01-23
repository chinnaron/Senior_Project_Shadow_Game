using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public bool moving;
	public bool falling;
	private bool onFloor;

	private readonly float speed = 11f;

	private Vector3 movement;
	private Vector3 destination;

	private Rigidbody objRigidbody;
	private GridOverlay grid;
	private ObjectController objController;
	private PlayerController player;

	void Awake () {
		if (transform.position.y == 0f)
			onFloor = true;
		else
			onFloor = false;
		
		moving = falling = false;
		movement = Vector3.zero;
		destination = transform.position;
		objRigidbody = GetComponent<Rigidbody> ();
		objController = GetComponent<ObjectController> ();
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();
	}

	public bool GetOnFloor(){
		return onFloor;
	}

	public void SetMoveTo (Vector3 des, Vector3 dir) {
		transform.position = grid.ToPoint (transform.position);
		destination = des;
		destination.y = transform.position.y;
		moving = true;
		movement = dir;
	}

	public void SetFallTo (Vector3 des) {
		transform.position = grid.ToPoint (transform.position);
		destination = des;
		falling = true;
		onFloor = true;
		movement = Vector3.down;
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

				//is floating
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			objRigidbody.MovePosition (transform.position + movement);
		}

		if (falling) {
			if (transform.position == destination) {
				movement = Vector3.zero;
				falling = false;
				if (objController.GetType () == objController.player) {
					player.ContinueWalking ();
				}
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			objRigidbody.MovePosition (transform.position + movement);
		}
	}
}
