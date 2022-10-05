using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build input entity on request.
/// </summary>
public class ConstructInputEntitySystem : IFixedUpdateSystem
{
	private readonly RequestContext m_RequestContext;
	private readonly InputContext m_InputContext;

	private readonly IGroup<RequestEntity> m_ConstructUserInputGroup;
	private readonly IGroup<RequestEntity> m_ConstructAIInputGroup;

	public ConstructInputEntitySystem(Contexts contexts)
	{
		m_InputContext = contexts.Input;
		m_RequestContext = contexts.Request;

		m_ConstructUserInputGroup = m_RequestContext.GetGroup(RequestMatcher.ConstructUserInput);
		m_ConstructAIInputGroup = m_RequestContext.GetGroup(RequestMatcher.ConstructAIInput);
	}

	public void FixedUpdate()
	{
		foreach (var constructUserInputEntity in m_ConstructUserInputGroup.GetEntities())
		{
			InputEntity inputEntity = m_InputContext.CreateEntity();
			inputEntity.AddUserInput(constructUserInputEntity.ConstructUserInput.TargetPlayerId, constructUserInputEntity.ConstructUserInput.TargetPlayerId);

			constructUserInputEntity.Destroy();
		}

		foreach (var constructAIInputEntity in m_ConstructAIInputGroup.GetEntities())
		{
			InputEntity inputEntity = m_InputContext.CreateEntity();
			inputEntity.AddAIInput(constructAIInputEntity.ConstructAIInput.TargetPlayerId);

			constructAIInputEntity.Destroy();
		}
	}
}
