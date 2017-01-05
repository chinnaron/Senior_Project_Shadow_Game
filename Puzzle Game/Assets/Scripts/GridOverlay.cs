using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour {
	public static GridOverlay instance;

	private int[,] grid;

	private int lengthX;
	private int lengthZ;

	private Vector3 start;

	private Ray ray;
	private RaycastHit hit;

	private int floorMask;
	private int unWalkableMask;

	private Collider floorCollider;

	void Awake (){
		lengthX = (int)transform.localScale.x;
		lengthZ = (int)transform.localScale.y;

		start = transform.position - new Vector3 (lengthX / 2f, 0f, lengthZ / 2f);
		start.y = 10f;
		grid = new int[lengthX, lengthZ];
		ray = new Ray (start, Vector3.down);

		floorMask = LayerMask.GetMask ("Floor");
		unWalkableMask = LayerMask.GetMask ("UnWalkable");

		for (int x = 0; x < lengthX; x++) {
			for (int z = 0; z < lengthZ; z++) {
				ray.origin = new Vector3 (x + start.x + 0.5f, ray.origin.y, z + start.z + 0.5f);
				if (Physics.Raycast (ray, out hit, 11f, unWalkableMask)) {
					grid [x, z] = -1;
				} else if (Physics.Raycast (ray, out hit, 11f, floorMask)) {
					grid [x, z] = 0;
				} else {
					grid [x, z] = -1;
				}
			}
		}
	}

	public int GetLengthX (){
		return lengthX;
	}

	public int GetLengthZ (){
		return lengthZ;
	}

	public void SetGrid (Vector3 idx, int v){
		grid [(int)((idx.x > 0f ? (int)idx.x : (int)idx.x - 1f) - start.x), (int)((idx.z > 0f ? (int)idx.z : (int)idx.z - 1f) - start.z)] = v;
	}

	public void SetGrid (int x, int z, int v){
		grid [x, z] = v;
	}

	public int GetGrid (Vector3 v){
		return grid [(int)((v.x > 0f ? (int)v.x : (int)v.x - 1f) - start.x), (int)((v.z > 0f ? (int)v.z : (int)v.z - 1f) - start.z)];
	}
		
	public int GetGrid (int x, int z){
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
		v.z = (v.z > 0f ? (int)v.z : (int)v.z - 1f) + 0.5f;
		return v;
	}

	public Vector3 toPoint (int x, int z){
		return new Vector3 (start.x + 0.5f + x, 0f, start.z + 0.5f + z);
	}
		
	public List<Vector3> neighborOf (Vector3 v){
		List<Vector3> l = new List<Vector3> ();
		int gridX = toGrid (v, 'x');
		int gridZ = toGrid (v, 'z');
		if (gridX > 0 && grid [gridX - 1, gridZ] == 0)
			l.Add (v + Vector3.left);
		if (gridX < lengthX - 1 && grid [gridX + 1, gridZ] == 0)
			l.Add (v + Vector3.right);
		if (gridZ > 0 && grid [gridX, gridZ - 1] == 0)
			l.Add (v + Vector3.back);
		if (gridZ < lengthZ - 1 && grid [gridX, gridZ + 1] == 0)
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

//	Vector3 nearer (Vector3 a, Vector3 b, Vector3 des){
//		return Vector3.Distance (a, des) < Vector3.Distance (b, des) ? a : b;
//	}

//	public Stack<Vector3> findPath(Vector3 start, Vector3 stop){
//		start.x = (start.x > 0f ? (int)start.x : (int)start.x - 1f) + 0.5f;
//		start.z = (start.z > 0f ? (int)start.z : (int)start.z - 1f) + 0.5f;
//		stop.x = (stop.x > 0f ? (int)stop.x : (int)stop.x - 1f) + 0.5f;
//		stop.z = (stop.z > 0f ? (int)stop.z : (int)stop.z - 1f) + 0.5f;
//		Stack<Vector3> ans = new Stack<Vector3> ();
//		Queue<Vector3> q = new Queue<Vector3> ();
//		Vector3 now = start;
//		int[,] grid2 = grid;
//		int count = 0;
//		q.Enqueue (now);
//
//		while (now != stop) {
//			if (toGrid (now, 'z') > 0 && grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] == 0) {
//				grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] = count;
//				q.Enqueue (GetPoint (toGrid (now, 'x'), toGrid (now, 'z') - 1));
//			}
//			if (toGrid (now, 'z') <  lengthZ && grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] == 0) {
//				grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] = count;
//				q.Enqueue (GetPoint (toGrid (now, 'x'), toGrid (now, 'z') + 1));
//			}
//			if (toGrid (now, 'x') > 0 && grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] == 0) {
//				grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] = count;
//				q.Enqueue (GetPoint (toGrid (now, 'x') - 1, toGrid (now, 'z')));
//			}
//			if (toGrid (now, 'x') <  lengthX && grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] == 0) {
//				grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] = count;
//				q.Enqueue (GetPoint (toGrid (now, 'x') + 1, toGrid (now, 'z')));
//			}
//			q.Dequeue ();
//			if (q.Count < 1)
//				return ans;
//			now = q.Peek ();
//		}
//
//		count = GetGrid (now);
//		ans.Push (now);
//		Vector3 near = now;
//		while (ans.Peek () != start) {
//			count--;
//			if (toGrid (now, 'z') > 0 && grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] == count) {
//				near = nearer (near, GetPoint (toGrid (now, 'x'), toGrid (now, 'z') - 1), start);
//			}
//			if (toGrid (now, 'z') <  lengthZ && grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] == count) {
//				near = nearer (near, GetPoint (toGrid (now, 'x'), toGrid (now, 'z') + 1), start);
//			}
//			if (toGrid (now, 'x') > 0 && grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] == count) {
//				near = nearer (near, GetPoint (toGrid (now, 'x') - 1, toGrid (now, 'z')), start);
//			}
//			if (toGrid (now, 'x') <  lengthX && grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] == count) {
//				near = nearer (near, GetPoint (toGrid (now, 'x') + 1, toGrid (now, 'z')), start);
//			}
//			ans.Push (near);
//			now = near;
//		}
//
//		return ans;
//	}
}
