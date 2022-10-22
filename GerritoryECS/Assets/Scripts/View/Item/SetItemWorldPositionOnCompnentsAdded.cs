using JCMG.EntitasRedux;
using System.Text;
using UnityEngine;

public class SetItemWorldPositionOnCompnentsAdded : EntityCreationEventListenerBase
{
	[SerializeField]
	private Transform m_RootTransform;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
		ItemEntity itemEntity = entity as ItemEntity;

		if (!itemEntity.HasOnTileItem)
		{
			Debug.LogWarning($"The entity ({entity.CreationIndex}) doesn't have OnTileItem! Cannot set initial view transform position.");
			return;
		}

		// Set initial view transform position
		m_RootTransform.localPosition = GameConstants.TilePositionToWorldPosition(itemEntity.OnTileItem.Position);

		// Set the name of the gmae object for debug purpose.
		m_RootTransform.gameObject.name += $" {itemEntity.OnTileItem.Position}";
	}
}
