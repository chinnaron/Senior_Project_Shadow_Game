using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLight : MonoBehaviour {
	public int rayDistance = 5;
	public GridOverlay grid;

	private bool lastCheck = false;

	private float longN = 0;
	private float longE = 0;
	private float longS = 0;
	private float longW = 0;

	public LineRenderer lineN;
	public LineRenderer lineE;
	public LineRenderer lineS;
	public LineRenderer lineW;

	private ShadowController shadowN;
	private ShadowController shadowE;
	private ShadowController shadowS;
	private ShadowController shadowW;

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

	void Update () {
		if (grid.moving || lastCheck) {
			//Ray Direction z+
			if (Physics.Raycast (transform.position, Vector3.forward, out hitN, rayDistance)) {
				longN = grid.ToPoint (hitN.collider.transform.position).z - grid.ToPoint (transform.position).z;
				lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * longN);
				if (hitN.collider.GetComponent<ObjectController> ().isShadowable && hitN.collider.transform.position.x == transform.position.x) {
					shadowN = hitN.collider.GetComponent<ShadowController> ();
					shadowN.SetShadowN (true, rayDistance - longN);
					grid.SetWalkable (shadowN.transform.position, shadowN.transform.position + Vector3.forward * (rayDistance - longN));
				} else if (shadowN != null) {
					grid.SetWalkableBack (shadowN.transform.position, shadowN.transform.position + Vector3.forward * (rayDistance - longN));
					grid.SetWalkableBack (shadowN.transform.position + Vector3.right, shadowN.transform.position + Vector3.forward * (rayDistance - longN) + Vector3.right);
					grid.SetWalkableBack (shadowN.transform.position + Vector3.left, shadowN.transform.position + Vector3.forward * (rayDistance - longN) + Vector3.left);
					shadowN.SetShadowN (false, 0f);
					shadowN = null;
					longN = 0f;
				}
			} else {
				lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
				if (shadowN != null) {
					grid.SetWalkableBack (shadowN.transform.position, shadowN.transform.position + Vector3.forward * (rayDistance - longN));
					grid.SetWalkableBack (shadowN.transform.position + Vector3.right, shadowN.transform.position + Vector3.forward * (rayDistance - longN) + Vector3.right);
					grid.SetWalkableBack (shadowN.transform.position + Vector3.left, shadowN.transform.position + Vector3.forward * (rayDistance - longN) + Vector3.left);
					shadowN.SetShadowN (false, 0f);
					shadowN = null;
					longN = 0f;
				}
			}

			if (!grid.moving)
				lastCheck = false;
		} else
			lastCheck = false;
	}
}
