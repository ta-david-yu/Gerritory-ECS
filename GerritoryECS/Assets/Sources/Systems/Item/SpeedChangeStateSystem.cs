using JCMG.EntitasRedux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpeedChangeStateSystem : IInitializeSystem, ITearDownSystem
{
	private readonly GameContext m_GameContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly IGroup<PlayerStateEntity> m_SpeedChangeStateGroup;

	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnNewSpeedChangeStateCreated;
	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnSpeedChangeStateRemoved;

	public SpeedChangeStateSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
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

		// TODO:
		// ...
		Debug.Log($"Speed change: {entity.SpeedChangeState.SpeedMultiplier}, On state holder: {entity.State.HolderId}");
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
			Debug.LogWarning("Cannot aquire State component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		float speedMultiplier = 1.0f;
		if (entity.HasSpeedChangeState)
		{
			speedMultiplier = entity.SpeedChangeState.SpeedMultiplier;
		}
		else if (component is SpeedChangeState)
		{
			speedMultiplier = (component as SpeedChangeState).SpeedMultiplier;
		}
		else
		{
			Debug.LogWarning("Cannot aquire SpeedChangeState component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		// TODO: recover the speed
		// ...
		Debug.Log($"Speed recovered: {speedMultiplier}, On state holder: {stateHolderId}");
	}

}
