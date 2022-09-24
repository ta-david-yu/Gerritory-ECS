using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RespawnOnWaitingForRespawnStateRemovedSystem : IInitializeSystem, ITearDownSystem
{
	private readonly Contexts m_Contexts;
	private readonly GameContext m_GameContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly IGroup<PlayerStateEntity> m_WaitingForRespawnStateGroup;

	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnWaitingFoRespawnStateRemoved;

	public RespawnOnWaitingForRespawnStateRemovedSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_GameContext = contexts.Game;
		m_PlayerStateContext = contexts.PlayerState;

		m_WaitingForRespawnStateGroup = m_PlayerStateContext.GetGroup(PlayerStateMatcher.AllOf(PlayerStateMatcher.State, PlayerStateMatcher.WaitingForRespawnState));

		// Cache delegates to avoid GC allocations.
		m_CachedHandleOnWaitingFoRespawnStateRemoved = handleOnWaitingForRespawnStateRemoved;
	}

	public void Initialize()
	{
		m_WaitingForRespawnStateGroup.OnEntityRemoved += m_CachedHandleOnWaitingFoRespawnStateRemoved;
	}

	public void TearDown()
	{
		m_WaitingForRespawnStateGroup.OnEntityRemoved -= m_CachedHandleOnWaitingFoRespawnStateRemoved;
	}

	private void handleOnWaitingForRespawnStateRemoved(IGroup<PlayerStateEntity> group, PlayerStateEntity entity, int index, IComponent component)
	{
		// Respawn the state holder at a valid location.
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

		int targetRespawnAreaId = 0;
		if (entity.HasWaitingForRespawnState)
		{
			targetRespawnAreaId = entity.WaitingForRespawnState.RespawnAreaId;
		}
		else if (component is WaitingForRespawnState)
		{
			targetRespawnAreaId = (component as WaitingForRespawnState).RespawnAreaId;
		}
		else
		{
			Debug.LogWarning("Cannot acquire WaitingForRespawnState component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		// Find a valid position for respawn and revive the state holder entity.
		var stateHolderEntity = m_GameContext.GetEntityWithStateHolder(stateHolderId);
		TryGetValidRespawnPositionResult respawnPositionQueryResult = m_Contexts.TryGetValidRespawnPositionOfAreaIdFor(stateHolderEntity, targetRespawnAreaId);
		if (respawnPositionQueryResult.Success)
		{
			stateHolderEntity.IsDead = false;
			stateHolderEntity.ReplaceOnTilePosition(respawnPositionQueryResult.TilePosition);
		}
		else
		{
			Debug.LogWarning($"Cannot find a proper respawn position!");
		}

		Debug.Log($"State holder {stateHolderId} respawned on {respawnPositionQueryResult.TilePosition}, OnTileElementId {stateHolderEntity.OnTileElement.Id}");
	}
}
