using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SetTileWorldPositionOnComponentsAdded : EntityCreationEventListenerBase
{
	[SerializeField]
	private Transform m_RootTransform;

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		m_RootTransform.localPosition = GameConstants.TilePositionToWorldPosition(tileEntity.TilePosition.Value);

		// Set the name of the gmae object for debug purpose.
		StringBuilder stringBuilder = new StringBuilder(m_RootTransform.gameObject.name);
		stringBuilder.Insert(0, $"{tileEntity.TilePosition.Value} ");
		m_RootTransform.gameObject.name = stringBuilder.ToString();
		Debug.Log($"Set the name of the tile view - {m_RootTransform.gameObject.name}");
	}

	public override void HandleOnEntityCreated(IEntity entity)
	{
	}
}
