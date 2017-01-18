using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quitbtn : MonoBehaviour {
	public GameObject quitPopup;
	public GameObject quitPopupButtonYes;
	public GameObject quitPopupButtonNo;

	public bool quitPopupIsOpened;

	void Start () {
		quitPopupIsOpened = false;
		quitPopup.SetActive (false);
		quitPopupButtonYes.SetActive (false);
		quitPopupButtonNo.SetActive (false);
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		setQuitpopup (true);
		quitPopup.SetActive (true);
		quitPopupButtonYes.SetActive (true);
		quitPopupButtonNo.SetActive (true);
	}

	public bool quitpopupOpened(){
		return quitPopupIsOpened;
	}

	public void setQuitpopup(bool v){
		quitPopupIsOpened = v;
	}
}
