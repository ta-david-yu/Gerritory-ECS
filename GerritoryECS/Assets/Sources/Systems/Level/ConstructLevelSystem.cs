using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build the level based on the GameConfig instance
/// </summary>
public sealed class ConstructLevelSystem : IInitializeSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private readonly ConfigContext m_ConfigContext;

	public ConstructLevelSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		TileTypeTable tileTypeTable = m_ConfigContext.GameConfig.value.TileTypeTable;

		LevelData levelData = m_ConfigContext.GameConfig.value.LevelData;
		m_GameContext.SetLevel(levelData);

		GameObject tileUnityViewRoot = new GameObject("TileUnityViewRoot");

		foreach (var tileDataPair in levelData.TileDataPairs)
		{
			Vector2Int tilePosition = tileDataPair.Key;
			string tileId = tileDataPair.Value.TileId;

			if (!tileTypeTable.TileTypes.TryGetValue(tileId, out TileType tileType))
			{
				// Use fallback tile type if the given tile id is legal.
				tileType = tileTypeTable.FallbackTileType;
			}

			var tileEntity = m_TileContext.CreateEntity();
			var tileUnityView = GameObject.Instantiate(tileType.Prefab, GameConstants.TilePositionToWorldPosition(tilePosition), Quaternion.identity, tileUnityViewRoot.transform);
			if (tileUnityView.TryGetComponent<BlueprintEntityCreationEventController>(out BlueprintEntityCreationEventController blueprintEntityCreationEventController))
			{
				// Call OnEntityCreated callback if EventController is presented.
				blueprintEntityCreationEventController.OnEntityCreated(tileEntity);
			}

			tileType.Blueprint.ApplyToEntity(tileEntity);

			// Remember to set tile position!
			tileEntity.AddTilePosition(tilePosition);
			if (blueprintEntityCreationEventController != null)
			{
				// Call OnBlueprintApplied callback if EventController is presented.
				blueprintEntityCreationEventController.OnBlueprintApplied(tileEntity);
			}

			tileUnityView.Link(tileEntity);
		}
		/*
		for (int x = 0; x < levelSize.x; x++)
		{
			for (int y = 0; y < levelSize.y; y++)
			{
				Vector2Int position = new Vector2Int(x, y);

				var tileEntity = m_TileContext.CreateEntity();
				tileEntity.AddTilePosition(position);
				tileEntity.IsEnterable = true;
				tileEntity.IsItemHolder = true;

				if (Random.Range(0.0f, 1.0f) > 0.4f)
				{
					tileEntity.AddCollapseOnStepped(1);
				}
				else
				{
					tileEntity.AddOwnable(false, -1);
				}

				if (Random.Range(0.0f, 1.0f) > 0.8f)
				{
					var itemEntity = m_ItemContext.CreateEntity();
					itemEntity.AddOnTileItem(position);
					itemEntity.AddApplySpeedChangeStateForEaterOnEaten(3.0f, 2.0f);
				}

				tileEntity.AddCanBeRespawnedOn(newRespawnAreaId: 0);
			}
		}*/
	}
}
