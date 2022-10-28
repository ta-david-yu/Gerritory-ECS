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
	public static readonly Movement.Type[] TypeList = new Type[] { Type.Right, Type.Down, Type.Left, Type.Up, Type.Stay };

	public const int k_NumberOfMovements = 5;

	public static readonly Vector2Int[] TypeToOffset = new Vector2Int[5] { Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.zero };

	public static bool TryGetTypeFromOffset(Vector2Int moveOffset, out Type type)
	{
		for (int i = 0; i < TypeToOffset.Length; i++)
		{
			var offset = TypeToOffset[i];
			if (offset == moveOffset)
			{
				type = TypeList[i];
				return true;
			}
		}
		type = Type.Stay;
		return false;
	}
}

[Element]
public sealed class MovementInputActionComponent : IComponent
{
	public Movement.Type Type;
	public float DecayTimer;
}
