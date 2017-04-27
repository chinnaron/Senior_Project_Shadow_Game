using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {
	PlayerController player;
	Animator enemyAnimator;
	public GameObject Enemy;
	void Awake(){
		player = FindObjectOfType<PlayerController> ();
		enemyAnimator = Enemy.GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other){


		if (other.name == "Player2") {
			print (true);
			enemyAnimator.SetBool ("IsPunch",true);
			gameObject.GetComponentInParent<EnemyController> ().Stop ();
			player.YouDied ();
		}
	}

}
