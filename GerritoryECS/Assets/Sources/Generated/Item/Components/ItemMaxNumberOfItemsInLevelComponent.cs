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
	public MaxNumberOfItemsInLevelComponent MaxNumberOfItemsInLevel { get { return (MaxNumberOfItemsInLevelComponent)GetComponent(ItemComponentsLookup.MaxNumberOfItemsInLevel); } }
	public bool HasMaxNumberOfItemsInLevel { get { return HasComponent(ItemComponentsLookup.MaxNumberOfItemsInLevel); } }

	public void AddMaxNumberOfItemsInLevel(int newValue)
	{
		var index = ItemComponentsLookup.MaxNumberOfItemsInLevel;
		var component = (MaxNumberOfItemsInLevelComponent)CreateComponent(index, typeof(MaxNumberOfItemsInLevelComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceMaxNumberOfItemsInLevel(int newValue)
	{
		var index = ItemComponentsLookup.MaxNumberOfItemsInLevel;
		var component = (MaxNumberOfItemsInLevelComponent)CreateComponent(index, typeof(MaxNumberOfItemsInLevelComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyMaxNumberOfItemsInLevelTo(MaxNumberOfItemsInLevelComponent copyComponent)
	{
		var index = ItemComponentsLookup.MaxNumberOfItemsInLevel;
		var component = (MaxNumberOfItemsInLevelComponent)CreateComponent(index, typeof(MaxNumberOfItemsInLevelComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.Value = copyComponent.Value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveMaxNumberOfItemsInLevel()
	{
		RemoveComponent(ItemComponentsLookup.MaxNumberOfItemsInLevel);
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
public sealed partial class ItemMatcher
{
	static JCMG.EntitasRedux.IMatcher<ItemEntity> _matcherMaxNumberOfItemsInLevel;

	public static JCMG.EntitasRedux.IMatcher<ItemEntity> MaxNumberOfItemsInLevel
	{
		get
		{
			if (_matcherMaxNumberOfItemsInLevel == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<ItemEntity>)JCMG.EntitasRedux.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.MaxNumberOfItemsInLevel);
				matcher.ComponentNames = ItemComponentsLookup.ComponentNames;
				_matcherMaxNumberOfItemsInLevel = matcher;
			}

			return _matcherMaxNumberOfItemsInLevel;
		}
	}
}
