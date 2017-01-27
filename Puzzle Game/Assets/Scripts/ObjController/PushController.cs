using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushController : MonoBehaviour {
	public bool moving;
	public bool falling;
	public bool jumping;
	private bool onFloor;

	private readonly float speed = 10f;
	private float moveSpeed;
	private float jumpSpeed;
	private float height;

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
	}

	public bool GetOnFloor(){
		return onFloor;
	}

	public void SetMoveTo (Vector3 des, Vector3 dir) {
		transform.position = grid.ToPointY (transform.position, onFloor) + Vector3.up * height;
		destination = grid.ToPointY (des, onFloor) + Vector3.up * height;
		destination.y = (onFloor ? 0f : 1f) + height;
		moving = true;
		movement = dir;

		if (objController.GetType () == grid.block)
			grid.SetGrid (transform.position, grid.walkable);
		else if (objController.GetType () == grid.block2)
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

		movement = dir;
		if (num == 1) {
			moveSpeed = 4f;
			jumpSpeed = 15f;
		} else if (num == 2) {
			moveSpeed = speed;
			jumpSpeed = 9f;
		} else {
			moveSpeed = speed;
			jumpSpeed = 9f;
		}
		
		moveY = Vector3.up;
	}

	public bool CheckFall () {
		if (!onFloor && grid.GetGrid (transform.position) == grid.walkable) {
			return true;
		}

		return false;
	}

	public void SetFall () {
		if (!onFloor && grid.GetGrid (transform.position) == grid.walkable)
			SetFallTo (grid.ToPoint0Y (transform.position));
	}

	void FixedUpdate () {
		if (moving) {
			if (transform.position == destination) {
				movement = Vector3.zero;
				moving = false;

				if (CheckFall ())
					SetFall ();
				
				if (objController.GetType () != objController.player)
					grid.SetGrid (destination, objController.GetType ());
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
				movement = Vector3.zero;
				falling = false;

				if (objController.GetType () == objController.player) {
					player.ContinueWalking ();
				}

				if (objController.GetType () != objController.player)
					grid.SetGrid (destination, objController.GetType ());
			}

			movement = movement.normalized * speed * Time.deltaTime;
			print ("" + transform.position + destination);
			if (Vector3.Dot ((movement + transform.position - destination).normalized
				, (transform.position - destination).normalized) == -1f) {
				movement = destination - transform.position;
			}

			transform.position = transform.position + movement;
		}

		if (jumping) {
			if (grid.Set0Y (transform.position) == grid.Set0Y (destination)) {
				movement = Vector3.zero;
				moveY = Vector3.zero;
				jumping = false;

				if (objController.GetType () != objController.player)
					grid.SetGrid (destination, objController.GetType ());

				if (transform.position.y != destination.y) {
					SetFallTo (destination);
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
