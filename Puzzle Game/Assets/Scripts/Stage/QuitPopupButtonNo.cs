using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitPopupButtonNo : MonoBehaviour {
	public GameObject quitPopup;
	public GameObject quitPopupButtonYes;
	public GameObject quitPopupButtonNo;
	public GameObject quitButton;

	void Start () {
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick(){
		Quitbtn q = quitButton.GetComponent<Quitbtn> ();
		q.setQuitpopup (false);
		quitPopup.SetActive (false);
		quitPopupButtonYes.SetActive (false);
		quitPopupButtonNo.SetActive (false);
	}
}
