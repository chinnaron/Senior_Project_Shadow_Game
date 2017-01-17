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

	private PushController objN;

	public LineRenderer lineN;
	public LineRenderer lineE;
	public LineRenderer lineS;
	public LineRenderer lineW;

	private RaycastHit hitN;
	private RaycastHit hitE;
	private RaycastHit hitS;
	private RaycastHit hitW;

	void Awake () {
		lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		lineE.SetPosition (lineE.numPositions - 1, Vector3.right * rayDistance);
		lineS.SetPosition (lineS.numPositions - 1, Vector3.back * rayDistance);
		lineW.SetPosition (lineW.numPositions - 1, Vector3.left * rayDistance);
	}

	void Update ()	{
		//Ray Direction z+
		if (Physics.Raycast (transform.position, Vector3.forward, out hitN, rayDistance)) {
			longN = grid.ToPoint (hitN.collider.transform.position).z - grid.ToPoint (transform.position).z;
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * longN);
			if (hitN.collider.GetComponent<ObjectController> ().isPushable
				&& hitN.collider.transform.position.x < transform.position.x + 0.1f && hitN.collider.transform.position.x > transform.position.x - 0.1f
			    && hitN.collider.transform.position.z != (transform.position + Vector3.forward * (rayDistance + 1f)).z) {
				objN = hitN.collider.GetComponent<PushController> ();
				if (objN.gameObject == player.gameObject && !objN.moving) {
					objN.GetComponent<PlayerController> ().Stop ();
				} else if (objN.GetComponent<Rigidbody> () == player.GetGrabRigidbody ()) {
					if (player.transform.position.z == transform.position.z)
						player.SetPushController (transform.position + Vector3.forward * (rayDistance + 2f), Vector3.forward);
					else
						player.GrabRelease ();
					grid.SwapGrid (objN.transform.position, transform.position + Vector3.forward * (rayDistance + 1f));
				}
				objN.SetMoveTo (transform.position + Vector3.forward * (rayDistance + 1f), Vector3.forward);
			}
		} else {
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		}
	}
}
