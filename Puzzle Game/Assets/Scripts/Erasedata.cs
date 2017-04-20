using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Erasedata : MonoBehaviour {
	

	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}


	void TaskOnClick(){
		PlayerPrefs.DeleteAll();
		Application.LoadLevel (Application.loadedLevel);
	}
}
