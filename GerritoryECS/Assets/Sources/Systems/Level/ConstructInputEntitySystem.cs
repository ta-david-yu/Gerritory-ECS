using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build input entity on request.
/// </summary>
public class ConstructInputEntitySystem : IFixedUpdateSystem
{
	private readonly CommandContext m_CommandContext;
	private readonly InputContext m_InputContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<CommandEntity> m_ConstructUserInputGroup;
	private readonly IGroup<CommandEntity> m_ConstructAIInputGroup;

	public ConstructInputEntitySystem(Contexts contexts)
	{
		m_InputContext = contexts.Input;
		m_CommandContext = contexts.Command;
		m_Contexts = contexts;

		m_ConstructUserInputGroup = m_CommandContext.GetGroup(CommandMatcher.ConstructUserInput);
		m_ConstructAIInputGroup = m_CommandContext.GetGroup(CommandMatcher.ConstructAIInput);
	}

	public void FixedUpdate()
	{
		foreach (var constructUserInputEntity in m_ConstructUserInputGroup.GetEntities())
		{
			InputEntity inputEntity = m_InputContext.CreateEntity();
			inputEntity.AddUserInput(constructUserInputEntity.ConstructUserInput.UserId, constructUserInputEntity.ConstructUserInput.TargetElementId);

			constructUserInputEntity.Destroy();
		}

		foreach (var constructAIInputEntity in m_ConstructAIInputGroup.GetEntities())
		{
			InputEntity inputEntity = m_InputContext.CreateEntity();
			inputEntity.AddAIInput
			(
				constructAIInputEntity.ConstructAIInput.TargetElementId, 
				new AIHelper.SearchSimulationState().AllocateWithContexts(m_Contexts, Unity.Collections.Allocator.Persistent)
			);

			constructAIInputEntity.Destroy();
		}
	}
}
