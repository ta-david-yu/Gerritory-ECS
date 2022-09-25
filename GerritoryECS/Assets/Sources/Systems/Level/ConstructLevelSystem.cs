using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build the level based on the GameConfig instance
/// </summary>
public sealed class ConstructLevelSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private readonly ConfigContext m_ConfigContext;

	public ConstructLevelSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		ITileFactory tileFactory = m_ConfigContext.GameConfig.value.TileFactory;
		LevelData levelData = m_ConfigContext.GameConfig.value.LevelData;
		m_LevelContext.SetLevel(levelData);

		GameObject tileUnityViewRoot = new GameObject("TileUnityViewRoot");

		foreach (var tileDataPair in levelData.TileDataPairs)
		{
			Vector2Int tilePosition = tileDataPair.Key;
			string tileId = tileDataPair.Value.TileId;

			// Create entity and its view controller
			var tileEntity = m_TileContext.CreateEntity();
			IEntityCreationEventController viewController = tileFactory.CreateTileView(tileId);
			viewController.OnEntityCreated(tileEntity);

			// Apply blueprint and components
			var blueprint = tileFactory.GetTileBlueprint(tileId);
			blueprint.ApplyToEntity(tileEntity);
			tileEntity.AddTilePosition(tilePosition);
			viewController.OnComponentsAdded(tileEntity);

			// Link view controller with entity
			viewController.Link(tileEntity);


			if (Random.Range(0.0f, 1.0f) > 0.8f)
			{
				var itemEntity = m_ItemContext.CreateEntity();
				itemEntity.AddOnTileItem(tilePosition);
				itemEntity.AddApplySpeedChangeStateForEaterOnEaten(3.0f, 2.0f);
			}
		}

	}
}
