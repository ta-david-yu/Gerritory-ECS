using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpawnItemSystem : IFixedUpdateSystem
{
	private readonly CommandContext m_CommandContext;
	private readonly ItemContext m_ItemContext;
	private readonly TileContext m_TileContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<CommandEntity> m_SpawnItemCommandGroup;

	public SpawnItemSystem(Contexts contexts)
	{
		m_CommandContext = contexts.Command;
		m_ItemContext = contexts.Item;
		m_TileContext = contexts.Tile;
		m_Contexts = contexts;

		m_SpawnItemCommandGroup = m_CommandContext.GetGroup(CommandMatcher.SpawnItem);
	}

	public void FixedUpdate()
	{
		foreach (var spawnItemCommand in m_SpawnItemCommandGroup.GetEntities())
		{
			Vector2Int targetPosition = spawnItemCommand.SpawnItem.TilePosition;
			ItemEntity itemEntity = m_ItemContext.GetEntityWithOnTileItem(targetPosition);
			if (itemEntity != null)
			{
				// There is already an item on the given location. Item not spawned.
				// Normally the system that creates the spawn command should check if the location is valid first!
				Debug.LogError($"There is already an item on the given position {targetPosition}. Item spawn is not executed and delayed.");
				continue;
			}

			TileEntity tileEntity = m_TileContext.GetEntityWithTilePosition(targetPosition);
			if (!tileEntity.IsItemHolder)
			{
				// The tile at the given position is not an item holder.
				// Normally the system that creates the spawn command should check if the location is valid first!
				Debug.LogWarning($"The tile at the given position {targetPosition} is not an item holder. Force spawn the item.");
			}

			if (!tileEntity.IsEnterable)
			{
				// Normally the system that creates the spawn command should check if the location is valid first!
				Debug.LogWarning($"The tile at the given position {targetPosition} is not enterable. Force spawn the item.");
			}

			ItemEntity newItemEntity = m_ItemContext.CreateEntity();
			IItemBlueprint itemBlueprint = spawnItemCommand.SpawnItem.ItemBlueprint;
			itemBlueprint.ApplyToEntity(newItemEntity);
			newItemEntity.ReplaceOnTileItem(targetPosition);

			if (spawnItemCommand.HasSpawnedByGlobalSpawner)
			{
				// Label the item with the global item spawner id.
				newItemEntity.AddSpawnedByGlobalSpawner(spawnItemCommand.SpawnedByGlobalSpawner.SpawnerId);
			}

			// TODO: Load View Controller in and register events to the item entity
			// ...

			spawnItemCommand.Destroy();
		}
	}
}
