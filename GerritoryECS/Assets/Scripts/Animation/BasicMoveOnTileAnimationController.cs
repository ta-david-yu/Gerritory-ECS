using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveOnTileAnimationController : EntityEventListenerBehaviour, IMoveOnTileAddedListener, IMoveOnTileCompleteAddedListener
{
	[SerializeField]
	private Transform m_Appearance;

	public override void RegisterListenerToEntity(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;
		gameEntity.AddMoveOnTileAddedListener(this);
		gameEntity.AddMoveOnTileCompleteAddedListener(this);
	}

	public void OnMoveOnTileAdded(GameEntity entity, float progress, Vector2Int fromPosition, Vector2Int toPosition)
	{
		float tileWorldPositionOffset = 1;

		Vector3 fromWorldPosition = new Vector3(fromPosition.x, 0, fromPosition.y) * tileWorldPositionOffset;
		Vector3 toWorldPosition = new Vector3(toPosition.x, 0, toPosition.y) * tileWorldPositionOffset;

		m_Appearance.localPosition = Vector3.Lerp(fromWorldPosition, toWorldPosition, progress);
	}

	public void OnMoveOnTileCompleteAdded(GameEntity entity)
	{
		// TODO: set to final position
	}
}
