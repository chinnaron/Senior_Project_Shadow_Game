using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GoToNextStage : MonoBehaviour {
	private string name;
	private int nextStage;
	private bool can;

	void Awake(){
		name = Application.loadedLevelName;
		can = int.TryParse (name.Substring (name.Length - 1, 1), out nextStage);
		nextStage += 1;

		if(!can)
			print ("Error");
	}

	void OnTriggerEnter(Collider other){
		if (Application.CanStreamedLevelBeLoaded ("Scene" + nextStage))
			SceneManager.LoadScene ("Scene" + nextStage);
		else
			SceneManager.LoadScene ("StageSelection");
	}
}