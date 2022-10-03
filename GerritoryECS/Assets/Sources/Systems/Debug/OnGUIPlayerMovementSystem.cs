using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnGUIPlayerMovementSystem : IUpdateSystem
{
	private ElementContext m_ElementContext;

	private readonly IGroup<GameFlowEntity> m_GameFlowGroup;
	private readonly IGroup<LevelEntity> m_GameInfoGroup;
	private readonly IGroup<ElementEntity> m_PlayerGroup;
	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public OnGUIPlayerMovementSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_GameFlowGroup = contexts.GameFlow.GetGroup(GameFlowMatcher.GameFlow);
		m_GameInfoGroup = contexts.Level.GetGroup(LevelMatcher.GameInfo);
		m_PlayerGroup = contexts.Element.GetGroup(ElementMatcher.AllOf(ElementMatcher.Player, ElementMatcher.OnTileElement));
		m_TeamInfoGroup = contexts.Level.GetGroup(LevelMatcher.TeamInfo);
	}

	public void Update()
	{
		GUIStyle areaStyle = new GUIStyle("box");

		using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(250)))
		{
			foreach (var entity in m_GameFlowGroup.GetEntities())
			{
				if (entity.HasCountdownTimer)
				{
					GUILayout.Label($"Countdown: {entity.CountdownTimer.Value}");
				}

				if (entity.IsGameOver)
				{
					GUILayout.Label($"Game Over!");
				}
			}

			foreach (var entity in m_GameInfoGroup.GetEntities())
			{
				if (entity.HasGameTimer)
				{
					GUILayout.Label($"GameTimer: {entity.GameTimer.Value}");
				}
			}
		}

		foreach (var entity in m_PlayerGroup.GetEntities())
		{
			using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(250)))
			{
				GUILayout.Label($"Id: {entity.Player.Id}");

				if (entity.HasOnTilePosition)
				{
					GUILayout.Label($"Position: {entity.OnTilePosition.Value}");
				}
				else
				{
					GUILayout.Label($"Position: not on any position!");
				}

				if (entity.HasTeam)
				{
					GUILayout.Label($"Team: {entity.Team.Id}");
				}

				if (entity.HasMoveOnTile)
				{
					GUILayout.Label($"ToPosition: {entity.MoveOnTile.ToPosition}, Progress: {entity.MoveOnTile.Progress}");
				}
				else
				{
					GUILayout.Label("Idle");
				}
			}
		}

		foreach (var teamEntity in m_TeamInfoGroup.GetEntities())
		{
			int numberOfTeamMembers = m_ElementContext.GetEntitiesWithTeam(teamEntity.TeamInfo.Id).Count;

			using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(200)))
			{
				GUILayout.Label($"Team Id: {teamEntity.TeamInfo.Id}");
				GUILayout.Label($"Members#: {numberOfTeamMembers}");
				GUILayout.Label($"Score: {teamEntity.TeamScore.Value}");
				GUILayout.Label($"Ranking: {teamEntity.TeamGameRanking.Number}");
			}
		}
	}
}
