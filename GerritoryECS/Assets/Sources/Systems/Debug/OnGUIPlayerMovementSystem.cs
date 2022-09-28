using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnGUIPlayerMovementSystem : IUpdateSystem
{
	private GameContext m_GameContext;

	private readonly IGroup<GameEntity> m_PlayerGroup;
	private readonly IGroup<LevelEntity> m_TeamInfoGroup;

	public OnGUIPlayerMovementSystem(Contexts contexts)
	{
		m_GameContext = contexts.Game;
		m_PlayerGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.OnTileElement));
		m_TeamInfoGroup = contexts.Level.GetGroup(LevelMatcher.TeamInfo);
	}

	public void Update()
	{
		GUIStyle areaStyle = new GUIStyle("box");
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
			int numberOfTeamMembers = m_GameContext.GetEntitiesWithTeam(teamEntity.TeamInfo.Id).Count;

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
