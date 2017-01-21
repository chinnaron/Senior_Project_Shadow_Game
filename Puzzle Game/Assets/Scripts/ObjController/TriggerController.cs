using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {
	public GameObject showPic;

	private bool isOn;

	private GameObject show;

	void Awake () {
		isOn = false;
	}

	public void ShowOn () {
		if (isOn) {
			if (show == null)
				show = Instantiate (showPic, transform.position, Quaternion.LookRotation (Vector3.forward), gameObject.transform);
		} else {
			Destroy (show);
		}
	}

	public void SetOnTrue(){
		isOn = true;
	}

	public void SetOnFalse(){
		isOn = false;
	}
}
