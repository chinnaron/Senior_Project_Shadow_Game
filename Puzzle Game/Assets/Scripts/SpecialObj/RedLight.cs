using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLight : MonoBehaviour {
	public GameObject pic;
	public readonly float rayDistance = 5f;
	public LineRenderer[] line = new LineRenderer[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };

	private GridOverlay grid;
	private PlayerController player;

	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left }; 
	private float[] longL = { 0, 0, 0, 0 };
	private RaycastHit[] hit = new RaycastHit[4];
	private GameObject[] onPic = new GameObject[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();

		for (int i = 0; i < 4; i++) {
			if (lightOn [i])
				line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);
		}
	}

	void Update ()	{
		for (int i = 0; i < 4; i++) {
			if (lightOn [i]) {
				if (onPic [i] == null)
					onPic [i] = Instantiate (pic, grid.Set0Y (transform.position) + wayP [i] * 0.3f, Quaternion.LookRotation (wayP [i]), transform);

				if (Physics.Raycast (transform.position, wayP [i], out hit [i], rayDistance)) {
					if (i % 2 == 0)
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z);
					else
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x);

					if (hit [i].collider.GetComponent<ObjectController> ().isWall)
						longL [i] -= 1f;
					
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * longL [i]);

					if (hit [i].collider.GetComponent<ObjectController> ().isDestroyable
					    && (i % 2 == 0 && (hit [i].collider.transform.position.x < transform.position.x + 0.1f
					    && hit [i].collider.transform.position.x > transform.position.x - 0.1f)
					    || i % 2 == 1 && (hit [i].collider.transform.position.z < transform.position.z + 0.1f
					    && hit [i].collider.transform.position.z > transform.position.z - 0.1f))) {
						if (hit [i].collider.GetComponent<PushController> () == player.GetGrabPush ()) {
							Destroy (hit [i].collider.gameObject, Time.deltaTime * 2f);
						} else if (hit [i].collider.gameObject == player.gameObject)
							player.GrabRelease ();
						Destroy (hit [i].collider.gameObject, Time.deltaTime * 2f);
						//create particle
					} else {
						//create particle
					}
				} else {
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);
				}
			} else {
				if (onPic [i] != null)
					Destroy (onPic [i]);
				line [i].SetPosition (line [i].numPositions - 1, Vector3.zero);
			}
		}
	}
}
