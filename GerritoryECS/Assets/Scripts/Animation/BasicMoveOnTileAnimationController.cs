using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveOnTileAnimationController : EntityEventListenerBehaviour, IMoveOnTileAddedListener, IMoveOnTileCompleteAddedListener
{
	[SerializeField]
	private Transform m_TransformToMove;

	const float k_TileWorldPositionOffset = 1;

	public override void RegisterListenerToEntity(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

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

		Vector2Int position = gameEntity.OnTileElement.Position;
		m_TransformToMove.localPosition = new Vector3(position.x, 0, position.y) * k_TileWorldPositionOffset;
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		Vector3 fromWorldPosition = new Vector3(fromPosition.x, 0, fromPosition.y) * k_TileWorldPositionOffset;
		Vector3 toWorldPosition = new Vector3(toPosition.x, 0, toPosition.y) * k_TileWorldPositionOffset;

		m_TransformToMove.localPosition = Vector3.Lerp(fromWorldPosition, toWorldPosition, progress);
	}

	public void OnMoveOnTileCompleteAdded(GameEntity entity)
	{
		Vector2Int finalPosition = entity.OnTileElement.Position;
		m_TransformToMove.localPosition = new Vector3(finalPosition.x, 0, finalPosition.y) * k_TileWorldPositionOffset;
	}
}
