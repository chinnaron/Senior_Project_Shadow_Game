using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public bool moving;
	public bool falling;

	private readonly float speed = 10f;

	private Vector3 movement;
	private Vector3 destination;

	private Rigidbody objRigidbody;
	private GridOverlay grid;
	private ObjectController objController;
	private PlayerController player;

	void Awake () {
		moving = falling = false;
		movement = Vector3.zero;
		destination = transform.position;
		objRigidbody = GetComponent<Rigidbody> ();
		objController = GetComponent<ObjectController> ();
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();
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
		destination.y = transform.position.y - 1f;
		falling = true;
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
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
				moving = false;
			}

			objRigidbody.MovePosition (transform.position + movement);
		}

		if (falling) {
			if (transform.position == destination) {
				print ("hi");
				movement = Vector3.zero;
				falling = false;
				if (objController.GetType () == objController.player)
					player.ContinueWalking ();
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
				falling = false;
			}

			objRigidbody.MovePosition (transform.position + movement);
		}
	}
}
