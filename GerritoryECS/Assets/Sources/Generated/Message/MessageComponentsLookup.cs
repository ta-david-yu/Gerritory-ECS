//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public static class MessageComponentsLookup
{
	public const int Consumed = 0;
	public const int ConsumeInFixedUpdate = 1;
	public const int LeaveBecauseOfDeath = 2;
	public const int OnTileElementEnterTile = 3;
	public const int OnTileElementLeaveTile = 4;
	public const int OnTileElementRespawn = 5;

	public const int TotalComponents = 6;

	public static readonly string[] ComponentNames =
	{
		"Consumed",
		"ConsumeInFixedUpdate",
		"LeaveBecauseOfDeath",
		"OnTileElementEnterTile",
		"OnTileElementLeaveTile",
		"OnTileElementRespawn"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ConsumedComponent),
		typeof(ConsumeInFixedUpdateComponent),
		typeof(LeaveBecauseOfDeathComponent),
		typeof(OnTileElementEnterTileComponent),
		typeof(OnTileElementLeaveTileComponent),
		typeof(OnTileElementRespawnComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ConsumedComponent), 0 },
		{ typeof(ConsumeInFixedUpdateComponent), 1 },
		{ typeof(LeaveBecauseOfDeathComponent), 2 },
		{ typeof(OnTileElementEnterTileComponent), 3 },
		{ typeof(OnTileElementLeaveTileComponent), 4 },
		{ typeof(OnTileElementRespawnComponent), 5 }
	};

	/// <summary>
	/// Returns a component index based on the passed <paramref name="component"/> type; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="component"></param>
	public static int GetComponentIndex(IComponent component)
	{
		return GetComponentIndex(component.GetType());
	}

	/// <summary>
	/// Returns a component index based on the passed <paramref name="componentType"/>; where an index cannot be found
	/// -1 will be returned instead.
	/// </summary>
	/// <param name="componentType"></param>
	public static int GetComponentIndex(Type componentType)
	{
		return ComponentTypeToIndex.TryGetValue(componentType, out var index) ? index : -1;
	}
}
