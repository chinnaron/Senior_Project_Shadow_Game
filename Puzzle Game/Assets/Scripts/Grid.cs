using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public int width = 20;
	public int height = 20;

	private GameObject[,] grid;

	void Awake (){
		grid = new GameObject[width, height];
		for (int x = 0; x < width; x++) {
			for (int z = 0; z < width; z++) {
				
			}
		}
	}
}
