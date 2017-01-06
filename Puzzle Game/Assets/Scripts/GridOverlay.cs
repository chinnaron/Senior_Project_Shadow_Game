using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
	private bool[,] grid;

	private int lengthX;
	private int lengthZ;

	private Vector3 start;

	private Ray ray;
	private RaycastHit hit;

	private int unWalkableMask;
	private int moveableMask;

	void Awake (){
		lengthX = (int)transform.localScale.x;
		lengthZ = (int)transform.localScale.y;

		start = transform.position - new Vector3 (lengthX / 2f, 0f, lengthZ / 2f);
		start.y = 10f;
		grid = new bool[lengthX, lengthZ];
		ray = new Ray (start, Vector3.down);

		unWalkableMask = LayerMask.GetMask ("UnWalkable");
		moveableMask = LayerMask.GetMask ("Moveable");

		for (int x = 0; x < lengthX; x++) {
			for (int z = 0; z < lengthZ; z++) {
				ray.origin = new Vector3 (x + start.x + 0.5f, ray.origin.y, z + start.z + 0.5f);
				if (Physics.Raycast (ray, out hit, 11f, unWalkableMask) || Physics.Raycast (ray, out hit, 11f, moveableMask))
					grid [x, z] = false;
				else
					grid [x, z] = true;
			}
		}
	}

	public int GetLengthX (){
		return lengthX;
	}

	public int GetLengthZ (){
		return lengthZ;
	}

	public void SetGridTrue (Vector3 v){
		grid [toGrid (v, 'x'), toGrid (v, 'z')] = true;
	}

	public void SetGridFalse (Vector3 v){
		grid [toGrid (v, 'x'), toGrid (v, 'z')] = false;
	}

	public void SetGridTrue (int x, int z){
		grid [x, z] = true;
	}

	public void SetGridFalse (int x, int z){
		grid [x, z] = false;
	}
	public bool GetGrid (Vector3 v){
		return grid [toGrid (v, 'x'), toGrid (v, 'z')];
	}
		
	public bool GetGrid (int x, int z){
		return grid [x, z];
	}

	public int toGrid (Vector3 v, char c){
		if (c == 'x')
			return (int)((v.x > 0f ? (int)v.x : (int)v.x - 1f) - start.x);
		else
			return (int)((v.z > 0f ? (int)v.z : (int)v.z - 1f) - start.z);
	}

	public Vector3 toPoint (Vector3 v){
		v.x = (v.x > 0f ? (int)v.x : (int)v.x - 1f) + 0.5f;
		v.y = 0f;
		v.z = (v.z > 0f ? (int)v.z : (int)v.z - 1f) + 0.5f;
		return v;
	}

	public Vector3 toPoint (int x, int z){
		return new Vector3 (start.x + 0.5f + x, 0f, start.z + 0.5f + z);
	}

	public bool isOutOfGrid (Vector3 v){
		int gridX = toGrid (v, 'x');
		int gridZ = toGrid (v, 'z');
		if (!grid [gridX, gridZ] || gridX < 0 || gridX > lengthX - 1 || gridZ < 0 || gridZ > lengthZ - 1)
			return false;
		return true;
	}

	public List<Vector3> neighborOf (Vector3 v){
		List<Vector3> l = new List<Vector3> ();
		int gridX = toGrid (v, 'x');
		int gridZ = toGrid (v, 'z');
		if (gridX > 0 && grid [gridX - 1, gridZ])
			l.Add (v + Vector3.left);
		if (gridX < lengthX - 1 && grid [gridX + 1, gridZ])
			l.Add (v + Vector3.right);
		if (gridZ > 0 && grid [gridX, gridZ - 1])
			l.Add (v + Vector3.back);
		if (gridZ < lengthZ - 1 && grid [gridX, gridZ + 1])
			l.Add (v + Vector3.forward);
		return l;
	}

	public Stack<Vector3> findPath(Vector3 start, Vector3 goal){
		Stack<Vector3> ans = new Stack<Vector3> ();
		HashSet<Vector3> closedSet = new HashSet<Vector3> ();
		HashSet<Vector3> openSet = new HashSet<Vector3> ();
		openSet.Add (start);
		Dictionary<Vector3,Vector3> cameFrom = new Dictionary<Vector3, Vector3> ();
		Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> ();
		gScore [start] = 0;
		Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float> ();
		fScore [start] = Vector3.Distance (start, goal);
		List<Vector3> l = new List<Vector3> ();
		Vector3 current = start;
		float tGScore;

		while (openSet.Count > 0) {
			float lowest = float.MaxValue;
			foreach(Vector3 v in openSet){
				if (lowest > fScore [v]) {
					lowest = fScore [v];
					current = v;
				}
			}

			if (goal == current) {
				ans.Push (current);
				while (cameFrom.ContainsKey (current)) {
					current = cameFrom [current];
					ans.Push (current);
				}
				return ans;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			l = neighborOf (current);
			foreach(Vector3 neighbor in l){
				if (closedSet.Contains (neighbor))
					continue;
				
				tGScore = gScore [current] + 1f;
				if (!openSet.Contains (current))
					openSet.Add (neighbor);
				else if (tGScore >= gScore [neighbor])
					continue;
				
				cameFrom [neighbor] = current;
				gScore [neighbor] = tGScore;
				fScore [neighbor] = gScore [neighbor] + Vector3.Distance (neighbor, goal);
			}
			l.Clear ();
		}
		return ans;
	}
}
