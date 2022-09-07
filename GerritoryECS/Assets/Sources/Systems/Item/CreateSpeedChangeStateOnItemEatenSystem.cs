using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateSpeedChangeStateOnItemEatenSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly ItemContext m_ItemContext;
	private readonly PlayerStateContext m_PlayerStateContext;

	private readonly IGroup<ItemEntity> m_EatenSpeedChangePowerupGroup;

	private readonly PlayerStateEntity[] m_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];

	public CreateSpeedChangeStateOnItemEatenSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_ItemContext = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;
		m_EatenSpeedChangePowerupGroup = m_ItemContext.GetGroup(ItemMatcher.AllOf(ItemMatcher.OnTileItem, ItemMatcher.ApplySpeedChangeStateOnEaten, ItemMatcher.Eaten));
	}

	public void FixedUpdate()
	{
		foreach (var powerupEntity in m_EatenSpeedChangePowerupGroup.GetEntities())
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

			// Consume & destroy the item.
			powerupEntity.Destroy();
		}
	}
}
