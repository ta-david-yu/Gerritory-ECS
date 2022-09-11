using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateWaitingForRespawnStateOnDeathSystem : ReactiveSystem<GameEntity>
{
	private readonly GameContext m_GameContext;
	private readonly PlayerStateContext m_PlayerStateContext;

	private readonly PlayerStateEntity[] m_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];

	public CreateWaitingForRespawnStateOnDeathSystem(Contexts contexts) : base(contexts.Game)
	{
		m_GameContext = contexts.Game;
		m_PlayerStateContext = contexts.PlayerState;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.CanBeDead, GameMatcher.Dead));
	}

	protected override bool Filter(GameEntity entity)
	{
		return entity.IsDead;
	}

	protected override void Execute(List<GameEntity> entities)
	{
		foreach (var deadEntity in entities)
		{
			if (!deadEntity.HasStateHolder)
			{
				// The dead entity cannot hold a state, skip it.
				continue;
			}

			int stateHolderId = deadEntity.StateHolder.Id;
			var playerStateEntitiesSet = m_PlayerStateContext.GetEntitiesWithState(stateHolderId);
			if (playerStateEntitiesSet.Count > 1)
			{
				Debug.LogWarning($"There should only be at most 1 state targetting a state holder at the same time, but there are {playerStateEntitiesSet.Count}." +
					$"Something could be wrong!");
			}

			// We need to do copy the list because playerStateEntitiesSet is a shared HashSet that could be modified during foreach iteration, which is not allowed.
			// Thus we copy HashSet to a preallocated array to avoid heap/GC allocation, and do the destruction process with that array.
			playerStateEntitiesSet.CopyTo(m_PreallocatedPlayerStateEntitiesToBeDestroyed);
			for (int i = 0; i < playerStateEntitiesSet.Count; i++)
			{
				var playerStateEntity = m_PreallocatedPlayerStateEntitiesToBeDestroyed[i];
				// Remove existing states targetting the holder because there should only be 1 state at a time.
				playerStateEntity.Destroy();
			}

			// Create a new state entity targetting the holder.
			PlayerStateEntity newStateEntity = m_PlayerStateContext.CreateEntity();
			newStateEntity.AddState(stateHolderId);
			newStateEntity.AddTimer(GameConstants.WaitingForRespawnDuration);
			newStateEntity.AddWaitingForRespawnState(newRespawnAreaId: 0);	// TODO: add custom respawn area id here
		}
	}
}
