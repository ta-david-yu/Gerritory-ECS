using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public class CleanupDebugMessageSystem : ICleanupSystem
{
	private readonly GameContext m_Context;
	private readonly IGroup<GameEntity> m_EntitiesWithDebugMessage;

	public CleanupDebugMessageSystem(Contexts contexts)
	{
		m_Context = contexts.Game;
		m_EntitiesWithDebugMessage = m_Context.GetGroup(GameMatcher.DebugMessage);
	}

	public void Cleanup()
	{
		foreach (var entity in m_EntitiesWithDebugMessage.GetEntities())
		{
			entity.Destroy();
		}
	}
}
