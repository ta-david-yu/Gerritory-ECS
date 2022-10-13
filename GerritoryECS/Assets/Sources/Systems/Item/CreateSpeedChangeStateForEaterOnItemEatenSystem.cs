using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreateSpeedChangeStateForEaterOnItemEatenSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly ItemContext m_ItemContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ItemEntity> m_EatenSpeedChangePowerupGroup;

	private readonly PlayerStateEntity[] m_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];

	public CreateSpeedChangeStateForEaterOnItemEatenSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_ItemContext = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;
		m_Contexts = contexts;
		m_EatenSpeedChangePowerupGroup = m_ItemContext.GetGroup(ItemMatcher.AllOf(ItemMatcher.OnTileItem, ItemMatcher.ApplySpeedChangeStateForEaterOnEaten, ItemMatcher.Eaten));
	}

	public void FixedUpdate()
	{
		foreach (var powerupEntity in m_EatenSpeedChangePowerupGroup.GetEntities())
		{
			ElementEntity eaterEntity = m_ElementContext.GetEntityWithItemEater(powerupEntity.Eaten.EaterId);
			if (!eaterEntity.HasStateHolder)
			{
				// The eater cannot hold a state, skip it.
				continue;
			}

			int stateHolderId = eaterEntity.StateHolder.Id;

			// Remove existing states targetting the holder because there should only be 1 state at a time.
			m_Contexts.RemovePlayerStateFor(stateHolderId);

			// Create a new state entity targetting the holder.
			PlayerStateEntity newStateEntity = m_Contexts.AddPlayerStateFor(stateHolderId);
			newStateEntity.AddTimer(powerupEntity.ApplySpeedChangeStateForEaterOnEaten.Duration);
			newStateEntity.AddSpeedChangeState(powerupEntity.ApplySpeedChangeStateForEaterOnEaten.SpeedMultiplier);

			// Consume & destroy the item.
			powerupEntity.Destroy();
		}
	}
}
