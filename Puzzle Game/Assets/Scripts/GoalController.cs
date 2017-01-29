using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

	//When player stand NEXT to collider box
	void OnTriggerEnter(Collider other){
<<<<<<< HEAD
//		Debug.Log ("Goal Collided");
		Application.LoadLevel (Application.loadedLevel);
=======
		Debug.Log ("Goal Collided");
		//Application.LoadLevel (Application.loadedLevel);
>>>>>>> origin/master
	}
}