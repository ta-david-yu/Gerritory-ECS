using JCMG.EntitasRedux;
using System.Text;
using UnityEngine;

public class SetOnTileElementWorldPositionOnCompnentsAdded : EntityCreationEventListenerBase
{
	[SerializeField]
	private Transform m_RootTransform;

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		if (!gameEntity.HasOnTilePosition)
		{
			Debug.LogWarning($"The entity ({entity.CreationIndex}) doesn't have OnTilePosition! Cannot set initial view transform position.");
			return;
		}

		// Set initial view transform position
		m_RootTransform.localPosition = GameConstants.TilePositionToWorldPosition(gameEntity.OnTilePosition.Value) + Vector3.up * GameConstants.TileOffset * 0.5f;

		// Set the name of the gmae object for debug purpose.
		m_RootTransform.gameObject.name += $"[OnTileElementId: {gameEntity.OnTileElement.Id}]";
		Debug.Log($"Set the name of the player view - {m_RootTransform.gameObject.name}");
	}

	public override void HandleOnEntityCreated(IEntity entity)
	{
	}
}
