using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeRandomSeedSystem : IInitializeSystem
{
	private readonly ConfigContext m_ConfigContext;

	public InitializeRandomSeedSystem(Contexts contexts)
	{
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		UnityEngine.Random.InitState(m_ConfigContext.GameConfig.value.InitialRandomSeed);
	}
}
