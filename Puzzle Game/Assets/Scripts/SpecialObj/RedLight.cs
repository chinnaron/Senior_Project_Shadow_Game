using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedLight : MonoBehaviour {
	public float rayDistance = 5f;
	public LineRenderer[] line = new LineRenderer[4];
	public GameObject[] onPic = new GameObject[4];
	public bool[] lightOn = new bool[]{ false, false, false, false };
	public bool[] LightTriggerDirection = new bool[]{false,false,false,false};
	public GameObject redrender;
	public Material[] red_states;
	public AudioClip[] sound;
	public AudioSource[] sounds;
	private PlayerController player;
	private GridOverlay grid;
	private Color cOn = new Color(0f, 0.8f, 0f, 1f);
	private Color cOff = new Color(0.8f, 0f, 0f, 1f);

	private float distance;
	private List<Vector3> positions = new List<Vector3> ();
	private Vector3 old;
	private Vector3 reflect;
	private readonly Vector3[] wayP = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
	private RaycastHit[] hit = new RaycastHit[4];

	void Awake () {
		grid = FindObjectOfType<GridOverlay> ();
		player = FindObjectOfType<PlayerController> ();
		bool tr = false;

		for (int i = 0; i < 4; i++) {
			if (LightTriggerDirection [i]) {
				onPic [i].SetActive (true);
				tr = true;
			}
		}

		for (int i = 0; i < 4; i++) {
			if (lightOn [i]) {
				if(!tr)
					onPic [i].SetActive (true);
				
				line [i].SetPosition (line [i].numPositions - 1, wayP [i] * rayDistance);
			}
		}
	}

	void Update ()	{
		if (GetComponent<ObjectController> ().isMoveable) {
			if (IsLightOn()) {
				redrender.GetComponent<Renderer> ().material = red_states[3];
			} else {
				redrender.GetComponent<Renderer> ().material = red_states[2];
			}
		} else {
			if (IsLightOn()) {
				redrender.GetComponent<Renderer> ().material = red_states[1];
			} else {
				redrender.GetComponent<Renderer> ().material = red_states[0];
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

					if (hit [i].collider != null && hit [i].collider.GetComponent<ObjectController> ().isDestroyable
						&& (((reflect.z != 0) && (hit [i].collider.transform.position.x < old.x + 0.2f
							&& hit [i].collider.transform.position.x > old.x - 0.2f))
							|| ((reflect.x != 0) && (hit [i].collider.transform.position.z < old.z + 0.2f
								&& hit [i].collider.transform.position.z > old.z - 0.2f)))) {
						if (hit [i].collider.gameObject == player.gameObject) {
							Destroy (hit [i].collider.gameObject, Time.deltaTime * 2f);
							Application.LoadLevel (Application.loadedLevel);
						} else {
							if (hit [i].collider.GetComponent<PushController> () == player.GetGrabPush ())
								player.GrabRelease ();

							Destroy (hit [i].collider.gameObject, Time.deltaTime * 2f);

							if (hit [i].collider.GetComponent<ObjectController> ().isBlock) {
								grid.SetGrid (hit [i].point, grid.walkable);
							} else {
								grid.SetGrid (hit [i].point, grid.walkable2);
							}
						}
						//create particle
						PlaySound(0);
					} else {
						//create particle
					}
				} else {
					line [i].numPositions = 2;
					line [i].SetPosition (0, Vector3.zero);
					line [i].SetPosition (1, wayP [i] * rayDistance);
				}
			} else {
				if (onPic [i].activeSelf)
					onPic [i].GetComponentInChildren<RawImage> ().color = cOff;

				line [i].numPositions = 0;
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

	public void PlaySound(int s){
		sounds = GetComponents<AudioSource> ();
		sounds[1].clip = sound [s];
		sounds[1].Play ();
	}
}
