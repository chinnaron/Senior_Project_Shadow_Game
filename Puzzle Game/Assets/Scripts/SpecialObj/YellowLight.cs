using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YellowLight : MonoBehaviour {
	public float rayDistance = 5f;
	public LineRenderer[] line = new LineRenderer[4];
	public GameObject[] onPic = new GameObject[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	public GameObject yellowrender;
	public Material[] yellow_states;
	private GridOverlay grid;
	private Color cOn = new Color(0f, 0.8f, 0f, 1f);
	private Color cOff = new Color(0.8f, 0f, 0f, 1f);

	private float distance;
	private List<Vector3> positions = new List<Vector3> ();
	private Vector3 old;
	private Vector3 reflect;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private RaycastHit[] hit = new RaycastHit[4];
	private TriggerController[] obj = new TriggerController[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		bool tr = false;

		for (int i = 0; i < 4; i++) {
			if (LightTriggerDirection [i]) {
				onPic [i].SetActive (true);
				tr = true;
			}
		}

		for (int i = 0; i < 4; i++) {
			if (lightOn [i]){
				if(!tr)
					onPic [i].SetActive (true);
				
				line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);
			}
		}
	}

	void Update ()	{
		if (GetComponent<ObjectController> ().isMoveable) {
			if (IsLightOn()) {
				yellowrender.GetComponent<Renderer> ().material = yellow_states[3];
			} else {
				yellowrender.GetComponent<Renderer> ().material = yellow_states[2];
			}
		} else {
			if (IsLightOn()) {
				yellowrender.GetComponent<Renderer> ().material = yellow_states[1];
			} else {
				yellowrender.GetComponent<Renderer> ().material = yellow_states[0];
			}
		}
		for (int i = 0; i < 4; i++) {
			if (lightOn [i]) {
				onPic [i].GetComponentInChildren<RawImage> ().color = cOn;

				if (Physics.Raycast (transform.position, wayP [i], out hit [i], rayDistance)) {
					distance = rayDistance;
					positions.Clear ();
					positions.Add (Vector3.zero);
					old = transform.position;
					reflect = wayP [i];

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
						reflect = hit [i].collider.GetComponent<Mirror> ().Reflect ((hit [i].collider.transform.position - old).normalized);

						if (reflect != Vector3.zero) {
							old = hit [i].collider.transform.position;

							if (reflect.z != 0) {
								distance = distance - Mathf.Abs (positions [positions.Count - 1].x - positions [positions.Count - 2].x);
								if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit [i], distance)) {
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
								if (Physics.Raycast (grid.SetYFrom (hit [i].collider.transform.position, transform.position), reflect, out hit [i], distance)) {
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

						if (!obj [i].IsLit ()) {
							obj [i].LightOn ();
						}


					} else if (obj [i] != null) {
						obj [i].SetOnFalse ();
						//obj [i].ShowOn ();

						//	if(obj[i]==WhiteLight)
						//	obj[i].WhiteLightOff();
						//obj[i].BlueLightOff();
						//obj [i].RedLightOff ();
						if (obj [i].IsLit ()) {
							obj [i].LightOff ();
						}
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
				if (onPic [i].activeSelf)
					onPic [i].GetComponentInChildren<RawImage> ().color = cOff;
				
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
	public bool IsLightOn() {
		for (int i = 0; i < 4; i++) {
			if (lightOn [i])
				return true;
		}
		return false;
	}

}