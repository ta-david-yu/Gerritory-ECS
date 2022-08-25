using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically the roll module in the original game. Roll the cube based on MoveOnTile progress
/// </summary>
public class RollOnTileAnimationController : MonoBehaviour, IMoveOnTileAddedListener, IMoveOnTileCompleteAddedListener
{
	[SerializeField]
	private Transform m_TransformToMove;

	const float k_TileWorldPositionOffset = 1;

	private Vector3 m_RollingPivot;
	private Vector3 m_RollingAxis;
	private float m_PreviousProgress = 0;

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
		m_TransformToMove.localPosition = getWorldPositionFromTilePosition(gameEntity.OnTileElement.Position) + Vector3.up * k_TileWorldPositionOffset * 0.5f;
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		if (progress == 0.0f)
		{
			// The initial frame where the cube player is moving.
			// This looks dangerous, but since the listener is triggered with system, this will always be stable.
			Vector3 fromWorldPosition = getWorldPositionFromTilePosition(fromPosition);
			Vector3 toWorldPosition = getWorldPositionFromTilePosition(toPosition);
			Vector3 moveDirection = (toWorldPosition - fromWorldPosition).normalized;
			m_RollingPivot = fromWorldPosition + k_TileWorldPositionOffset * moveDirection * 0.5f;
			m_RollingAxis = Vector3.Cross(-moveDirection, Vector3.up);

			m_PreviousProgress = 0.0f;
		}
		else
		{
			float progressDiff = progress - m_PreviousProgress;
			m_TransformToMove.RotateAround(m_RollingPivot, m_RollingAxis, 90.0f * progressDiff);
			m_PreviousProgress = progress;
		}
		//m_TransformToMove.localPosition = Vector3.Lerp(fromWorldPosition, toWorldPosition, progress);

		// TODO: Fix consequent rotation movement bug
	}

	public void OnMoveOnTileCompleteAdded(GameEntity gameEntity)
	{
		float adjustStep = (1.0f - m_PreviousProgress);
		float adjustAngle = 90.0f * adjustStep;
		m_TransformToMove.RotateAround(m_RollingPivot, m_RollingAxis, adjustAngle);
		//m_TransformToMove.localPosition = getWorldPositionFromTilePosition(gameEntity.OnTileElement.Position) + Vector3.up * k_TileWorldPositionOffset * 0.5f;
	}

	private Vector3 getWorldPositionFromTilePosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * k_TileWorldPositionOffset;
	}
}
