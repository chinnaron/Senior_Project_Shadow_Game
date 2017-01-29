using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeverController : MonoBehaviour {

	public GameObject lightBox;
	public GameObject leverHand;
	public bool state;
	public float tiltAngle = 20.0F;
	//public float smooth = 0.1f;

	// Use this for initialization
	void Start () {
		setLight ();
		setInitialLeverHand ();
	}
	
	// Update is called once per frame
	void Update () {
		
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
		Debug.Log ("Lever state changed from "+!state+" to "+state);
		setLeverhand ();
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

	public void setLeverhand(){
		Debug.Log ("Set Lever Hand");
		if (!state) {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, tiltAngle*2);
		} else {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, -tiltAngle*2);
		}
	}

	public void setInitialLeverHand(){
		Debug.Log ("Set Initial Lever Hand");
		if (!state) {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, tiltAngle);
		} else {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, -tiltAngle);
		}
	}
}
