using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFogPlaneColorOnLeadingTeamChangedAnimation : EntityCreationEventListenerBase, ILeadingTeamAddedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[SerializeField]
	private Renderer m_PlaneRenderer;

	[SerializeField]
	private float m_ColorTransitionDuration = 1.0f;

	private Camera m_Camera;

	private Tweener m_ColorShiftTweener = null;

	private void Awake()
	{
		// TODO: inject the camera with a different way.
		m_Camera = Camera.main;
	}

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{
		LevelEntity gameInfoEntity = entity as LevelEntity;
		gameInfoEntity.AddLeadingTeamAddedListener(this);

		m_PlaneRenderer.material.color = m_ColorPalette.GetDefaultFogColor();
		m_Camera.backgroundColor = m_ColorPalette.GetDefaultFogColor();
	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
	}

	public void OnLeadingTeamAdded(LevelEntity entity, int teamId)
	{
		Color prevColor = m_PlaneRenderer.material.color;
		Color nextColor = m_ColorPalette.GetFogColorForTeam(teamId);

		Tweener.SafeAbortTweener(ref m_ColorShiftTweener);

		m_ColorShiftTweener = TweenManager.Instance
			.Tween((float progress) =>
			{
				Color newColor = Color.Lerp(prevColor, nextColor, progress);
				m_PlaneRenderer.material.color = newColor;
				m_Camera.backgroundColor = newColor;
			})
			.SetTime(m_ColorTransitionDuration)
			.SetEndCallback(() =>
			{
				m_ColorShiftTweener = null;
				m_PlaneRenderer.material.color = nextColor;
				m_Camera.backgroundColor = nextColor;
			});
	}
}
