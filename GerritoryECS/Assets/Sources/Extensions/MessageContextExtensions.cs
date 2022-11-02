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

	public static MessageEntity EmitOnTileElementEnterTileMessage(this MessageContext context, int onTileElementId, Vector2Int enterPosition)
	{
		var enterTileMessageEntity = CreateFixedUpdateMessageEntity(context);
		enterTileMessageEntity.ReplaceOnTileElementEnterTile(onTileElementId, enterPosition);
		return enterTileMessageEntity;
	}

	public static MessageEntity EmitOnTileElementLeaveTileMessage(this MessageContext context, int onTileElementId, Vector2Int leavePosition)
	{
		var leaveTileMessageEntity = CreateFixedUpdateMessageEntity(context);
		leaveTileMessageEntity.ReplaceOnTileElementLeaveTile(onTileElementId, leavePosition);
		return leaveTileMessageEntity;
	}

	public static MessageEntity EmitOnTileElementRespawnMessage(this MessageContext context, int onTileElementId, Vector2Int respawnPosition)
	{
		var respawnMessageEntity = CreateFixedUpdateMessageEntity(context);
		respawnMessageEntity.ReplaceOnTileElementRespawn(onTileElementId, respawnPosition);
		return respawnMessageEntity;
	}

	public static MessageEntity EmitOnTileElementDieMessage(this MessageContext context, int onTileElementId)
	{
		var dieMessageEntity = CreateFixedUpdateMessageEntity(context);
		dieMessageEntity.ReplaceOnTileElementDie(onTileElementId);
		return dieMessageEntity;
	}
}
