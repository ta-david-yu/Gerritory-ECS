using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageContextExtensions
{
	public static MessageEntity CreateFixedUpdateMessageEntity(this MessageContext context)
	{
		var entity = context.CreateEntity();
		entity.IsConsumeInFixedUpdate = true;
		return entity;
	}
}
