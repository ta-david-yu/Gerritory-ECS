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

public static class LevelComponentsLookup
{
	public const int GameInfo = 0;
	public const int GameTimer = 1;
	public const int ItemEaterIdCounter = 2;
	public const int OnTileElementIdCounter = 3;
	public const int StateHolderIdCounter = 4;
	public const int TeamGameRanking = 5;
	public const int TeamInfo = 6;
	public const int TeamScore = 7;

	public const int TotalComponents = 8;

	public static readonly string[] ComponentNames =
	{
		"GameInfo",
		"GameTimer",
		"ItemEaterIdCounter",
		"OnTileElementIdCounter",
		"StateHolderIdCounter",
		"TeamGameRanking",
		"TeamInfo",
		"TeamScore"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(GameInfoComponent),
		typeof(GameTimerComponent),
		typeof(ItemEaterIdCounterComponent),
		typeof(OnTileElementIdCounterComponent),
		typeof(StateHolderIdCounterComponent),
		typeof(TeamGameRankingComponent),
		typeof(TeamInfoComponent),
		typeof(TeamScoreComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(GameInfoComponent), 0 },
		{ typeof(GameTimerComponent), 1 },
		{ typeof(ItemEaterIdCounterComponent), 2 },
		{ typeof(OnTileElementIdCounterComponent), 3 },
		{ typeof(StateHolderIdCounterComponent), 4 },
		{ typeof(TeamGameRankingComponent), 5 },
		{ typeof(TeamInfoComponent), 6 },
		{ typeof(TeamScoreComponent), 7 }
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
