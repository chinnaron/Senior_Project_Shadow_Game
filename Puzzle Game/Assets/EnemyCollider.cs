using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {

	void OnTriggerEnter(Collider other){
//		print (other.name);
		if(other.name == "Player2")
			Application.LoadLevel (Application.loadedLevel);
	}

}
