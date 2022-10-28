using JCMG.EntitasRedux;
using UnityEngine;

/// <summary>
/// An input entity with this component would try to chase the nearest OnTileElementComponent entity 
/// that is killable by the controlling OnTileElement. Namely elements that 1. can be dead, 2. has lower priority
/// </summary>
[Input]
public sealed class ChaseNearestOnTileElementVictimInputComponent : IComponent
{
	/// <summary>
	/// The id of the on tile element this component is controlling.
	/// </summary>
	[PrimaryEntityIndex]
	public int ControllingElementId;

	/// <summary>
	/// This AI system would filter out <see cref="OnTileElementComponent"/>s that are located outside of this heuristic range distance.
	/// </summary>
	public int MaxSearchHeuristicDistance;

	public AIHelper.PathfindingSimulationState PathfindingSimulationState;
}
