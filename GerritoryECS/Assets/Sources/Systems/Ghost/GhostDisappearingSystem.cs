using JCMG.EntitasRedux;
using UnityEngine;

/// <summary>
/// Update disappear timer and remove the timer once the time is up.
/// </summary>
public class GhostDisappearingSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly CommandContext m_CommandContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<ElementEntity> m_AppearingGhostGroup;

	public GhostDisappearingSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_CommandContext = contexts.Command;
		m_Contexts = contexts;
		m_AppearingGhostGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.Ghost, ElementMatcher.GhostDisappearing));
	}


	public void FixedUpdate()
	{
		foreach (var ghostEntity in m_AppearingGhostGroup.GetEntities())
		{
			float progress = ghostEntity.GhostDisappearing.Progress;

			if (progress > 1.0f)
			{
				// Remove GhostDisappearing component.
				ghostEntity.RemoveGhostDisappearing();
			}
			else
			{
				progress += Time.fixedDeltaTime / GameConstants.GhostDisappearingTime;
				ghostEntity.ReplaceGhostDisappearing(progress);
			}
		}
	}
}
