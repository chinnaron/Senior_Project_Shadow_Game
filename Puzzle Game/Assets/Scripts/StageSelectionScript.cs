using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectionScript : MonoBehaviour {

	public GameObject Stage2;
	public GameObject Stage3;
	public GameObject Stage4;
	public GameObject Stage5;
	public GameObject Stage6;
	public GameObject Stage7;
	public GameObject Stage8;
	public GameObject Stage9;
	

	void Start(){
		
		

		if(PlayerPrefs.GetInt("Stage 1")==1)
			Stage2.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 2")==1)
			Stage3.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 3")==1)
			Stage4.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 4")==1)
			Stage5.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 5")==1)
			Stage6.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 6")==1)
			Stage7.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 7")==1)
			Stage8.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 8")==1)
			Stage9.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 9")==1);
			//Stage10.SetActive(true);	
	}		
	
	void awake(){
		
		if(PlayerPrefs.GetInt("Stage 1")==1)
			Stage2.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 2")==1)
			Stage3.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 3")==1)
			Stage4.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 4")==1)
			Stage5.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 5")==1)
			Stage6.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 6")==1)
			Stage7.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 7")==1)
			Stage8.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 8")==1)
			Stage9.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 9")==1);
			//Stage10.SetActive(true);
		

	}	
}
