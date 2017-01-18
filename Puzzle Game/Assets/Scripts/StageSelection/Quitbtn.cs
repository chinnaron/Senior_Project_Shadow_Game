using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quitbtn : MonoBehaviour {

	// Use this for initialization
	public GameObject quitpopup;
	public GameObject quitpopupbuttonyes;
	public GameObject quitpopupbuttonno;

	public bool quitpopupIsOpened;
	void Start () {
		quitpopupIsOpened = false;
		quitpopup.SetActive (false);
		quitpopupbuttonyes.SetActive (false);
		quitpopupbuttonno.SetActive (false);
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}
	
	// Update is called once per frame
	void TaskOnClick(){
		
	
		setQuitpopup (true);
		quitpopup.SetActive (true);
		quitpopupbuttonyes.SetActive (true);
		quitpopupbuttonno.SetActive (true);
	}

	public bool quitpopupOpened(){
		return quitpopupIsOpened;
	}

	public void setQuitpopup(bool v){
		quitpopupIsOpened = v;
	}
}
