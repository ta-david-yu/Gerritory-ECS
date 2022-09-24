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

public static class GameComponentsLookup
{
	public const int CanBeDead = 0;
	public const int DeadAddedListener = 1;
	public const int Dead = 2;
	public const int DeadRemovedListener = 3;
	public const int DebugMessage = 4;
	public const int IComponentRef = 5;
	public const int ItemEater = 6;
	public const int Level = 7;
	public const int MovementInputAction = 8;
	public const int MoveOnTileAddedListener = 9;
	public const int MoveOnTileBeginAddedListener = 10;
	public const int MoveOnTileBegin = 11;
	public const int MoveOnTile = 12;
	public const int MoveOnTileEndAddedListener = 13;
	public const int MoveOnTileEnd = 14;
	public const int OnTileElement = 15;
	public const int OnTileElementKiller = 16;
	public const int OnTilePositionAddedListener = 17;
	public const int OnTilePosition = 18;
	public const int Player = 19;
	public const int SpeedChangeable = 20;
	public const int StateHolder = 21;
	public const int TileCollapser = 22;
	public const int TileOwner = 23;

	public const int TotalComponents = 24;

	public static readonly string[] ComponentNames =
	{
		"CanBeDead",
		"DeadAddedListener",
		"Dead",
		"DeadRemovedListener",
		"DebugMessage",
		"IComponentRef",
		"ItemEater",
		"Level",
		"MovementInputAction",
		"MoveOnTileAddedListener",
		"MoveOnTileBeginAddedListener",
		"MoveOnTileBegin",
		"MoveOnTile",
		"MoveOnTileEndAddedListener",
		"MoveOnTileEnd",
		"OnTileElement",
		"OnTileElementKiller",
		"OnTilePositionAddedListener",
		"OnTilePosition",
		"Player",
		"SpeedChangeable",
		"StateHolder",
		"TileCollapser",
		"TileOwner"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(CanBeDeadComponent),
		typeof(DeadAddedListenerComponent),
		typeof(DeadComponent),
		typeof(DeadRemovedListenerComponent),
		typeof(DebugMessageComponent),
		typeof(IComponentRefComponent),
		typeof(ItemEaterComponent),
		typeof(LevelComponent),
		typeof(MovementInputActionComponent),
		typeof(MoveOnTileAddedListenerComponent),
		typeof(MoveOnTileBeginAddedListenerComponent),
		typeof(MoveOnTileBeginComponent),
		typeof(MoveOnTileComponent),
		typeof(MoveOnTileEndAddedListenerComponent),
		typeof(MoveOnTileEndComponent),
		typeof(OnTileElementComponent),
		typeof(OnTileElementKillerComponent),
		typeof(OnTilePositionAddedListenerComponent),
		typeof(OnTilePositionComponent),
		typeof(PlayerComponent),
		typeof(SpeedChangeableComponent),
		typeof(StateHolderComponent),
		typeof(TileCollapserComponent),
		typeof(TileOwnerComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(CanBeDeadComponent), 0 },
		{ typeof(DeadAddedListenerComponent), 1 },
		{ typeof(DeadComponent), 2 },
		{ typeof(DeadRemovedListenerComponent), 3 },
		{ typeof(DebugMessageComponent), 4 },
		{ typeof(IComponentRefComponent), 5 },
		{ typeof(ItemEaterComponent), 6 },
		{ typeof(LevelComponent), 7 },
		{ typeof(MovementInputActionComponent), 8 },
		{ typeof(MoveOnTileAddedListenerComponent), 9 },
		{ typeof(MoveOnTileBeginAddedListenerComponent), 10 },
		{ typeof(MoveOnTileBeginComponent), 11 },
		{ typeof(MoveOnTileComponent), 12 },
		{ typeof(MoveOnTileEndAddedListenerComponent), 13 },
		{ typeof(MoveOnTileEndComponent), 14 },
		{ typeof(OnTileElementComponent), 15 },
		{ typeof(OnTileElementKillerComponent), 16 },
		{ typeof(OnTilePositionAddedListenerComponent), 17 },
		{ typeof(OnTilePositionComponent), 18 },
		{ typeof(PlayerComponent), 19 },
		{ typeof(SpeedChangeableComponent), 20 },
		{ typeof(StateHolderComponent), 21 },
		{ typeof(TileCollapserComponent), 22 },
		{ typeof(TileOwnerComponent), 23 }
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
