using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
	public readonly int block2 = -3;
	public readonly int block = -2;
	public readonly int unwalkable = -1;
	public readonly int player = 0;
	public readonly int walkable = 1;
	public readonly int walkable2 = 2;
	public readonly int tempWalkable = 3;
	public readonly int tempWalkable2 = 4;

	public bool moving;

	private ObjectController objCon;

	private int[,] grid;
	private int lengthX;
	private int lengthZ;

	private Vector3 start;

	private Ray ray;
	private RaycastHit hit;
	private RaycastHit[] hitList;

	void Awake (){
		lengthX = (int)transform.localScale.x;
		lengthZ = (int)transform.localScale.y;
		moving = false;

		start = transform.position - (Vector3.right * (lengthX / 2f) + Vector3.forward * (lengthZ / 2f));
		start.y = 5f;
		grid = new int[lengthX, lengthZ];
		ray = new Ray (start, Vector3.down);

		for (int x = 0; x < lengthX; x++) {
			for (int z = 0; z < lengthZ; z++) {
				ray.origin = Vector3.right * (x + start.x + 0.5f) + Vector3.up * ray.origin.y + Vector3.forward * (z + start.z + 0.5f);
				hitList = Physics.RaycastAll (ray, 10f);
				if (hitList.Length > 0) {
					grid [x, z] = walkable;
					foreach (RaycastHit hit in hitList) {
						objCon = hit.collider.GetComponent<ObjectController> ();
						if (objCon.isBlock2)
							grid [x, z] = block2;
						else if (objCon.isTempWalkable2 && grid [x, z] != block2)
							grid [x, z] = tempWalkable2;
						else if (objCon.isWalkable2 && grid [x, z] != block2 && grid [x, z] != tempWalkable2)
							grid [x, z] = walkable2;
						else if (objCon.isBlock && grid [x, z] != block2 && grid [x, z] != walkable2 && grid [x, z] != tempWalkable2)
							grid [x, z] = block;
						else if (objCon.isTempWalkable && grid [x, z] != block && grid [x, z] != block2 && grid [x, z] != walkable2 && grid [x, z] != tempWalkable2)
							grid [x, z] = tempWalkable;
						else if (objCon.isUnwalkable && grid [x, z] != block && grid [x, z] != block2 && grid [x, z] != walkable2 && grid [x, z] != tempWalkable && grid [x, z] != tempWalkable2)
							grid [x, z] = unwalkable;
					}
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

	private float Distance (int x1, int z1, int x2, int z2){
		if (x1 == x2)
			return Mathf.Abs (z1 - z2);
		if (z1 == z2)
			return Mathf.Abs (x1 - x2);
		return Mathf.Sqrt ((float)(Mathf.Pow (x1 - x2, 2) + Mathf.Pow (z1 - z2, 2)));
	}

	private List<Vector3> NeighborOf (Vector3 v, bool isOnfloor){
		List<Vector3> l = new List<Vector3> ();
		int gridX = ToGridX (v);
		int gridZ = ToGridZ (v);

		if (gridX > 0) {
			if (isOnfloor) {
				if (grid [gridX - 1, gridZ] == walkable || grid [gridX - 1, gridZ] == tempWalkable)
					l.Add (v + Vector3.left);
			} else {
				if (grid [gridX - 1, gridZ] == walkable2 || grid [gridX - 1, gridZ] == tempWalkable2)
					l.Add (v + Vector3.left);
				else if (grid [gridX - 1, gridZ] == walkable || grid [gridX - 1, gridZ] == tempWalkable)
					l.Add (v + Vector3.left + Vector3.down);
			}
		}

		if (gridX < lengthX - 1) {
			if (isOnfloor) {
				if (grid [gridX + 1, gridZ] == walkable || grid [gridX + 1, gridZ] == tempWalkable)
					l.Add (v + Vector3.right);
			} else {
				if (grid [gridX + 1, gridZ] == walkable2 || grid [gridX + 1, gridZ] == tempWalkable2)
					l.Add (v + Vector3.right);
				else if (grid [gridX + 1, gridZ] == walkable || grid [gridX + 1, gridZ] == tempWalkable)
					l.Add (v + Vector3.right + Vector3.down);
			}
		}

		if (gridZ > 0) {
			if (isOnfloor) {
				if (grid [gridX, gridZ - 1] == walkable || grid [gridX, gridZ - 1] == tempWalkable)
					l.Add (v + Vector3.back);
			} else {
				if (grid [gridX, gridZ - 1] == walkable2 || grid [gridX, gridZ - 1] == tempWalkable2)
					l.Add (v + Vector3.back);
				else if (grid [gridX, gridZ - 1] == walkable || grid [gridX, gridZ - 1] == tempWalkable)
					l.Add (v + Vector3.back + Vector3.down);
			}
		}
		
		if (gridZ < lengthZ - 1) {
			if (isOnfloor) {
				if (grid [gridX, gridZ + 1] == walkable || grid [gridX, gridZ + 1] == tempWalkable)
					l.Add (v + Vector3.forward);
			} else {
				if (grid [gridX, gridZ + 1] == walkable2 || grid [gridX, gridZ + 1] == tempWalkable2)
					l.Add (v + Vector3.forward);
				else if (grid [gridX, gridZ + 1] == walkable || grid [gridX, gridZ + 1] == tempWalkable)
					l.Add (v + Vector3.forward + Vector3.down);
			}
		}

		return l;
	}

	//public methods
	public void SetGrid (Vector3 v, int i){
		grid [ToGridX (v), ToGridZ (v)] = i;
	}

	public void SetGridHere (Vector3 v){
		if (Physics.Raycast (ToPoint0Y (v) + Vector3.up * 5, Vector3.down, out hit, 10f)) {
			if (objCon.isBlock2)
				grid [ToGridX (v), ToGridZ (v)] = block2;
			else if (objCon.isTempWalkable2)
				grid [ToGridX (v), ToGridZ (v)] = tempWalkable2;
			else if (objCon.isWalkable2)
				grid [ToGridX (v), ToGridZ (v)] = walkable2;
			else if (objCon.isBlock)
				grid [ToGridX (v), ToGridZ (v)] = block;
			else if (objCon.isUnwalkable)
				grid [ToGridX (v), ToGridZ (v)] = unwalkable;
			else if (objCon.isTempWalkable)
				grid [ToGridX (v), ToGridZ (v)] = tempWalkable;
			else
				grid [ToGridX (v), ToGridZ (v)] = walkable;
		}
	}

	public void SwapGrid (Vector3 v1, Vector3 v2){
		int tmp = grid [ToGridX (v1), ToGridZ (v1)];
		grid [ToGridX (v1), ToGridZ (v1)] = grid [ToGridX (v2), ToGridZ (v2)];
		grid [ToGridX (v2), ToGridZ (v2)] = tmp;
	}

	public int GetGrid (Vector3 v){
		return grid [ToGridX (v), ToGridZ (v)];
	}

	public Vector3 ToPointIgnoreY (Vector3 v){
		v.x = Mathf.Floor (v.x) + 0.5f;
		v.z = Mathf.Floor (v.z) + 0.5f;
		return v;
	}

	public Vector3 ToPoint (Vector3 v){
		v.x = Mathf.Floor (v.x) + 0.5f;
		if (v.y > 1.4f)
			v.y = 1.5f;
		else if (v.y > 0.9f)
			v.y = 1f;
		else if (v.y > 0.4)
			v.y = 0.5f;
		else
			v.y = 0f;
		v.z = Mathf.Floor (v.z) + 0.5f;
		return v;
	}

	public Vector3 ToPointY (Vector3 v, bool onFloor){
		v.x = Mathf.Floor (v.x) + 0.5f;
		v.y = onFloor ? 0 : 1;
		v.z = Mathf.Floor (v.z) + 0.5f;
		return v;
	}

	public Vector3 ToPoint0Y (Vector3 v){
		v.x = Mathf.Floor (v.x) + 0.5f;
		v.y = 0f;
		v.z = Mathf.Floor (v.z) + 0.5f;
		return v;
	}

	public Vector3 Set0Y(Vector3 v){
		v.y = 0f;
		return v;
	}

	public Vector3 SetYFrom(Vector3 v, Vector3 v2){
		v.y = v2.y;
		return v;
	}

	public Vector3 Set1Y(Vector3 v){
		v.y = 1f;
		return v;
	}

	public bool IsOutOfGrid (Vector3 v){
		int gridX = ToGridX (v);
		int gridZ = ToGridZ (v);
		if (gridX >= 0 && gridX <= lengthX - 1 && gridZ >= 0 && gridZ <= lengthZ - 1 && (grid [gridX, gridZ] == walkable2 || grid [gridX, gridZ] == tempWalkable2 || grid [gridX, gridZ] == walkable || grid [gridX, gridZ] == tempWalkable || grid [gridX, gridZ] == player))
			return false;
		return true;
	}

	public bool IsWalkable(Vector3 v1, Vector3 v2, bool onFloor){
		int v1X = ToGridX (v1);
		int v1Z = ToGridZ (v1);
		int v2X = ToGridX (v2);
		int v2Z = ToGridZ (v2);
		int far;
		int dir;

		if ((onFloor && (grid [v1X, v1Z] != walkable && grid [v1X, v1Z] != tempWalkable))
		    || (!onFloor && (grid [v1X, v1Z] != walkable && grid [v1X, v1Z] != tempWalkable)
		    && (grid [v1X, v1Z] != walkable2 && grid [v1X, v1Z] != tempWalkable2)))
			return false;

		if (v1X == v2X) {
			dir = v1Z > v2Z ? 1 : -1;
			far = (v1Z - v2Z) * dir;
			for (int i = 0; i < far; i++) {
				if ((onFloor && (grid [v2X, v2Z + i * dir] != walkable && grid [v2X, v2Z + i * dir] != tempWalkable))
				    || (!onFloor && (grid [v2X, v2Z + i * dir] != walkable && grid [v2X, v2Z + i * dir] != tempWalkable)
				    && (grid [v2X, v2Z + i * dir] != walkable2 && grid [v2X, v2Z + i * dir] != tempWalkable2)))
					return false;
			}
			return true;
		} else if (v1Z == v2Z) {
			dir = v1X > v2X ? 1 : -1;
			far = (v1X - v2X) * dir;
			for (int i = 0; i < far; i++) {
				if ((onFloor && grid [v2X + i * dir, v2Z] != walkable && grid [v2X + i * dir, v2Z] != tempWalkable)
				    || (!onFloor && (grid [v2X + i * dir, v2Z] != walkable && grid [v2X + i * dir, v2Z] != tempWalkable)
				    && (grid [v2X + i * dir, v2Z] != walkable2 && grid [v2X + i * dir, v2Z] != tempWalkable2)))
					return false;
			}
			return true;
		}

		return false;
	}

//	public void SetWalkable(Vector3 v1, Vector3 v2){
//		int v1X = ToGridX (v1);
//		int v1Z = ToGridZ (v1);
//		int v2X = ToGridX (v2);
//		int v2Z = ToGridZ (v2);
//		int far;
//		int dir;
//
//		if (IsOutOfGrid (v1))
//			return;
//
//		if (v2X < 0)
//			v2X = 0;
//		
//		if (v2X > lengthX - 1)
//			v2X = lengthX - 1;
//
//		if (v2Z < 0)
//			v2Z = 0;
//		
//		if (v2Z > lengthZ - 1)
//			v2Z = lengthZ - 1;
//
//		if (v1X == v2X) {
//			dir = v1Z > v2Z ? 1 : -1;
//			far = (v1Z - v2Z) * dir;
//			for (int i = 0; i < far; i++) {
//				if (grid [v2X, v2Z + i * dir] == unwalkable)
//					grid [v2X, v2Z + i * dir] = tempWalkable;
//			}
//		} else if (v1Z == v2Z) {
//			dir = v1X > v2X ? 1 : -1;
//			far = (v1X - v2X) * dir;
//			for (int i = 0; i < far; i++) {
//				if (grid [v2X + i * dir, v2Z] == unwalkable)
//					grid [v2X + i * dir, v2Z] = tempWalkable;
//			}
//		} else
//			return;
//	}
//
//	public void SetWalkableBack(Vector3 v1, Vector3 v2){
//		int v1X = ToGridX (v1);
//		int v1Z = ToGridZ (v1);
//		int v2X = ToGridX (v2);
//		int v2Z = ToGridZ (v2);
//		int far;
//		int dir;
//
//		if (IsOutOfGrid (v1))
//			return;
//
//		if (v2X < 0)
//			v2X = 0;
//
//		if (v2X > lengthX - 1)
//			v2X = lengthX - 1;
//
//		if (v2Z < 0)
//			v2Z = 0;
//
//		if (v2Z > lengthZ - 1)
//			v2Z = lengthZ - 1;
//		
//		if (v1X == v2X) {
//			dir = v1Z > v2Z ? 1 : -1;
//			far = (v1Z - v2Z) * dir;
//			for (int i = 0; i < far; i++) {
//				if (grid [v2X, v2Z + i * dir] == tempWalkable)
//					grid [v2X, v2Z + i * dir] = unwalkable;
//			}
//		} else if (v1Z == v2Z) {
//			dir = v1X > v2X ? 1 : -1;
//			far = (v1X - v2X) * dir;
//			for (int i = 0; i < far; i++) {
//				if (grid [v2X + i * dir, v2Z] == tempWalkable)
//					grid [v2X + i * dir, v2Z] = unwalkable;
//			}
//		} else
//			return;
//	}

	public Stack<Vector3> FindGrabPath(Vector3 goal, Vector3 start, Vector3 direction){
		int b = 0;
		start = ToPoint (start);
		Vector3 current = goal = ToPoint (goal);
		start.y = (GetGrid (start) == walkable ? 0f : 1f);
		current.y = goal.y = (GetGrid (goal) == walkable ? 0f : 1f);
		Stack<Vector3> ans = new Stack<Vector3> ();

		if (ToPoint0Y (goal) + direction != ToPoint0Y (start)) {
			ans.Push (goal);
			while (current != start || b > 1000) {
				if (GetGrid (current - direction) == walkable2) {
					current = current - direction;
					current.y = 1f;
				} else {
					current = current - direction;
					current.y = 0f;
				}
				ans.Push (current);
				b++;
			}
		}

		return ans;
	}

	public Stack<Vector3> FindPath(Vector3 start, Vector3 goal, bool startFloor){
		int b = 0;
		Vector3 current = start = ToPointY (start, startFloor);
		goal = ToPoint (goal);
		Stack<Vector3> ans = new Stack<Vector3> ();
		HashSet<Vector3> closedSet = new HashSet<Vector3> ();
		HashSet<Vector3> openSet = new HashSet<Vector3> ();
		openSet.Add (start);
		Dictionary<Vector3,Vector3> cameFrom = new Dictionary<Vector3, Vector3> ();
		Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> ();
		gScore [start] = Vector3.Distance (start, goal);
		List<Vector3> l = new List<Vector3> ();
		float tGScore;
		bool onFloor;

		if (start == goal) {
			return ans;
		}
		
		while (openSet.Count > 0 && b < 10000) {
			float lowest = float.MaxValue;
			foreach (Vector3 v in openSet) {
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

				if (ans.Peek () == start)
					ans.Pop ();

				return ans;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			if (current.y == 0f)
				onFloor = true;
			else
				onFloor = false;

			l = NeighborOf (current, onFloor);

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
			b++;
		}

		return ans;
	}
}