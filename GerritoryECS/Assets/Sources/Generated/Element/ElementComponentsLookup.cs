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

public static class ElementComponentsLookup
{
	public const int CanBeDead = 0;
	public const int CanRespawnAfterDeath = 1;
	public const int DeadAddedListener = 2;
	public const int Dead = 3;
	public const int DeadRemovedListener = 4;
	public const int DebugMessage = 5;
	public const int EnterStateAddedListener = 6;
	public const int EnterState = 7;
	public const int IComponentRef = 8;
	public const int ItemEater = 9;
	public const int LeaveStateAddedListener = 10;
	public const int LeaveState = 11;
	public const int MovementInputAction = 12;
	public const int MoveOnTileAddedListener = 13;
	public const int MoveOnTileBeginAddedListener = 14;
	public const int MoveOnTileBegin = 15;
	public const int MoveOnTile = 16;
	public const int MoveOnTileEndAddedListener = 17;
	public const int MoveOnTileEnd = 18;
	public const int OnTileElement = 19;
	public const int OnTileElementKiller = 20;
	public const int OnTilePositionAddedListener = 21;
	public const int OnTilePosition = 22;
	public const int Player = 23;
	public const int SpeedChangeable = 24;
	public const int StateHolder = 25;
	public const int TeamAddedListener = 26;
	public const int Team = 27;
	public const int TileCollapser = 28;
	public const int TileOwner = 29;

	public const int TotalComponents = 30;

	public static readonly string[] ComponentNames =
	{
		"CanBeDead",
		"CanRespawnAfterDeath",
		"DeadAddedListener",
		"Dead",
		"DeadRemovedListener",
		"DebugMessage",
		"EnterStateAddedListener",
		"EnterState",
		"IComponentRef",
		"ItemEater",
		"LeaveStateAddedListener",
		"LeaveState",
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
		"TeamAddedListener",
		"Team",
		"TileCollapser",
		"TileOwner"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(CanBeDeadComponent),
		typeof(CanRespawnAfterDeathComponent),
		typeof(DeadAddedListenerComponent),
		typeof(DeadComponent),
		typeof(DeadRemovedListenerComponent),
		typeof(DebugMessageComponent),
		typeof(EnterStateAddedListenerComponent),
		typeof(EnterStateComponent),
		typeof(IComponentRefComponent),
		typeof(ItemEaterComponent),
		typeof(LeaveStateAddedListenerComponent),
		typeof(LeaveStateComponent),
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
		typeof(TeamAddedListenerComponent),
		typeof(TeamComponent),
		typeof(TileCollapserComponent),
		typeof(TileOwnerComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(CanBeDeadComponent), 0 },
		{ typeof(CanRespawnAfterDeathComponent), 1 },
		{ typeof(DeadAddedListenerComponent), 2 },
		{ typeof(DeadComponent), 3 },
		{ typeof(DeadRemovedListenerComponent), 4 },
		{ typeof(DebugMessageComponent), 5 },
		{ typeof(EnterStateAddedListenerComponent), 6 },
		{ typeof(EnterStateComponent), 7 },
		{ typeof(IComponentRefComponent), 8 },
		{ typeof(ItemEaterComponent), 9 },
		{ typeof(LeaveStateAddedListenerComponent), 10 },
		{ typeof(LeaveStateComponent), 11 },
		{ typeof(MovementInputActionComponent), 12 },
		{ typeof(MoveOnTileAddedListenerComponent), 13 },
		{ typeof(MoveOnTileBeginAddedListenerComponent), 14 },
		{ typeof(MoveOnTileBeginComponent), 15 },
		{ typeof(MoveOnTileComponent), 16 },
		{ typeof(MoveOnTileEndAddedListenerComponent), 17 },
		{ typeof(MoveOnTileEndComponent), 18 },
		{ typeof(OnTileElementComponent), 19 },
		{ typeof(OnTileElementKillerComponent), 20 },
		{ typeof(OnTilePositionAddedListenerComponent), 21 },
		{ typeof(OnTilePositionComponent), 22 },
		{ typeof(PlayerComponent), 23 },
		{ typeof(SpeedChangeableComponent), 24 },
		{ typeof(StateHolderComponent), 25 },
		{ typeof(TeamAddedListenerComponent), 26 },
		{ typeof(TeamComponent), 27 },
		{ typeof(TileCollapserComponent), 28 },
		{ typeof(TileOwnerComponent), 29 }
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
