using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchController : MonoBehaviour {

	public GameObject lightBox;
	private bool state;
	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		state = false;
		setLight ();
		originalPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnTriggerEnter(Collider other){
		changeState ();
	}

	public void OnTriggerExit(Collider other){
		changeState ();
	}

	public string getType(){
		ObjectController a = lightBox.GetComponent<ObjectController> ();
		if (a.isBlueLight)
			return "BlueLight";
		else if (a.isRedLight)
			return "RedLight";
		else if (a.isYellowLight)
			return "YellowLight";
		else if (a.isWhiteLight)
			return "WhiteLight";
		else
			return null;
	}


	public void changeState(){
		state = !state;
		Debug.Log ("Pressure Switch state changed from "+!state+" to "+state);
		setPressureSwitch ();
		setLight ();
	}

	public void setLight(){
		string type = getType ();
		if (type == "WhiteLight") {
			Debug.Log ("White light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<WhiteLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<WhiteLight> ().lightOn [i] = state;
			}
		} else if (type == "RedLight") {
			Debug.Log ("Red light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<RedLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<RedLight> ().lightOn [i] = state;
			}
		} else if (type == "BlueLight") {
			Debug.Log ("Blue light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<BlueLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<BlueLight> ().lightOn [i] = state;
			}
		} else if (type == "YellowLight") {
			Debug.Log ("Yellow light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<YellowLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<YellowLight> ().lightOn [i] = state;
			}
		} else {
			Debug.Log ("No light script!");
		}
	}

	public void setPressureSwitch(){
		Debug.Log ("Set Pressure Switch");
		if (!state) {
			transform.position = originalPosition;
		} else {
			transform.position = originalPosition + new Vector3(0,-0.175f,0);
		}
	}
}
