using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
	public readonly int block = -2;
	public readonly int unwalkable = -1;
	public readonly int player = 0;
	public readonly int walkable = 1;
	public readonly int moveable = 2;
	public readonly int shadowable = 3;
	public readonly int pushable = 5;
	public readonly int destroyable = 7;
	public readonly int triggerable = 11;

	public bool isBlock;
	public bool isUnwalkable;
	public bool isPlayer;
	public bool isWalkable;
	public bool isMoveable;
	public bool isShadowable;
	public bool isPushable;
	public bool isDestroyable;
	public bool isTriggerable;

	void Awake (){
		
	}
}
