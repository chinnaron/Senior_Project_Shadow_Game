using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToRotate : MonoBehaviour {
	private PlayerController player;
	// Use this for initialization
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;

	private Quaternion lookAt;
	private Quaternion start;

	private readonly float turnSpeed = 10f;
	private readonly float minSwipeLength = 200f;

	private static Swipe swipeDirection;
	 
	public GameObject bg;
	public GameObject camera;
	
	private enum Swipe { None, Up, Down, Left, Right };

	private bool moving;

	void Start () {
		player = FindObjectOfType<PlayerController> ();
		lookAt = start = camera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		DetectSwipe();
	}

	void FixedUpdate () {
		camera.transform.rotation = Quaternion.Lerp (start, lookAt, Time.deltaTime * turnSpeed);
		bg.transform.rotation = Quaternion.Lerp (start, lookAt, Time.deltaTime * turnSpeed);

		if (camera.transform.rotation == lookAt)
			moving = false;
	}

	void  DetectSwipe(){
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
					player.Click ();;
					return;
				}
           
				currentSwipe.Normalize ();
				if (!moving) {
					// Swipe up
					if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
						swipeDirection = Swipe.Up;
						// Swipe down
					} else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
						swipeDirection = Swipe.Down;
						// Swipe left
					} else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
						swipeDirection = Swipe.Left;
						moving = true;
						lookAt = Quaternion.Euler (camera.transform.rotation.eulerAngles + Vector3.up * -90);
						start = transform.rotation;
//					camera.transform.Rotate (new Vector3 (0, -90, 0));
//					bg.transform.Rotate (new Vector3 (0, -90, 0));
						// Swipe right
					} else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
						swipeDirection = Swipe.Right;
						moving = true;
						lookAt = Quaternion.Euler (camera.transform.rotation.eulerAngles + Vector3.up * 90);
						start = transform.rotation;
//					camera.transform.Rotate (new Vector3 (0, 90, 0));
//					bg.transform.Rotate (new Vector3 (0, 90, 0));
					}
				}
			}
		} else {
			swipeDirection = Swipe.None;
		}
	}
}
