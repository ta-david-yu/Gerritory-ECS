using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build player entity on request.
/// </summary>
public sealed class ConstructPlayerSystem : IFixedUpdateSystem
{
	private readonly RequestContext m_RequestContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<RequestEntity> m_ConstructPlayerRequestGroup;

	public ConstructPlayerSystem(Contexts contexts)
	{
		m_Contexts = contexts;
		m_RequestContext = contexts.Request;

		m_ConstructPlayerRequestGroup = m_RequestContext.GetGroup(RequestMatcher.ConstructPlayer);
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
