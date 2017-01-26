using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLight : MonoBehaviour {
	public GameObject pic;
	public LineRenderer[] line = new LineRenderer[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	private PlayerController player;
	private GridOverlay grid;
	private readonly float rayDistanceDefault = 6f;

	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };  
	private float[] rayDistance = new float[4];
	private float[] longL = { 0, 0, 0, 0 };
	private PushController[] obj = new PushController[4];
	private RaycastHit[] hit = new RaycastHit[4];
	private RaycastHit[] list;
	private GameObject[] onPic = new GameObject[4];
	private bool[] hitWall = new bool[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();

		for (int i = 0; i < 4; i++) {
			rayDistance [i] = rayDistanceDefault;
			if (lightOn [i]) {
				line [i].SetPosition (line [i].numPositions - 1, wayP [i] * (rayDistance [i] - 1f));
			}
		}
	}

	void Update ()	{
		for (int i = 0; i < 4; i++) {
			if (lightOn [i]) {
				if (onPic [i] == null)
					onPic [i] = Instantiate (pic, grid.Set0Y (transform.position) + wayP [i] * 0.3f, Quaternion.LookRotation (wayP [i]), transform);

				if (Physics.Raycast (transform.position, wayP [i], out hit [i], rayDistanceDefault)) {
					list = Physics.RaycastAll (transform.position, wayP [i], rayDistanceDefault);

					if (list.Length < 2)
						rayDistance [i] = rayDistanceDefault;
					else {
						if (i % 2 == 0)
							rayDistance [i] = Mathf.Abs (grid.ToPoint0Y (list [1].collider.transform.position).z - grid.ToPoint0Y (transform.position).z) - 1f;
						else
							rayDistance [i] = Mathf.Abs (grid.ToPoint0Y (list [1].collider.transform.position).x - grid.ToPoint0Y (transform.position).x) - 1f;
					}

					if (i % 2 == 0)
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z) - 1f;
					else
						longL [i] = Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x) - 1f;

					if (longL [i] > rayDistanceDefault - 1)
						longL [i] = rayDistanceDefault - 1;
					else if (longL [i] < 0)
						longL [i] = 0;
					
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * longL [i]);

					if (hit [i].collider.GetComponent<ObjectController> ().isPushable
					    && ((i % 2 == 0 && hit [i].collider.transform.position.x < transform.position.x + 0.1f
					    && hit [i].collider.transform.position.x > transform.position.x - 0.1f
					    && Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).z - grid.ToPoint0Y (transform.position).z) < rayDistance [i])
					    || (i % 2 == 1 && hit [i].collider.transform.position.z < transform.position.z + 0.1f
					    && hit [i].collider.transform.position.z > transform.position.z - 0.1f
					    && Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).x - grid.ToPoint0Y (transform.position).x) < rayDistance [i]))) {
						obj [i] = hit [i].collider.GetComponent<PushController> ();

						if (!obj [i].moving) {
							if (obj [i].gameObject == player.gameObject)
								player.Stop ();
							else if (obj [i] == player.GetGrabPush ()) {
								if ((i % 2 == 0 && grid.ToPoint0Y (player.transform.position).x == grid.ToPoint0Y (transform.position).x)
									|| (i % 2 == 1 && grid.ToPoint0Y (player.transform.position).z == grid.ToPoint0Y (transform.position).z)) {
									player.Stop ();
									player.SetPushController (transform.position + wayP [i] * (rayDistance [i] + 1), wayP [i]);
								} else {
									player.GrabRelease ();
								}
							}
							
							obj [i].SetMoveTo (transform.position + wayP [i] * (rayDistance [i]), wayP [i]);
						}
					}
				} else {
					rayDistance [i] = rayDistanceDefault;
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * (rayDistance [i] - 1));
				}
			} else {
				if (onPic [i] != null)
					Destroy (onPic [i]);
				rayDistance [i] = rayDistanceDefault;
				line [i].SetPosition (line [i].numPositions - 1, Vector3.zero);
			}
		}
	}
}