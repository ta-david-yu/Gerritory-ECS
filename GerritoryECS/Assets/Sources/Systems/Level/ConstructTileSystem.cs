using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build tile entities on request.
/// </summary>
public sealed class ConstructTileSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly ItemContext m_ItemContext;
	private readonly ConfigContext m_ConfigContext;

	private readonly IGroup<LevelEntity> m_ConstructTileRequestGroup;

	public ConstructTileSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_ItemContext = contexts.Item;
		m_ConfigContext = contexts.Config;

		m_ConstructTileRequestGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructTile);
	}

	public void FixedUpdate()
	{
		foreach (var constructTileRequest in m_ConstructTileRequestGroup.GetEntities())
		{
			ConstructTileComponent constructTileComponent = constructTileRequest.ConstructTile;
			var tileEntity = createTileEntity(constructTileComponent);

			constructTileRequest.Destroy();
		}
	}

	private TileEntity createTileEntity(ConstructTileComponent constructTileComponent)
	{
		ITileFactory tileFactory = m_ConfigContext.GameConfig.value.TileFactory;

		Vector2Int tilePosition = constructTileComponent.TilePosition;
		string tileId = constructTileComponent.TileData.TileId;

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

		return tileEntity;
	}
}
