﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToRotate : MonoBehaviour {
	private PlayerController player;
	// Use this for initialization
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;
	private Quaternion lookAt;
//	private Quaternion start;
	private Quaternion[] axises = new Quaternion[4];
	private readonly float turnSpeed = 10f;
	private readonly float minSwipeLength = 200f;
	private static Swipe swipeDirection;
	public GameObject camera;
	private enum Swipe { None, Up, Down, Left, Right };
	private int current;
	private bool moving;

	void Start () {
		#if UNITY_EDITOR
		gameObject.SetActive (false);
		#elif UNITY_ANDROID
		gameObject.SetActive(true);
		#endif

		player = FindObjectOfType<PlayerController> ();
		lookAt = axises [0] = camera.transform.rotation;
		current = 0;

		for (int i = 0; i < 3; i++)
			axises [i + 1] = Quaternion.Euler (axises [i].eulerAngles + Vector3.up * 90);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate () {
		DetectSwipe();
		camera.transform.rotation = Quaternion.Lerp (camera.transform.rotation, lookAt, Time.deltaTime * turnSpeed);

//		print (camera.transform.rotation + "" + lookAt);
//		print (camera.transform.rotation.eulerAngles + "" + lookAt.eulerAngles);
//		if (camera.transform.rotation.eulerAngles - lookAt.eulerAngles)
//			moving = false;
	}
//
//	Vector3 SetAxis(Quaternion q){
//		if (Mathf.Abs (q.eulerAngles.y - Mathf.Floor (q.eulerAngles.y)) > Mathf.Abs (q.eulerAngles.y - Mathf.Ceil (q.eulerAngles.y)))
//			return new Vector3 (q.eulerAngles.x, Mathf.Ceil (q.eulerAngles.y), q.eulerAngles.z);
//		else
//			return new Vector3 (q.eulerAngles.x, Mathf.Floor (q.eulerAngles.y), q.eulerAngles.z);
//	}
//
	void DetectSwipe(){
		if (Input.touches.Length > 0) {
			Touch t = Input.GetTouch (0);
 
			if (t.phase == TouchPhase.Began) {
				firstPressPos = new Vector2 (t.position.x, t.position.y);
			}

			if (t.phase == TouchPhase.Ended) {
				secondPressPos = new Vector2 (t.position.x, t.position.y);
				currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
           		
				// Make sure it was a legit swipe, not a tap
				if (currentSwipe.magnitude < minSwipeLength) {
					swipeDirection = Swipe.None;
					player.Click ();
					return;
				}
           
				currentSwipe.Normalize ();

				// Swipe up
				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					swipeDirection = Swipe.Up;
					// Swipe down
				} else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					swipeDirection = Swipe.Down;
					// Swipe left
				} else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					swipeDirection = Swipe.Left;
//						moving = true;
//					start = camera.transform.rotation;
					current = (current + 3) % 4;
					lookAt = axises [current];
//					camera.transform.Rotate (new Vector3 (0, -90, 0));
//					bg.transform.Rotate (new Vector3 (0, -90, 0));
					// Swipe right
				} else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					swipeDirection = Swipe.Right;
//						moving = true;
					//					start = camera.transform.rotation;
					current = (current + 1) % 4;
					lookAt = axises [current];
//					camera.transform.Rotate (new Vector3 (0, 90, 0));
//					bg.transform.Rotate (new Vector3 (0, 90, 0));
				}

			}
		} else {
			swipeDirection = Swipe.None;
		}
	}
}
