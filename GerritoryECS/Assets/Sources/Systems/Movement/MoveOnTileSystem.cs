using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class MoveOnTileSystem : IFixedUpdateSystem
{
	private readonly GameContext m_GameContext;

	private readonly IGroup<GameEntity> m_MoveOnTileGroup;
	private readonly IGroup<GameEntity> m_MoveOnTileBeginGroup;
	private readonly IGroup<GameEntity> m_MoveOnTileEndGroup;

	public MoveOnTileSystem(Contexts contexts)
	{
		m_MoveOnTileGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MoveOnTile));
		m_MoveOnTileBeginGroup = contexts.Game.GetGroup(GameMatcher.MoveOnTileBegin);
		m_MoveOnTileEndGroup = contexts.Game.GetGroup(GameMatcher.MoveOnTileEnd);
	}

	public void FixedUpdate()
	{
		// Move OnTileElements with MoveOnTile component
		foreach (var entity in m_MoveOnTileGroup.GetEntities())
		{
			float progress = entity.MoveOnTile.Progress;
			progress += (1 / GameConstants.MoveOnTileDurationBase) * Time.fixedDeltaTime;

			if (progress < 1.0f)
			{
				entity.ReplaceMoveOnTile(progress, entity.MoveOnTile.FromPosition, entity.MoveOnTile.ToPosition);
			}
			else if (progress >= 1.0f)
			{
				Vector2Int from = entity.MoveOnTile.FromPosition;
				Vector2Int to = entity.MoveOnTile.ToPosition;

				// TODO: progress overflow movement
				// Sometimes the progress value might go over 1.0, when that happens we want to move the entity based on buffered movement input
				
				entity.RemoveMoveOnTile();
				

				// Set the position to the target move position and set IsComplete flag to true
				entity.ReplaceOnTileElement(entity.OnTileElement.Id, to);
				entity.AddMoveOnTileEnd(from, to);
			}
		}
	}
}
