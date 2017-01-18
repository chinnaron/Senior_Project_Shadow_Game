using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScript : MonoBehaviour {
	
	// Use this for initialization

	public GameObject menu;

	void Start () {
		
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);



	}


	void TaskOnClick(){


		MenuScript a = menu.GetComponent<MenuScript>();
		if (a.isPaused()) {
		} else {
			//Debug.Log ("You have clicked the button!");
			Application.LoadLevel (Application.loadedLevel);
		}
	}
	// Update is called once per frame

}
