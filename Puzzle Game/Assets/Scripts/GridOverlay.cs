using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
	public readonly int block = -2;
	public readonly int unwalkable = -1;
	public readonly int player = 0;
	public readonly int walkable = 1;
	public readonly int moveable = 2;
	public readonly int shadowable = 3;
	public readonly int pushable = 5;
	public readonly int destroyable = 7;
	public readonly int triggerable = 11;

	private int[,] grid;

	private int lengthX;
	private int lengthZ;

	private Vector3 start;

	private Ray ray;
	private RaycastHit hit;

	private int unwalkableMask;
	private int playerMask;
	private int walkableMask;
	private int moveableMask;
	private int shadowableMask;
	private int pushableMask;
	private int destroyableMask;
	private int triggerableMask;
	private int blockMask;

	void Awake (){
		lengthX = (int)transform.localScale.x;
		lengthZ = (int)transform.localScale.y;

		start = transform.position - new Vector3 (lengthX / 2f, 0f, lengthZ / 2f);
		start.y = 10f;
		grid = new int[lengthX, lengthZ];
		ray = new Ray (start, Vector3.down);

		unwalkableMask = LayerMask.GetMask ("Unwalkable");
		playerMask = LayerMask.GetMask ("Player");
		walkableMask = LayerMask.GetMask ("Walkable");
		moveableMask = LayerMask.GetMask ("Moveable");
		shadowableMask = LayerMask.GetMask ("Shadowable");
		pushableMask = LayerMask.GetMask ("Pushable");
		destroyableMask = LayerMask.GetMask ("Destroyable");
		triggerableMask = LayerMask.GetMask ("Triggerable");
		blockMask = LayerMask.GetMask ("Block");

		for (int x = 0; x < lengthX; x++) {
			for (int z = 0; z < lengthZ; z++) {
				ray.origin = new Vector3 (x + start.x + 0.5f, ray.origin.y, z + start.z + 0.5f);
				grid [x, z] = 1;
				if (Physics.Raycast (ray, out hit, 11f, playerMask)) {
					grid [x, z] *= player;
					continue;
				}
				if (Physics.Raycast (ray, out hit, 11f, blockMask))
					grid [x, z] *= block;
				if (Physics.Raycast (ray, out hit, 11f, unwalkableMask))
					grid [x, z] *= unwalkable;
				if (Physics.Raycast (ray, out hit, 11f, moveableMask))
					grid [x, z] *= moveable;
				if (Physics.Raycast (ray, out hit, 11f, shadowableMask))
					grid [x, z] *= shadowable;
				if (Physics.Raycast (ray, out hit, 11f, pushableMask))
					grid [x, z] *= pushable;
				if (Physics.Raycast (ray, out hit, 11f, destroyableMask))
					grid [x, z] *= destroyable;
				if (Physics.Raycast (ray, out hit, 11f, triggerableMask))
					grid [x, z] *= triggerable;
			}
		}
	}

	//private methods
	private int toGridX (Vector3 v){
		return (int)((v.x > 0f ? (int)v.x : (int)v.x - 1f) - start.x);
	}

	private int toGridZ (Vector3 v){
		return (int)((v.z > 0f ? (int)v.z : (int)v.z - 1f) - start.z);
	}

	private Vector3 toPoint (int x, int z){
		return new Vector3 (start.x + 0.5f + x, 0f, start.z + 0.5f + z);
	}
		
	private void SetGrid (int x, int z, int i){
		grid [x, z] = i;
	}

	private List<Vector3> neighborOf (Vector3 v){
		List<Vector3> l = new List<Vector3> ();
		int gridX = toGridX (v);
		int gridZ = toGridZ (v);
		if (gridX > 0 && grid [gridX - 1, gridZ] == walkable)
			l.Add (v + Vector3.left);
		if (gridX < lengthX - 1 && grid [gridX + 1, gridZ] == walkable)
			l.Add (v + Vector3.right);
		if (gridZ > 0 && grid [gridX, gridZ - 1] == walkable)
			l.Add (v + Vector3.back);
		if (gridZ < lengthZ - 1 && grid [gridX, gridZ + 1] == walkable)
			l.Add (v + Vector3.forward);
		return l;
	}

	//public methods
	public void createGrid (int[,] g){
		grid = g;
	}
	public int GetLengthX (){
		return lengthX;
	}

	public int GetLengthZ (){
		return lengthZ;
	}

	public int GetGrid (int x, int z){
		return grid [x, z];
	}

	public void SetGrid (Vector3 v, int i){
		grid [toGridX (v), toGridZ (v)] = i;
	}

	public void swapGrid (Vector3 v1, Vector3 v2){
		int tmp = grid [toGridX (v1), toGridZ (v1)];
		grid [toGridX (v1), toGridZ (v1)] = grid [toGridX (v2), toGridZ (v2)];
		grid [toGridX (v2), toGridZ (v2)] = tmp;
	}

	public int GetGrid (Vector3 v){
		return grid [toGridX (v), toGridZ (v)];
	}

	public Vector3 toPoint (Vector3 v){
		v.x = (v.x > 0f ? (int)v.x : (int)v.x - 1f) + 0.5f;
		v.y = 0f;
		v.z = (v.z > 0f ? (int)v.z : (int)v.z - 1f) + 0.5f;
		return v;
	}

	public Vector3 nearAround (Vector3 start, Vector3 goal){
		start = toPoint (start);
		goal = toPoint (goal);
		List<Vector3> n = neighborOf (goal);
		Vector3 ans = goal;
		float min = float.MaxValue;
		foreach (Vector3 v in n) {
			if (Vector3.Distance (start, v) < min)
				ans = v;
		}
		return ans;
	}

	public bool isOutOfGrid (Vector3 v){
		int gridX = toGridX (v);
		int gridZ = toGridZ (v);
		if (gridX < 0 || gridX > lengthX - 1 || gridZ < 0 || gridZ > lengthZ - 1 || grid [gridX, gridZ] < 0)
			return false;
		return true;
	}

	public Stack<Vector3> findPath(Vector3 start, Vector3 goal){
		Stack<Vector3> ans = new Stack<Vector3> ();
		HashSet<Vector3> closedSet = new HashSet<Vector3> ();
		HashSet<Vector3> openSet = new HashSet<Vector3> ();
		openSet.Add (start);
		Dictionary<Vector3,Vector3> cameFrom = new Dictionary<Vector3, Vector3> ();
		Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> ();
		gScore [start] = Vector3.Distance (start, goal);
		List<Vector3> l = new List<Vector3> ();
		Vector3 current = start;
		float tGScore;

		while (openSet.Count > 0) {
			float lowest = float.MaxValue;
			foreach(Vector3 v in openSet){
				if (lowest > gScore [v]) {
					lowest = gScore [v];
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
				
				tGScore = Vector3.Distance (neighbor, goal);
				if (!openSet.Contains (current))
					openSet.Add (neighbor);
				else if (tGScore >= gScore [neighbor])
					continue;
				
				cameFrom [neighbor] = current;
				gScore [neighbor] = tGScore;
			}
			l.Clear ();
		}
		return ans;
	}
}