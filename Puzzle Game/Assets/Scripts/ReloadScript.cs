using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}


	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		Application.LoadLevel (Application.loadedLevel);
	}
	// Update is called once per frame

}
