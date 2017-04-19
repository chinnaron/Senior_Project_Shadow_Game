using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	public GameObject menu;
	public GameObject blacksceen;
	public GameObject pauseCloseButton;
	public bool _isPaused;

	void Start () {
		menu.SetActive (false);
		blacksceen.SetActive (false);
		pauseCloseButton.SetActive (false);
		_isPaused = false;
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick(){
		
		Time.timeScale = 0;
		menu.SetActive (true);
		blacksceen.SetActive (true);
		pauseCloseButton.SetActive (true);
		_isPaused = true;
	}

	public void SetPause(bool value){
		_isPaused = value;
	}

	public bool isPaused(){
		return _isPaused;
	}
}
