using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveOnTileAnimationController : MonoBehaviour, IMoveOnTileAddedListener, IMoveOnTileCompleteAddedListener
{
	[SerializeField]
	private Transform m_TransformToMove;

	const float k_TileWorldPositionOffset = 1;

	public void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		// Register listener to relevant components
		gameEntity.AddMoveOnTileAddedListener(this);
		gameEntity.AddMoveOnTileCompleteAddedListener(this);
	}

	public void HandleOnBlueprintApplied(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;
		
		if (!gameEntity.HasOnTileElement)
		{
			Debug.LogWarning($"The entity ({entity.CreationIndex}) doesn't have OnTileElement! Cannot set initial view transform position.");
			return;
		}

		// Set initial view transform position
		m_TransformToMove.localPosition = getWorldPositionFromTilePosition(gameEntity.OnTileElement.Position);
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = getWorldPositionFromTilePosition(fromPosition);
		Vector3 toWorldPosition = getWorldPositionFromTilePosition(toPosition);

		m_TransformToMove.localPosition = Vector3.Lerp(fromWorldPosition, toWorldPosition, progress);
	}

	public void OnMoveOnTileCompleteAdded(GameEntity gameEntity)
	{
		m_TransformToMove.localPosition = getWorldPositionFromTilePosition(gameEntity.OnTileElement.Position);
	}

	private Vector3 getWorldPositionFromTilePosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * k_TileWorldPositionOffset;
	}
}
