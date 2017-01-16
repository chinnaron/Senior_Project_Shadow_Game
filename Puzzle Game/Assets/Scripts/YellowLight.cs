using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowLight : MonoBehaviour {
	public int rayDistance = 5;
	public GridOverlay grid;

	private float longN = 0;
	private float longE = 0;
	private float longS = 0;
	private float longW = 0;

	private TriggerController objN;

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
			if (hitN.collider.GetComponent<ObjectController> ().isTriggerable && hitN.collider.transform.position.x == transform.position.x) {
				objN = hitN.collider.GetComponent<TriggerController> ();
				objN.SetOnTrue ();
				objN.ShowOn ();
			} else if (objN != null) {
				objN.SetOnFalse ();
				objN.ShowOn ();
				objN = null;
			}
		} else {
			if (objN != null) {
				objN.SetOnFalse ();
				objN.ShowOn ();
				objN = null;
			}
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		}
	}
}