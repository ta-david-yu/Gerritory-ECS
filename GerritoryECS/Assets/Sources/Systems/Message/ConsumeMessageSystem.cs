using JCMG.EntitasRedux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeMessageSystem : IFixedUpdateSystem, IUpdateSystem, ICleanupSystem
{
	private readonly MessageContext m_MessageContext;

	private readonly IGroup<MessageEntity> m_ConsumedMessageGroup;
	private readonly IGroup<MessageEntity> m_UnconsumedFixedUpdateMessageGroup;

	public ConsumeMessageSystem(Contexts contexts)
	{
		m_MessageContext = contexts.Message;

		m_ConsumedMessageGroup = m_MessageContext.GetGroup(MessageMatcher.Consumed);
		m_UnconsumedFixedUpdateMessageGroup = m_MessageContext.GetGroup(MessageMatcher.AllOf(MessageMatcher.ConsumeInFixedUpdate).NoneOf(MessageMatcher.Consumed));
	}


	public void FixedUpdate()
	{
		foreach (var entity in m_UnconsumedFixedUpdateMessageGroup.GetEntities())
		{
			// Consume the message.
			entity.IsConsumed = true;
		}
	}

	public void Update()
	{
		// TODO: consume update message
		// ...
	}

	public void Cleanup()
	{
		foreach (var entity in m_ConsumedMessageGroup.GetEntities())
		{
			// Destory the message entity.
			entity.Destroy();
		}
	}
}
