using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// Do collapse/collapse counting logic if entities that start a movement have <see cref="TileCollapserComponent"/> + the departing tile is <see cref="CollapseOnSteppedComponent"/>.
/// </summary>
public class CollapseTileOnMoveBeginSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;

	private readonly IGroup<GameEntity> m_DepartingCollapserGroup;

	public CollapseTileOnMoveBeginSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;

		m_DepartingCollapserGroup = m_GameContext.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.TileCollapser, GameMatcher.MoveOnTileBegin));
	}

	public void FixedUpdate()
	{
		foreach (GameEntity collapserEntity in m_DepartingCollapserGroup.GetEntities())
		{
			Vector2Int departPosition = collapserEntity.MoveOnTileBegin.FromPosition;
			TileEntity departTileEntity = m_TileContext.GetEntityWithTilePosition(departPosition);

			if (!departTileEntity.HasCollapseOnStepped)
			{
				// The tile is not collapsable, do nothing.
				continue;
			}

			if (!departTileEntity.IsEnterable)
			{
				// The tile is already unenterable. Normally this shouldn't happen because one won't be able to depart from an unenterable tile.
				// Therefore there might be a bug here.
				Debug.LogWarning($"The to-collapse tile {departPosition} has already been unenterable. There might be some bugs happening :P");
				continue;
			}

			departTileEntity.ReplaceCollapseOnStepped(departTileEntity.CollapseOnStepped.NumberOfStepsLeft - 1);

			if (departTileEntity.CollapseOnStepped.NumberOfStepsLeft <= 0)
			{
				// The tile collapses.
				departTileEntity.IsEnterable = false;
			}
		}
	}
}
