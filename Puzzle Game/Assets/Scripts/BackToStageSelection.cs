using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToStageSelection : MonoBehaviour {

	// Use this for initialization

	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	// Update is called once per frame
	void TaskOnClick(){


		SceneManager.LoadScene("StageSelection");
	}
}
