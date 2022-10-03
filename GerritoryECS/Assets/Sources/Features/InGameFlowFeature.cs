using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InGameFlowFeature : Feature
{
	public InGameFlowFeature(Contexts contexts)
	{
		// In-game state machine system
		Add(new InGameStateMachineSystem(contexts));

		// Setup game rule related systems
		Add(new SetupGameRuleSystem(contexts));

		// Update general game info systems
		Add(new UpdateTeamGameRankingSystem(contexts));

		// End game check systems
		Add(new CheckEliminationConditionSystem(contexts));
		Add(new CheckGoalConditionSystem(contexts));
		Add(new CheckTimeoutConditionSystem(contexts));

		// Elimination setup system
		Add(new DisableRespawnForPlayerInEliminationConditionSystem(contexts));
	}
}
