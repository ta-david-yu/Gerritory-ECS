using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TryCommandSpawnItemResult
{
	public bool Success;
	public CommandEntity CommandEntity;
}

public static partial class GameHelper
{
	public static TryCommandSpawnItemResult TryCommandSpawnItemAt(this Contexts contexts, IItemData itemData, Vector2Int tilePosition)
	{
		ItemEntity itemEntity = contexts.Item.GetEntityWithOnTileItem(tilePosition);
		if (itemEntity != null)
		{
			// There is already an item on the given location. Command not sent.
			Debug.LogWarning($"There is already an item on the given position {tilePosition}.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		TileEntity tileEntity = contexts.Tile.GetEntityWithTilePosition(tilePosition);
		if (!tileEntity.IsItemHolder)
		{
			// The tile at the given position is not an item holder.
			Debug.LogWarning($"The tile at the given position {tilePosition} is not an item holder.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		if (!tileEntity.IsEnterable)
		{
			// Normally the system that creates the spawn command should check if the location is valid first!
			Debug.LogWarning($"The tile at the given position {tilePosition} is not enterable.");
			return new TryCommandSpawnItemResult { Success = false };
		}

		// Create spawn item command entity.
		var command = contexts.Command.CreateEntity();
		command.AddSpawnItem(tilePosition, itemData);
		return new TryCommandSpawnItemResult { Success = true, CommandEntity = command };
	}

	public static ItemEntity CreateGlobalItemSpawner(this Contexts contexts, GlobalItemSpawnerConfig config)
	{
		ItemEntity spawnerEntity = contexts.Item.CreateEntity();
		spawnerEntity.AddGlobalItemSpawner(contexts.Level.GetNewItemSpawnerId(), config.ItemDataPool);
		spawnerEntity.AddSpawnInterval(config.MinimumSpawnInterval, config.MaximumSpawnInterval);
		spawnerEntity.AddMaxNumberOfItemsInLevel(config.MaxNumberOfItemsOnLevelAtTheSameTime);
		return spawnerEntity;
	}

	public static ItemEntity ConstructGlobalSpawnerItemAtPosition(this Contexts contexts, IItemData itemData, Vector2Int position, int globalSpawnerId)
	{
		ItemEntity itemEntity = contexts.Item.CreateEntity();
		IEntityCreationEventController viewController = itemData.CreateItemView();
		viewController.OnEntityCreated(contexts, itemEntity);

		IItemBlueprint itemBlueprint = itemData.ItemBlueprint;
		itemBlueprint.ApplyToEntity(itemEntity);
		itemEntity.ReplaceOnTileItem(position);
		itemEntity.AddSpawnedByGlobalSpawner(globalSpawnerId);
		viewController.OnComponentsAdded(contexts, itemEntity);

		// Link view controller with entity
		viewController.Link(itemEntity);

		return itemEntity;
	}

	public static ItemEntity ConstructItemAtPosition(this Contexts contexts, IItemData itemData, Vector2Int position)
	{
		ItemEntity itemEntity = contexts.Item.CreateEntity();
		IEntityCreationEventController viewController = itemData.CreateItemView();
		viewController.OnEntityCreated(contexts, itemEntity);

		IItemBlueprint itemBlueprint = itemData.ItemBlueprint;
		itemBlueprint.ApplyToEntity(itemEntity);
		itemEntity.ReplaceOnTileItem(position);
		viewController.OnComponentsAdded(contexts, itemEntity);

		// Link view controller with entity
		viewController.Link(itemEntity);

		return itemEntity;
	}
}
