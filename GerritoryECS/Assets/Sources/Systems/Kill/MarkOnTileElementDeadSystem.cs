using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MarkOnTileElementDeadSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly CommandContext m_CommandContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<CommandEntity> m_MarkDeadRequestGroup;

	public MarkOnTileElementDeadSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_CommandContext = contexts.Command;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;

		m_MarkDeadRequestGroup = m_CommandContext.GetGroup(CommandMatcher.MarkOnTileElementDead);
	}

	public void FixedUpdate()
	{
		foreach (var markDeadRequest in m_MarkDeadRequestGroup.GetEntities())
		{
			ElementEntity onTileEntity = m_ElementContext.GetEntityWithOnTileElement(markDeadRequest.MarkOnTileElementDead.TargetOnTileElementId);

			onTileEntity.IsDead = true;

			m_Contexts.RemoveOnTileElementPositionFromLevel(onTileEntity);

			var dieMessageEntity = m_MessageContext.EmitOnTileElementDieMessage(onTileEntity.OnTileElement.Id);
			
			// Provides the cause of death if there is given one.
			if (markDeadRequest.HasStepKilledByOnTileElement)
			{
				Debug.Log("Killed by step!");
				dieMessageEntity.ReplaceStepKilledByOnTileElement(markDeadRequest.StepKilledByOnTileElement.KillerOnTileElementId);
			}

			markDeadRequest.Destroy();
		}
	}
}
