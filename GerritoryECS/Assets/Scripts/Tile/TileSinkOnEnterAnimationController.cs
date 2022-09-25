using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSinkOnEnterAnimationController : BlueprintEntityCreationEventListenerBase, IEnteredByOnTileElementAddedListener, ILeftByOnTileElementAddedListener
{
	[SerializeField]
	private Transform m_Appearance;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddEnteredByOnTileElementAddedListener(this);
		tileEntity.AddLeftByOnTileElementAddedListener(this);
	}

	public override void HandleOnBlueprintApplied(IEntity entity)
	{
	}

	public void OnEnteredByOnTileElementAdded(TileEntity entity, int onTileElementId)
	{
		m_Appearance.localPosition -= new Vector3(0.0f, 0.2f, 0.0f);
	}

	public void OnLeftByOnTileElementAdded(TileEntity entity, int onTileElementId)
	{
		m_Appearance.localPosition += new Vector3(0.0f, 0.2f, 0.0f);
	}
}
