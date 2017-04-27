using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueNextStage : MonoBehaviour {

	public bool can;
	private string name;
	private int nextStage;
	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);

		name = Application.loadedLevelName;

			
		can = int.TryParse (name.Substring (name.Length - 1, 1), out nextStage);

		if (name.Length == 7)
			nextStage += 10;
		nextStage += 1;
		if (!can)
			Debug.Log ("error");
	}
	
	// Update is called once per frame
	void TaskOnClick(){
		if (Application.CanStreamedLevelBeLoaded ("Scene" + nextStage)) {	
			Time.timeScale = 1;
			SceneManager.LoadScene ("Scene" + nextStage);
		} else {
			Time.timeScale = 1;
			SceneManager.LoadScene ("StageSelection");
		}

	}
}
