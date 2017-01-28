using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToStage2 : MonoBehaviour {

	//When player stand NEXT to collider box
	void OnTriggerEnter(Collider other){
		Debug.Log ("Goal Collided");
		SceneManager.LoadScene("Scene2");
	}
}