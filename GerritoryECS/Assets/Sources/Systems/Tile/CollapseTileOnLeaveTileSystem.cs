using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// Do collapse/collapse counting logic if entities that leave a tile have <see cref="TileCollapserComponent"/> + the leaving tile is <see cref="CollapseOnSteppedComponent"/>.
/// LeaveTile event caused by death will not trigger collapse.
/// </summary>
public class CollapseTileOnLeaveTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_LeaveTileMessageGroup;

	public CollapseTileOnLeaveTileSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
		m_MessageContext = contexts.Message;

		m_LeaveTileMessageGroup = m_MessageContext.GetGroup(MessageMatcher.
			AllOf(MessageMatcher.OnTileElementLeaveTile).
			NoneOf(MessageMatcher.LeaveBecauseOfDeath, MessageMatcher.Consumed));
	}

	public void FixedUpdate()
	{
		foreach (MessageEntity leaveMessageEntity in m_LeaveTileMessageGroup.GetEntities())
		{
			int onTileElementId = leaveMessageEntity.OnTileElementLeaveTile.OnTileElementId;
			GameEntity leaverEntity = m_GameContext.GetEntityWithOnTileElement(onTileElementId);
			if (!leaverEntity.IsTileCollapser)
			{
				// The leaving OnTileElement is not a TileCollapser, do nothing.
				continue;
			}

			Vector2Int leavePosition = leaveMessageEntity.OnTileElementLeaveTile.Position;
			TileEntity leaveTileEntity = m_TileContext.GetEntityWithTilePosition(leavePosition);
			if (!leaveTileEntity.HasCollapseOnStepped)
			{
				// The tile is not collapsable, do nothing.
				continue;
			}

			if (!leaveTileEntity.IsEnterable)
			{
				// The tile is already unenterable. Normally this shouldn't happen because one won't be able to depart from an unenterable tile.
				// Therefore there might be a bug here.
				Debug.LogWarning($"The to-collapse tile {leavePosition} has already been unenterable. There might be some bugs happening :P");
				continue;
			}

			leaveTileEntity.ReplaceCollapseOnStepped(leaveTileEntity.CollapseOnStepped.NumberOfStepsLeft - 1);

			if (leaveTileEntity.CollapseOnStepped.NumberOfStepsLeft <= 0)
			{
				// The tile collapses.
				leaveTileEntity.IsEnterable = false;
			}
		}
	}
}
