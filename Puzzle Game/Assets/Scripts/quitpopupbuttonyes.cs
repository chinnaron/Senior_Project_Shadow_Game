using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class quitpopupbuttonyes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	// Update is called once per frame
	void TaskOnClick(){
		Application.Quit ();
		Debug.Log ("QUIT");
	}
}
