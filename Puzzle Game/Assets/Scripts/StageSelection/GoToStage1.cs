using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GoToStage1 : MonoBehaviour {

	// Use this for initialization
	public GameObject quitbutton;
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void TaskOnClick(){
		Quitbtn q = quitbutton.GetComponent<Quitbtn> ();
		if(!q.quitpopupOpened())
		SceneManager.LoadScene("Scene");
	}
}
