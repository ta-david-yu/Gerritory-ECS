using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class RespawnOnWaitingForRespawnStateRemovedSystem : IInitializeSystem, ITearDownSystem
{
	private readonly Contexts m_Contexts;
	private readonly ElementContext m_ElementContext;
	private readonly MessageContext m_MessageContext;
	private readonly PlayerStateContext m_PlayerStateContext;
	private readonly IGroup<PlayerStateEntity> m_WaitingForRespawnStateGroup;

	private readonly GroupChanged<PlayerStateEntity> m_CachedHandleOnWaitingFoRespawnStateRemoved;

	public RespawnOnWaitingForRespawnStateRemovedSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_ElementContext = contexts.Element;
		m_MessageContext = contexts.Message;
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
		else if (component is WaitingForRespawnStateComponent)
		{
			targetRespawnAreaId = (component as WaitingForRespawnStateComponent).RespawnAreaId;
		}
		else
		{
			Debug.LogWarning("Cannot acquire WaitingForRespawnState component from group.removed event. Therefore state removal cannot be properly applied to the state holder.");
			return;
		}

		// Find a valid position for respawn and revive the state holder entity.
		var stateHolderEntity = m_ElementContext.GetEntityWithStateHolder(stateHolderId);
		TryGetValidRespawnPositionResult respawnPositionQueryResult = m_Contexts.TryGetValidRespawnPositionOfAreaIdFor(stateHolderEntity, targetRespawnAreaId);
		if (respawnPositionQueryResult.Success)
		{
			stateHolderEntity.IsDead = false;
			stateHolderEntity.ReplaceOnTilePosition(respawnPositionQueryResult.TilePosition);

			if (stateHolderEntity.HasOnTileElement)
			{
				m_MessageContext.EmitOnTileElementRespawnMessage(stateHolderEntity.OnTileElement.Id, respawnPositionQueryResult.TilePosition);
				m_MessageContext.EmitOnTileElementEnterTileMessage(stateHolderEntity.OnTileElement.Id, respawnPositionQueryResult.TilePosition);
			}
			else
			{
				Debug.LogWarning($"The respawned state holder is not an OnTileElement. EnterTile message is not emitted.");
			}
		}
		else
		{
			Debug.LogWarning($"Cannot find a proper respawn position!");
			// TODO: respawn later
			// ...
		}

		Debug.Log($"State holder {stateHolderId} respawned on {respawnPositionQueryResult.TilePosition}, OnTileElementId {stateHolderEntity.OnTileElement.Id}");
	}
}
