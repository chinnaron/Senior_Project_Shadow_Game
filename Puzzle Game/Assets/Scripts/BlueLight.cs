using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLight : MonoBehaviour {
	public PlayerController player;
	public int rayDistance = 5;
	public GridOverlay grid;

	private float longN = 0;
	private float longE = 0;
	private float longS = 0;
	private float longW = 0;

	private Rigidbody objN;

	public LineRenderer lineN;
	public LineRenderer lineE;
	public LineRenderer lineS;
	public LineRenderer lineW;

	private RaycastHit hitN;
	private RaycastHit hitE;
	private RaycastHit hitS;
	private RaycastHit hitW;

	private float speed = 5f;
	private bool moving;
	private Vector3 movement;

	void Awake () {
		lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		lineE.SetPosition (lineE.numPositions - 1, Vector3.right * rayDistance);
		lineS.SetPosition (lineS.numPositions - 1, Vector3.back * rayDistance);
		lineW.SetPosition (lineW.numPositions - 1, Vector3.left * rayDistance);
	}

	void Update ()	{
		//Ray Direction z+
		if (Physics.Raycast (transform.position, Vector3.forward, out hitN, rayDistance + 0.5f)) {
			longN = grid.ToPoint (hitN.collider.transform.position).z - grid.ToPoint (transform.position).z;
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * longN);
			if (hitN.collider.GetComponent<ObjectController> ().isPushable && hitN.collider.transform.position.x == transform.position.x
			    && hitN.collider.transform.position.z != (transform.position + Vector3.forward * (rayDistance + 1f)).z) {
				objN = hitN.collider.GetComponent<Rigidbody> ();
				if (!player.walking) {
					if (objN == player.grabRigidbody) {
						player.grabbing = false;
						Destroy (player.grabPlane);
						grid.SwapGrid (objN.transform.position, transform.position + Vector3.forward * (rayDistance + 1f));
						moving = true;
					} else {
						grid.SwapGrid (objN.transform.position, transform.position + Vector3.forward * (rayDistance + 1f));
						moving = true;
					}
				}
			}
		} else {
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		}
	}

	void FixedUpdate ()  {
		print (moving);
		if (moving && objN != null) {
			if (objN.gameObject == player) {
				player.pushing = true;
			}

			movement = Vector3.forward * speed * Time.deltaTime;

			if (movement + objN.transform.position == transform.position + Vector3.forward * (rayDistance + 1f)) {
				movement = Vector3.zero;
				moving = false;
			}

			if (Vector3.Dot ((movement + objN.transform.position - (transform.position + Vector3.forward * (rayDistance + 1f))).normalized
				, (objN.transform.position - (transform.position + Vector3.forward * (rayDistance + 1f))).normalized) == -1f) {
				movement = Vector3.forward * (rayDistance + 1f);
				moving = false;
			}

			objN.MovePosition (objN.transform.position + Vector3.forward * speed * Time.deltaTime);
		}
	}
}
