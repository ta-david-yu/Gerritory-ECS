using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make ghost disappear on enter tile if it's marked to be made disappear delayed.
/// </summary>
public sealed class MakeGhostDisappearOnEnterTileSystem : IFixedUpdateSystem
{
	private readonly ElementContext m_ElementContext;
	private readonly MessageContext m_MessageContext;
	private readonly Contexts m_Contexts;

	private readonly IGroup<ElementEntity> m_ToBeMadeDisappearGhostGroup;

	public MakeGhostDisappearOnEnterTileSystem(Contexts contexts)
	{
		m_ElementContext = contexts.Element;
		m_MessageContext = contexts.Message;

		m_Contexts = contexts;

		m_ToBeMadeDisappearGhostGroup = m_ElementContext.GetGroup(ElementMatcher.AllOf(ElementMatcher.Ghost, ElementMatcher.DelayGhostDisappearOnEnterTile));
	}

	public void FixedUpdate()
	{
		foreach (ElementEntity toBeDisappearEntity in m_ToBeMadeDisappearGhostGroup.GetEntities())
		{
			MessageEntity enterTileMessageEntity = m_MessageContext.GetEntityWithOnTileElementEnterTile(toBeDisappearEntity.OnTileElement.Id);
			if (enterTileMessageEntity == null)
			{
				// The ghost hasn't entered a tile yet, skip it.
				continue;
			}

			if (enterTileMessageEntity.IsConsumed)
			{
				// The message is an old message that has been consumed in the previous physics frame, ignore it.
				continue;
			}

			m_Contexts.TryMakeGhostDisappear(toBeDisappearEntity);
			toBeDisappearEntity.IsDelayGhostDisappearOnEnterTile = false;
		}
	}
}
