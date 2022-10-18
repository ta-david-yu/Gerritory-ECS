using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InitializeUniqueIdCounterSystem : IInitializeSystem
{
	private readonly LevelContext m_LevelContext;
	private readonly ConfigContext m_ConfigContext;

	public InitializeUniqueIdCounterSystem(Contexts contexts)
	{
		m_LevelContext = contexts.Level;
		m_ConfigContext = contexts.Config;
	}

	public void Initialize()
	{
		m_LevelContext.SetOnTileElementIdCounter(new UniqueIdCounter { Value = 0 });
		m_LevelContext.SetStateHolderIdCounter(new UniqueIdCounter { Value = 0 });
		m_LevelContext.SetItemEaterIdCounter(new UniqueIdCounter { Value = 0 });
		m_LevelContext.SetItemSpawnerIdCounter(new UniqueIdCounter { Value = 0 });
	}
}
