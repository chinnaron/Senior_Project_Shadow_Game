using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	// Use this for initialization

	public bool _isOpened;
	void Start () {

		_isOpened = false;
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}


	void TaskOnClick(){

		if (!_isOpened) {
			Debug.Log ("You have clicked the button!");
			Time.timeScale = 0;
			//OpenMenu()
		}
		else Time.timeScale = 1;
		
		_isOpened = !_isOpened;
			//CloseMenu()
		
		//Debug.Log ("You have clicked the button!");

	}
}
