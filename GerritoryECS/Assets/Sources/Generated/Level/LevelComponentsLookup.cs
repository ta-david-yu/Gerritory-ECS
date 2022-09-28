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
	public const int EndOnEliminated = 0;
	public const int EndOnGoalReached = 1;
	public const int EndOnTimeout = 2;
	public const int GameInfo = 3;
	public const int GameOver = 4;
	public const int GameTimer = 5;
	public const int ItemEaterIdCounter = 6;
	public const int Level = 7;
	public const int OnTileElementIdCounter = 8;
	public const int ScoreObjective = 9;
	public const int StateHolderIdCounter = 10;
	public const int SurvivalObjective = 11;
	public const int TeamGameRanking = 12;
	public const int TeamInfo = 13;
	public const int TeamScore = 14;

	public const int TotalComponents = 15;

	public static readonly string[] ComponentNames =
	{
		"EndOnEliminated",
		"EndOnGoalReached",
		"EndOnTimeout",
		"GameInfo",
		"GameOver",
		"GameTimer",
		"ItemEaterIdCounter",
		"Level",
		"OnTileElementIdCounter",
		"ScoreObjective",
		"StateHolderIdCounter",
		"SurvivalObjective",
		"TeamGameRanking",
		"TeamInfo",
		"TeamScore"
	};

	public static readonly System.Type[] ComponentTypes =
	{
		typeof(EndOnEliminatedComponent),
		typeof(EndOnGoalReachedComponent),
		typeof(EndOnTimeoutComponent),
		typeof(GameInfoComponent),
		typeof(GameOverComponent),
		typeof(GameTimerComponent),
		typeof(ItemEaterIdCounterComponent),
		typeof(LevelComponent),
		typeof(OnTileElementIdCounterComponent),
		typeof(ScoreObjectiveComponent),
		typeof(StateHolderIdCounterComponent),
		typeof(SurvivalObjectiveComponent),
		typeof(TeamGameRankingComponent),
		typeof(TeamInfoComponent),
		typeof(TeamScoreComponent)
	};

	public static readonly Dictionary<Type, int> ComponentTypeToIndex = new Dictionary<Type, int>
	{
		{ typeof(EndOnEliminatedComponent), 0 },
		{ typeof(EndOnGoalReachedComponent), 1 },
		{ typeof(EndOnTimeoutComponent), 2 },
		{ typeof(GameInfoComponent), 3 },
		{ typeof(GameOverComponent), 4 },
		{ typeof(GameTimerComponent), 5 },
		{ typeof(ItemEaterIdCounterComponent), 6 },
		{ typeof(LevelComponent), 7 },
		{ typeof(OnTileElementIdCounterComponent), 8 },
		{ typeof(ScoreObjectiveComponent), 9 },
		{ typeof(StateHolderIdCounterComponent), 10 },
		{ typeof(SurvivalObjectiveComponent), 11 },
		{ typeof(TeamGameRankingComponent), 12 },
		{ typeof(TeamInfoComponent), 13 },
		{ typeof(TeamScoreComponent), 14 }
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
