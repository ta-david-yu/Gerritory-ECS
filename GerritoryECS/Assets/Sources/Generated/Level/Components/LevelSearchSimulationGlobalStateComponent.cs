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

	public LevelEntity SearchSimulationGlobalStateEntity { get { return GetGroup(LevelMatcher.SearchSimulationGlobalState).GetSingleEntity(); } }
	public SearchSimulationGlobalStateComponent SearchSimulationGlobalState { get { return SearchSimulationGlobalStateEntity.SearchSimulationGlobalState; } }
	public bool HasSearchSimulationGlobalState { get { return SearchSimulationGlobalStateEntity != null; } }

	public LevelEntity SetSearchSimulationGlobalState(UnityEngine.Vector2Int[] newSortedTilePositions)
	{
		if (HasSearchSimulationGlobalState)
		{
			throw new JCMG.EntitasRedux.EntitasReduxException(
				"Could not set SearchSimulationGlobalState!\n" +
				this +
				" already has an entity with SearchSimulationGlobalStateComponent!",
				"You should check if the context already has a SearchSimulationGlobalStateEntity before setting it or use context.ReplaceSearchSimulationGlobalState().");
		}
		var entity = CreateEntity();
		#if !ENTITAS_REDUX_NO_IMPL
		entity.AddSearchSimulationGlobalState(newSortedTilePositions);
		#endif
		return entity;
	}

	public void ReplaceSearchSimulationGlobalState(UnityEngine.Vector2Int[] newSortedTilePositions)
	{
		#if !ENTITAS_REDUX_NO_IMPL
		var entity = SearchSimulationGlobalStateEntity;
		if (entity == null)
		{
			entity = SetSearchSimulationGlobalState(newSortedTilePositions);
		}
		else
		{
			entity.ReplaceSearchSimulationGlobalState(newSortedTilePositions);
		}
		#endif
	}

	public void RemoveSearchSimulationGlobalState()
	{
		SearchSimulationGlobalStateEntity.Destroy();
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
	public SearchSimulationGlobalStateComponent SearchSimulationGlobalState { get { return (SearchSimulationGlobalStateComponent)GetComponent(LevelComponentsLookup.SearchSimulationGlobalState); } }
	public bool HasSearchSimulationGlobalState { get { return HasComponent(LevelComponentsLookup.SearchSimulationGlobalState); } }

	public void AddSearchSimulationGlobalState(UnityEngine.Vector2Int[] newSortedTilePositions)
	{
		var index = LevelComponentsLookup.SearchSimulationGlobalState;
		var component = (SearchSimulationGlobalStateComponent)CreateComponent(index, typeof(SearchSimulationGlobalStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SortedTilePositions = newSortedTilePositions;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSearchSimulationGlobalState(UnityEngine.Vector2Int[] newSortedTilePositions)
	{
		var index = LevelComponentsLookup.SearchSimulationGlobalState;
		var component = (SearchSimulationGlobalStateComponent)CreateComponent(index, typeof(SearchSimulationGlobalStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SortedTilePositions = newSortedTilePositions;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySearchSimulationGlobalStateTo(SearchSimulationGlobalStateComponent copyComponent)
	{
		var index = LevelComponentsLookup.SearchSimulationGlobalState;
		var component = (SearchSimulationGlobalStateComponent)CreateComponent(index, typeof(SearchSimulationGlobalStateComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SortedTilePositions = (UnityEngine.Vector2Int[])copyComponent.SortedTilePositions.Clone();
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSearchSimulationGlobalState()
	{
		RemoveComponent(LevelComponentsLookup.SearchSimulationGlobalState);
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
	static JCMG.EntitasRedux.IMatcher<LevelEntity> _matcherSearchSimulationGlobalState;

	public static JCMG.EntitasRedux.IMatcher<LevelEntity> SearchSimulationGlobalState
	{
		get
		{
			if (_matcherSearchSimulationGlobalState == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<LevelEntity>)JCMG.EntitasRedux.Matcher<LevelEntity>.AllOf(LevelComponentsLookup.SearchSimulationGlobalState);
				matcher.ComponentNames = LevelComponentsLookup.ComponentNames;
				_matcherSearchSimulationGlobalState = matcher;
			}

			return _matcherSearchSimulationGlobalState;
		}
	}
}