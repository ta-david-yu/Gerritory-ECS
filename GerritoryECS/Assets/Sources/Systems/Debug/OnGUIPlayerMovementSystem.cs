using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnGUIPlayerMovementSystem : IUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<GameFlowEntity> m_GameFlowGroup;
	private readonly IGroup<LevelEntity> m_GameInfoGroup;
	private readonly IGroup<ElementEntity> m_PlayerGroup;
	private readonly IGroup<LevelEntity> m_TeamInfoGroup;
	private readonly IGroup<ElementEntity> m_GhostGroup;

	public OnGUIPlayerMovementSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_Contexts = contexts;

		m_GameFlowGroup = contexts.GameFlow.GetGroup(GameFlowMatcher.GameFlow);
		m_GameInfoGroup = contexts.Level.GetGroup(LevelMatcher.GameInfo);
		m_PlayerGroup = contexts.Element.GetGroup(ElementMatcher.AllOf(ElementMatcher.Player, ElementMatcher.OnTileElement));
		m_TeamInfoGroup = contexts.Level.GetGroup(LevelMatcher.TeamInfo);
		m_GhostGroup = contexts.Element.GetGroup(ElementMatcher.Ghost);
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


				if (GUILayout.Button("Kill"))
				{
					m_Contexts.TryCommandForceKillImmortal(entity);
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

		using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(200)))
		{
			GUILayout.Label("Ghost Related Stuff");
			if (GUILayout.Button("Spawn Ghost"))
			{
				var ghostEntity = m_Contexts.ConstructGhostEntity();

				// The ghost would not move immediately!
				ghostEntity.AddGhostAppearing(0);
			}


			foreach (var ghostEntity in m_GhostGroup.GetEntities())
			{
				using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(250)))
				{
					GUILayout.Label($"Element Id: {ghostEntity.OnTileElement.Id}");

					if (ghostEntity.IsDead && !ghostEntity.HasGhostAppearing)
					{
						if (GUILayout.Button("Appear"))
						{
							m_Contexts.TryMakeGhostReappear(ghostEntity);
						}
					}
					else
					{
						using (new GUILayout.HorizontalScope())
						{
							if (GUILayout.Button("Disappear"))
							{
								if (ghostEntity.HasMoveOnTile)
								{
									ghostEntity.IsDelayGhostDisappearOnEnterTile = true;
								}
								else
								{
									m_Contexts.TryMakeGhostDisappear(ghostEntity);
								}
							}

							if (GUILayout.Button("Instant Disappear"))
							{
								m_Contexts.TryMakeGhostDisappear(ghostEntity);
							}
						}
					}
				}
			}
		}
	}
}
