//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemEntity
{
	public SpawnedByGlobalSpawnerComponent SpawnedByGlobalSpawner { get { return (SpawnedByGlobalSpawnerComponent)GetComponent(ItemComponentsLookup.SpawnedByGlobalSpawner); } }
	public bool HasSpawnedByGlobalSpawner { get { return HasComponent(ItemComponentsLookup.SpawnedByGlobalSpawner); } }

	public void AddSpawnedByGlobalSpawner(int newSpawnerId)
	{
		var index = ItemComponentsLookup.SpawnedByGlobalSpawner;
		var component = (SpawnedByGlobalSpawnerComponent)CreateComponent(index, typeof(SpawnedByGlobalSpawnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpawnerId = newSpawnerId;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceSpawnedByGlobalSpawner(int newSpawnerId)
	{
		var index = ItemComponentsLookup.SpawnedByGlobalSpawner;
		var component = (SpawnedByGlobalSpawnerComponent)CreateComponent(index, typeof(SpawnedByGlobalSpawnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpawnerId = newSpawnerId;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopySpawnedByGlobalSpawnerTo(SpawnedByGlobalSpawnerComponent copyComponent)
	{
		var index = ItemComponentsLookup.SpawnedByGlobalSpawner;
		var component = (SpawnedByGlobalSpawnerComponent)CreateComponent(index, typeof(SpawnedByGlobalSpawnerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.SpawnerId = copyComponent.SpawnerId;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveSpawnedByGlobalSpawner()
	{
		RemoveComponent(ItemComponentsLookup.SpawnedByGlobalSpawner);
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
public partial class ItemEntity : ISpawnedByGlobalSpawnerEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ItemMatcher
{
	static JCMG.EntitasRedux.IMatcher<ItemEntity> _matcherSpawnedByGlobalSpawner;

	public static JCMG.EntitasRedux.IMatcher<ItemEntity> SpawnedByGlobalSpawner
	{
		get
		{
			if (_matcherSpawnedByGlobalSpawner == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ItemEntity>)JCMG.EntitasRedux.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.SpawnedByGlobalSpawner);
				matcher.ComponentNames = ItemComponentsLookup.ComponentNames;
				_matcherSpawnedByGlobalSpawner = matcher;
			}

			return _matcherSpawnedByGlobalSpawner;
		}
	}
}
