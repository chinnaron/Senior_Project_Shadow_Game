using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public bool moving;
	public bool falling;
	public bool jumping;
	private bool onFloor;

	private readonly float speed = 8f;
	private float moveSpeed;
	private float jumpSpeed;
	private float height;
	private float nearest;

	private Vector3 movement;
	private Vector3 moveY;
	private Vector3 destination;

	private GridOverlay grid;
	private ObjectController objController;
	private PlayerController player;

	void Awake () {
		moving = falling = jumping = false;
		movement = moveY = Vector3.zero;
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

		if (!objController.isPlayer) {
			if (onFloor) {
				objController.isBlock = true;
				objController.isBlock2 = false;
			} else {
				objController.isBlock = false;
				objController.isBlock2 = true;
			}
		}
	}

	public bool GetOnFloor(){
		return onFloor;
	}

	public void SetMoveTo (Vector3 des, Vector3 dir) {
		transform.position = grid.ToPointY (transform.position, onFloor) + Vector3.up * height;
		destination = grid.ToPointY (des, onFloor) + Vector3.up * height;
		destination.y = (onFloor ? 0f : 1f) + height;
		moving = true;
		nearest = Vector3.Distance (transform.position, destination);
		movement = dir;

		if (objController.isBlock)
			grid.SetGrid (transform.position, grid.walkable);
		else if (objController.isBlock2)
			grid.SetGrid (transform.position, grid.walkable2);
	}

	public void SetFallTo (Vector3 des) {
		transform.position = grid.ToPointIgnoreY (transform.position) + Vector3.up * height;
		destination = grid.ToPoint0Y (des) + Vector3.up * height;
		falling = true;
		onFloor = true;

		if (objController.isBlock2) {
			objController.isBlock2 = false;
			objController.isBlock = true;
		}

		nearest = Vector3.Distance (transform.position, destination);
		movement = Vector3.down;
	}

	public void SetJumpTo (Vector3 des, bool onF, Vector3 dir, int num) {
		transform.position = grid.ToPointY (transform.position, onFloor) + Vector3.up * height;
		destination = grid.ToPointY (des, onF) + Vector3.up * height;
		jumping = true;
		onFloor = onF;

		if (objController.isBlock) {
			objController.isBlock = false;
			objController.isBlock2 = true;
		}

		nearest = Vector3.Distance (transform.position, destination);
		movement = dir;

		if (num == 1) {
			moveSpeed = 4f;
			jumpSpeed = 15f;
		} else if (num == 2) {
			moveSpeed = speed;
			jumpSpeed = 12f;
		} else {
			moveSpeed = speed;
			jumpSpeed = 9f;
		}
		
		moveY = Vector3.up;
	}

	public bool CheckFall () {
		if (!onFloor && (grid.GetGrid (transform.position) == grid.walkable || grid.GetGrid (transform.position) == grid.tempWalkable)) {
			return true;
		}

		return false;
	}

	public void SetFall () {
		if (!onFloor && (grid.GetGrid (transform.position) == grid.walkable || grid.GetGrid (transform.position) == grid.tempWalkable))
			SetFallTo (grid.ToPoint0Y (transform.position));
	}

	void FixedUpdate () {
		if (moving) {
			print (transform.position + "" + destination);
			if (nearest > Vector3.Distance (transform.position, destination)) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination)) {
				nearest = 0;
				transform.position = destination;
			}

			if (transform.position == destination) {
				movement = Vector3.zero;
				moving = false;

				if (CheckFall ())
					SetFall ();
				
				if (!objController.isPlayer)
					grid.SetGridHere (destination);
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (movement + transform.position - destination).normalized
				, grid.Set0Y (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			transform.position = transform.position + movement;
		}

		if (falling) {
			if (nearest > Vector3.Distance (transform.position, destination)) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination)) {
				nearest = 0;
				transform.position = destination;
			}

			if (transform.position == destination) {
				movement = Vector3.zero;
				falling = false;

				if (objController.GetType () == objController.player) {
					player.ContinueWalking ();
				}

				if (!objController.isPlayer)
					grid.SetGridHere (destination);
			}

			movement = movement.normalized * speed * Time.deltaTime;

			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			transform.position = transform.position + movement;
		}

		if (jumping) {
			if (nearest > Vector3.Distance (transform.position, destination)) {
				nearest = Vector3.Distance (transform.position, destination);
			} else if (nearest < Vector3.Distance (transform.position, destination)) {
				nearest = 0;
				transform.position = destination;
			}

			if (grid.Set0Y (transform.position) == grid.Set0Y (destination)) {
				movement = Vector3.zero;
				moveY = Vector3.zero;
				jumping = false;

				if (!objController.isPlayer)
					grid.SetGridHere (destination);

				if (onFloor && transform.position.y != 0f) {
					SetFallTo (grid.ToPoint0Y (transform.position));
				} else if (!onFloor) {
					if (grid.GetGrid (transform.position) == grid.walkable || grid.GetGrid (transform.position) == grid.tempWalkable)
						SetFallTo (grid.ToPoint0Y (transform.position));
					else if (transform.position.y != 1f)
						SetFallTo (grid.ToPointY (transform.position, onFloor));
				}
			}

			movement = movement.normalized * moveSpeed * Time.deltaTime;
			moveY = moveY.normalized * jumpSpeed * Time.deltaTime;

			if (Vector3.Dot (grid.Set0Y (movement + transform.position - destination).normalized
				, grid.Set0Y (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
				moveY = Vector3.zero;
			}

			transform.position = transform.position + movement + moveY;

			jumpSpeed = jumpSpeed - 2f;
			moveY = Vector3.up;
		}
	}
}
