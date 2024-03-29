using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Group event-based system that modifies <see cref="StateHolder"/>'s speed on <see cref="SpeedChangeStateComponent"/> creation & destruction.
/// </summary>
public sealed class SpeedChangeStateSystem : IInitializeSystem, ITearDownSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly IGroup<PlayerStateEntity> m_SpeedChangeStateGroup;

	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnNewSpeedChangeStateCreated;
	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnSpeedChangeStateRemoved;

	public SpeedChangeStateSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_PlayerStateContext = contexts.PlayerState;

		m_SpeedChangeStateGroup = m_PlayerStateContext.GetGroup(PlayerStateMatcher.AllOf(PlayerStateMatcher.State, PlayerStateMatcher.SpeedChangeState));

		// Cache delegates to avoid GC allocations.
		m_CachedHandleOnNewSpeedChangeStateCreated = handleOnNewSpeedChangeStateCreated;
		m_CachedHandleOnSpeedChangeStateRemoved = handleOnSpeedChangeStateRemoved;
	}

	public void Initialize()
	{
		m_SpeedChangeStateGroup.OnEntityAdded += m_CachedHandleOnNewSpeedChangeStateCreated;
		m_SpeedChangeStateGroup.OnEntityRemoved += m_CachedHandleOnSpeedChangeStateRemoved;
	}

	public void TearDown()
	{
		m_SpeedChangeStateGroup.OnEntityAdded -= m_CachedHandleOnNewSpeedChangeStateCreated;
		m_SpeedChangeStateGroup.OnEntityRemoved -= m_CachedHandleOnSpeedChangeStateRemoved;
	}

	private void handleOnNewSpeedChangeStateCreated(IGroup<PlayerStateEntity> group, PlayerStateEntity entity, int index, IComponent component)
	{
		// Change the speed of the state holder based on the speed multiplier.
		int stateHolderId = entity.State.HolderId;
		float speedMultiplier = entity.SpeedChangeState.SpeedMultiplier;

		var stateHolderEntity = m_ElementContext.GetEntityWithOnTileElement(stateHolderId);
		if (stateHolderEntity.HasSpeedChangeable)
		{
			stateHolderEntity.ReplaceSpeedChangeable(
				stateHolderEntity.SpeedChangeable.BaseSpeed,
				stateHolderEntity.SpeedChangeable.SpeedMultiplier * speedMultiplier);
		}
		else
		{
			Debug.LogWarning($"The target state holder doesn't have SpeedChangeableComponent, therefore the speed change is not applied.");
		}
	}

	private void handleOnSpeedChangeStateRemoved(IGroup<PlayerStateEntity> group, PlayerStateEntity entity, int index, IComponent component)
	{
		// Recover the speed change previously applied on the state holder.

		int stateHolderId = -1;
		if (entity.HasState)
		{
			stateHolderId = entity.State.HolderId;
		}
		else if (component is StateComponent)
		{
			stateHolderId = (component as StateComponent).HolderId;
		}
		else
		{
			Debug.LogWarning("Cannot acquire State component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		float speedMultiplier = 1.0f;
		if (entity.HasSpeedChangeState)
		{
			speedMultiplier = entity.SpeedChangeState.SpeedMultiplier;
		}
		else if (component is SpeedChangeStateComponent)
		{
			speedMultiplier = (component as SpeedChangeStateComponent).SpeedMultiplier;
		}
		else
		{
			Debug.LogWarning("Cannot acquire SpeedChangeState component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		// Recover the speed of the state holder based on the speed multiplier.
		var stateHolderEntity = m_ElementContext.GetEntityWithStateHolder(stateHolderId);
		if (stateHolderEntity.HasSpeedChangeable)
		{
			stateHolderEntity.ReplaceSpeedChangeable(
				stateHolderEntity.SpeedChangeable.BaseSpeed,
				stateHolderEntity.SpeedChangeable.SpeedMultiplier / speedMultiplier);
		}
		else
		{
			Debug.LogWarning($"The target state holder doesn't have SpeedChangeableComponent, therefore the speed change is not recovered.");
		}
	}

}
