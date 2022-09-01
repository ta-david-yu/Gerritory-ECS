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
		foreach (var entity in entities)
		{
			Vector2Int tilePosition = entity.MoveOnTileEnd.ToPosition;
			GameEntity tileEntity = m_GameContext.GetEntityWithTilePosition(tilePosition);
			
			if (!tileEntity.HasOwnable)
			{
				// The tile is not ownable, do nothing.
				continue;
			}

			// TODO: take-overable check logic
			// ...

			// TODO: do take over points counting
			// ...

			// Take over the tile
			tileEntity.ReplaceOwnable(true, entity.TileOwner.Id);
		}
	}
}
