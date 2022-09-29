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
	private readonly ElementContext m_ElementContext;
	private readonly InputContext m_InputContext;
	private readonly ConfigContext m_ConfigContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<LevelEntity> m_ConstructInputRequestGroup;

	public ConstructInputEntitySystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ElementContext = contexts.Element;
		m_InputContext = contexts.Input;
		m_ConfigContext = contexts.Config;
		m_MessageContext = contexts.Message;
		m_Contexts = contexts;

		m_ConstructInputRequestGroup = m_LevelContext.GetGroup(LevelMatcher.ConstructPlayer);
	}

	public void FixedUpdate()
	{
		throw new System.NotImplementedException();
	}
}
