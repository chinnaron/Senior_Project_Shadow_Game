using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {
	public GameObject[] shadow = new GameObject[4];

	private ObjectController[] shadowObj = new ObjectController[4];
	private readonly Vector3 height = Vector3.up * 0.0001f;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private float[] oldScale = new float[4];
	private GridOverlay grid;
	private PushController pushController;
	private RaycastHit hit;
	private ObjectController objCon;
	private int playerMask;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		pushController = GetComponent<PushController> ();
		playerMask = LayerMask.GetMask ("Player");

		for (int i = 0; i < 4; i++) {
			shadowObj [i] = shadow [i].GetComponent<ObjectController> ();
			shadow [i].SetActive (false);
		}
	}

	void Update () {
		for (int i = 0; i < 4; i++) {
			if (pushController.GetOnFloor ()) {
				shadowObj [i].isTempWalkable = true;
				shadowObj [i].isTempWalkable2 = false;
			} else {
				shadowObj [i].isTempWalkable = false;
				shadowObj [i].isTempWalkable2 = true;
			}
		}
	}

	public void SetShadow (bool active, float scale, int i) {
		if (!active) {
			shadow [i].SetActive (false);
			for (int j = 0; j < (int)oldScale [i]; j++) {
				if (Physics.Raycast (transform.position + wayP [i] * (1 + j) + Vector3.up * 5f, Vector3.down, out hit, 10f, ~playerMask)) {
					objCon = hit.collider.GetComponent<ObjectController> ();

					if (objCon.isBlock2)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.block2);
					else if (objCon.isTempWalkable2)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.tempWalkable2);
					else if (objCon.isWalkable2)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.walkable2);
					else if (objCon.isBlock)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.block);
					else if (objCon.isTempWalkable)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.tempWalkable);
					else if (objCon.isWalkable)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.walkable);
					else if (objCon.isUnwalkable)
						grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.unwalkable);
				} else
					grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.unwalkable);
			}
		} else if (active) {

			shadow [i].SetActive (true);

			if (i % 2 == 0) {
				shadow [i].transform.localScale = new Vector3 (1f, scale + 0.5f, 1f);
			} else {
				shadow [i].transform.localScale = new Vector3 (scale + 0.5f, 1f, 1f);
			}

			shadow [i].transform.position = Vector3.forward * transform.position.z + wayP [i] * (scale / 2f + 0.25f) + Vector3.right * transform.position.x + height;

			for (int j = 0; j < (int)scale; j++) {
				grid.SetGrid (transform.position + wayP [i] * (1 + j), pushController.GetOnFloor () ? grid.tempWalkable : grid.tempWalkable2);
			}

			oldScale [i] = scale;
		}
	}
}
