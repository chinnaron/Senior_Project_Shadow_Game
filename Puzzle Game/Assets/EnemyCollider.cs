using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {
	private PlayerController player;
	private Animator enemyAnimator;
	void Awake(){
		player = FindObjectOfType<PlayerController> ();
		enemyAnimator = gameObject.GetComponentInParent<Animator> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.name == "Player2") {
			gameObject.GetComponentInParent<EnemyController> ().Punch ();
//			print (true);
			enemyAnimator.SetBool ("IsPunch",true);
			player.YouDied ();
		}
	}

}
