using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCMG.EntitasRedux;

/// <summary>
/// Do take over logic if entities that end movement have TileOwner component.
/// </summary>
public sealed class TakeOverOwnableOnMoveEndSystem : ReactiveSystem<GameEntity> 
{
	private readonly GameContext m_GameContext;

	public TakeOverOwnableOnMoveEndSystem(Contexts contexts) : base(contexts.Game)
	{
		m_GameContext = contexts.Game;
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
			Vector2Int tilePosition = ownerEntity.MoveOnTileEnd.ToPosition;
			GameEntity tileEntity = m_GameContext.GetEntityWithTilePosition(tilePosition);
			
			if (!tileEntity.HasOwnable)
			{
				// The tile is not ownable, do nothing.
				continue;
			}


			// TODO: take-overable check logic
			// ...

			if (tileEntity.Ownable.HasOwner)
			{
				// Decrement previous owner's number of owned tiles.
				GameEntity previousOwnerEntity = m_GameContext.GetEntityWithTileOwner(tileEntity.Ownable.OwnerId);
				previousOwnerEntity.ReplaceTileOwner(previousOwnerEntity.TileOwner.Id, previousOwnerEntity.TileOwner.NumberOfOwnedTiles - 1);
			}

			// Take over the tile and increment owner's number of owned tiles.
			tileEntity.ReplaceOwnable(true, ownerEntity.TileOwner.Id);
			ownerEntity.ReplaceTileOwner(ownerEntity.TileOwner.Id, ownerEntity.TileOwner.NumberOfOwnedTiles + 1);
		}
	}
}
