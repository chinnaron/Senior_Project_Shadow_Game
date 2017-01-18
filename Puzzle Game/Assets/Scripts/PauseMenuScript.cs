using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

	// Use this for initialization
	public GameObject menupopup;
	public GameObject pausebtn;
	public GameObject menubutton;



	//public gameObject pausebutton;

	void Start () {



		//_isOpened = false;
	
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		
	}
	
	// Update is called once per frame
	void TaskOnClick(){

			MenuScript a = menubutton.GetComponent<MenuScript>();
			
			pausebtn.SetActive(false);
			menupopup.SetActive (false);
			a.SetPause(false);
			Time.timeScale = 1;
			
			//_isOpened = !_isOpened;
		   

	}
}
