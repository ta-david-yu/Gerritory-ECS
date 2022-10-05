using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movement
{
	public enum Type
	{
		Right = 0,
		Down,
		Left,
		Up,

		Stay // Don't move at all
	}

	public static readonly Vector2Int[] TypeToOffset = new Vector2Int[5] { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.zero };
}

[Element]
public sealed class MovementInputActionComponent : IComponent
{
	public Movement.Type Type;
	public float DecayTimer;
}
