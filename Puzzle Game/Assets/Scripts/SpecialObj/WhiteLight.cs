using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLight : MonoBehaviour {
	public GameObject pic;

	public readonly float rayDistance = 5f;
	public LineRenderer[] line = new LineRenderer[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	private GridOverlay grid;

	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left }; 
	private float[] longL = { 0, 0, 0, 0 };
	private RaycastHit[] hit = new RaycastHit[4];
	private ShadowController[] shadow = new ShadowController[4];
	private GameObject[] onPic = new GameObject[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();

		for (int i = 0; i < 4; i++) {
			if (lightOn [i])
				line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);
		}
	}

	void Update () {
		for (int i = 0; i < 4; i++) {
			if (lightOn [i]) {
				if (onPic [i] == null)
					onPic [i] = Instantiate (pic, grid.Set0Y (transform.position) + wayP [i] * 0.3f, Quaternion.LookRotation (wayP [i]), transform);

				if (Physics.Raycast (transform.position, wayP [i], out hit [i], rayDistance)) {
					if (i % 2 == 0)
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z);
					else
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x);

					if (hit [i].collider.GetComponent<ObjectController> ().isWall)
						longL [i] -= 1f;
					
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * longL [i]);

					if (hit [i].collider.GetComponent<ObjectController> ().isShadowable
					    && (i % 2 == 0 && (hit [i].collider.transform.position.x < transform.position.x + 0.1f
					    && hit [i].collider.transform.position.x > transform.position.x - 0.1f)
					    || i % 2 == 1 && (hit [i].collider.transform.position.z < transform.position.z + 0.1f
					    && hit [i].collider.transform.position.z > transform.position.z - 0.1f))) {
						shadow [i] = hit [i].collider.GetComponent<ShadowController> ();
						shadow [i].SetShadow (true, rayDistance - longL [i], i);
//						grid.SetWalkable (shadow [i].transform.position, shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]));
					} else if (shadow [i] != null) {
//						grid.SetWalkableBack (shadow [i].transform.position, shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]));
//						grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 1) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 1) % 4]);
//						grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 3) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 3) % 4]);
						shadow [i].SetShadow (false, 0f, i);
						shadow [i] = null;
						longL [i] = 0f;
					}
				} else {
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);

					if (shadow [i] != null) {
//						grid.SetWalkableBack (shadow [i].transform.position, shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]));
//						grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 1) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 1) % 4]);
//						grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 3) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 3) % 4]);
						shadow [i].SetShadow (false, 0f, i);
						shadow [i] = null;
						longL [i] = 0f;
					}
				}
			} else {
				if (onPic [i] != null)
					Destroy (onPic [i]);
				
				line [i].SetPosition (line [i].numPositions - 1, Vector3.zero);

				if (shadow [i] != null) {
//					grid.SetWalkableBack (shadow [i].transform.position, shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]));
//					grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 1) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 1) % 4]);
//					grid.SetWalkableBack (shadow [i].transform.position + wayP [(i + 3) % 4], shadow [i].transform.position + wayP [i] * (rayDistance - longL [i]) + wayP [(i + 3) % 4]);
					shadow [i].SetShadow (false, 0f, i);
					shadow [i] = null;
					longL [i] = 0f;
				}
			}
		}
	}
}