using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileOnTakenOverAnimationController : EntityCreationEventListenerBase, IOwnableAddedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[Space]

	[SerializeField]
	private List<Renderer> m_Renderers;

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
		if (!hasOwner)
		{
			changeRenderersColor(m_ColorPalette.GetDefaultTileBodyColor());
			return;
		}

		GameEntity ownerEntity = Contexts.SharedInstance.Game.GetEntityWithTileOwner(ownerId);
		if (!ownerEntity.HasTeam)
		{
			changeRenderersColor(m_ColorPalette.GetDefaultTileBodyColor());
			return;
		}

		changeRenderersColor(m_ColorPalette.GetTileBodyColorForTeam(ownerEntity.Team.Id));
	}

	private void changeRenderersColor(Color color)
	{
		foreach (Renderer renderer in m_Renderers)
		{
			renderer.material.SetColor("_BaseColor", color);
		}
	}
}
