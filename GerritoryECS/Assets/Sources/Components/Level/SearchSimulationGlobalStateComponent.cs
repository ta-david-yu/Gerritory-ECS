using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A unique component that caches sorted tiles list based on their position.
/// </summary>
[Level, Unique]
public sealed class SearchSimulationGlobalStateComponent : IComponent
{
	public Vector2Int[] SortedTilePositions;
}
