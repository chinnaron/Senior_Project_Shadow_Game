using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLight : MonoBehaviour {
	private int shadowable;
	private float rayDistance = 5f;

	private float shadowLong;

	private Ray rayN;
	private Ray rayE;
	private Ray rayS;
	private Ray rayW;

	private RaycastHit objHitN;
	private RaycastHit objHitE;
	private RaycastHit objHitS;
	private RaycastHit objHitW;

	private RaycastHit hitN;
	private RaycastHit hitE;
	private RaycastHit hitS;
	private RaycastHit hitW;

	void Awake () {
		rayN = new Ray (transform.position, Vector3.forward);
		rayE = new Ray (transform.position, Vector3.right);
		rayS = new Ray (transform.position, Vector3.back);
		rayW = new Ray (transform.position, Vector3.left);

		shadowable = LayerMask.GetMask ("Shadowable");
	}

	void Update () {
		if (Physics.Raycast (rayN, out hitN, rayDistance) && Physics.Raycast (rayN, out objHitN, rayDistance, shadowable) && objHitN.collider != hitN.collider) {
			
		}
		if (Physics.Raycast (rayE, out hitE, rayDistance) && Physics.Raycast (rayE, out objHitE, rayDistance, shadowable) && objHitE.collider != hitE.collider) {

		}
		if (Physics.Raycast (rayS, out hitS, rayDistance) && Physics.Raycast (rayS, out objHitS, rayDistance, shadowable) && objHitS.collider != hitS.collider) {
			
		}
		if (Physics.Raycast (rayW, out hitW, rayDistance) && Physics.Raycast (rayW, out objHitW, rayDistance, shadowable) && objHitW.collider != hitW.collider) {
			
		}
	}
}
