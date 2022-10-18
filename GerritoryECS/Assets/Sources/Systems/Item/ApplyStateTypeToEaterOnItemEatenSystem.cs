using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ApplyStateTypeToEaterOnItemEatenSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly ItemContext m_ItemContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ItemEntity> m_EatenStateTypeItemGroup;

	public ApplyStateTypeToEaterOnItemEatenSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_ItemContext = contexts.Item;
		m_PlayerStateContext = contexts.PlayerState;
		m_ConfigContext = contexts.Config;
		m_Contexts = contexts;
		m_EatenStateTypeItemGroup = m_ItemContext.GetGroup(ItemMatcher.AllOf(ItemMatcher.OnTileItem, ItemMatcher.ApplyStateTypeToEaterOnEaten, ItemMatcher.Eaten));
	}

	public void FixedUpdate()
	{
		IStateTypeFactory stateTypeFactory = m_ConfigContext.GameConfig.value.StateTypeFactory;

		foreach (var powerupEntity in m_EatenStateTypeItemGroup.GetEntities())
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
			StateTypeEnum newStateType = powerupEntity.ApplyStateTypeToEaterOnEaten.StateType;
			var result = m_Contexts.TryAddPlayerStateTypeFor(newStateType, stateHolderId);
			if (!result.Success)
			{
				Debug.LogError($"Powerup StateType '{powerupEntity.ApplyStateTypeToEaterOnEaten.StateType}' doesn't have a valid blueprint provided. " +
					$"Therefore the state is not applied to the state holder.");
			}

			// Consume & destroy the item.
			powerupEntity.Destroy();
		}
	}
}
