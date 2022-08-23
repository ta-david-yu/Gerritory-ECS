using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class HelloWorldSystem : IInitializeSystem
{
	private readonly GameContext m_Context;

	public HelloWorldSystem(Contexts contexts)
	{
		m_Context = contexts.Game;
	}

	public void Initialize()
	{
		m_Context.CreateEntity().AddDebugMessage("Hello World!");
	}
}
