//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using JCMG.EntitasRedux;

public partial class GameEntity
{
	/// <summary>
	/// Copies <paramref name="component"/> to this entity as a new component instance.
	/// </summary>
	public void CopyComponentTo(IComponent component)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		if (component is DeadComponent Dead)
		{
			IsDead = true;
		}
		else if (component is MovementInputActionComponent MovementInputAction)
		{
			CopyMovementInputActionTo(MovementInputAction);
		}
		else if (component is MoveOnTileComponent MoveOnTile)
		{
			CopyMoveOnTileTo(MoveOnTile);
		}
		else if (component is TeamComponent Team)
		{
			CopyTeamTo(Team);
		}
		else if (component is OnTileElementKillerComponent OnTileElementKiller)
		{
			IsOnTileElementKiller = true;
		}
		else if (component is OnTileElementComponent OnTileElement)
		{
			CopyOnTileElementTo(OnTileElement);
		}
		else if (component is DebugMessageComponent DebugMessage)
		{
			CopyDebugMessageTo(DebugMessage);
		}
		else if (component is OnTilePositionComponent OnTilePosition)
		{
			CopyOnTilePositionTo(OnTilePosition);
		}
		else if (component is MoveOnTileEndComponent MoveOnTileEnd)
		{
			CopyMoveOnTileEndTo(MoveOnTileEnd);
		}
		else if (component is TileCollapserComponent TileCollapser)
		{
			IsTileCollapser = true;
		}
		else if (component is CanBeDeadComponent CanBeDead)
		{
			IsCanBeDead = true;
		}
		else if (component is TileOwnerComponent TileOwner)
		{
			IsTileOwner = true;
		}
		else if (component is StateHolderComponent StateHolder)
		{
			CopyStateHolderTo(StateHolder);
		}
		else if (component is SpeedChangeableComponent SpeedChangeable)
		{
			CopySpeedChangeableTo(SpeedChangeable);
		}
		else if (component is IComponentRefComponent IComponentRef)
		{
			CopyIComponentRefTo(IComponentRef);
		}
		else if (component is ItemEaterComponent ItemEater)
		{
			CopyItemEaterTo(ItemEater);
		}
		else if (component is MoveOnTileBeginComponent MoveOnTileBegin)
		{
			CopyMoveOnTileBeginTo(MoveOnTileBegin);
		}
		else if (component is PlayerComponent Player)
		{
			CopyPlayerTo(Player);
		}
		else if (component is DeadAddedListenerComponent DeadAddedListener)
		{
			CopyDeadAddedListenerTo(DeadAddedListener);
		}
		else if (component is DeadRemovedListenerComponent DeadRemovedListener)
		{
			CopyDeadRemovedListenerTo(DeadRemovedListener);
		}
		else if (component is MoveOnTileAddedListenerComponent MoveOnTileAddedListener)
		{
			CopyMoveOnTileAddedListenerTo(MoveOnTileAddedListener);
		}
		else if (component is TeamAddedListenerComponent TeamAddedListener)
		{
			CopyTeamAddedListenerTo(TeamAddedListener);
		}
		else if (component is OnTilePositionAddedListenerComponent OnTilePositionAddedListener)
		{
			CopyOnTilePositionAddedListenerTo(OnTilePositionAddedListener);
		}
		else if (component is MoveOnTileEndAddedListenerComponent MoveOnTileEndAddedListener)
		{
			CopyMoveOnTileEndAddedListenerTo(MoveOnTileEndAddedListener);
		}
		else if (component is MoveOnTileBeginAddedListenerComponent MoveOnTileBeginAddedListener)
		{
			CopyMoveOnTileBeginAddedListenerTo(MoveOnTileBeginAddedListener);
		}
		#endif
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>.
	/// </summary>
	public void CopyTo(GameEntity copyToEntity)
	{
		for (var i = 0; i < GameComponentsLookup.TotalComponents; ++i)
		{
			if (HasComponent(i))
			{
				if (copyToEntity.HasComponent(i))
				{
					throw new EntityAlreadyHasComponentException(
						i,
						"Cannot copy component '" +
						GameComponentsLookup.ComponentNames[i] +
						"' to " +
						this +
						"!",
						"If replacement is intended, please call CopyTo() with `replaceExisting` set to true.");
				}

				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies all components on this entity to <paramref name="copyToEntity"/>; if <paramref name="replaceExisting"/>
	/// is true any of the components that <paramref name="copyToEntity"/> has that this entity has will be replaced,
	/// otherwise they will be skipped.
	/// </summary>
	public void CopyTo(GameEntity copyToEntity, bool replaceExisting)
	{
		for (var i = 0; i < GameComponentsLookup.TotalComponents; ++i)
		{
			if (!HasComponent(i))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(i) || replaceExisting)
			{
				var component = GetComponent(i);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}

	/// <summary>
	/// Copies components on this entity at <paramref name="indices"/> in the <see cref="GameComponentsLookup"/> to
	/// <paramref name="copyToEntity"/>. If <paramref name="replaceExisting"/> is true any of the components that
	/// <paramref name="copyToEntity"/> has that this entity has will be replaced, otherwise they will be skipped.
	/// </summary>
	public void CopyTo(GameEntity copyToEntity, bool replaceExisting, params int[] indices)
	{
		for (var i = 0; i < indices.Length; ++i)
		{
			var index = indices[i];

			// Validate that the index is within range of the component lookup
			if (index < 0 && index >= GameComponentsLookup.TotalComponents)
			{
				const string OUT_OF_RANGE_WARNING =
					"Component Index [{0}] is out of range for [{1}].";

				const string HINT = "Please ensure any CopyTo indices are valid.";

				throw new IndexOutOfLookupRangeException(
					string.Format(OUT_OF_RANGE_WARNING, index, nameof(GameComponentsLookup)),
					HINT);
			}

			if (!HasComponent(index))
			{
				continue;
			}

			if (!copyToEntity.HasComponent(index) || replaceExisting)
			{
				var component = GetComponent(index);
				copyToEntity.CopyComponentTo(component);
			}
		}
	}
}
