using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLight : MonoBehaviour {
	public int rayDistance = 5;

	public GridOverlay grid;

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

	void FixedUpdate () {
		//Ray Direction z+
		if (Physics.Raycast (transform.position, Vector3.forward, out hitN, rayDistance)) {
			longN = grid.ToPoint (hitN.collider.transform.position).z - grid.ToPoint (transform.position).z;
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * longN);
			if (hitN.collider.GetComponent<ObjectController> ().isShadowable) {
				shadowN = hitN.collider.GetComponent<ShadowController> ();
				shadowN.SetShadowN (true, rayDistance - longN);
				grid.SetWalkable (Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f)
					, Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f + rayDistance - longN));
			} else if (shadowN != null) {
				shadowN.SetShadowN (false, 0f);
				grid.SetWalkableBack (Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f)
					, Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f + rayDistance - longN));
				shadowN = null;
				longN = 0f;
			}
		} else {
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
			if (shadowN != null) {
				shadowN.SetShadowN (false, 0f);
				grid.SetWalkableBack (Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f)
					, Vector3.right * transform.position.x + Vector3.forward * (shadowN.transform.position.z + 1f + rayDistance - longN));
				shadowN = null;
				longN = 0f;
			}
		}
		//Ray Direction x+
		if (Physics.Raycast (transform.position, Vector3.right, out hitE, rayDistance) && shadowE == null) {
			longE = grid.ToPoint (hitE.collider.transform.position).x - grid.ToPoint (transform.position).x;
			lineE.SetPosition (lineE.numPositions - 1, Vector3.right * longE);
			if (hitE.collider.GetComponent<ObjectController> ().isShadowable) {
				shadowE = hitE.collider.GetComponent<ShadowController> ();
				shadowE.SetShadowE (true, rayDistance - longE);
				grid.SetWalkable (Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + longE + 1f)
					, Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + 1f + rayDistance));
			} else if (shadowE != null) {
				shadowE.SetShadowE (false, 0f);
				grid.SetWalkableBack (Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + longE + 1f)
					, Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + 1f + rayDistance));
				shadowE = null;
				longE = 0f;
			}
		} else {
			lineE.SetPosition (lineE.numPositions - 1, Vector3.right * rayDistance);
			if (shadowE != null) {
				shadowE.SetShadowE (false, 0f);
				grid.SetWalkableBack (Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + longE + 1f)
					, Vector3.forward * transform.position.z + Vector3.right * (shadowE.transform.position.x + 1f + rayDistance));
				shadowE = null;
				longE = 0f;
			}
		}
		//Ray Direction z-
		if (Physics.Raycast (transform.position, Vector3.back, out hitS, rayDistance) && shadowS == null) {
			longS = grid.ToPoint (hitS.collider.transform.position).z - grid.ToPoint (transform.position).z;
			lineS.SetPosition (lineS.numPositions - 1, Vector3.back * longS);
			if (hitS.collider.GetComponent<ObjectController> ().isShadowable) {
				shadowS = hitS.collider.GetComponent<ShadowController> ();
				shadowS.SetShadowS (true, rayDistance - longS);
				grid.SetWalkable (Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + longS + 1f)
					, Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + 1f + rayDistance));
			} else if (shadowS != null) {
				shadowS.SetShadowS (false, 0f);
				grid.SetWalkableBack (Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + longS + 1f)
					, Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + 1f + rayDistance));
				shadowS = null;
				longS = 0f;
			}
		} else {
			lineS.SetPosition (lineS.numPositions - 1, Vector3.back * rayDistance);
			if (shadowS != null) {
				shadowS.SetShadowS (false, 0f);
				grid.SetWalkableBack (Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + longS + 1f)
					, Vector3.right * transform.position.x + Vector3.back * (shadowS.transform.position.z + 1f + rayDistance));
				shadowS = null;
				longS = 0f;
			}
		}
		//Ray Direction x-
		if (Physics.Raycast (transform.position, Vector3.left, out hitW, rayDistance) && shadowW == null) {
			longW = grid.ToPoint (hitW.collider.transform.position).x - grid.ToPoint (transform.position).x;
			lineW.SetPosition (lineW.numPositions - 1, Vector3.left * longW);
			if (hitW.collider.GetComponent<ObjectController> ().isShadowable) {
				shadowW = hitW.collider.GetComponent<ShadowController> ();
				shadowW.SetShadowW (true, rayDistance - longW);
				grid.SetWalkable (Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + longW + 1f)
					, Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + 1f + rayDistance));
			} else if (shadowW != null) {
				shadowW.SetShadowW (false, 0f);
				grid.SetWalkableBack (Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + longW + 1f)
					, Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + 1f + rayDistance));
				shadowW = null;
				longW = 0f;
			}
		} else {
			lineW.SetPosition (lineW.numPositions - 1, Vector3.left * rayDistance);
			if (shadowW != null) {
				shadowW.SetShadowW (false, 0f);
				grid.SetWalkableBack (Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + longW + 1f)
					, Vector3.forward * transform.position.z + Vector3.left * (shadowW.transform.position.x + 1f + rayDistance));
				shadowW = null;
				longW = 0f;
			}
		}
	}
}
