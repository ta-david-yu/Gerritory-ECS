using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TimedGlobalItemSpawnerSystem : IFixedUpdateSystem
{
	private readonly CommandContext m_CommandContext;
	private readonly ItemContext m_ItemContext;
	private readonly ElementContext m_ElementContext;
	private readonly TileContext m_TileContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ItemEntity> m_TimedGlobalItemSpawnerGroup;
	private readonly IGroup<TileEntity> m_ItemSpawnableTileGroup;

	public TimedGlobalItemSpawnerSystem(Contexts contexts)
	{
		m_CommandContext = contexts.Command;
		m_ItemContext = contexts.Item;
		m_ElementContext = contexts.Element;
		m_TileContext = contexts.Tile;
		m_Contexts = contexts;

		m_TimedGlobalItemSpawnerGroup = m_ItemContext.GetGroup(ItemMatcher.AllOf(ItemMatcher.GlobalItemSpawner, ItemMatcher.SpawnInterval));
		m_ItemSpawnableTileGroup = m_TileContext.GetGroup(TileMatcher.AllOf(TileMatcher.ItemHolder, TileMatcher.Enterable));
	}

	public void FixedUpdate()
	{
		foreach (var spawnerEntity in m_TimedGlobalItemSpawnerGroup.GetEntities())
		{
			if (!spawnerEntity.HasSpawnItemTimer)
			{
				spawnerEntity.AddSpawnItemTimer(spawnerEntity.SpawnInterval.GetNextSpawnInterval());
			}

			bool hasMaxLimitOnNumberOfItemsSpawned = spawnerEntity.HasMaxNumberOfItemsInLevel;
			if (hasMaxLimitOnNumberOfItemsSpawned)
			{
				int numberOfItemsSpawnedWithSpawner = m_ItemContext.GetEntitiesWithSpawnedByGlobalSpawner(spawnerEntity.GlobalItemSpawner.Id).Count;
				if (numberOfItemsSpawnedWithSpawner >= spawnerEntity.MaxNumberOfItemsInLevel.Value)
				{
					// The number of items on the level spawned with this spawner has reached the max threshold, don't spawn any new item until the existing one is eaten.
					continue;
				}
			}


			float timer = spawnerEntity.SpawnItemTimer.Value;
			timer -= Time.fixedDeltaTime;

			if (timer <= 0.0f)
			{
				// Time is out, spawn a new item!
				// Update timer.
				spawnerEntity.ReplaceSpawnItemTimer(timer + spawnerEntity.SpawnInterval.GetNextSpawnInterval());

				// Randomly pick a tile from the candidiates list to spawn the item.
				TileEntity[] tileCandidates = m_ItemSpawnableTileGroup.GetEntities();
				TileEntity tileToSpawnOn = null;
				int randomStartIndex = UnityEngine.Random.Range(0, tileCandidates.Length);
				for (int i = 0; i < tileCandidates.Length; i++)
				{
					int index = (randomStartIndex + i) % tileCandidates.Length;
					Vector2Int position = tileCandidates[index].TilePosition.Value;
					bool isOccupied = m_Contexts.IsTileAtPositionOccupied(position);
					if (isOccupied)
					{
						continue;
					}

					tileToSpawnOn = tileCandidates[index];
				}
				
				if (tileToSpawnOn == null)
				{
					// None of the tiles are valid for spawning a new item, skip the spawn this time.
					continue;
				}

				IItemData itemToSpawn = spawnerEntity.GlobalItemSpawner.GetRandomItemFromPool();
				var commandResult = m_Contexts.TryCommandSpawnItemAt(itemToSpawn, tileToSpawnOn.TilePosition.Value);
				if (!commandResult.Success)
				{
					// Failed to spawn an item at the given location, skip the spawn this time.
					continue;
				}

				// Label the command with the spawner id.
				commandResult.CommandEntity.AddSpawnedByGlobalSpawner(spawnerEntity.GlobalItemSpawner.Id);
			}
			else
			{
				spawnerEntity.ReplaceSpawnItemTimer(timer);
			}
		}
	}
}
