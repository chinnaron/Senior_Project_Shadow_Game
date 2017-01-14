﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {
	public GameObject showPic;

	public bool isOn;

	private readonly Vector3 picV = new Vector3 (-0.5f, 1.2f, 0f);
	private GameObject show;

	void Awake () {
		isOn = false;
	}

	public void ShowOn () {
		if (isOn) {
			if (show == null)
				show = Instantiate (showPic, transform.position + picV, Quaternion.LookRotation (Vector3.forward));
		} else {
			Destroy (show);
		}
	}
}
