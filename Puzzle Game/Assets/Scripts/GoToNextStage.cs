using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToNextStage : MonoBehaviour {
	private string name;
	private int nextStage;
	private bool can;

	void Awake(){
		name = Application.loadedLevelName;
		can = int.TryParse (name.Substring (5, 1), out nextStage);
		nextStage += 1;

		if(!can)
			print ("Error");
	}

	void OnTriggerEnter(Collider other){
		SceneManager.LoadScene("Scene" + nextStage);
	}
}