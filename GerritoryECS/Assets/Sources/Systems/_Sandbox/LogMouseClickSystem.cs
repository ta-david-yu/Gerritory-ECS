using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

public class LogMouseClickSystem : IUpdateSystem
{
	private readonly GameContext m_Context;

	public LogMouseClickSystem(Contexts contexts)
	{
		m_Context = contexts.Game;
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_Context.CreateEntity().AddDebugMessage("Left");
		}

		if (Input.GetMouseButtonDown(1))
		{
			m_Context.CreateEntity().AddDebugMessage("Right");
		}
	}
}
