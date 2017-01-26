using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {
	public GameObject[] shadow = new GameObject[4];

	private readonly Vector3 high = Vector3.up * -0.4999f;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private GridOverlay grid;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();

		for (int i = 0; i < 4; i++)
			shadow [i].SetActive (false);
	}

	public void SetShadow (bool active, float scale, int i) {
		if (!active) {
			shadow[i].SetActive (false);
			return;
		}

		shadow[i].SetActive (true);

		if (i % 2 == 0) {
			shadow [i].transform.localPosition = wayP [i] * (scale / 2f + 0.25f) + high;
			shadow [i].transform.localScale = new Vector3 (1f, scale + 0.5f, 1f);

		} else {
			shadow [i].transform.localPosition = wayP [i] * (scale / 2f + 0.25f) + high;
			shadow [i].transform.localScale = new Vector3 (scale + 0.5f, 1f, 1f);
		}

	}
}
