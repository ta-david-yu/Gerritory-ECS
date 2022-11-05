using JCMG.EntitasRedux;
using UnityEngine;

/// <summary>
/// Update appearing timer and revive ghost / spawn input entity once the time is up!
/// </summary>
public sealed class GhostAppearingSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly Contexts m_Contexts;
	private readonly IGroup<ElementEntity> m_AppearingGhostGroup;

	public GhostAppearingSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_Contexts = contexts;
		m_AppearingGhostGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.Ghost, ElementMatcher.GhostAppearing));
	}


	public void FixedUpdate()
	{
		foreach (var ghostEntity in m_AppearingGhostGroup.GetEntities())
		{
			float progress = ghostEntity.GhostAppearing.Progress;

			if (progress > 1.0f)
			{
				// Remove GhostAppearing component.
				ghostEntity.RemoveGhostAppearing();

				var inputEntity = m_Contexts.Input.GetEntityWithChaseNearestOnTileElementVictimInput(ghostEntity.OnTileElement.Id);
				if (inputEntity == null)
				{
					// Spawn input entity for the ghost if there isn't already one.
					inputEntity = ghostEntity.ConstructChaseNearestOnTileElementInputEntity(m_Contexts);
				}

				// Revive the ghost in case it was hidden before (reappearing now).
				ghostEntity.IsDead = false;
			}
			else
			{
				progress += Time.fixedDeltaTime / GameConstants.GhostAppearingTime;
				ghostEntity.ReplaceGhostAppearing(progress);
			}
		}
	}
}
