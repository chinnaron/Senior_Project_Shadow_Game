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
	public readonly int tempWalkable = 13;
	public bool moving;

	private ObjectController objCon;

	private int[,] grid;

	private int lengthX;
	private int lengthZ;

	private Vector3 start;

	private Ray ray;
	private RaycastHit hit;

	void Awake (){
		lengthX = (int)transform.localScale.x;
		lengthZ = (int)transform.localScale.y;
		moving = false;

		start = transform.position - (Vector3.right * (lengthX / 2f) + Vector3.forward * (lengthZ / 2f));
		start.y = 10f;
		grid = new int[lengthX, lengthZ];
		ray = new Ray (start, Vector3.down);

		for (int x = 0; x < lengthX; x++) {
			for (int z = 0; z < lengthZ; z++) {
				ray.origin = Vector3.right * (x + start.x + 0.5f) + Vector3.up * ray.origin.y + Vector3.forward * (z + start.z + 0.5f);
				grid [x, z] = unwalkable;
				if (Physics.Raycast (ray, out hit, 11f)) {
					grid [x, z] = walkable;
					objCon = hit.collider.GetComponent<ObjectController> ();
					if (objCon.isPlayer) {
						grid [x, z] *= player;
						continue;
					}
					if (objCon.isBlock)
						grid [x, z] *= block;
					else if (objCon.isUnwalkable)
						grid [x, z] *= unwalkable;
					if (objCon.isMoveable)
						grid [x, z] *= moveable;
					if (objCon.isShadowable)
						grid [x, z] *= shadowable;
					if (objCon.isPushable)
						grid [x, z] *= pushable;
					if (objCon.isDestroyable)
						grid [x, z] *= destroyable;
					if (objCon.isTriggerable)
						grid [x, z] *= triggerable;
				}
			}
		}
	}

	//private methods
	private int ToGridX (Vector3 v){
		return (int)(Mathf.Floor(v.x) - start.x);
	}

	private int ToGridZ (Vector3 v){
		return (int)(Mathf.Floor(v.z) - start.z);
	}

	private Vector3 ToPoint (int x, int z){
		return Vector3.right * (start.x + 0.5f + x) + Vector3.forward * (start.z + 0.5f + z);
	}
		
	private void SetGrid (int x, int z, int i){
		grid [x, z] = i;
	}

	private float Distance (int x1, int z1, int x2, int z2){
		if (x1 == x2)
			return Mathf.Abs (z1 - z2);
		if (z1 == z2)
			return Mathf.Abs (x1 - x2);
		return Mathf.Sqrt ((float)(Mathf.Pow (x1 - x2, 2) + Mathf.Pow (z1 - z2, 2)));
	}

	private List<Vector3> NeighborOf (Vector3 v){
		List<Vector3> l = new List<Vector3> ();
		int gridX = ToGridX (v);
		int gridZ = ToGridZ (v);
		if (gridX > 0 && (grid [gridX - 1, gridZ] == walkable || grid [gridX - 1, gridZ] == tempWalkable))
			l.Add (v + Vector3.left);
		if (gridX < lengthX - 1 && (grid [gridX + 1, gridZ] == walkable || grid [gridX + 1, gridZ] == tempWalkable))
			l.Add (v + Vector3.right);
		if (gridZ > 0 && (grid [gridX, gridZ - 1] == walkable || grid [gridX, gridZ - 1] == tempWalkable))
			l.Add (v + Vector3.back);
		if (gridZ < lengthZ - 1 && (grid [gridX, gridZ + 1] == walkable || grid [gridX, gridZ + 1] == tempWalkable))
			l.Add (v + Vector3.forward);
		return l;
	}

	//public methods
	public void CreateGrid (int[,] g){
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
		grid [ToGridX (v), ToGridZ (v)] = i;
	}

	public void SwapGrid (Vector3 v1, Vector3 v2){
		int tmp = grid [ToGridX (v1), ToGridZ (v1)];
		grid [ToGridX (v1), ToGridZ (v1)] = grid [ToGridX (v2), ToGridZ (v2)];
		grid [ToGridX (v2), ToGridZ (v2)] = tmp;
	}

	public int GetGrid (Vector3 v){
		return grid [ToGridX (v), ToGridZ (v)];
	}

	public Vector3 ToPoint (Vector3 v){
		v.x = Mathf.Floor(v.x) + 0.5f;
		v.z = Mathf.Floor(v.z) + 0.5f;
		return v;
	}

	public Vector3 ToPoint0Y (Vector3 v){
		v.x = Mathf.Floor(v.x) + 0.5f;
		v.y = 0f;
		v.z = Mathf.Floor(v.z) + 0.5f;
		return v;
	}

	public Vector3 NearAround (Vector3 start, Vector3 goal){
		start = ToPoint (start);
		goal = ToPoint (goal);
		List<Vector3> n = NeighborOf (goal);
		Vector3 ans = goal;
		float min = float.MaxValue;
		foreach (Vector3 v in n) {
			if (Vector3.Distance (start, v) < min)
				ans = v;
		}
		return ans;
	}

	public bool IsOutOfGrid (Vector3 v){
		int gridX = ToGridX (v);
		int gridZ = ToGridZ (v);
		if (gridX >= 0 && gridX <= lengthX - 1 && gridZ >= 0 && gridZ <= lengthZ - 1 && (grid [gridX, gridZ] == walkable || grid [gridX, gridZ] == tempWalkable || grid [gridX, gridZ] == player))
			return false;
		return true;
	}

	public void SetWalkable(Vector3 v1, Vector3 v2){
		int v1X = ToGridX (v1);
		int v1Z = ToGridZ (v1);
		int v2X = ToGridX (v2);
		int v2Z = ToGridZ (v2);
		int far;
		int dir;

		if (v1X == v2X) {
			dir = v1Z > v2Z ? 1 : -1;
			far = (v1Z - v2Z) * dir;
			for (int i = 0; i < far; i++) {
				if (grid [v2X, v2Z + i * dir] == unwalkable)
					grid [v2X, v2Z + i * dir] = tempWalkable;
			}
		} else if (v1Z == v2Z) {
			dir = v1X > v2X ? 1 : -1;
			far = (v1X - v2X) * dir;
			for (int i = 0; i < far; i++) {
				if (grid [v2X + i * dir, v2Z] == unwalkable)
					grid [v2X + i * dir, v2Z] = tempWalkable;
			}
		} else
			return;
	}

	public void SetWalkableBack(Vector3 v1, Vector3 v2){
		int v1X = ToGridX (v1);
		int v1Z = ToGridZ (v1);
		int v2X = ToGridX (v2);
		int v2Z = ToGridZ (v2);
		int far;
		int dir;

		if (v1X == v2X) {
			dir = v1Z > v2Z ? 1 : -1;
			far = (v1Z - v2Z) * dir;
			for (int i = 0; i < far; i++) {
				if (grid [v2X, v2Z + i * dir] == tempWalkable)
					grid [v2X, v2Z + i * dir] = walkable;
			}
		} else if (v1Z == v2Z) {
			dir = v1X > v2X ? 1 : -1;
			far = (v1X - v2X) * dir;
			for (int i = 0; i < far; i++) {
				if (grid [v2X + i * dir, v2Z] == tempWalkable)
					grid [v2X + i * dir, v2Z] = walkable;
			}
		} else
			return;
	}

	public Stack<Vector3> FindPath(Vector3 start, Vector3 goal){
		start = ToPoint (start);
		goal = ToPoint (goal);
		Stack<Vector3> ans = new Stack<Vector3> ();
		HashSet<Vector3> closedSet = new HashSet<Vector3> ();
		HashSet<Vector3> openSet = new HashSet<Vector3> ();
		openSet.Add (start);
		Dictionary<Vector3,Vector3> cameFrom = new Dictionary<Vector3, Vector3> ();
		Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> ();
		gScore [start] = Vector3.Distance (start, goal);
		List<Vector3> l = new List<Vector3> ();
		Vector3 current = start;
		Vector3 last = goal;
		float tGScore;

		if (start == goal) {
			return ans;
		}
		
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
				last = current;
				while (cameFrom.ContainsKey (current)) {
					current = cameFrom [current];
					ans.Push (current);
					if (cameFrom.ContainsKey (current) && (cameFrom [current].x == last.x || cameFrom [current].z == last.z))
						ans.Pop ();
					else
						last = current;
				}
				if (ans.Peek () == start)
					ans.Pop ();
				return ans;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			l = NeighborOf (current);
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