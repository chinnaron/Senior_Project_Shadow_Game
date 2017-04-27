using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScript : MonoBehaviour {
	public GameObject menu;

	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		MenuScript a = menu.GetComponent<MenuScript>();
		a.SetPause(false);
		Time.timeScale = 1;
		Application.LoadLevel (Application.loadedLevel);
	}
}
