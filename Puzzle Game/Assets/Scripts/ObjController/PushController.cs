using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public bool moving;
	public bool falling;
	private bool onFloor;

	private readonly float speed = 10f;
	private float height;

	private Vector3 movement;
	private Vector3 destination;

	private GridOverlay grid;
	private ObjectController objController;
	private PlayerController player;

	void Awake () {
		moving = falling = false;
		movement = Vector3.zero;
		destination = transform.position;
		objController = GetComponent<ObjectController> ();
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();

		if (objController.isPlayer)
			height = 0f;
		else
			height = 0.5f;
		
		if (transform.position.y == height)
			onFloor = true;
		else
			onFloor = false;
	}

	public bool GetOnFloor(){
		return onFloor;
	}

	public void SetMoveTo (Vector3 des, Vector3 dir) {
		transform.position = grid.ToPointY (transform.position, onFloor);
		destination = des;
		destination.y = (onFloor ? 0f : 1f) + height;
		print (destination);
		moving = true;
		movement = dir;
	}

	public void SetFallTo (Vector3 des) {
		transform.position = grid.ToPointY (transform.position, onFloor);
		destination = des + Vector3.up * height;
		falling = true;
		onFloor = true;
		movement = Vector3.down;
	}

	public bool CheckFall () {
		if (!onFloor && grid.GetGrid (transform.position) == grid.walkable) {
			return true;
		}

		return false;
	}

	public void SetFall () {
		print ("" + onFloor + grid.GetGrid (transform.position) + grid.walkable);
		if (!onFloor && grid.GetGrid (transform.position) == grid.walkable)
			SetFallTo (grid.ToPoint0Y (transform.position));
	}

	void FixedUpdate () {
		if (moving) {
			if (transform.position == destination) {
				if (objController.GetType () != objController.player)
					grid.SetGrid (destination, objController.GetType ());

				movement = Vector3.zero;
				moving = false;

				CheckFall ();
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (movement + transform.position - destination).normalized
				, grid.Set0Y (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			transform.position = transform.position + movement;
		}

		if (falling) {
			if (transform.position == destination) {
				if (objController.GetType () == objController.player) {
					player.ContinueWalking ();
				}
				movement = Vector3.zero;
				falling = false;
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			transform.position = transform.position + movement;
		}
	}
}
