using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movement
{
	[System.Serializable]
	public enum Type
	{
		Right = 0,
		Down,
		Left,
		Up,
	}

	public static readonly Vector2Int[] TypeToOffset = new Vector2Int[4] { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up };
}

[Game]
public sealed class MovementInputActionComponent : IComponent
{
	public Movement.Type Type;
}
