using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	public GameObject menu;
	public GameObject pauseCloseButton;
	public bool _isPaused;

	void Start () {
		menu.SetActive (false);
		pauseCloseButton.SetActive (false);
		_isPaused = false;
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick(){
		//if (!_isOpened) {
		//Debug.Log ("You have clicked the button!");
		Time.timeScale = 0;
		menu.SetActive (true);
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
