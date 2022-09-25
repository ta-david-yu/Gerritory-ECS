using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseTileAnimationController : EntityCreationEventListenerBase, IEnterableAddedListener, IEnterableRemovedListener
{
	[SerializeField]
	private Transform m_Appearance;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddEnterableAddedListener(this);
		tileEntity.AddEnterableRemovedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
	}

	public void OnEnterableRemoved(TileEntity entity)
	{
		m_Appearance.gameObject.SetActive(false);
	}

	public void OnEnterableAdded(TileEntity entity)
	{
		m_Appearance.gameObject.SetActive(true);
	}
}
