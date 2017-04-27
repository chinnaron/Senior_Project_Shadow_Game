using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeverController : MonoBehaviour {

	public GameObject lightBox;
	public GameObject leverHand;
	public AudioClip[] sound;
	private AudioSource[] sounds;
	public bool state;
	public float tiltAngle = 20.0F;
	//public float smooth = 0.1f;

	// Use this for initialization
	void Start () {
		setInitialLeverHand ();
        setInitialLight();
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
		PlaySound (0, 0);
		state = !state;
		//		Debug.Log ("Lever state changed from "+!state+" to "+state);
		setLeverhand ();
		setLight ();
	}

	public void setLight(){
		if (state) {
			PlaySound (1,1);
			setLightOn ();
		}else if(!state){
			PlaySound (1,2);
			setLightOff();
		}
	}

    public void setInitialLight() {
        if (state) {
            setLightOn();
        }
        else if (!state) {
            setLightOff();
        }
    }

    public void setLightOn(){
		string type = getType ();
		if (type == "WhiteLight") {
			//			Debug.Log ("White light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<WhiteLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<WhiteLight> ().lightOn [i] = true;
			}
		} else if (type == "RedLight") {
			//			Debug.Log ("Red light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<RedLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<RedLight> ().lightOn [i] = true;
			}
		} else if (type == "BlueLight") {
			//			Debug.Log ("Blue light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<BlueLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<BlueLight> ().lightOn [i] = true;
			}
		} else if (type == "YellowLight") {
			//			Debug.Log ("Yellow light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<YellowLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<YellowLight> ().lightOn [i] = true;
			}
		} else {
			//			Debug.Log ("No light script!");
		}
	}

	public void setLightOff(){
		string type = getType ();
		if (type == "WhiteLight") {
			//			Debug.Log ("White light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<WhiteLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<WhiteLight> ().lightOn [i] = false;
			}
		} else if (type == "RedLight") {
			//			Debug.Log ("Red light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<RedLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<RedLight> ().lightOn [i] = false;
			}
		} else if (type == "BlueLight") {
			//			Debug.Log ("Blue light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<BlueLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<BlueLight> ().lightOn [i] = false;
			}
		} else if (type == "YellowLight") {
			//			Debug.Log ("Yellow light on = " + state);
			for (int i = 0; i < 4; i++) {
				if (lightBox.GetComponent<YellowLight> ().LightTriggerDirection [i])
					lightBox.GetComponent<YellowLight> ().lightOn [i] = false;
			}
		} else {
			//			Debug.Log ("No light script!");
		}
	}

	public void setLeverhand(){
		//		Debug.Log ("Set Lever Hand");
		if (!state) {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, tiltAngle*2);
		} else {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, -tiltAngle*2);
		}
	}

	public void setInitialLeverHand(){
		//		Debug.Log ("Set Initial Lever Hand");
		if (!state) {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, tiltAngle);
		} else {
			leverHand.transform.RotateAround(transform.position, Vector3.forward, -tiltAngle);
		}
	}
	public void PlaySound(int n, int s){
		sounds = GetComponents<AudioSource> ();
		sounds[n].clip = sound [s];
		sounds[n].Play ();
	}
}