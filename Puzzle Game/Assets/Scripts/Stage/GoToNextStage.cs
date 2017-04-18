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
	public AudioClip[] sound;
	public DataScript datascript;

	void Awake(){
		name = Application.loadedLevelName;
		can = int.TryParse (name.Substring (name.Length - 1, 1), out nextStage);
		nextStage += 1;

		if(!can)
			print ("Error");
	}

	IEnumerator OnTriggerEnter(Collider other){
		PlaySound (0);
		yield return new WaitForSeconds (0.8f);

		if (Application.CanStreamedLevelBeLoaded ("Scene" + nextStage))
		{
			datascript.setClear(nextStage-1,true);
			//show Stage Clear popup
			
			SceneManager.LoadScene ("Scene" + nextStage);	
		}		
		else
			SceneManager.LoadScene ("StageSelection");
	}
	public void PlaySound(int s){
		//GetComponent<AudioSource>().clip = sound [s];
		//GetComponent<AudioSource>().Play ();
	}
}