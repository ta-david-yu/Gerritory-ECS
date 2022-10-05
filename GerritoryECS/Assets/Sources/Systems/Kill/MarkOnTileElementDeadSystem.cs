using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MarkOnTileElementDeadSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly CommandContext m_CommandContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<CommandEntity> m_MarkDeadRequestGroup;

	public MarkOnTileElementDeadSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_CommandContext = contexts.Command;
		m_Contexts = contexts;

		m_MarkDeadRequestGroup = m_CommandContext.GetGroup(CommandMatcher.MarkOnTileElementDead);
	}

	public void FixedUpdate()
	{
		foreach (var markDeadRequest in m_MarkDeadRequestGroup.GetEntities())
		{
			ElementEntity onTileEntity = m_ElementContext.GetEntityWithOnTileElement(markDeadRequest.MarkOnTileElementDead.TargetOnTileElementId);

			onTileEntity.IsDead = true;

			if (onTileEntity.HasOnTilePosition)
			{
				// If the OnTileEntity is occupying a tile, be sure to remove it from the tile.
				Vector2Int position = onTileEntity.OnTilePosition.Value;
				onTileEntity.RemoveOnTilePosition();

				if (!onTileEntity.HasMoveOnTile)
				{
					// Emit global LeaveTile message if the killed entity was not moving away from its tile.
					var leaveTileMessageEntity = m_Contexts.Message.CreateFixedUpdateMessageEntity();
					leaveTileMessageEntity.ReplaceOnTileElementLeaveTile(onTileEntity.OnTileElement.Id, position);
					leaveTileMessageEntity.IsLeaveBecauseOfDeath = true;
				}
			}

			if (onTileEntity.HasMoveOnTile)
			{
				// If the OnTileEntity is moving, be sure to cancel the movement.
				Vector2Int fromPosition = onTileEntity.MoveOnTile.FromPosition;
				Vector2Int toPosition = onTileEntity.MoveOnTile.ToPosition;
				onTileEntity.RemoveMoveOnTile();
				onTileEntity.AddMoveOnTileEnd(fromPosition, toPosition);
			}

			markDeadRequest.Destroy();
		}
	}
}
