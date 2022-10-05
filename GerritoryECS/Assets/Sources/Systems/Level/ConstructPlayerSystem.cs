using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build player entity on request.
/// </summary>
public sealed class ConstructPlayerSystem : IFixedUpdateSystem
{
	private readonly CommandContext m_CommandContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<CommandEntity> m_ConstructPlayerRequestGroup;

	public ConstructPlayerSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_CommandContext = contexts.Command;

		m_ConstructPlayerRequestGroup = m_CommandContext.GetGroup(CommandMatcher.ConstructPlayer);
	}

	public void FixedUpdate()
	{
		foreach (var constructPlayerRequest in m_ConstructPlayerRequestGroup.GetEntities())
		{
			ConstructPlayerComponent constructPlayerComponent = constructPlayerRequest.ConstructPlayer;
			ElementEntity playerEntity = m_Contexts.ConstructPlayerEntity(constructPlayerComponent.PlayerId, constructPlayerComponent.TeamId, constructPlayerComponent.SkinId);

			constructPlayerRequest.Destroy();
		}
	}
}
