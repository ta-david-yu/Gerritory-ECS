using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class GameHelper
{
	/// <summary>
	/// Check if the tile itself is moveable to or not. <see cref="OnTileElementComponent"/> is not taken into consideration.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public static bool IsTileAtPositionMoveableTo(this Contexts contexts, Vector2Int position)
	{
		TileEntity tileEntity = contexts.Tile.GetEntityWithTilePosition(position);
		if (tileEntity == null)
		{
			// There is no tile here, cannot move here.
			return false;
		}

		if (!tileEntity.IsEnterable)
		{
			// The tile is not enterable, cannot move here.
			return false;
		}

		return true;
	}

	/// <summary>
	/// Check if the tile at the given position is occupied by any <see cref="OnTileElementComponent"/>.
	/// </summary>
	/// <param name="contexts"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public static bool IsTileAtPositionOccupied(this Contexts contexts, Vector2Int position)
	{
		HashSet<ElementEntity> onTileEntities = contexts.Element.GetEntitiesWithOnTilePosition(position);
		bool isEntityOccupyingTheGiveTile = onTileEntities.Any(entity => !entity.HasMoveOnTile || entity.HasMoveOnTileBegin);
		if (isEntityOccupyingTheGiveTile)
		{
			// There are already more than 1 OnTileElement entity on the given tile position & not moving away.
			return true;
		}

		HashSet<ElementEntity> movingToTargetPositionEntities = contexts.Element.GetEntitiesWithMoveOnTile(position);
		if (movingToTargetPositionEntities.Count > 0)
		{
			// This position has already been reserved by another MoveOnTile entity.
			return true;
		}

		return false;
	}

	public static TileEntity ConstructTileEntityAtPosition(this Contexts contexts, TileData tileData, Vector2Int tilePosition)
	{
		ITileFactory tileFactory = contexts.Config.GameConfig.value.TileFactory;

		string tileId = tileData.TileId;

		// Create entity and its view controller
		var tileEntity = contexts.Tile.CreateEntity();
		IEntityCreationEventController viewController = tileFactory.CreateTileView(tileId);
		viewController.OnEntityCreated(contexts, tileEntity);

		// Apply blueprint and components
		var blueprint = tileFactory.GetTileBlueprint(tileId);
		blueprint.ApplyToEntity(tileEntity);
		tileEntity.AddTilePosition(tilePosition);
		viewController.OnComponentsAdded(contexts, tileEntity);

		// Link view controller with entity
		viewController.Link(tileEntity);

		return tileEntity;
	}

}
