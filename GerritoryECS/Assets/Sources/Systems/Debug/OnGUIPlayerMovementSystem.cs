using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OnGUIPlayerMovementSystem : IUpdateSystem
{
	private readonly IGroup<GameEntity> m_PlayerGroup;

	public OnGUIPlayerMovementSystem(Contexts contexts)
	{
		m_PlayerGroup = contexts.Game.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.OnTileElement));
	}

	public void Update()
	{
		GUIStyle areaStyle = new GUIStyle("box");
		foreach (var entity in m_PlayerGroup.GetEntities())
		{
			using (new GUILayout.VerticalScope(areaStyle, GUILayout.Width(250)))
			{
				GUILayout.Label($"Id: {entity.Player.Id}");
				GUILayout.Label($"Position: {entity.OnTileElement.Position}");

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
	}
}
