using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpawnPlayerPointerUIController : EntityCreationEventListenerBase, ITeamScoreAddedListener, IEnterStateAddedListener, ILeaveStateAddedListener
{
	[SerializeField]
	private ColorPalette m_ColorPalette;

	[SerializeField]
	private PlayerPointerUIAnimationController m_PlayerPointerUIPrefab;

	private PlayerPointerUIAnimationController m_SpawnedPlayerPointerUI;

	public override void HandleOnEntityCreated(Contexts contexts, IEntity entity)
	{

	}

	public override void HandleOnComponentsAdded(Contexts contexts, IEntity entity)
	{
		// Spawn pointer UI view
		m_SpawnedPlayerPointerUI = Instantiate(m_PlayerPointerUIPrefab);
		m_SpawnedPlayerPointerUI.SetFollowingTargetTransform(transform);

		ElementEntity elementEntity = entity as ElementEntity;
		if (elementEntity.HasTeam)
		{
			LevelEntity teamEntity = contexts.Level.GetEntityWithTeamInfo(elementEntity.Team.Id);
			teamEntity.AddTeamScoreAddedListener(this);

			m_SpawnedPlayerPointerUI.SetScoreTextColor(m_ColorPalette.GetPlayerBodyColorForTeam(elementEntity.Team.Id));
		}

		// Register to player state.
		elementEntity.AddEnterStateAddedListener(this);
		elementEntity.AddLeaveStateAddedListener(this);

		// Change UI depth value to avoid overlapping issue.
		m_SpawnedPlayerPointerUI.SetZValue(elementEntity.OnTileElement.Id * 0.1f);

		//contexts.Level.GetEntityWithTeamInfo(elementEntity.Team)
	}

	public void OnTeamScoreAdded(LevelEntity entity, int value)
	{
		m_SpawnedPlayerPointerUI.ChangeScoreText(value.ToString());
	}

	public void OnEnterStateAdded(ElementEntity entity)
	{
		m_SpawnedPlayerPointerUI.PlayChangeStateAnimation(Color.yellow);

		// TODO: update state timer UI
	}

	public void OnLeaveStateAdded(ElementEntity entity)
	{
		m_SpawnedPlayerPointerUI.PlayChangeStateAnimation(Color.yellow);

		// TODO: update state timer UI
	}
}
