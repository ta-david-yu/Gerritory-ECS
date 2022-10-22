using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TryAddPlayerStateTypeResult
{
	public bool Success;
	public PlayerStateEntity PlayerStateEntity;
}

public static partial class GameHelper
{
	private static readonly PlayerStateEntity[] s_PreallocatedPlayerStateEntitiesToBeDestroyed = new PlayerStateEntity[4];
	public static void RemovePlayerStateFor(this Contexts contexts, int stateHolderId)
	{
		var playerStateEntitiesSet = contexts.PlayerState.GetEntitiesWithState(stateHolderId);
		if (playerStateEntitiesSet.Count > 1)
		{
			Debug.LogError($"There should only be at most 1 state targetting a state holder at the same time, but there are {playerStateEntitiesSet.Count}." +
				$"Something could be wrong!");
		}

		// We need to do copy the list because playerStateEntitiesSet is a shared HashSet that could be modified during foreach iteration, which is not allowed.
		// Thus we copy HashSet to a preallocated array to avoid heap/GC allocation, and do the destruction process with that array.
		playerStateEntitiesSet.CopyTo(s_PreallocatedPlayerStateEntitiesToBeDestroyed);
		for (int i = 0; i < playerStateEntitiesSet.Count; i++)
		{
			var playerStateEntity = s_PreallocatedPlayerStateEntitiesToBeDestroyed[i];
			contexts.DestroyPlayerStateEntity(playerStateEntity);
		}

		return;
	}

	public static void DestroyPlayerStateEntity(this Contexts contexts, PlayerStateEntity playerStateEntity)
	{
		int stateHolderId = playerStateEntity.State.HolderId;
		playerStateEntity.Destroy();

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsLeaveState = true;
	}

	public static TryAddPlayerStateTypeResult TryAddPlayerStateTypeFor(this Contexts contexts, StateTypeEnum stateType, int stateHolderId)
	{
		if (!contexts.Config.GameConfig.value.StateTypeFactory.TryGetStateBlueprint(stateType, out var blueprint))
		{
			return new TryAddPlayerStateTypeResult() { Success = false };
		}

		PlayerStateEntity newStateEntity = contexts.PlayerState.CreateEntity();
		blueprint.ApplyToEntity(newStateEntity);
		newStateEntity.AddState(stateHolderId);
		newStateEntity.AddStateFactoryType(stateType);

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsEnterState = true;

		return new TryAddPlayerStateTypeResult() { Success = true, PlayerStateEntity = newStateEntity };
	}

	public static PlayerStateEntity AddPlayerStateFor(this Contexts contexts, int stateHolderId)
	{
		PlayerStateEntity newStateEntity = contexts.PlayerState.CreateEntity();
		newStateEntity.AddState(stateHolderId);

		// Create one-frame event component on the state holder.
		ElementEntity elementEntity = contexts.Element.GetEntityWithStateHolder(stateHolderId);
		elementEntity.IsEnterState = true;

		return newStateEntity;
	}
}
