using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {
	public GameObject shadowN;
	public GameObject shadowE;
	public GameObject shadowS;
	public GameObject shadowW;

	private GridOverlay grid;

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();

		shadowN.SetActive (false);
		shadowE.SetActive (false);
		shadowS.SetActive (false);
		shadowW.SetActive (false);
	}

	public void SetShadowN (bool active, float scale) {
		if (!active) {
			shadowN.SetActive (false);
			return;
		}
		shadowN.SetActive (true);
		shadowN.transform.localPosition = Vector3.up * shadowN.transform.localPosition.y + Vector3.forward * (scale / 2f + 0.5f);
		shadowN.transform.localScale = new Vector3 (1f, scale, 1f);

	}

	public void SetShadowE (bool active, float scale) {
		if (!active) {
			shadowE.SetActive (false);
			return;
		}
		shadowE.SetActive (true);
		shadowE.transform.localPosition = Vector3.up * shadowE.transform.localPosition.y + Vector3.forward * (scale / 2f + 0.5f);
		shadowE.transform.localScale = new Vector3 (scale, 1f, 1f);
	}

	public void SetShadowS (bool active, float scale) {
		if (!active) {
			shadowS.SetActive (false);
			return;
		}
		shadowS.SetActive (true);
		shadowS.transform.localPosition = Vector3.up * shadowS.transform.localPosition.y + Vector3.forward * (scale / 2f + 0.5f);
		shadowS.transform.localScale = new Vector3 (1f, scale, 1f);
	}

	public void SetShadowW (bool active, float scale) {
		if (!active) {
			shadowW.SetActive (false);
			return;
		}
		shadowW.SetActive (true);
		shadowW.transform.localPosition = Vector3.up * shadowW.transform.localPosition.y + Vector3.forward * (scale / 2f + 0.5f);
		shadowW.transform.localScale = new Vector3 (scale, 1f, 1f);
	}
}
