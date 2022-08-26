using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basically the roll module in the original game. Roll the cube based on MoveOnTile progress
/// </summary>
public class RollOnTileAnimationController : MonoBehaviour, IMoveOnTileStartAddedListener, IMoveOnTileAddedListener, IMoveOnTileCompleteAddedListener
{
	[SerializeField]
	private Transform m_TransformToMove;

	const float k_TileWorldPositionOffset = 1;

	private float m_PreviousProgress = 0;

	public void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		// Register listener to relevant components
		gameEntity.AddMoveOnTileStartAddedListener(this);
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

	public void OnMoveOnTileStartAdded(GameEntity entity)
	{
		//throw new System.NotImplementedException();
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = getWorldPositionFromTilePosition(fromPosition);
		Vector3 toWorldPosition = getWorldPositionFromTilePosition(toPosition);
		Vector3 moveDirection = (toWorldPosition - fromWorldPosition).normalized;
		Vector3 rollingPivot = fromWorldPosition + k_TileWorldPositionOffset * moveDirection * 0.5f;
		Vector3 rollingAxis = Vector3.Cross(-moveDirection, Vector3.up);

		float progressDiff = progress - m_PreviousProgress;
		float rotateAngle = 90.0f * progressDiff;
		m_TransformToMove.RotateAround(rollingPivot, rollingAxis, rotateAngle);
		m_PreviousProgress = progress;
	}

	public void OnMoveOnTileCompleteAdded(GameEntity gameEntity, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = getWorldPositionFromTilePosition(fromPosition);
		Vector3 toWorldPosition = getWorldPositionFromTilePosition(toPosition);
		Vector3 moveDirection = (toWorldPosition - fromWorldPosition).normalized;
		Vector3 rollingPivot = fromWorldPosition + k_TileWorldPositionOffset * moveDirection * 0.5f;
		Vector3 rollingAxis = Vector3.Cross(-moveDirection, Vector3.up);

		float adjustStep = (1.0f - m_PreviousProgress);
		float adjustAngle = 90.0f * adjustStep;
		m_TransformToMove.RotateAround(rollingPivot, rollingAxis, adjustAngle);

		m_PreviousProgress = 0.0f;
	}

	private Vector3 getWorldPositionFromTilePosition(Vector2Int tilePosition)
	{
		return new Vector3(tilePosition.x, 0, tilePosition.y) * k_TileWorldPositionOffset;
	}
}
