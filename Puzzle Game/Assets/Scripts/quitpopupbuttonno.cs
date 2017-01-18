using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class quitpopupbuttonno : MonoBehaviour {

	// Use this for initialization
	public GameObject quitpopup;
	public GameObject quitpopupbuttonyes;
	public GameObject quitpopupbuttonNo;
	public GameObject quitbutton;

	void Start () {
		
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

	}

	// Update is called once per frame
	void TaskOnClick(){
		
		Quitbtn q = quitbutton.GetComponent<Quitbtn> ();

		q.setQuitpopup (false);
		quitpopup.SetActive (false);
		quitpopupbuttonyes.SetActive (false);
		quitpopupbuttonNo.SetActive (false);
	}
}
