﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
	public readonly int block2 = -3;
	public readonly int block = -2;
	public readonly int unwalkable = -1;
	public readonly int player = 0;
	public readonly int walkable = 1;
	public readonly int walkable2 = 2;
	public readonly int moveable = 5;
	public readonly int shadowable = 6;
	public readonly int pushable = 7;
	public readonly int destroyable = 8;
	public readonly int triggerable = 9;
	public readonly int wall = 10;
	public readonly int tempWalkable = 11;
	public readonly int tempWalkable2 = 12;


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
	public bool isWall;
	public bool isTempWalkable;
	public bool isTempWalkable2;

	public bool isBlueLight;
	public bool isRedLight;
	public bool isWhiteLight;
	public bool isYellowLight;

	public int GetType (){
		if (isPlayer)
			return player;
		if (isBlock)
			return block;
		if (isBlock2)
			return block2;
		if (isWalkable)
			return walkable;
		if (isWalkable2)
			return walkable2;
		if (isTempWalkable)
			return tempWalkable;
		if (isTempWalkable2)
			return tempWalkable2;
		return unwalkable;
	}
}
