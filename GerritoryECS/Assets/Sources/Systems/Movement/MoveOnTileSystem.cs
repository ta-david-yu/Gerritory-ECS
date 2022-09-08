using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class MoveOnTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<GameEntity> m_JustBeginMoveOnTileGroup;
	private readonly IGroup<GameEntity> m_MoveOnTileGroup;

	public MoveOnTileSystem(Contexts contexts)
	{
		m_MessageContext = contexts.Message;

		m_JustBeginMoveOnTileGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MoveOnTileBegin).NoneOf(GameMatcher.MoveOnTile));
		m_MoveOnTileGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MoveOnTile));
	}

	public void FixedUpdate()
	{
		// Cache group entities first, so MoveOnTileBegin group won't move right away in the second foreach loop.
		var justBeginMoveOnTileGroupCache = m_JustBeginMoveOnTileGroup.GetEntities();
		var moveOnTileGroupCache = m_MoveOnTileGroup.GetEntities();

		// Add MoveOnTile component to entities that just start moving (have MoveOnTileBegin component but no MoveOnTile).
		foreach (var beginMoveEntity in justBeginMoveOnTileGroupCache)
		{
			beginMoveEntity.AddMoveOnTile(0, beginMoveEntity.MoveOnTileBegin.FromPosition, beginMoveEntity.MoveOnTileBegin.ToPosition);
		}

		// Move OnTileElements with MoveOnTile component.
		foreach (var moveEntity in moveOnTileGroupCache)
		{
			float multiplier = 1.0f;
			if (moveEntity.HasSpeedChangeable)
			{
				multiplier = moveEntity.SpeedChangeable.BaseSpeed * moveEntity.SpeedChangeable.SpeedMultiplier;
			}

			float progress = moveEntity.MoveOnTile.Progress;
			progress += (1 / GameConstants.MoveOnTileDuration) * Time.fixedDeltaTime * multiplier;

			if (progress < 1.0f)
			{
				moveEntity.ReplaceMoveOnTile(progress, moveEntity.MoveOnTile.FromPosition, moveEntity.MoveOnTile.ToPosition);
			}
			else if (progress >= 1.0f)
			{
				Vector2Int fromPosition = moveEntity.MoveOnTile.FromPosition;
				Vector2Int toPosition = moveEntity.MoveOnTile.ToPosition;

				// TODO: progress overflow movement
				// Sometimes the progress value might go over 1.0, when that happens we want to move the entity based on buffered movement input
				
				moveEntity.RemoveMoveOnTile();
				
				// Set the position to the target move position and set IsComplete flag to true
				moveEntity.ReplaceOnTileElement(moveEntity.OnTileElement.Id, toPosition);
				moveEntity.AddMoveOnTileEnd(fromPosition, toPosition);

				// Emit global EnterTile message.
				var leaveTileMessageEntity = m_MessageContext.CreateFixedUpdateMessageEntity();
				leaveTileMessageEntity.ReplaceOnTileElementEnterTile(moveEntity.OnTileElement.Id, toPosition);
			}
		}
	}
}
