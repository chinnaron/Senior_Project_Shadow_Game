using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLight : MonoBehaviour {
	public GameObject pic;
	public float rayDistanceDefault = 6f;
	public LineRenderer[] line = new LineRenderer[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	private PlayerController player;
	private GridOverlay grid;

	private float distance;
	private List<Vector3> positions = new List<Vector3> ();
	private Vector3 old;
	private Vector3 reflect;
	private int[] pushDirection = new int[4];
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };  
	private float[] rayDistance = new float[4];
	private PushController[] obj = new PushController[4];
	private RaycastHit[] hit = new RaycastHit[4];
	private RaycastHit[] hit2 = new RaycastHit[4];
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

				if (Physics.Raycast (transform.position, wayP [i], out hit [i], rayDistanceDefault - 1f)) {
					rayDistance [i] = rayDistanceDefault;
					distance = rayDistanceDefault;
					positions.Clear ();
					positions.Add (Vector3.zero);
					old = transform.position;
					reflect = wayP [i];

					if (i % 2 == 0) {
						positions.Add (wayP [i] * (Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z) - 1f));

						if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit2 [i], distance - Mathf.Abs (hit [i].collider.transform.position.z - old.z)))
							rayDistance [i] = Mathf.Abs (hit2 [i].collider.transform.position.z - old.z) - 1f;
						else
							rayDistance [i] = distance;
					} else {
						positions.Add (wayP [i] * (Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x) - 1f));

						if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit2 [i], distance - Mathf.Abs (hit [i].collider.transform.position.x - old.x))) {
							rayDistance [i] = Mathf.Abs (hit2 [i].collider.transform.position.x - old.x) - 1f;
						} else
							rayDistance [i] = distance;
					}

					while (hit [i].collider.GetComponent<ObjectController> ().isMirror) {
						reflect = hit [i].collider.GetComponent<Mirror> ().Reflect ((hit [i].collider.transform.position - old).normalized);

						if (reflect != Vector3.zero) {
							old = hit [i].collider.transform.position;

							if (reflect.z != 0) {
								distance = distance - Mathf.Abs (positions [positions.Count - 1].x - positions [positions.Count - 2].x);
								if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit [i], distance)) {
									positions.Add (reflect * (Mathf.Abs (hit [i].collider.transform.position.z - old.z) - 1f) + positions [positions.Count - 1]);

									if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit2 [i], distance - Mathf.Abs (hit [i].collider.transform.position.z - old.z)))
										rayDistance [i] = Mathf.Abs (hit2 [i].collider.transform.position.z - old.z) - 1f;
									else
										rayDistance [i] = distance;
								} else {
									positions.Add (reflect * distance + positions [positions.Count - 1]);
									break;
								}
							} else {
								distance = distance - Mathf.Abs (positions [positions.Count - 1].z - positions [positions.Count - 2].z);
								if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit [i], distance)) {
									positions.Add (reflect * (Mathf.Abs (hit [i].collider.transform.position.x - old.x) - 1f) + positions [positions.Count - 1]);

									if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit2 [i], distance - Mathf.Abs (hit [i].collider.transform.position.x - old.x)))
										rayDistance [i] = Mathf.Abs (hit2 [i].collider.transform.position.x - old.x) - 1f;
									else
										rayDistance [i] = distance;
								} else {
									positions.Add (reflect * distance + positions [positions.Count - 1]);
									break;
								}
							}
						} else
							break;
					}

					if (reflect == Vector3.forward)
						pushDirection [i] = 0;
					else if (reflect == Vector3.right)
						pushDirection [i] = 1;
					else if (reflect == Vector3.back)
						pushDirection [i] = 2;
					else if (reflect == Vector3.left)
						pushDirection [i] = 3;

					line [i].numPositions = positions.Count;
					Vector3[] positionsFinal = new Vector3[positions.Count];

					for (int j = 0; j < positions.Count; j++) {
						positionsFinal [j] = positions [j];
					}

					line [i].SetPositions (positionsFinal);

					if (hit [i].collider != null && hit [i].collider.GetComponent<ObjectController> ().isPushable
					    && (((reflect.z != 0) && hit [i].collider.transform.position.x < old.x + 0.1f
					    && hit [i].collider.transform.position.x > old.x - 0.1f
					    && Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).z - grid.ToPoint0Y (old).z) < rayDistance [i])
					    || ((reflect.x != 0) && hit [i].collider.transform.position.z < old.z + 0.1f
					    && hit [i].collider.transform.position.z > old.z - 0.1f
					    && Mathf.Abs (grid.ToPoint0Y (hit [i].collider.transform.position).x - grid.ToPoint0Y (old).x) < rayDistance [i]))) {
						obj [i] = hit [i].collider.GetComponent<PushController> ();

						if (!obj [i].moving && !obj [i].jumping && !obj [i].falling) {
							if (obj [i].gameObject == player.gameObject)
								player.Stop ();
							else if (obj [i] == player.GetGrabPush ()) {
								if (((reflect.z != 0) && grid.ToPoint0Y (player.transform.position).x == grid.ToPoint0Y (transform.position).x)
								    || ((reflect.x != 0) && grid.ToPoint0Y (player.transform.position).z == grid.ToPoint0Y (transform.position).z)) {
									player.Stop ();
									player.SetPushController (old + wayP [pushDirection [i]] * (rayDistance [i] + 1), wayP [pushDirection [i]]);
								} else {
									player.GrabRelease ();
								}
							}

							obj [i].SetMoveTo (old + wayP [pushDirection [i]] * (rayDistance [i]), wayP [pushDirection [i]]);
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