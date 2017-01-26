using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {
	public GameObject[] shadow = new GameObject[4];

	private readonly Vector3 high = Vector3.up * -0.4999f;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private float[] oldScale = new float[4];
	private GridOverlay grid;
	private PushController pushController;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		pushController = GetComponent<PushController> ();

		for (int i = 0; i < 4; i++)
			shadow [i].SetActive (false);
	}

	public void SetShadow (bool active, float scale, int i) {
		if (!active) {
			shadow[i].SetActive (false);
			for (int j = 0; j < (int)oldScale[i]; j++) {
				print (grid.GetGrid (transform.position + wayP [i] * (1 + j)));
				if (pushController.GetOnFloor () && grid.GetGrid (transform.position + wayP [i] * (1 + j)) == grid.tempWalkable)
					grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.unwalkable);

				if (!pushController.GetOnFloor () && grid.GetGrid (transform.position + wayP [i] * (1 + j)) == grid.tempWalkable2)
					grid.SetGrid (transform.position + wayP [i] * (1 + j), grid.unwalkable);
			}
			for (int j = 0; j < (int)oldScale[i]; j++)
			print (grid.GetGrid (transform.position + wayP [i] * (1 + j)));
			return;
		}

		shadow[i].SetActive (true);

		if (i % 2 == 0) {
			shadow [i].transform.localScale = new Vector3 (1f, scale + 0.5f, 1f);
		} else {
			shadow [i].transform.localScale = new Vector3 (scale + 0.5f, 1f, 1f);
		}

		shadow [i].transform.localPosition = wayP [i] * (scale / 2f + 0.25f) + high;

		for (int j = 0; j < (int)scale; j++) {
			if (grid.GetGrid (transform.position + wayP [i] * (1 + j)) == grid.unwalkable)
				grid.SetGrid (transform.position + wayP [i] * (1 + j), pushController.GetOnFloor () ? grid.tempWalkable : grid.tempWalkable2);
		}

		oldScale [i] = scale;
	}
}
