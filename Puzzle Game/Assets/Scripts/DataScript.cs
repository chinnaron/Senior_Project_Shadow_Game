using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataScript : MonoBehaviour {
	
	public bool isCleared(int stage){
		bool isClear = false;
		if(PlayerPrefs.HasKey("Stage "+stage))
		{
			if(PlayerPrefs.GetInt("Stage "+stage)==1)
				isClear = true;
			else if(PlayerPrefs.GetInt("Stage "+stage)==0)
				isClear = false;
		}
		return isClear;
	}

	public void setClear(int stage,bool status){
		
		

		if(status)PlayerPrefs.SetInt(("Stage "+stage),1);
		else PlayerPrefs.SetInt(("Stage "+stage),0);
		
	}
	
}
