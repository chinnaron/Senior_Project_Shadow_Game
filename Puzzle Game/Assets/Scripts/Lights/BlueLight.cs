using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLight : MonoBehaviour {
	public PlayerController player;
	public GridOverlay grid;

	private float longN = 0;
	private float longE = 0;
	private float longS = 0;
	private float longW = 0;

	private float rayDistance = 6f;
	private float rayDistanceN;
	private float rayDistanceE;
	private float rayDistanceS;
	private float rayDistanceW;

	private RaycastHit[] listN;

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
		rayDistanceN = rayDistance;
		rayDistanceE = rayDistance;
		rayDistanceS = rayDistance;
		rayDistanceW = rayDistance;

		lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		lineE.SetPosition (lineE.numPositions - 1, Vector3.right * rayDistance);
		lineS.SetPosition (lineS.numPositions - 1, Vector3.back * rayDistance);
		lineW.SetPosition (lineW.numPositions - 1, Vector3.left * rayDistance);
	}

	void Update ()	{
		//Ray Direction z+
		if (Physics.Raycast (transform.position, Vector3.forward, out hitN, rayDistanceN)) {
			longN = grid.ToPoint (hitN.collider.transform.position).z - grid.ToPoint (transform.position).z - 1f;

			if (longN > rayDistance)
				longN = rayDistance;
			
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * longN);
			listN = Physics.RaycastAll (transform.position, Vector3.forward, rayDistance + 1);

			if (listN.Length < 2)
				rayDistanceN = rayDistance + 1f;
			else
				rayDistanceN = grid.ToPoint0Y (listN [1].collider.transform.position).z - grid.ToPoint0Y (transform.position).z - 1f;
			
			if (hitN.collider.GetComponent<ObjectController> ().isPushable
				&& hitN.collider.transform.position.x < transform.position.x + 0.1f && hitN.collider.transform.position.x > transform.position.x - 0.1f
				&& grid.ToPoint0Y(hitN.collider.transform.position).z != grid.ToPoint0Y(transform.position).z + rayDistanceN) {
				objN = hitN.collider.GetComponent<PushController> ();
				if (!objN.moving) {
					if (objN.gameObject == player.gameObject)
						player.Stop ();
					else if (objN.GetComponent<Rigidbody> () == player.GetGrabRigidbody ()) {
						if (grid.ToPoint0Y (player.transform.position).x == grid.ToPoint0Y (transform.position).x) {
							player.Stop ();
							player.SetPushController (transform.position + Vector3.forward * (rayDistanceN + 1), Vector3.forward);
						}
						else
							player.GrabRelease ();
					}
					
					objN.SetMoveTo (transform.position + Vector3.forward * (rayDistanceN), Vector3.forward);
				}
			}
		} else {
			rayDistanceN = rayDistance + 1;
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * (rayDistanceN - 1));
		}
	}
}
