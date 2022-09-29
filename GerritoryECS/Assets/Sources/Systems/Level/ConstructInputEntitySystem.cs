using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build input entity on request.
/// </summary>
public class ConstructInputEntitySystem : IFixedUpdateSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly InputContext m_InputContext;

	private readonly IGroup<LevelEntity> m_ConstructUserInputGroup;
	private readonly IGroup<LevelEntity> m_ConstructAIInputGroup;

	public ConstructInputEntitySystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_InputContext = contexts.Input;

		m_ConstructUserInputGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructUserInput);
		m_ConstructAIInputGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructAIInput);
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
			inputEntity.AddAIInput(constructAIInputEntity.ConstructAIInput.Movement, constructAIInputEntity.ConstructAIInput.TargetPlayerId);

			constructAIInputEntity.Destroy();
		}
	}
}
