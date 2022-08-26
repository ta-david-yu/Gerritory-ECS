using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class MoveOnTileSystem : IFixedUpdateSystem, ICleanupSystem
{
	private readonly IGroup<GameEntity> m_MoveOnTileGroup;
	private readonly IGroup<GameEntity> m_MoveOnTileCompleteGroup;

	// Placeholder value, to be replaced
	const float k_MoveDuration = 0.5f;

	public MoveOnTileSystem(Contexts contexts)
	{
		m_MoveOnTileGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.OnTileElement, GameMatcher.MoveOnTile));
		m_MoveOnTileCompleteGroup = contexts.Game.GetGroup(GameMatcher.MoveOnTileComplete);
	}

	public void FixedUpdate()
	{
		foreach (var entity in m_MoveOnTileGroup.GetEntities())
		{
			float progress = entity.MoveOnTile.Progress;
			progress += (1 / k_MoveDuration) * Time.fixedDeltaTime;

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
				entity.ReplaceOnTileElement(to);
				entity.AddMoveOnTileComplete(from, to);
			}
		}
	}

	public void Cleanup()
	{
		foreach (var entity in m_MoveOnTileCompleteGroup.GetEntities())
		{
			// Clean up the IsComplete flag in Cleanup phase in case other system needs the information
			entity.RemoveMoveOnTileComplete();
		}
	}
}
