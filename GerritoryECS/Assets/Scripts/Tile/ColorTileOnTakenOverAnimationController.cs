using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileOnTakenOverAnimationController : EntityCreationEventListenerBase, IOwnableAddedListener
{
	[SerializeField]
	private Renderer m_Renderer;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddOwnableAddedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
	}

	public void OnOwnableAdded(TileEntity entity, bool hasOwner, int ownerId)
	{
		if (hasOwner)
		{
			Debug.Log($"{ownerId} took over {entity.TilePosition.Value}!");
		}
	}
}
