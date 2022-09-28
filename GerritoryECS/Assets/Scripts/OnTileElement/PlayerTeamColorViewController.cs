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

	public override void HandleOnEntityCreated(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		gameEntity.AddTeamAddedListener(this);
	}

	public override void HandleOnComponentsAdded(IEntity entity)
	{
		GameEntity gameEntity = entity as GameEntity;

		if (gameEntity.HasTeam)
		{
			Color playerColor = m_ColorPalette.GetPlayerBodyColorForTeam(gameEntity.Team.Id);
			changeRenderersColor(playerColor);
		}
	}

	public void OnTeamAdded(GameEntity entity, int id)
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
