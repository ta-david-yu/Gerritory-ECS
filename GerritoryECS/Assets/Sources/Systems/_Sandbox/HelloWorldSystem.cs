using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public sealed class HelloWorldSystem : IInitializeSystem
{
	private readonly ElementContext m_Context;

	public HelloWorldSystem(Contexts contexts)
	{
		m_Context = contexts.Element;
	}

	public void Initialize()
	{
		m_Context.CreateEntity().AddDebugMessage("Hello World!");
	}
}
