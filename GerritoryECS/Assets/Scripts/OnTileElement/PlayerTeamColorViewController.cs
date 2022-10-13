using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamColorViewController : EntityCreationEventListenerBase, ITeamAddedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[Space]

	[SerializeField]
	private List<Renderer> m_Renderers;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		ElementEntity gameEntity = entity as ElementEntity;

		gameEntity.AddTeamAddedListener(this);
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
		ElementEntity gameEntity = entity as ElementEntity;

		if (gameEntity.HasTeam)
		{
			Color playerColor = m_ColorPalette.GetPlayerBodyColorForTeam(gameEntity.Team.Id);
			changeRenderersColor(playerColor);
		}
	}

	public void OnTeamAdded(ElementEntity entity, int id)
	{
		Color playerColor = m_ColorPalette.GetPlayerBodyColorForTeam(id);
		changeRenderersColor(playerColor);
	}

	private void changeRenderersColor(Color color)
	{
		foreach (Renderer renderer in m_Renderers)
		{
			renderer.material.SetColor("_BaseColor", color);
		}
	}
}
