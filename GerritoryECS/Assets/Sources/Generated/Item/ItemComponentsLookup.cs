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

public static class ItemComponentsLookup
{
	public const int ApplySpeedChangeStateForEaterOnEaten = 0;
	public const int ApplyStateTypeToEaterOnEaten = 1;
	public const int DebugMessage = 2;
	public const int Eaten = 3;
	public const int GlobalItemSpawner = 4;
	public const int MaxNumberOfItemsInLevel = 5;
	public const int OnTileItem = 6;
	public const int SpawnedByGlobalSpawner = 7;
	public const int SpawnInterval = 8;
	public const int SpawnItemTimer = 9;

	public const int TotalComponents = 10;

	public static readonly string[] ComponentNames =
	{
		"ApplySpeedChangeStateForEaterOnEaten",
		"ApplyStateTypeToEaterOnEaten",
		"DebugMessage",
		"Eaten",
		"GlobalItemSpawner",
		"MaxNumberOfItemsInLevel",
		"OnTileItem",
		"SpawnedByGlobalSpawner",
		"SpawnInterval",
		"SpawnItemTimer"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(ApplySpeedChangeStateForEaterOnEatenComponent),
		typeof(ApplyStateTypeToEaterOnEatenComponent),
		typeof(DebugMessageComponent),
		typeof(EatenComponent),
		typeof(GlobalItemSpawnerComponent),
		typeof(MaxNumberOfItemsInLevelComponent),
		typeof(OnTileItemComponent),
		typeof(SpawnedByGlobalSpawnerComponent),
		typeof(SpawnIntervalComponent),
		typeof(SpawnItemTimerComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(ApplySpeedChangeStateForEaterOnEatenComponent), 0 },
		{ typeof(ApplyStateTypeToEaterOnEatenComponent), 1 },
		{ typeof(DebugMessageComponent), 2 },
		{ typeof(EatenComponent), 3 },
		{ typeof(GlobalItemSpawnerComponent), 4 },
		{ typeof(MaxNumberOfItemsInLevelComponent), 5 },
		{ typeof(OnTileItemComponent), 6 },
		{ typeof(SpawnedByGlobalSpawnerComponent), 7 },
		{ typeof(SpawnIntervalComponent), 8 },
		{ typeof(SpawnItemTimerComponent), 9 }
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
