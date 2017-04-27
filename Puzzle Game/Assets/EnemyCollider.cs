using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {
	PlayerController player;

	void Awake(){
		player = FindObjectOfType<PlayerController> ();
	}

	void OnTriggerEnter(Collider other){
//		print (other.name);
		if (other.name == "Player2") {
			gameObject.GetComponentInParent<EnemyController> ().Stop ();
			player.YouDied ();
		}
	}

}
