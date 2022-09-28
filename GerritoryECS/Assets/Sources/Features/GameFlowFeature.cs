using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameFlowFeature : Feature
{
	public GameFlowFeature(Contexts contexts)
	{
		// Update general game info systems
		Add(new UpdateGameInfoSystem(contexts));
		Add(new UpdateTeamGameRankingSystem(contexts));

		// End game check systems
		Add(new CheckEliminationConditionSystem(contexts));
		Add(new CheckGoalConditionSystem(contexts));
		Add(new CheckTimeoutConditionSystem(contexts));
	}
}
