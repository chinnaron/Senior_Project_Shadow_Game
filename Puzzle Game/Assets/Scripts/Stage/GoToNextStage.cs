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
	private DataScript datascript;
	public AudioClip[] sound;
	public GameObject Stageclear;
	public GameObject blackscreen;
	private MenuScript menu;

	void Awake(){
		datascript = GetComponent<DataScript> ();
		name = Application.loadedLevelName;
		can = int.TryParse (name.Substring (name.Length - 1, 1), out nextStage);
		if(name.Length==7)nextStage+=10;
		nextStage += 1;
		menu = FindObjectOfType<MenuScript> ();

		if(!can)
			print ("Error");
	}

	IEnumerator OnTriggerEnter(Collider other){
		if (other.GetComponent<ObjectController>().isPlayer) {
			menu.SetPause (true);
			PlaySound (0);
			yield return new WaitForSeconds (0.8f);

			menu.SetPause (true);
	
			datascript.setClear (nextStage - 1, true);
			
			blackscreen.SetActive (true);
			Stageclear.SetActive (true);


		}
	}

	public void PlaySound(int s){
		GetComponent<AudioSource>().clip = sound [s];
		GetComponent<AudioSource>().Play ();
	}
}