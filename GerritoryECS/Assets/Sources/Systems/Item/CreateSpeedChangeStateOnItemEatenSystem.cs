using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateSpeedChangeStateOnItemEatenSystem : ReactiveSystem<ItemEntity>
{
	private readonly GameContext m_GameContext;
	private readonly ItemContext m_ItemContex;
	private readonly PlayerStateContext m_PlayerStateContext;

	private readonly PlayerStateEntity[] m_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];

	public CreateSpeedChangeStateOnItemEatenSystem(Contexts contexts) : base(contexts.Item)
	{
		m_GameContext = contexts.Game;
		m_ItemContex = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;
	}

	protected override ICollector<ItemEntity> GetTrigger(IContext<ItemEntity> context)
	{
		return context.CreateCollector(ItemMatcher.Eaten.Added());
	}

	protected override bool Filter(ItemEntity entity)
	{
		return entity.HasOnTileItem && entity.HasApplySpeedChangeStateOnEaten && entity.HasEaten;
	}

	protected override void Execute(List<ItemEntity> entities)
	{
		// Create SpeedChangeState entity for the target state holder.
		foreach (var powerupEntity in entities)
		{
			GameEntity eaterEntity = m_GameContext.GetEntityWithItemEater(powerupEntity.Eaten.EaterId);

			if (!eaterEntity.HasStateHolder)
			{
				// The eater doesn't hold a state, skip it.
				continue;
			}

			int stateHolderId = eaterEntity.StateHolder.Id;

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
			newStateEntity.AddStateTimer(powerupEntity.ApplySpeedChangeStateOnEaten.Duration);
			newStateEntity.AddSpeedChangeState(powerupEntity.ApplySpeedChangeStateOnEaten.SpeedMultiplier);
		}
	}
}
