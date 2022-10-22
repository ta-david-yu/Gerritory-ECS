using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JCMG.EntitasRedux;

public static partial class GameHelper
{
	public static int GetNewOnTileElementId(this LevelContext context)
	{
		int id = context.OnTileElementIdCounter.value.Value;
		context.ReplaceOnTileElementIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewItemEaterId(this LevelContext context)
	{
		int id = context.ItemEaterIdCounter.value.Value;
		context.ReplaceItemEaterIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewStateHolderId(this LevelContext context)
	{
		int id = context.StateHolderIdCounter.value.Value;
		context.ReplaceStateHolderIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	public static int GetNewItemSpawnerId(this LevelContext context)
	{
		int id = context.ItemSpawnerIdCounter.value.Value;
		context.ReplaceItemSpawnerIdCounter(new UniqueIdCounter { Value = id + 1 });
		return id;
	}

	/// <summary>
	/// Use this instead of InputContext.DestoryAllEntities, because some of the input components hold unmanaged data (i.e. AIInput.SearchSimulationState)
	/// </summary>
	/// <param name="context"></param>
	public static void DeallocateAndDestroyAllEntities(this InputContext context)
	{
		// Deallocate AIInput simulation state data
		var aiInputGroup = context.GetGroup(InputMatcher.AIInput);
		foreach (var inputEntity in aiInputGroup)
		{
			if (inputEntity.HasEvaluatingForMovementInput)
			{
				inputEntity.EvaluatingForMovementInput.JobHandle.Complete();
				inputEntity.EvaluatingForMovementInput.Job.ResultContainer.Dispose();
			}

			inputEntity.AIInput.SearchSimulationState.Deallocate();
		}

		context.DestroyAllEntities();
	}
}