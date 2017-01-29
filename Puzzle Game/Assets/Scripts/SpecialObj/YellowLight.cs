using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowLight : MonoBehaviour {
	public GameObject pic;
	public readonly float rayDistance = 5f;
	public LineRenderer[] line = new LineRenderer[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	private GridOverlay grid;

	private float distance;
	private List<Vector3> positions = new List<Vector3> ();
	private Vector3 old;
	private Vector3 reflect;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private RaycastHit[] hit = new RaycastHit[4];
	private TriggerController[] obj = new TriggerController[4];
	private GameObject[] onPic = new GameObject[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();

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
					distance = rayDistance;
					positions.Clear ();
					positions.Add (Vector3.zero);
					old = transform.position;

					if (i % 2 == 0) {
						if (hit [i].collider.GetComponent<ObjectController> ().isWall)
							positions.Add (wayP [i] * (Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z) - 1f));
						else
							positions.Add (wayP [i] * Mathf.Abs (hit [i].collider.transform.position.z - transform.position.z));
					} else {
						if (hit [i].collider.GetComponent<ObjectController> ().isWall)
							positions.Add (wayP [i] * (Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x) - 1f));
						else
							positions.Add (wayP [i] * Mathf.Abs (hit [i].collider.transform.position.x - transform.position.x));
					}

					while (hit [i].collider.GetComponent<ObjectController> ().isMirror) {
						reflect = hit [i].collider.GetComponent<Mirror> ().Reflect ((hit [i].transform.position - old).normalized);

						if (reflect != Vector3.zero) {
							old = hit [i].transform.position;

							if (reflect.z != 0) {
								distance = distance - Mathf.Abs (positions [positions.Count - 1].x - positions [positions.Count - 2].x);
								if (Physics.Raycast (hit [i].collider.transform.position, reflect, out hit [i], distance)) {
									if (hit [i].collider.GetComponent<ObjectController> ().isWall)
										positions.Add (reflect * (Mathf.Abs (hit [i].collider.transform.position.z - old.z) - 1f) + positions [positions.Count - 1]);
									else
										positions.Add (reflect * Mathf.Abs (hit [i].collider.transform.position.z - old.z) + positions [positions.Count - 1]);
								} else {
									positions.Add (reflect * distance + positions [positions.Count - 1]);
									break;
								}
							} else {
								distance = distance - Mathf.Abs (positions [positions.Count - 1].z - positions [positions.Count - 2].z);
								if (Physics.Raycast (hit [i].collider.transform.position, reflect, out hit [i], distance)) {
									if (hit [i].collider.GetComponent<ObjectController> ().isWall)
										positions.Add (reflect * (Mathf.Abs (hit [i].collider.transform.position.x - old.x) - 1f) + positions [positions.Count - 1]);
									else
										positions.Add (reflect * Mathf.Abs (hit [i].collider.transform.position.x - old.x) + positions [positions.Count - 1]);
								} else {
									positions.Add (reflect * distance + positions [positions.Count - 1]);
									break;
								}
							}
						} else
							break;
					}

					line [i].numPositions = positions.Count;
					Vector3[] positionsFinal = new Vector3[positions.Count];

					for (int j = 0; j < positions.Count; j++) {
						positionsFinal [j] = positions [j];
					}

					line [i].SetPositions (positionsFinal);

					if (hit [i].collider != null && hit [i].collider.GetComponent<ObjectController> ().isTriggerable
					    && (((reflect.z != 0) && (hit [i].collider.transform.position.x < old.x + 0.1f
					    && hit [i].collider.transform.position.x > old.x - 0.1f))
					    || ((reflect.x != 0) && (hit [i].collider.transform.position.z < old.z + 0.1f
					    && hit [i].collider.transform.position.z > old.z - 0.1f)))) {
						obj [i] = hit [i].collider.GetComponent<TriggerController> ();
						obj [i].SetOnTrue ();
						//obj [i].ShowOn ();

						//	if(obj[i].GetComponent<>.name=="WhiteLight")
						//	obj[i].WhiteLightOn();
						//obj[i].BlueLightOn();
						//obj [i].RedLightOn ();
						obj [i].LightOn ();


					} else if (obj [i] != null) {
						obj [i].SetOnFalse ();
						//obj [i].ShowOn ();

						//	if(obj[i]==WhiteLight)
						//	obj[i].WhiteLightOff();
						//obj[i].BlueLightOff();
						//obj [i].RedLightOff ();
						obj [i].LightOff ();
						obj [i] = null;

					}
				} else {
					line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);

					if (obj [i] != null) {
						obj [i].SetOnFalse ();
						//obj [i].ShowOn ();
						obj [i].LightOff ();
						obj [i] = null;
					}
				}
			} else {
				if (onPic [i] != null)
					Destroy (onPic [i]);
				
				line [i].SetPosition (line [i].numPositions - 1, Vector3.zero);

				if (obj [i] != null) {
					obj [i].SetOnFalse ();
					//obj [i].ShowOn ();
					obj [i].LightOff ();
					obj [i] = null;
				}
			}
		}
	}
}