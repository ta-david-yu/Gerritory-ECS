using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public class CleanupDebugMessageSystem : ICleanupSystem
{
	private readonly ElementContext m_Context;
	private readonly IGroup<ElementEntity> m_EntitiesWithDebugMessage;

	public CleanupDebugMessageSystem(Contexts contexts)
	{
		m_Context = contexts.Element;
		m_EntitiesWithDebugMessage = m_Context.GetGroup(ElementMatcher.DebugMessage);
	}

	public void Cleanup()
	{
		foreach (var entity in m_EntitiesWithDebugMessage.GetEntities())
		{
			entity.Destroy();
		}
	}
}
