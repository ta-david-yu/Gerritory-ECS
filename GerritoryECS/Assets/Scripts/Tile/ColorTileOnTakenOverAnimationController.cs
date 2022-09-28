using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileOnTakenOverAnimationController : EntityCreationEventListenerBase, IOwnerAddedListener, IOwnerRemovedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[Space]

	[SerializeField]
	private List<Renderer> m_Renderers;

	public override void HandleOnEntityCreated(IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddOwnerAddedListener(this);
		tileEntity.AddOwnerRemovedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
	}

	public void OnOwnerAdded(TileEntity entity, int ownerTeamId)
	{
		changeRenderersColor(m_ColorPalette.GetTileBodyColorForTeam(ownerTeamId));
	}

	public void OnOwnerRemoved(TileEntity entity)
	{
		changeRenderersColor(m_ColorPalette.GetDefaultTileBodyColor());
	}

	private void changeRenderersColor(Color color)
	{
		foreach (Renderer renderer in m_Renderers)
		{
			renderer.material.SetColor("_BaseColor", color);
		}
	}
}
