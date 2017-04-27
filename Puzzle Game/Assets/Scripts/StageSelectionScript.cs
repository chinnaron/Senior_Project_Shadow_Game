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
	public GameObject Stage10;
	public GameObject Stage11;
	public GameObject Stage12;
	public GameObject Stage13;
	public GameObject Stage14;
	public GameObject Stage15;
	public GameObject Stage16;
	public GameObject Stage17;
	public GameObject Stage18;


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
			Stage10.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 10")==1)
			Stage11.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 11")==1)
    		Stage12.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 12")==1)
			Stage13.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 13")==1)
			Stage14.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 14")==1)
			Stage15.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 15")==1)
			Stage16.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 16")==1)
			Stage17.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 17")==1)
			Stage18.SetActive(true);
		
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
		if(PlayerPrefs.GetInt("Stage 9")==1)
	    	Stage10.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 10")==1)
		    Stage11.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 11")==1)
		    Stage12.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 12")==1)
		    Stage13.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 13")==1)
		    Stage14.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 14")==1)
		    Stage15.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 15")==1)
		    Stage16.SetActive(true);
		if(PlayerPrefs.GetInt("Stage 16")==1)
		    Stage17.SetActive(true);	
		if(PlayerPrefs.GetInt("Stage 17")==1)
		    Stage18.SetActive(true);
		

	}	
}
