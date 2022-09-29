using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InGameFlowFeature : Feature
{
	public InGameFlowFeature(Contexts contexts)
	{
		// Setup game info related systems
		Add(new SetupGameInfoSystem(contexts));
		Add(new SetupGameRuleSystem(contexts));

		// In-game state machine system
		Add(new InGameStateMachineSystem(contexts));

		// Update general game info systems
		Add(new UpdateGameInfoSystem(contexts));
		Add(new UpdateTeamGameRankingSystem(contexts));

		// End game check systems
		Add(new CheckEliminationConditionSystem(contexts));
		Add(new CheckGoalConditionSystem(contexts));
		Add(new CheckTimeoutConditionSystem(contexts));
	}
}
