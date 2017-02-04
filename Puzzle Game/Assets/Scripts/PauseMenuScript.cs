using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {
	public GameObject menupopup;
	public GameObject pausebtn;
	public GameObject menubutton;

	void Awake () {
		//_isOpened = false;
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick(){
		print ("sth");
		MenuScript a = menubutton.GetComponent<MenuScript> ();
		pausebtn.SetActive (false);
		menupopup.SetActive (false);
		Time.timeScale = 1;
		a.SetPause (false);
		//_isOpened = !_isOpened;
	}
}
