using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	//When player stand NEXT to collider box
	void OnTriggerEnter(Collider other){
		Debug.Log ("Goal Collided");
		Application.LoadLevel (Application.loadedLevel);
	}
}