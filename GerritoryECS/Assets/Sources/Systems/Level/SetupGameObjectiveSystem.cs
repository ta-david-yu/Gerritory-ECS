using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SetupGameObjectiveSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;

	public SetupGameObjectiveSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
	}
}
