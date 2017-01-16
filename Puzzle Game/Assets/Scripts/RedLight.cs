using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLight : MonoBehaviour {
	public int rayDistance = 5;
	public GridOverlay grid;
	public PlayerController player;

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
			if (hitN.collider.GetComponent<ObjectController> ().isDestroyable && hitN.collider.transform.position.x == transform.position.x) {
				if (hitN.collider.GetComponent<Rigidbody> () == player.grabRigidbody) {
					if (!player.walking) {
						player.grabbing = false;
						Destroy (hitN.collider.gameObject, Time.deltaTime * 2f);
					}
				}
				Destroy (hitN.collider.gameObject, Time.deltaTime * 2f);
				//create particle
			} else {
				//create particle
			}
		} else {
			lineN.SetPosition (lineN.numPositions - 1, Vector3.forward * rayDistance);
		}
	}
}
