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

	public int GetGrid (Vector3 idx){
		return grid [(int)((idx.x > 0f ? (int)idx.x : (int)idx.x - 1f) - start.x), (int)((idx.z > 0f ? (int)idx.z : (int)idx.z - 1f) - start.z)];
	}
		
	public int GetGrid (int x, int z){
		return grid [x, z];
	}

	public int toGrid (Vector3 idx, char c){
		if (c == 'x')
			return (int)((idx.x > 0f ? (int)idx.x : (int)idx.x - 1f) - start.x);
		else
			return (int)((idx.z > 0f ? (int)idx.z : (int)idx.z - 1f) - start.z);
	}

	public Vector3 GetPoint (Vector3 idx){
		idx.x = (idx.x > 0f ? (int)idx.x : (int)idx.x - 1f) + 0.5f;
		idx.z = (idx.z > 0f ? (int)idx.z : (int)idx.z - 1f) + 0.5f;
		return idx;
	}

	public Vector3 GetPoint (int x, int z){
		return new Vector3 (start.x + 0.5f + x, 0f, start.z + 0.5f + z);
	}

	Vector3 nearer (Vector3 a, Vector3 b, Vector3 des){
		return Vector3.Distance (a, des) < Vector3.Distance (b, des) ? a : b;
	}

	public Queue<Vector3> findPath(Vector3 start, Vector3 stop){
		start.x = (start.x > 0f ? (int)start.x : (int)start.x - 1f) + 0.5f;
		start.z = (start.z > 0f ? (int)start.z : (int)start.z - 1f) + 0.5f;
		stop.x = (stop.x > 0f ? (int)stop.x : (int)stop.x - 1f) + 0.5f;
		stop.z = (stop.z > 0f ? (int)stop.z : (int)stop.z - 1f) + 0.5f;
		Stack<Vector3> ans = new Stack<Vector3> ();
		Queue<Vector3> q = new Queue<Vector3> ();
		Vector3 now = start;
		int[,] grid2 = grid;
		int count = 0;
		q.Enqueue (now);

		while (now != stop) {
			if (toGrid (now, 'z') > 0 && grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] == 0) {
				grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] = count;
				q.Enqueue (GetPoint (toGrid (now, 'x'), toGrid (now, 'z') - 1));
			}
			if (toGrid (now, 'z') <  lengthZ && grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] == 0) {
				grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] = count;
				q.Enqueue (GetPoint (toGrid (now, 'x'), toGrid (now, 'z') + 1));
			}
			if (toGrid (now, 'x') > 0 && grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] == 0) {
				grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] = count;
				q.Enqueue (GetPoint (toGrid (now, 'x') - 1, toGrid (now, 'z')));
			}
			if (toGrid (now, 'x') <  lengthX && grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] == 0) {
				grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] = count;
				q.Enqueue (GetPoint (toGrid (now, 'x') + 1, toGrid (now, 'z')));
			}
			q.Dequeue ();
			if (q.Count < 1)
				return ans;
			now = q.Peek ();
		}

		count = GetGrid (now);
		ans.Push (now);
		Vector3 near = now;
		while (ans.Peek () != start) {
			count--;
			if (toGrid (now, 'z') > 0 && grid2 [toGrid (now, 'x'), toGrid (now, 'z') - 1] == count) {
				near = nearer (near, GetPoint (toGrid (now, 'x'), toGrid (now, 'z') - 1), start);
			}
			if (toGrid (now, 'z') <  lengthZ && grid2 [toGrid (now, 'x'), toGrid (now, 'z') + 1] == count) {
				near = nearer (near, GetPoint (toGrid (now, 'x'), toGrid (now, 'z') + 1), start);
			}
			if (toGrid (now, 'x') > 0 && grid2 [toGrid (now, 'x') - 1, toGrid (now, 'z')] == count) {
				near = nearer (near, GetPoint (toGrid (now, 'x') - 1, toGrid (now, 'z')), start);
			}
			if (toGrid (now, 'x') <  lengthX && grid2 [toGrid (now, 'x') + 1, toGrid (now, 'z')] == count) {
				near = nearer (near, GetPoint (toGrid (now, 'x') + 1, toGrid (now, 'z')), start);
			}
			ans.Push (near);
			now = near;
		}

		return ans;
	}
}
