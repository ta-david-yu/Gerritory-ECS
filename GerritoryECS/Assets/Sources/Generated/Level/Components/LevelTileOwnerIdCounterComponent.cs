//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class LevelContext {

	public LevelEntity TileOwnerIdCounterEntity { get { return GetGroup(LevelMatcher.TileOwnerIdCounter).GetSingleEntity(); } }
	public TileOwnerIdCounterComponent TileOwnerIdCounter { get { return TileOwnerIdCounterEntity.TileOwnerIdCounter; } }
	public bool HasTileOwnerIdCounter { get { return TileOwnerIdCounterEntity != null; } }

	public LevelEntity SetTileOwnerIdCounter(UniqueIdCounter newValue)
	{
		if (HasTileOwnerIdCounter)
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				"Could not set TileOwnerIdCounter!\n" +
				this +
				" already has an entity with TileOwnerIdCounterComponent!",
				"You should check if the context already has a TileOwnerIdCounterEntity before setting it or use context.ReplaceTileOwnerIdCounter().");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.AddTileOwnerIdCounter(newValue);
		#endif
		return entity;
	}

	public void ReplaceTileOwnerIdCounter(UniqueIdCounter newValue)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = TileOwnerIdCounterEntity;
		if (entity == null)
		{
			entity = SetTileOwnerIdCounter(newValue);
		}
		else
		{
			entity.ReplaceTileOwnerIdCounter(newValue);
		}
		#endif
	}

	public void RemoveTileOwnerIdCounter()
	{
		TileOwnerIdCounterEntity.Destroy();
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class LevelEntity
{
	public TileOwnerIdCounterComponent TileOwnerIdCounter { get { return (TileOwnerIdCounterComponent)GetComponent(LevelComponentsLookup.TileOwnerIdCounter); } }
	public bool HasTileOwnerIdCounter { get { return HasComponent(LevelComponentsLookup.TileOwnerIdCounter); } }

	public void AddTileOwnerIdCounter(UniqueIdCounter newValue)
	{
		var index = LevelComponentsLookup.TileOwnerIdCounter;
		var component = (TileOwnerIdCounterComponent)CreateComponent(index, typeof(TileOwnerIdCounterComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceTileOwnerIdCounter(UniqueIdCounter newValue)
	{
		var index = LevelComponentsLookup.TileOwnerIdCounter;
		var component = (TileOwnerIdCounterComponent)CreateComponent(index, typeof(TileOwnerIdCounterComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyTileOwnerIdCounterTo(TileOwnerIdCounterComponent copyComponent)
	{
		var index = LevelComponentsLookup.TileOwnerIdCounter;
		var component = (TileOwnerIdCounterComponent)CreateComponent(index, typeof(TileOwnerIdCounterComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveTileOwnerIdCounter()
	{
		RemoveComponent(LevelComponentsLookup.TileOwnerIdCounter);
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class LevelMatcher
{
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherTileOwnerIdCounter;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> TileOwnerIdCounter
	{
		get
		{
			if (_matcherTileOwnerIdCounter == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.TileOwnerIdCounter);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherTileOwnerIdCounter = matcher;
			}

			return _matcherTileOwnerIdCounter;
		}
	}
}