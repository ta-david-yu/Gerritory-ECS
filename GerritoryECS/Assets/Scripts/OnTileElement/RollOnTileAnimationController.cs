using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically the roll module in the original game. Roll the cube based on MoveOnTile progress
/// </summary>
public class RollOnTileAnimationController : EntityCreationEventListenerBase, IMoveOnTileAddedListener, IMoveOnTileEndAddedListener, IOnTilePositionAddedListener
{
	[SerializeField]
	private Transform m_TransformToMove;

	private float m_PreviousProgress = 0;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		// Register listener to relevant components
		gameEntity.AddMoveOnTileAddedListener(this);
		gameEntity.AddMoveOnTileEndAddedListener(this);
		gameEntity.AddOnTilePositionAddedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = GameConstants.TilePositionToWorldPosition(fromPosition);
		Vector3 toWorldPosition = GameConstants.TilePositionToWorldPosition(toPosition);
		Vector3 moveDirection = (toWorldPosition - fromWorldPosition).normalized;
		Vector3 rollingPivot = fromWorldPosition + GameConstants.TileOffset * moveDirection * 0.5f;
		Vector3 rollingAxis = Vector3.Cross(-moveDirection, Vector3.up);

		float progressDiff = progress - m_PreviousProgress;
		float rotateAngle = 90.0f * progressDiff;
		m_TransformToMove.RotateAround(rollingPivot, rollingAxis, rotateAngle);

		m_PreviousProgress = progress;
	}

	public void OnMoveOnTileEndAdded(GameEntity gameEntity, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = GameConstants.TilePositionToWorldPosition(fromPosition);
		Vector3 toWorldPosition = GameConstants.TilePositionToWorldPosition(toPosition);
		Vector3 moveDirection = (toWorldPosition - fromWorldPosition).normalized;
		Vector3 rollingPivot = fromWorldPosition + GameConstants.TileOffset * moveDirection * 0.5f;
		Vector3 rollingAxis = Vector3.Cross(-moveDirection, Vector3.up);

		float adjustStep = (1.0f - m_PreviousProgress);
		float adjustAngle = 90.0f * adjustStep;
		m_TransformToMove.RotateAround(rollingPivot, rollingAxis, adjustAngle);

		m_PreviousProgress = 0.0f;
	}

	public void OnOnTilePositionAdded(GameEntity entity, Vector2Int value)
	{
		m_TransformToMove.localPosition = GameConstants.TilePositionToWorldPosition(value) + Vector3.up * GameConstants.TileOffset * 0.5f;
	}
}
