using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGamePiece : MonoBehaviour {
	public enum eObjectType{
		//Non-physical
		Undefined,
		Canvas,
		Data,

		//Tile Brushes
		Tile,
		Exit,

		//Interactive Tiles
		Decoration,

		//Scripted
		Player,
		Pawn,
		Item
	}
	public eObjectType objectType;
}
