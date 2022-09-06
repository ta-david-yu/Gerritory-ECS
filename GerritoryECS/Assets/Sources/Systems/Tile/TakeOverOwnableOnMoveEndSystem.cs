using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// Do take over logic if entities that end a movement have <see cref="TileOwnerComponent"/> + the move-to tile is <see cref="OwnableComponent"/>
/// </summary>
public sealed class TakeOverOwnableOnMoveEndSystem : ReactiveSystem<GameEntity> 
{
	private readonly GameContext m_GameContext;
	private readonly TileContext m_TileContext;

	public TakeOverOwnableOnMoveEndSystem(Contexts contexts) : base(contexts.Game)
	{
		m_GameContext = contexts.Game;
		m_TileContext = contexts.Tile;
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.MoveOnTileEnd.Added());
	}

	protected override bool Filter(GameEntity entity)
	{
		// Only true if the entity is a tile owner & just finished moving
		return entity.HasTileOwner && entity.HasMoveOnTileEnd;
	}

	protected override void Execute(List<GameEntity> entities)
	{
		foreach (var ownerEntity in entities)
		{
			Vector2Int enterTilePosition = ownerEntity.MoveOnTileEnd.ToPosition;
			TileEntity enterTileEntity = m_TileContext.GetEntityWithTilePosition(enterTilePosition);
			
			if (!enterTileEntity.HasOwnable)
			{
				// The tile is not ownable, do nothing.
				continue;
			}


			// TODO: take-overable check logic
			// ...

			if (enterTileEntity.Ownable.HasOwner)
			{
				// Decrement previous owner's number of owned tiles.
				GameEntity previousOwnerEntity = m_GameContext.GetEntityWithTileOwner(enterTileEntity.Ownable.OwnerId);
				previousOwnerEntity.ReplaceTileOwner(previousOwnerEntity.TileOwner.Id, previousOwnerEntity.TileOwner.NumberOfOwnedTiles - 1);
			}

			// Take over the tile and increment owner's number of owned tiles.
			enterTileEntity.ReplaceOwnable(true, ownerEntity.TileOwner.Id);
			ownerEntity.ReplaceTileOwner(ownerEntity.TileOwner.Id, ownerEntity.TileOwner.NumberOfOwnedTiles + 1);
		}
	}
}
