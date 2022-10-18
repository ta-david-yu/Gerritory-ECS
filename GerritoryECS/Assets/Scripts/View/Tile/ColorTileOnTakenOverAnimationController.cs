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

	[SerializeField]
	private float m_FadeToColorDuration = 0.4f;

	[SerializeField]
	private Color m_InitialColorOnTakenOver = Color.white;

	private Color m_NextColor;
	private float m_FadeToColorTimer = 0.0f;

	private void Update()
	{
		if (m_FadeToColorTimer > 0)
		{
			m_FadeToColorTimer -= Time.deltaTime;
			float progress = 1.0f - m_FadeToColorTimer / m_FadeToColorDuration;
			Color color = Color.Lerp(m_InitialColorOnTakenOver, m_NextColor, progress);
			changeRenderersColor(color);
		}
	}

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		TileEntity tileEntity = entity as TileEntity;
		tileEntity.AddOwnerAddedListener(this);
		tileEntity.AddOwnerRemovedListener(this);
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
	}

	public void OnOwnerAdded(TileEntity entity, int ownerTeamId)
	{
		m_NextColor = m_ColorPalette.GetTileBodyColorForTeam(ownerTeamId);
		m_FadeToColorTimer = m_FadeToColorDuration;
	}

	public void OnOwnerRemoved(TileEntity entity)
	{
		m_NextColor = m_ColorPalette.GetDefaultTileBodyColor();
		m_FadeToColorTimer = m_FadeToColorDuration;
	}

	private void changeRenderersColor(Color color)
	{
		foreach (Renderer renderer in m_Renderers)
		{
			renderer.material.SetColor("_BaseColor", color);
		}
	}
}
