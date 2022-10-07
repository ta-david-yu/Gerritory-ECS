using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Level, Unique]
public sealed class SearchSimulationGlobalStateComponent : IComponent
{
	public Vector2Int[] SortedTilePositions;
}
