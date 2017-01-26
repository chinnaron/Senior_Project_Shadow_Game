using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {
	//public GameObject showPic;

	private bool isOn;

	private GameObject show;

	void Awake () {
		isOn = false;
	}

//	public void ShowOn () {
//		if (isOn) {
//			if (show == null)
//				show = Instantiate (showPic, transform.position, Quaternion.LookRotation (Vector3.forward), gameObject.transform);
//		} else {
//			Destroy (show);
//		}
//	}

	public void SetOnTrue(){
		isOn = true;
	}

	public void SetOnFalse(){
		isOn = false;
	}

	public string getType(){
		ObjectController a = GetComponent<ObjectController> ();
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


	public void LightOn(){
		string type = getType ();
	//print(type);
		if (type == "WhiteLight")
			WhiteLightOn ();
		else if (type == "RedLight")
			RedLightOn ();
		else if (type == "BlueLight")
			BlueLightOn ();
		else if (type == "YellowLight")
			YellowLightOn ();
	}

	public void LightOff(){
		string type = getType ();
		if (type == "WhiteLight")
			WhiteLightOff();
		else if (type == "RedLight")
			RedLightOff ();
		else if (type == "BlueLight")
			BlueLightOff ();
		else if (type == "YellowLight")
			YellowLightOff ();
	}


	public void WhiteLightOn(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<WhiteLight> ().LightTriggerDirection [i])
				GetComponent<WhiteLight> ().lightOn [i] = true;
		}
	}

	public void WhiteLightOff(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<WhiteLight> ().LightTriggerDirection [i])
				GetComponent<WhiteLight> ().lightOn [i] = false;
		}
	}

	public void BlueLightOn(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<BlueLight> ().LightTriggerDirection [i])
				GetComponent<BlueLight> ().lightOn [i] = true;
		}
	}

	public void BlueLightOff(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<BlueLight> ().LightTriggerDirection [i])
				GetComponent<BlueLight> ().lightOn [i] = false;
		}
	}

	public void RedLightOn(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<RedLight> ().LightTriggerDirection [i])
				GetComponent<RedLight> ().lightOn [i] = true;
		}
	}

	public void RedLightOff(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<RedLight> ().LightTriggerDirection [i])
				GetComponent<RedLight> ().lightOn [i] = false;
		}
	}

	public void YellowLightOn(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<YellowLight> ().LightTriggerDirection [i])
				GetComponent<YellowLight> ().lightOn [i] = true;
		}
	}

	public void YellowLightOff(){
		for (int i = 0; i < 4; i++) {
			if (GetComponent<YellowLight> ().LightTriggerDirection [i])
				GetComponent<YellowLight> ().lightOn [i] = false;
		}
	}



}
