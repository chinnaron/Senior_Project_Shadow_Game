using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GoToNextStage : MonoBehaviour {
	public bool isOpen;
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
		//check if it's player
		if (other.gameObject.GetComponent<ObjectController> ().isPlayer) {
			//check if goal is open and player is sill alive (in case player get hit by red light while entering goal)
			if (isOpen && other.gameObject.active) {
				if (Application.CanStreamedLevelBeLoaded ("Scene" + nextStage)) {
					SceneManager.LoadScene ("Scene" + nextStage);
				} else {
					SceneManager.LoadScene ("StageSelection");
				}
			}
		}
	}

	public void setOpen(bool status){
		isOpen = status;
	}
}