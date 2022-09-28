using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateWaitingForRespawnStateOnDeathSystem : ReactiveSystem<GameEntity>
{
	private readonly GameContext m_GameContext;
	private readonly PlayerStateContext m_PlayerStateContext;

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
		return entity.IsDead && entity.IsCanRespawnAfterDeath;
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

			// Remove existing states targetting the holder because there should only be 1 state at a time.
			m_PlayerStateContext.RemovePlayerStateFor(stateHolderId);

			// Create a new state entity targetting the holder.
			PlayerStateEntity newStateEntity = m_PlayerStateContext.CreateEntity();
			newStateEntity.AddState(stateHolderId);
			newStateEntity.AddTimer(GameConstants.WaitingForRespawnDuration);
			newStateEntity.AddWaitingForRespawnState(newRespawnAreaId: 0);	// TODO: add custom respawn area id here
		}
	}
}
