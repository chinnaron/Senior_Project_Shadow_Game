using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectionGoToStage : MonoBehaviour {
//	public GameObject quitButton;
	private string name;
	private int stage;
	private bool can;

	void Start () {
		Button btn = GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);

		name = gameObject.name;
		can = int.TryParse (name.Substring (name.Length - 1, 1), out stage);

		if(!can)
			print ("Error");
	}

	void TaskOnClick(){
		SceneManager.LoadScene ("Scene" + stage);
	}
}
