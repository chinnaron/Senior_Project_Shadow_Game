using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {
	public GameObject menupopup;
	public GameObject playbtn;
	public GameObject menubutton;
	public GameObject blackscreen;


	void Awake () {
		//_isOpened = false;
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick(){
		
		MenuScript a = menubutton.GetComponent<MenuScript> ();
		playbtn.SetActive (false);
		menupopup.SetActive (false);
		blackscreen.SetActive (false);
		Time.timeScale = 1;
		a.SetPause (false);
		//_isOpened = !_isOpened;
	}
}
