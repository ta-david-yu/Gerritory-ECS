using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build player entity on request.
/// </summary>
public sealed class ConstructPlayerSystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<LevelEntity> m_ConstructPlayerRequestGroup;

	public ConstructPlayerSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;

		m_ConstructPlayerRequestGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructPlayer);
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
