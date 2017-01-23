using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
	public readonly int block2 = -3;
	public readonly int block = -2;
	public readonly int unwalkable = -1;
	public readonly int player = 0;
	public readonly int walkable = 1;
	public readonly int walkable2 = 2;
	public readonly int moveable = 3;
	public readonly int shadowable = 4;
	public readonly int pushable = 5;
	public readonly int destroyable = 6;
	public readonly int triggerable = 7;

	public bool isBlock2;
	public bool isBlock;
	public bool isUnwalkable;
	public bool isPlayer;
	public bool isWalkable;
	public bool isWalkable2;
	public bool isMoveable;
	public bool isShadowable;
	public bool isPushable;
	public bool isDestroyable;
	public bool isTriggerable;

	public int GetType (){
		if (isPlayer)
			return player;
		if (isBlock)
			return block;
		if (isBlock2)
			return block;
		if (isWalkable)
			return unwalkable;
		if (isWalkable2)
			return walkable2;
		return unwalkable;
	}
}
