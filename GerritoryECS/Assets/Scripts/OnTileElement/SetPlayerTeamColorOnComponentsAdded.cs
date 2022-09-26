using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerTeamColorOnComponentsAdded : EntityCreationEventListenerBase
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[Space]

	[SerializeField]
	private List<Renderer> m_Renderers;

	public override void HandleOnEntityCreated(IEntity entity)
	{
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		if (gameEntity.HasTeam)
		{
			Color playerColor = m_ColorPalette.GetPlayerBodyColorForTeam(gameEntity.Team.Id);
			foreach (Renderer renderer in m_Renderers)
			{
				renderer.material.SetColor("_BaseColor", playerColor);
			}
		}
	}
}
