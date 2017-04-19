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
	public readonly int moveable = 5;
	public readonly int shadowable = 6;
	public readonly int pushable = 7;
	public readonly int destroyable = 8;
	public readonly int triggerable = 9;
	public readonly int wall = 10;
	public readonly int tempWalkable = 11;
	public readonly int tempWalkable2 = 12;
	public readonly int mirror = 13;
	public readonly int lever = 14;
	public readonly int enemy = 15;

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
	public bool isMirror;
	public bool isEnemy;

	public bool isBlueLight;
	public bool isRedLight;
	public bool isWhiteLight;
	public bool isYellowLight;

	public bool isLever;

	private GameObject pic;
	private GameObject show;

	void Awake(){
		pic = Resources.Load ("MovePic", typeof(GameObject)) as GameObject;

		if (isMoveable) {
			if (isBlock || isBlock2) {
				if (show == null)
					show = Instantiate (pic, transform.position + Vector3.up * 0.5001f, Quaternion.LookRotation (Vector3.forward), transform);
			} else {
				
			}
		}
	}

	public int GetType (){
		if (isPlayer)
			return player;
		if (isEnemy)
			return enemy;
		if (isMirror)
			return mirror;
		if (isLever)
			return lever;
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
